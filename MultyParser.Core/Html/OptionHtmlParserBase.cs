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
    public abstract class OptionHtmlParserBase : HtmlParserBase
    {
        public Dictionary<HtmlOption, List<string>> DictionaryOfOptions { get; set; }

        protected abstract Dictionary<HtmlOption, List<string>> GetDictionaryOfOptions();

        /* Получение опции товара из словаря */
        protected abstract List<Dictionary<int, string>> GatherOptionFromDictionary(Dictionary<HtmlOption, List<string>> options, out string pageName);

        /* Получение значения опции товара из словаря */
        protected abstract List<Dictionary<int, string>> GatherOptionValueFromDictionary(Dictionary<HtmlOption, List<string>> options, out string pageName);

        protected override void BeforeEntityLoop()
        {
            DictionaryOfOptions = this.GetDictionaryOfOptions();
        }

        protected override void ProcessingEntityInLoop(IHtmlDocument docDetails)
        {
            foreach (HtmlOption option in DictionaryOfOptions.Keys)
            {
                var listItems = docDetails.QuerySelectorAll(option.HtmlSelector);
                foreach (var item in listItems)
                {
                    string optionName = item.TextContent.Trim();
                    if (option.TestRegular(optionName) && !DictionaryOfOptions[option].Contains(optionName))
                        DictionaryOfOptions[option].Add(optionName);
                }
            }
        }

        protected override void AfterEntityLoop()
        {
            string pageName;
            Dictionary<string, List<Dictionary<int, string>>> rowsForBook
                = new Dictionary<string, List<Dictionary<int, string>>>();

            /* Формируем страницу с опциями */
            List<Dictionary<int, string>> dataOptions = this.GatherOptionFromDictionary(DictionaryOfOptions, out pageName);
            rowsForBook.Add(pageName, dataOptions);

            /* Формируем страницу со значениями опций */
            List<Dictionary<int, string>> dataValues = this.GatherOptionValueFromDictionary(DictionaryOfOptions, out pageName);
            rowsForBook.Add(pageName, dataValues);

            ResultBookCreater.WriteDataToBook(rowsForBook);
        }
    }
}
