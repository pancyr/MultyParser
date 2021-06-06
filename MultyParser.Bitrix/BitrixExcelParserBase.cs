using MultyParser.Core.Excel;

namespace MultyParser.Bitrix
{
    public abstract class BitrixExcelParserBase : ExcelParserBase
    {
        public BitrixExcelParserBase()
        {
            this._resultBookCreater = new BitrixResultBookCreater();
        }
    }
}