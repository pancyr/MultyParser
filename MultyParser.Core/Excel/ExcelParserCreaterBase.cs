using System;
using System.Collections.Generic;
using System.Text;

namespace MultyParser.Core.Excel
{
    public abstract class ExcelParserCreaterBase : IParserCreater
    {
        public virtual ParserBase GetParserObjectForProducts() => null;
        public virtual ParserBase GetParserObjectForOptions() => null;

        public virtual bool DetectIncomingPageForParserClass(ExcelPage page)
        {
            page.Measure();
            bool isFound;

            Object[] attrs = this.GetType().GetCustomAttributes(typeof(TableColumnAttribute), false);
            SortedList<int, string> sortedColumns = new SortedList<int, string>();
            foreach (TableColumnAttribute column in attrs)
                sortedColumns.Add(column.Num, column.Title);
            page.CurrentRow = 1;
            while (!page.IsEndPosition())
            {
                int columnNum = sortedColumns.Keys[0];
                page.CurrentRow = page.FindInColumn(columnNum, sortedColumns[columnNum], page.CurrentRow);
                if (page.CurrentRow == 0)
                    return false;
                isFound = true;
                for (int i = 1; i < sortedColumns.Count; i++)
                {
                    columnNum = sortedColumns.Keys[i];
                    if (page.ReadText(page.CurrentRow, columnNum) != sortedColumns[columnNum])
                    {
                        isFound = false;
                        break;
                    }
                }
                if (isFound)
                    return true;
                page.CurrentRow++;
            }
            return false;
        }

    }
}
