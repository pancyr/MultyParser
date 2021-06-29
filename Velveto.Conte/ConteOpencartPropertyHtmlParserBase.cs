using MultyParser.Opencart;

namespace Velveto.Conte
{
    public abstract class ConteOpencartPropertyHtmlParserBase : OpencartPropertyHtmlParserBase
    {
        public override string GetSiteName() => "contesale.ru";

        protected override string GetBaseUrl() => "https://contesale.ru/";
        protected override string GetCodeOfTovarGroup() => "FEMALE";
        protected override string GetSelectorForList() => "a.product-title";
        protected override string GetSelectorForNextButton() => "a.ty-pagination__next";
    }
}