using System.Collections.Generic;
using MultyParser.Core;
using MultyParser.Core.Html;
using MultyParser.Opencart;

namespace Velveto.Conte
{
    public class ConteOpencartFilterHtmlParser : ConteOpencartPropertyHtmlParserBase
    {
        public ConteOpencartFilterHtmlParser()
        {
            _optionReportBuilder = new OpencartFilterExcelReportBuilder();
            Options = new DictionaryOfOptions();
            Options.Add(new HtmlOption(1000, "Цвет", "radio", 1, ".content-description>div>*", ".ty-product-options__radio--label", @"[A-zА-яЁё]+(-[A-zА-яЁё]+)?\b(?!:)", true), new List<string>());
            Options.Add(new HtmlOption(2000, "Размер", "radio", 1, ".content-description>div>*", ".ty-product-options__radio--label", @"\d+(-\d+)?", true), new List<string>());
            Options.Add(new HtmlOption(3000, "Плотность", "radio", 1, null, "span.ty-control-group>*", @"\d+ ден", true), new List<string>());
            Options.Add(new HtmlOption(4000, "Бренд", "radio", 1, null, "span.ty-control-group>*", @"[A-z]\b(?!:)", true), new List<string>());
        }

        public override string GetDefaultTemplate() => this.Templates["FILT"];
        public override string GetNameOfPageProperties() => OpencartFilterExcelReportBuilder.PROPERTIES_PAGE_NAME;
        public override string GetNameOfPageValues() => OpencartFilterExcelReportBuilder.VALUES_PAGE_NAME;
    }
}