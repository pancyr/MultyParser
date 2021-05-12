using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

using Leaf.xNet;
using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Dom;

namespace MultyParser.Core.Html
{
    public abstract class HtmlParserBase : ParserBase
    {
        public abstract string GetSiteName();

        protected abstract string GetBaseUrl();                             // базовый адрес сайта
        protected abstract string GetSelectorForList();                     // селектор списка ссылок на товар
        protected abstract string GetSelectorForNextButton();               // селектор кнопки пагинации
        protected abstract string GetSelectorForTovarName();                // селектор названия товара
        protected abstract string GetSelectorForTovarDescription();         // селектор описания товара
        protected abstract string GetSelectorForPrice();                    // селектор для цены
        protected abstract string GetSelectorForListOfImages();             // селектор списка фотографий (по умолчанию нулевой)
        protected abstract string GetSelectorForTableOfSpecifications();    // селектор таблицы спецификаций
        protected abstract string GetTableCellName();       // селектор названия спецификации
        protected abstract string GetTableCellValue();      // селектор значения спецификации
        protected abstract string GetSizeUnit();    // единицы измерения для габаритов
        protected abstract string GetMassUnit();    // единицы измерения для массы


        protected HtmlTovar PriorTovar { get; set; }                        // товар, полученный на предыдущей итерации

        /* Получение основной информации о товаре из объекта класса HtmlTovar */
        protected abstract Dictionary<int, string> GatherCommonDataFromTovarObject(int tovarID, HtmlTovar tovarObject, out string pageName);

        /* Получение дополнительных изображений товара товара из объекта класса HtmlTovar */
        protected abstract List<Dictionary<int, string>> GatherAdditionalImagesFromTovarObject(int tovarID, HtmlTovar tovarObject, out string pageName);

        /* Получение атрибутов товара из объекта класса HtmlTovar */
        protected abstract List<Dictionary<int, string>> GatherAttributesFromTovarObject(
            int tovarID, HtmlTovar tovarObject, TovarGroup tovarGroup, Dictionary<int, string> dataCommon,
            Dictionary<string, int> parserSpecifications, Dictionary<string, int> forTransfer, out string pageName);

        /* Получение опции товара из объекта класса HtmlTovar */
        protected abstract List<Dictionary<int, string>> GatherOptionFromTovarObject(int tovarID, out string pageName);

        /* Получение значения опции товара из объекта класса HtmlTovar */
        protected abstract List<Dictionary<int, string>> GatherOptionValueFromTovarObject(int tovarID, HtmlTovar tovarObject, out string pageName);

        /* Получение SEO-заголовков из объекта класса HtmlTovar */
        protected abstract Dictionary<int, string> GatherSeoKeywordsFromTovarObject(int tovarID, HtmlTovar tovarObject, out string pageName);

        protected abstract Dictionary<string, int> GetForTransferToMainPage();  // список того, что нужно перенести из спецификаций на главную

        public virtual string GetDepartmentByUrl(string url)
        {
            if (url.EndsWith("/"))
                url = url.Substring(0, url.Length - 1);
            Regex reg = new Regex(@"[^/]*", RegexOptions.RightToLeft);
            return reg.Match(url).ToString();
        }

        public virtual int CountPages(string link)
        {
            List<string> pages = new List<string>();
            GetListOfPages(link, pages);
            return pages.Count;
        }

