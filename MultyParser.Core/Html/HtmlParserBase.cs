using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Leaf.xNet;
using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Dom;

using MultyParser.Core;

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
        protected abstract string GetSelectorForMainPhoto();                // селектор главного фото товара
        protected abstract string GetAttributeForMainPhotoUrl();            // атрибут со ссылкой на главное фото
        protected abstract string GetSelectorForTableOfSpecifications();    // селектор таблицы спецификаций
        protected abstract string GetTableCellName();                       // селектор названия спецификации
        protected abstract string GetTableCellValue();                      // селектор значения спецификации

        /* Преобразование объекта товара в структуру для заполнения книги Excel */
        protected abstract Dictionary<string, List<Dictionary<int, string>>> GatherValuesFromTovarObject(int tovarID, HtmlTovar tovarObject, out bool flagNew);

        protected abstract Dictionary<string, int> GetForTransferToMainPage();  // список того, что нужно перенести из спецификаций на главную

        public int CurrentTovarID { get; set; }

        public virtual int CountPages(string link)
        {
            List<string> pages = new List<string>();
            GetListOfPages(link, pages);
            return pages.Count;
        }

        public virtual bool DoParsingOfIncomingHtml(int startTovarID, string link, DoWorkEventArgs args)
        {
            List<string> pages = new List<string>();
            GetListOfPages(link, pages);
            this.CurrentTovarID = startTovarID;

            foreach (string p in pages)
            {
                MultyParserApp.CurrentPageNum++;
                IHtmlDocument doc = GetWebDocument(p);
                var listItems = doc.QuerySelectorAll(GetSelectorForList());
                MultyParserApp.TotalRows = listItems.Count();
                foreach (var item in listItems)
                {
                    if (args.Cancel) break;
                    this.OnSetProgressValue(listItems.Index(item) + 1);

                    try
                    {
                        bool flagNew;
                        HtmlTovar tovarObject = ProcessTovar(item.GetAttribute("href"));
                        Dictionary<string, List<Dictionary<int, string>>> values =
                            GatherValuesFromTovarObject(CurrentTovarID, tovarObject, out flagNew);
                        if (flagNew)
                            CurrentTovarID++;
                        if (values != null)
                            this.ResultBookCreater.WriteDataToBook(values);
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

        protected virtual HtmlTovar ProcessTovar(string link)
        {
            IHtmlDocument doc = GetWebDocument(GetBaseUrl() + link);
            HtmlTovar result = new HtmlTovar();
            result.Name = doc.QuerySelector(GetSelectorForTovarName()).TextContent.Trim();
            result.MainPhotoUrl = doc.QuerySelector(GetSelectorForMainPhoto())
                .GetAttribute(GetAttributeForMainPhotoUrl());
            result.Description = doc.QuerySelector(GetSelectorForTovarDescription()).TextContent.Trim();

            var specItems = doc.QuerySelectorAll(GetSelectorForTableOfSpecifications());
            foreach (var item in specItems)
            {
                string key = item.QuerySelector(GetTableCellName()).TextContent.Trim();
                string value = item.QuerySelector(GetTableCellValue()).TextContent.Trim();
                result.Specifications.Add(key, value);
            }

            return result;
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
