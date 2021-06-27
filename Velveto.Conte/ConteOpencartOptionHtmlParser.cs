using System.Collections.Generic;
using MultyParser.Core;
using MultyParser.Core.Html;
using MultyParser.Opencart;

namespace Velveto.Conte
{
    public class ConteOpencartOptionHtmlParser : ConteOpencartPropertyHtmlParserBase
    {
        public ConteOpencartOptionHtmlParser()
        {
            _optionReportBuilder = new OpencartOptionExcelReportBuilder();
            Options = new DictionaryOfOptions();
            Options.Add(new HtmlOption(1000, "Цвет", "radio", 1, ".content-description>div>*", ".ty-product-options__radio--label", @"[A-zА-яЁё]+(-[A-zА-яЁё]+)?\b(?!:)", true), new List<string>());
            Options.Add(new HtmlOption(2000, "Размер", "radio", 1, ".content-description>div>*", ".ty-product-options__radio--label", @"\d+(-\d+)?", true), new List<string>());
        }

        public override string GetDefaultTemplate() => this.Templates["OPTI"];
        public override string GetNameOfPageProperties() => OpencartOptionExcelReportBuilder.PROPERTIES_PAGE_NAME;
        public override string GetNameOfPageValues() => OpencartOptionExcelReportBuilder.VALUES_PAGE_NAME;
    }
}
