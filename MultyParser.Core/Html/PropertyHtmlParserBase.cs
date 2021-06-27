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
    public abstract class PropertyHtmlParserBase : HtmlParserBase
    {
        protected PropertyExcelReportBuilder _optionReportBuilder;
        public PropertyExcelReportBuilder OptionReportBuilder
        {
            get
            {
                return _optionReportBuilder;
            }
        }

        public abstract string GetNameOfPageProperties();
        public abstract string GetNameOfPageValues();

        public override ExcelReportBuilderBase GetReportBuilderInstance() => OptionReportBuilder;

        /* Справочник опций - он будет заполнен при обходе страниц товара */
        public DictionaryOfOptions Options { get; set; }

        /* Получение опции товара из словаря */
        protected abstract List<Dictionary<int, object>> GatherOptionFromDictionary(Dictionary<HtmlOption, List<string>> options, out string pageName);

        /* Получение значения опции товара из словаря */
        protected abstract List<Dictionary<int, object>> GatherOptionValueFromDictionary(Dictionary<HtmlOption, List<string>> options, out string pageName);

        protected override void ProcessingEntityInLoop(IHtmlDocument docDetails)
        {
            Options.ReadOptionValuesFromDocument(docDetails);
        }

        protected override void AfterEntityLoop()
        {
            string pageName;
            Dictionary<string, List<Dictionary<int, object>>> rowsForBook
                = new Dictionary<string, List<Dictionary<int, object>>>();

            /* Формируем страницу с опциями */
            List<Dictionary<int, object>> dataOptions = this.GatherOptionFromDictionary(Options.Members, out pageName);
            rowsForBook.Add(pageName, dataOptions);

            /* Формируем страницу со значениями опций */
            List<Dictionary<int, object>> dataValues = this.GatherOptionValueFromDictionary(Options.Members, out pageName);
            rowsForBook.Add(pageName, dataValues);

            OptionReportBuilder.WriteDataToBook(rowsForBook);
        }
    }
}
