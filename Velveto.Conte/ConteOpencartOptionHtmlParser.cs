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
            Options = ContePropertyManager.GetListForTovarOptions();
        }

        public override string GetDefaultTemplate() => this.Templates["OPTI"];
        public override string GetNameOfPageProperties() => OpencartOptionExcelReportBuilder.PROPERTIES_PAGE_NAME;
        public override string GetNameOfPageValues() => OpencartOptionExcelReportBuilder.VALUES_PAGE_NAME;
    }
}
