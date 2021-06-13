using System.Collections.Generic;
using MultyParser.Core;
using MultyParser.Core.Html;
using MultyParser.Opencart;

namespace Velveto.Conte
{
    public class ConteOptionOpencartHtmlParser : OpencartOptionHtmlParserBase
    {
        public ConteOptionOpencartHtmlParser()
        {
            Options = new VelvetoDictionaryOfOptions();
        }

        public override string GetSiteName() => "contesale.ru";

        protected override string GetBaseUrl() => "https://contesale.ru/";
        protected override string GetSelectorForList() => "a.product-title";
        protected override string GetSelectorForNextButton() => "a.ty-pagination__next";
    }
}