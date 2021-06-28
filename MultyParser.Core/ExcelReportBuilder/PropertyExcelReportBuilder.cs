using MultyParser.Core.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Core.ExcelReportBuilder
{
    public abstract class PropertyExcelReportBuilder : ExcelReportBuilderBase
    {
        public abstract Dictionary<int, object> MakeLineForOption(TovarProperty option);
        public abstract Dictionary<int, object> MakeLineForOptionValue(TovarProperty option, string value);
    }
}
