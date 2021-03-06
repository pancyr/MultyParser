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
using MultyParser.Core.ExcelReportBuilder;

namespace MultyParser.Core.Html
{
    public abstract class TovarHtmlParserBase : HtmlParserBase
    {
        protected TovarExcelReportBuilder _productBookCreater;
        public TovarExcelReportBuilder ProductBookCreater
        {
            get
            {
                return _productBookCreater;
            }
        }

        public override ExcelReportBuilderBase GetReportBuilderInstance() => ProductBookCreater;

        public virtual Dictionary<string, string> DepartmentCategories => null;

        /* Обязательные параметры, которые должны быть определены в производных классах */
        protected abstract string GetSelectorForTovarName();                // селектор названия товара
        protected abstract string GetSelectorForPrice();                    // селектор для цены
        protected abstract string GetSelectorForListOfImages();             // селектор списка фотографий (по умолчанию нулевой)
        protected abstract string GetSizeUnit();            // единицы измерения для габаритов
        protected abstract string GetMassUnit();            // единицы измерения для массы

        /* Параметры, часть из которых может не использоваться */
        protected virtual string GetSelectorForTovarDescription() => null;          // селектор описания товара
        protected virtual string GetSelectorForTableOfSpecifications() => null;     // селектор таблицы спецификаций
        protected virtual string GetTableCellName() => null;                        // селектор названия спецификации
        protected virtual string GetTableCellValue() => null;                       // селектор значения спецификации
        protected virtual string GetSelectorForAppendInfo() => null;                // селектор данных для обработки специальной ф-ей

        /* Данные, которые понадобятся нам для заполнения информации о товаре */
        protected Tovar PriorTovar { get; set; }            // товар, полученный на предыдущей итерации
        protected TovarGroup CommonTovarGroup;                  // группу товаров берём из глобального класса
        protected Dictionary<string, int> ParserSpecifications; // названия спецификаций на сайте - из парсера
        protected Dictionary<string, int> ForTransfer;          // то что перенести в основную таблицу - тоже

        
        protected abstract Dictionary<string, int> GetSpecifications();

        /* Получение основной информации о товаре из объекта класса HtmlTovar */
        protected abstract Dictionary<int, object> GatherCommonDataFromTovarObject(int tovarID, Tovar tovarObject, out string pageName);

        /* Получение дополнительных изображений товара товара из объекта класса HtmlTovar */
        protected abstract List<Dictionary<int, object>> GatherAdditionalImagesFromTovarObject(int tovarID, Tovar tovarObject, out string pageName);

        /* Получение атрибутов товара из объекта класса HtmlTovar */
        protected abstract List<Dictionary<int, object>> GatherAttributesFromTovarObject(
            int tovarID, Tovar tovarObject, TovarGroup tovarGroup, Dictionary<int, object> dataCommon,
            Dictionary<string, int> parserSpecifications, Dictionary<string, int> forTransfer, out string pageName);

        /* Получение опции товара из объекта класса HtmlTovar */
        protected abstract List<Dictionary<int, object>> GatherOptionFromTovarObject(int tovarID, string optionName, out string pageName);

        /* Получение значения опции товара из объекта класса HtmlTovar */
        protected abstract List<Dictionary<int, object>> GatherOptionValueFromTovarObject(int tovarID, string optionName, string optionValue, out string pageName);

        /* Получение значения фильтра из объекта класса HtmlTovar */
        protected abstract List<Dictionary<int, object>> GatherFilterValueFromTovarObject(int tovarID, string optionName, string optionValue, out string pageName);

        /* Получение SEO-заголовков из объекта класса HtmlTovar */
        protected abstract Dictionary<int, object> GatherSeoKeywordsFromTovarObject(int tovarID, Tovar tovarObject, out string pageName);

        protected abstract Dictionary<string, int> GetForTransferToMainPage();  // список того, что нужно перенести из спецификаций на главную

        protected virtual DictionaryOfProperty GetOptionsOfTovar() => null;
        protected virtual DictionaryOfProperty GetFiltersOfTovar() => null;
        protected virtual void ProcessOfAppendInfo(Tovar tovarObject, List<string> properties) { }

        protected override void BeforeEntityLoop()
        {
            this.CommonTovarGroup = MultyParserApp.TovarGroups[this.GetCodeOfTovarGroup()];
            this.ParserSpecifications = this.GetSpecifications();
            this.ForTransfer = this.GetForTransferToMainPage();
        }

