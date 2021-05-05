using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Core.Excel
{
    public abstract class ExcelParserBase : ParserBase
    {
        protected abstract bool PositionToFirstRow(ExcelPage page);
        protected abstract bool GetFirstOrNextColumns(ExcelPage page, ref Dictionary<string, int> columnNumbers);
        protected abstract Dictionary<string, List<Dictionary<int, string>>> GatherValuesFromPage(ExcelPage page, Dictionary<string, int> columnNumbers);

        public virtual bool DoParsingOfIncomingPage(ExcelPage incomPage, DoWorkEventArgs args)
        {
            Dictionary<string, int> columnNumbers = null;
            while (GetFirstOrNextColumns(incomPage, ref columnNumbers))
                if (PositionToFirstRow(incomPage))
                    do
                    {
                        if (args.Cancel) break;
                        Dictionary<string, List<Dictionary<int, string>>> values = GatherValuesFromPage(incomPage, columnNumbers);
                        if (values != null)
                            this.ResultBookCreater.WriteDataToBook(values);
                        this.OnSetProgressValue(incomPage.CurrentRow);
                        incomPage.CurrentRow++;
                    }
                    while (!incomPage.IsEndPosition());
            ResultBookCreater.FinalProcessing();
            return true;
        }
    }
}