        public virtual bool DoParsingOfIncomingHtml(ref int currentTovarID, string link, DoWorkEventArgs args)
        {
            /* Получаем все ссылки пагинации, чтобы подсчитать ко-во страниц */
            List<string> pages = new List<string>();
            GetListOfPages(link, pages);

            /* Добываем данные, которые понадобятся нам для заполнения информации о товаре */
            TovarGroup commonTovarGroup = MultyParserApp.TovarGroups[this.GetCodeOfTovarGroup()];   // группу товаров берём из глобального класса
            Dictionary<string, int> parserSpecifications = this.GetSpecifications();                // названия спецификаций на сайте - из парсера
            Dictionary<string, int> forTransfer = this.GetForTransferToMainPage();                  // то что перенести в основную таблицу - тоже

            foreach (string p in pages)
            {
                MultyParserApp.CurrentPageNum++;
                IHtmlDocument docList = GetWebDocument(p);
                var listItems = docList.QuerySelectorAll(GetSelectorForList()); // список всех товарных позиций на странице каталога
                MultyParserApp.TotalRows = listItems.Count();               // это для отображения в диалоге хода процесса
                foreach (var item in listItems)
                {
                    if (args.Cancel) break;
                    this.OnSetProgressValue(listItems.Index(item) + 1);     // ставим прогресс-бар на актуальную позицию

                    try
                    {
                        string pageName;
                        IHtmlDocument docDetails = GetWebDocument(GetBaseUrl() + item.GetAttribute("href"));
                        HtmlTovar tovarObject = ProcessTovar(
                            docDetails, this.GetBrandName(), this.GetMainOption()); // теперь переходим по ссылке из каталога на страницу товара
                        Dictionary<string, List<Dictionary<int, string>>> rowsForBook
                            = new Dictionary<string, List<Dictionary<int, string>>>();

                        if (PriorTovar == null || PriorTovar.Name != tovarObject.Name) // если прочитан новый товар, заполняем основную информацию и атрибуты
                        {
                            currentTovarID++;
                            this.PriorTovar = tovarObject;

                            /* Записываем основные данные о товаре */
                            Dictionary<int, string> dataCommon = this.GatherCommonDataFromTovarObject
                                (currentTovarID, tovarObject, out pageName);
                            rowsForBook.Add(pageName, new List<Dictionary<int, string>> { dataCommon });

                            /* Дополнительные фотографии товара */
                            List<Dictionary<int, string>> photos = this.GatherAdditionalImagesFromTovarObject(
                                    currentTovarID, tovarObject, out pageName);
                            rowsForBook.Add(pageName, photos);

                            /* Данные атрибутов товара */
                            List<Dictionary<int, string>> attributes = this.GatherAttributesFromTovarObject(
                                    currentTovarID, tovarObject, commonTovarGroup, dataCommon,
                                    parserSpecifications, forTransfer, out pageName);
                            rowsForBook.Add(pageName, attributes);

                            if (this.GetMainOption() != null)
                            {
                                /* Главная опция товара */
                                List<Dictionary<int, string>> options = this.GatherOptionFromTovarObject(
                                        currentTovarID, out pageName);
                                rowsForBook.Add(pageName, options);
                            }

                            /* Записываем SEO-заголовки товара для всех версий сайта */
                            Dictionary<int, string> dataSeo = this.GatherSeoKeywordsFromTovarObject
                                (currentTovarID, tovarObject, out pageName);
                            rowsForBook.Add(pageName, new List<Dictionary<int, string>> { dataSeo });
                            this.ResultBookCreater.TovarPos++;
                        }
                        if (this.GetMainOption() != null)
                        {
                            /* Значение главной опции товара */
                            List<Dictionary<int, string>> optionValues = this.GatherOptionValueFromTovarObject(
                                currentTovarID, tovarObject, out pageName);
                            rowsForBook.Add(pageName, optionValues);
                        }
                        
                        this.ResultBookCreater.WriteDataToBook(rowsForBook);
                    }
                    catch //(Exception exp)
                    {
                        //Console.WriteLine(exp.Message);
                        continue;
                    }
                }
            }

            ResultBookCreater.FinalProcessing();
            return true;
        }

        public virtual bool GetListOfPages(string link, List<string> pages)
        {
            pages.Add(link);
            IHtmlDocument doc = GetWebDocument(link);
            IElement next = doc.QuerySelector(GetSelectorForNextButton());

            if (next != null)
            {
                string n_link = GetBaseUrl() + next.GetAttribute("href");
                if (link != n_link)
                    GetListOfPages(n_link, pages);
            }

            return true;
        }

        protected virtual HtmlTovar ProcessTovar(IHtmlDocument doc, string brandName, string optionName)
        {
            string productName, productModel, productOption, groupName;
            HtmlTovar result = new HtmlTovar();

            result.Name = doc.QuerySelector(GetSelectorForTovarName()).TextContent.Trim();
            if (optionName != null)
                if (this.DivideByKeyword(result.Name, optionName, out productName, out productOption))
                {
                    result.Name = productName;
                    result.Option = productOption;
                }

            if (brandName != null)
                if (this.DivideByKeyword(result.Name, brandName, out groupName, out productModel))
                {
                    result.Group = groupName;
                    result.Model = productModel;
                }

            result.Photos = doc.QuerySelectorAll(GetSelectorForListOfImages())
                .Select(a => GetFullUrl(a.GetAttribute("href"))).ToList();
            result.Description = doc.QuerySelector(GetSelectorForTovarDescription()).TextContent.Trim();

            string strPrice = doc.QuerySelector(GetSelectorForPrice()).TextContent;
            strPrice = Regex.Replace(strPrice, @"\D", "").Replace(".", ",");
            Regex regPrice = new Regex(@"\d+[,]?\d*");
            Match m = regPrice.Match(strPrice);
            result.Price = (m.Success) ? m.ToString() : "";

            var specItems = doc.QuerySelectorAll(GetSelectorForTableOfSpecifications());
            foreach (var item in specItems)
            {
                string key = item.QuerySelector(GetTableCellName()).TextContent.Trim();
                string value = item.QuerySelector(GetTableCellValue()).TextContent.Trim();
                result.Specifications.Add(key, value);
            }

            return result;
        }

        protected string GetFullUrl(string address)
        {
            int q_index = address.IndexOf("?");
            if (q_index > -1)
                address = address.Substring(0, q_index);
            return  (address.StartsWith(@"//")) ? "https:" + address : GetBaseUrl() + address;
        }

        protected IHtmlDocument GetWebDocument(string link)
        {
            HttpRequest request = new HttpRequest();
            string response = request.Get(link).ToString();

            HtmlParser parser = new HtmlParser();
            IHtmlDocument result = parser.ParseDocument(response);
            return result;
        }
    }
}