        protected override void ProcessingEntityInLoop(IHtmlDocument docDetails)
        {
            string pageName;
            Tovar tovarObject = GetTovarFromDocument(docDetails, GetBrandName(), GetMainOptionName());
            Dictionary<string, List<Dictionary<int, object>>> rowsForBook
                = new Dictionary<string, List<Dictionary<int, object>>>();
            Dictionary<int, object> dataCommon = null;

            if (PriorTovar == null || PriorTovar.Name != tovarObject.Name) // если прочитан новый товар, заполняем основную информацию и атрибуты
            {
                this.EntityID++;

                /* Записываем основные данные о товаре */
               dataCommon = this.GatherCommonDataFromTovarObject(this.EntityID, tovarObject, out pageName);
                rowsForBook.Add(pageName, new List<Dictionary<int, object>> { dataCommon });

                /* Дополнительные фотографии товара */
                List<Dictionary<int, object>> photos = this.GatherAdditionalImagesFromTovarObject(this.EntityID, tovarObject, out pageName);
                rowsForBook.Add(pageName, photos);

                /* Данные атрибутов товара */
                List<Dictionary<int, object>> attributes = this.GatherAttributesFromTovarObject(
                        this.EntityID, tovarObject, CommonTovarGroup, dataCommon,
                        ParserSpecifications, ForTransfer, out pageName);
                rowsForBook.Add(pageName, attributes);

                if (GetMainOptionName() != null)
                {
                    /* Главная опция товара */
                    List<Dictionary<int, object>> options = this.GatherOptionFromTovarObject(
                            this.EntityID, GetMainOptionName(), out pageName);
                    rowsForBook.Add(pageName, options);
                }

                /* Работа со списком опций */
                if (tovarObject.Options != null)
                {
                    string optionsPage = null, optionValuesPage = null;
                    List<Dictionary<int, object>> options = new List<Dictionary<int, object>>();
                    List<Dictionary<int, object>> optionValues = new List<Dictionary<int, object>>();
                    foreach (TovarProperty option in tovarObject.Options.Members.Keys)
                    {
                        string name = option.DisplayName ?? option.Name;
                        options.AddRange(GatherOptionFromTovarObject(
                            this.EntityID, name, out optionsPage));

                        foreach (string val in tovarObject.Options.Members[option])
                        {
                            optionValues.AddRange(GatherOptionValueFromTovarObject(
                                this.EntityID, name, val, out optionValuesPage));
                        }
                        
                    }
                    if (optionValues.Count > 0)
                    {
                        rowsForBook.Add(optionsPage, options);
                        rowsForBook.Add(optionValuesPage, optionValues);
                    }
                }

                /* Работа со списком фильтров */
                if (tovarObject.Filters != null)
                {
                    string filterValuesPage = null;
                    List<Dictionary<int, object>> filterValues = new List<Dictionary<int, object>>();
                    foreach (TovarProperty filter in tovarObject.Filters.Members.Keys)
                    {
                        foreach (string val in tovarObject.Filters.Members[filter])
                        {
                            string name = filter.DisplayName ?? filter.Name;
                            filterValues.AddRange(GatherFilterValueFromTovarObject(
                                this.EntityID, name, val, out filterValuesPage));
                        }

                    }
                    if (filterValues.Count > 0)
                        rowsForBook.Add(filterValuesPage, filterValues);
                }

                /* Записываем SEO-заголовки товара для всех версий сайта */
                Dictionary<int, object> dataSeo = this.GatherSeoKeywordsFromTovarObject
                    (this.EntityID, tovarObject, out pageName);
                rowsForBook.Add(pageName, new List<Dictionary<int, object>> { dataSeo });

                this.ProductBookCreater.EntityPos++;
                this.PriorTovar = tovarObject;
            }
            if (this.GetMainOptionName() != null)
            {
                /* Значение главной опции товара */
                List<Dictionary<int, object>> optionValues = this.GatherOptionValueFromTovarObject(
                    this.EntityID, GetMainOptionName(), tovarObject.MainOptionValue, out pageName);
                rowsForBook.Add(pageName, optionValues);
            }
            if (dataCommon != null)
                this.PostProcessData(tovarObject, dataCommon);
            this.ProductBookCreater.WriteDataToBook(rowsForBook);
        }

        protected virtual Tovar GetTovarFromDocument(IHtmlDocument doc, string brandName, string optionName)
        {
            string productName, productModel, productOption, groupName;
            Tovar result = new Tovar();

            result.Name = doc.QuerySelector(GetSelectorForTovarName()).TextContent.Trim();
            if (optionName != null)
                if (this.DivideByKeyword(result.Name, optionName, out productName, out productOption))
                {
                    result.Name = productName;
                    result.MainOptionValue = productOption;
                }

            if (brandName != null)
                if (this.DivideByKeyword(result.Name, brandName, out groupName, out productModel))
                {
                    result.Group = groupName;
                    result.Model = productModel;
                }

            result.Photos = doc.QuerySelectorAll(GetSelectorForListOfImages())
                .Select(a => GetFullUrl(a.GetAttribute("href"))).ToList();

            string tagDescription = GetSelectorForTovarDescription();
            if (tagDescription != null)
                result.Description = doc.QuerySelector(tagDescription).TextContent.Trim();

            result.Price = GetPrice(doc.QuerySelector(GetSelectorForPrice()).TextContent);

            result.Options = GetOptionsOfTovar();
            if (result.Options != null)
                result.Options.ReadPropertyValuesFromDocument(doc);

            result.Filters = GetFiltersOfTovar();
            if (result.Filters != null)
                result.Filters.ReadPropertyValuesFromDocument(doc);

            string tagSpecifics = GetSelectorForTableOfSpecifications();
            if (tagSpecifics != null)
            {
                var specItems = doc.QuerySelectorAll(tagSpecifics);
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
            }

            string tagAppend = GetSelectorForAppendInfo();
            if (tagAppend != null)
            {
                List<string> properties = doc.QuerySelectorAll(tagAppend)
                    .Select(a => a.TextContent).ToList();
                ProcessOfAppendInfo(result, properties);
            }

            return result;
        }
    }
}
