using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultyParser.Core.Excel;

namespace MultyParser.Core
{
    public abstract class ResultBookCreaterBase
    {
        protected abstract ExcelBook CreateBookForResultData(string filePath = null);

        public string ResultFilePrefix { get; set; }    // начало имени выходного файла
        public string TemplateFile { get; set; }        // шаблон, на основе которого создаётся выходной файл
        public string SubFolder { get; set; }       // подпапка для сохранения файлов
        public int Number { get; set; }             // текущий номер тома
        public int VolumeSize { get; set; }         // размер тома
        public int DigitNum { get; set; }           // кол-во цифр в номер тома
        public int TovarPos { get; set; }

        public void Init(string filePrefix, string template = null, string subFolder = null, int num = 1, int volume = 500, int digit = 3)
        {
            this.ResultFilePrefix = filePrefix;
            this.TemplateFile = template;
            this.SubFolder = subFolder;
            this.Number = num;
            this.VolumeSize = volume;
            this.DigitNum = digit;
        }

        protected ExcelBook _book;
        public ExcelBook Book
        {
            get
            {
                if (_book == null)
                    _book = CreateBookForResultData(this.TemplateFile);
                return _book;
            }
        }

        public void ResetBook()
        {
            this._book.Close();
            this._book = null;
        }

        public virtual bool WriteDataToBook(Dictionary<string, List<Dictionary<int, string>>> argsData)
        {
            foreach (string key in argsData.Keys)
            {
                ExcelPage page = Book.Pages[key];
                List<Dictionary<int, string>> listOfRows = argsData[key];
                foreach (Dictionary<int, string> values in listOfRows)
                    WriteLineToPage(page, values);
            }
            if (VolumeSize != 0)
            {
                if (TovarPos == VolumeSize)
                {
                    this.SaveBookAsPartialFile();
                    TovarPos = 1;
                }
            }
            return true;
        }

        protected virtual bool WriteLineToPage(ExcelPage page, Dictionary<int, string> values)
        {
            return page.WriteLine(values);
        }

        protected void SaveBookAsSingleFile()
        {
            string fileName = this.ResultFilePrefix + ".xlsx";
            StoreFileToDisk(fileName);
        }

        protected void SaveBookAsPartialFile()
        {
            string format = "{0}-{1:d" + this.DigitNum + "}.xlsx";
            string fileName = String.Format(format, this.ResultFilePrefix, this.Number++);
            StoreFileToDisk(fileName);
        }

        protected void StoreFileToDisk(string fileName)
        {
            string basePath = MultyParserApp.BaseDirForResultFiles + "\\";

            if (SubFolder != null)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(basePath + SubFolder);
                if (!dirInfo.Exists)
                    dirInfo.Create();
                basePath += SubFolder + "\\";
            }
            
            string filePath = basePath + fileName;

            if (File.Exists(filePath))
                File.Delete(filePath);
            this.Book.Save(filePath);
            this.ResetBook();
        }

        protected virtual ExcelBook CreateBookForResultData(Dictionary<string, Dictionary<int, string>> titlesOfPages, string filePath)
        {
            List<string> keys = new List<string>();
            foreach (string key in titlesOfPages.Keys)
                keys.Add(key);

            ExcelBook resultBook;

            // проверяем, был ли указан шыблон,
            // на основе которого будут формироваться файлы с данными

            if (filePath == null)
            {
                // если шаблона нет, создаём Excel-файл, вставляем листы и выводим заголовки
                resultBook = ExcelBook.Create(keys);
                foreach (string key in keys)
                {
                    resultBook.Pages[key].CurrentRow = 1;
                    resultBook.Pages[key].WriteLine(titlesOfPages[key], true);
                }
            }
            else
            {
                // если шаблон указан, проверяем, есть ли нужные листы
                resultBook = ExcelBook.Open(filePath);
                foreach (string key in keys)
                {
                    if (resultBook.HasPage(key))
                    {
                        // если лист есть, измеряем его, заголовки не трогаем
                        resultBook.Pages[key].Measure();
                        resultBook.Pages[key].CurrentRow = resultBook.Pages[key].TotalRows + 1;
                    }
                    else
                    {
                        // если листа нет, вставляем его и заполняем заголовки
                        resultBook.AddNewPage(key);
                        resultBook.Pages[key].CurrentRow = 1;
                        resultBook.Pages[key].WriteLine(titlesOfPages[key], true);
                    }
                }
            }
            this.TovarPos = 1;
            return resultBook;
        }

        // сохранение книги с обработанными данными
        // или, в случае разбиения на тома, последней её части
        public virtual bool FinalProcessing()
        {
            if (this.TovarPos > 1)
            {
                if (VolumeSize > 0 && Number > 1)
                    SaveBookAsPartialFile();
                else
                    SaveBookAsSingleFile();
            }
            return true;
        }
    }
}
