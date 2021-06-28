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
            Options = ContePropertyManager.GetListForTovarFilters();
        }

        public override string GetDefaultTemplate() => this.Templates["FILT"];
        public override string GetNameOfPageProperties() => OpencartFilterExcelReportBuilder.PROPERTIES_PAGE_NAME;
        public override string GetNameOfPageValues() => OpencartFilterExcelReportBuilder.VALUES_PAGE_NAME;
    }
}