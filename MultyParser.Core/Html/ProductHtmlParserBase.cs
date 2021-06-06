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
    public abstract class ProductHtmlParserBase : HtmlParserBase
    {
        protected abstract string GetSelectorForTovarName();                // селектор названия товара
        protected abstract string GetSelectorForTovarDescription();         // селектор описания товара
        protected abstract string GetSelectorForPrice();                    // селектор для цены
        protected abstract string GetSelectorForListOfImages();             // селектор списка фотографий (по умолчанию нулевой)
        protected abstract string GetSelectorForTableOfSpecifications();    // селектор таблицы спецификаций
        protected abstract string GetTableCellName();       // селектор названия спецификации
        protected abstract string GetTableCellValue();      // селектор значения спецификации
        protected abstract string GetSizeUnit();            // единицы измерения для габаритов
        protected abstract string GetMassUnit();            // единицы измерения для массы

        /* Данные, которые понадобятся нам для заполнения информации о товаре */
        protected HtmlTovar PriorTovar { get; set; }            // товар, полученный на предыдущей итерации
        protected TovarGroup CommonTovarGroup;                  // группу товаров берём из глобального класса
        protected Dictionary<string, int> ParserSpecifications; // названия спецификаций на сайте - из парсера
        protected Dictionary<string, int> ForTransfer;          // то что перенести в основную таблицу - тоже

        protected abstract Dictionary<int, string> MakeDictionaryForAttributesPageRow(int tovarID, string groupName, string attributeName, string value);
        protected abstract string GetCodeOfTovarGroup();
        protected abstract Dictionary<string, int> GetSpecifications();

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

        protected override void BeforeEntityLoop()
        {
            this.CommonTovarGroup = MultyParserApp.TovarGroups[this.GetCodeOfTovarGroup()];
            this.ParserSpecifications = this.GetSpecifications();
            this.ForTransfer = this.GetForTransferToMainPage();
        }

        protected override void ProcessingEntityInLoop(IHtmlDocument docDetails)
        {
            string pageName;
            HtmlTovar tovarObject = GetTovarFromDocument(docDetails, this.GetBrandName(), this.GetMainOption());
            Dictionary<string, List<Dictionary<int, string>>> rowsForBook
                = new Dictionary<string, List<Dictionary<int, string>>>();

            if (PriorTovar == null || PriorTovar.Name != tovarObject.Name) // если прочитан новый товар, заполняем основную информацию и атрибуты
            {
                this.EntityID++;

                /* Записываем основные данные о товаре */
                Dictionary<int, string> dataCommon = this.GatherCommonDataFromTovarObject(this.EntityID, tovarObject, out pageName);
                rowsForBook.Add(pageName, new List<Dictionary<int, string>> { dataCommon });

                /* Дополнительные фотографии товара */
                List<Dictionary<int, string>> photos = this.GatherAdditionalImagesFromTovarObject(this.EntityID, tovarObject, out pageName);
                rowsForBook.Add(pageName, photos);

                /* Данные атрибутов товара */
                List<Dictionary<int, string>> attributes = this.GatherAttributesFromTovarObject(
                        this.EntityID, tovarObject, CommonTovarGroup, dataCommon,
                        ParserSpecifications, ForTransfer, out pageName);
                rowsForBook.Add(pageName, attributes);

                if (this.GetMainOption() != null)
                {
                    /* Главная опция товара */
                    List<Dictionary<int, string>> options = this.GatherOptionFromTovarObject(
                            this.EntityID, out pageName);
                    rowsForBook.Add(pageName, options);
                }

                /* Записываем SEO-заголовки товара для всех версий сайта */
                Dictionary<int, string> dataSeo = this.GatherSeoKeywordsFromTovarObject
                    (this.EntityID, tovarObject, out pageName);
                rowsForBook.Add(pageName, new List<Dictionary<int, string>> { dataSeo });

                this.ResultBookCreater.EntityPos++;
                this.PriorTovar = tovarObject;
            }
            if (this.GetMainOption() != null)
            {
                /* Значение главной опции товара */
                List<Dictionary<int, string>> optionValues = this.GatherOptionValueFromTovarObject(
                    this.EntityID, tovarObject, out pageName);
                rowsForBook.Add(pageName, optionValues);
            }

            this.ResultBookCreater.WriteDataToBook(rowsForBook);
        }

        protected virtual HtmlTovar GetTovarFromDocument(IHtmlDocument doc, string brandName, string optionName)
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

            result.Price = GetPrice(doc.QuerySelector(GetSelectorForPrice()).TextContent);


            var specItems = doc.QuerySelectorAll(GetSelectorForTableOfSpecifications());
            string cellKey = GetTableCellName();
            string cellValue = GetTableCellValue();
            foreach (var item in specItems)
            {
                string key, value;
                if (cellKey == cellValue)
                {
                    key = item.Children[0].TextContent.ToString();
                    value = item.Children[1].TextContent.ToString();
                }
                else
                {
                    key = item.QuerySelector(cellKey).TextContent.Trim();
                    value = item.QuerySelector(cellValue).TextContent.Trim();
                }
                result.Specifications.Add(key, value);
            }

            return result;
        }
    }
}
