using System.Collections.Generic;
using MultyParser.Core;
using MultyParser.Core.Html;
using MultyParser.Opencart;

namespace Velveto.Conte
{
    public class ConteOptionOpencartHtmlParser : OpencartOptionHtmlParserBase
    {
        public override string GetSiteName() => "contesale.ru";

        protected override string GetBaseUrl() => "https://contesale.ru/";
        protected override string GetSelectorForList() => "a.product-title";
        protected override string GetSelectorForNextButton() => "a.ty-pagination__next";

        protected override Dictionary<HtmlOption, List<string>> GetDictionaryOfOptions()
        {
            Dictionary<HtmlOption, List<string>> result = new Dictionary<HtmlOption, List<string>>();
            result.Add(new HtmlOption(1000, "Цвет", "radio", 1, ".ty-control-group>.ty-clear-both>.ty-product-options__radio--label", @"\D+"), new List<string>());
            result.Add(new HtmlOption(2000, "Размер", "radio", 1, ".ty-control-group>.ty-clear-both>.ty-product-options__radio--label", @"\d+"), new List<string>());
            return result;
        }
    }
}