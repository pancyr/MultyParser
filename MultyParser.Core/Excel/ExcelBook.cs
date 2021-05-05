using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Office.Interop.Excel;

namespace MultyParser.Core.Excel
{
    public class ExcelBook
    {
        public ExcelBook(Workbook _book)
        {
            this._excelBook = _book;
            this.Styles = _excelBook.Styles;
        }

        protected Workbook _excelBook; // рабочая книга
        public readonly Styles Styles;

        public string Name { get; set; }

        private Dictionary<string, ExcelPage> _pages;
        public Dictionary<string, ExcelPage> Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new Dictionary<string, ExcelPage>();
                return _pages;
            }
        }

        public static ExcelBook Create(List<string> pageNames)
        {
            Workbook _workbook = MultyParserApp.GetExcelApp().Workbooks.Add(Type.Missing);
            int count = _workbook.Worksheets.Count;
            for (int i = count; i > 1; i--)
                ((Worksheet)_workbook.Worksheets[i]).Delete();

            ((Worksheet)_workbook.Worksheets[1]).Name = pageNames[0];
            ExcelBook result = new ExcelBook(_workbook);
            result.Pages.Add(pageNames[0], new ExcelPage((Worksheet)_workbook.Worksheets[1], _workbook.Styles));

            for (int i = 1; i < pageNames.Count; i++)
                result.AddNewPage(pageNames[i]);

            return result;
        }

        public static ExcelBook Open(string path)
        {
            Workbook _workbook = MultyParserApp.GetExcelApp().Workbooks.Open(path,
                Type.Missing, true, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);

            int pos = path.LastIndexOf("\\") + 1;
            ExcelBook result = new ExcelBook(_workbook);
            string fileName = path.Substring(pos, path.Length - pos);
            int pset = fileName.LastIndexOf(".");
            result.Name = fileName.Substring(0, pset);

            int count = _workbook.Worksheets.Count;
            for (int i = 1; i <= count; i++)
            {
                Worksheet sheet = (Worksheet)_workbook.Worksheets[i];
                result.Pages.Add(sheet.Name, new ExcelPage(sheet));
            }

            return result;
        }

        public ExcelPage AddNewPage(string pageName)
        {
            int totalSheets = _excelBook.Worksheets.Count;
            Worksheet sheet = (Worksheet)_excelBook.Worksheets.Add(Type.Missing, _excelBook.Worksheets[totalSheets], Type.Missing, Type.Missing);
            sheet.Name = pageName;
            ExcelPage result = new ExcelPage(sheet, Styles);
            Pages.Add(sheet.Name, result);
            return result;
        }

        public bool HasPage(string pageName) => this.Pages.ContainsKey(pageName);

        public void Save(string savePath)
        {
            _excelBook.Sheets[1].Select();
            _excelBook.SaveAs(savePath, XlFileFormat.xlWorkbookDefault, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlShared,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }

        public void Close()
        {
            _excelBook.Close(false, Type.Missing, Type.Missing);
        }
    }
}
