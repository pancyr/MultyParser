using MultyParser.Core.Excel;

namespace MultyParser.Bitrix
{
    public abstract class BitrixHtmlParserBase : ExcelParserBase
    {
        public BitrixHtmlParserBase()
        {
            this._resultBookCreater = new BitrixResultBookCreater();
        }
    }
}