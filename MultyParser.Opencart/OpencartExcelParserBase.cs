using MultyParser.Core.Excel;

namespace MultyParser.Opencart
{
    public abstract class OpencartExcelParserBase : ExcelParserBase
    {
        public OpencartExcelParserBase()
        {
            this._resultBookCreater = new OpencartResultBookCreater();
        }
    }
}
