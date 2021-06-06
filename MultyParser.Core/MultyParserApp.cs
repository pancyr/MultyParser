using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Configuration;

using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using MultyParser.Configuration;
using MultyParser.Core.Excel;
using MultyParser.Core.Html;

namespace MultyParser.Core
{
    public sealed class MultyParserApp
    {
        public static Dictionary<string, TovarGroup> TovarGroups { get; set; }

        private static Microsoft.Office.Interop.Excel.Application _excelApp;
        public static Microsoft.Office.Interop.Excel.Application GetExcelApp()
        {
            try
            {
                if (_excelApp == null)
                {
                    _excelApp = new Microsoft.Office.Interop.Excel.Application(); //Пробуем получить новый экземпляр Excel  
                    _excelApp.DisplayAlerts = true;
                }
                return _excelApp;
            }
            catch
            {
                throw new Exception("Ошибка создания экземпляра MS Excel");
            }
        }

        public static string BaseDirForResultFiles
        {
            get
            {
                return System.Windows.Forms.Application.StartupPath + "\\Result";
            }
        }

        public static void InitConfiguration(MultyParserConfigSection section)
        {
            TovarGroups = new Dictionary<string, TovarGroup>();
            foreach (TovarGroupConfigElement group in section.TovarGroups)
                TovarGroups.Add(group.Code, new TovarGroup(group));
        }

        /*public static bool DoParsingOfExcelBook(ExcelBook book, string reportTemplate, ParserBase.SetProgressValueHandler handler = null)
        {
            CurrentPageNum = 0;
            foreach (string key in book.Pages.Keys)
            {
                ExcelParserCreaterBase creater = MultyParserApp.SelectExcelParserCreater(book.Pages[key], System.Windows.Forms.Application.StartupPath + "\\Modules");
                if (creater != null)
                {
                    ExcelParserBase parser = creater.GetParserObjectForProducts() as ExcelParserBase;
                    CurrentParsingTitle = "Книга: " + book.Name;
                    TotalPages = book.Pages.Count;
                    CurrentPageNum++;
                    TotalRows = book.Pages[key].TotalRows;
                    string templateFile = (reportTemplate.Length > 0) ? reportTemplate : null;
                    parser.ResultBookCreater.Init(book.Pages[key].Name, templateFile, book.Name);

                    if (handler != null)
                        parser.SetProgressValue += handler;

                    parser.DoParsingOfIncomingPage(book.Pages[key], WorkerArgs);

                    if (WorkerArgs.Cancel)
                        return false;
                }
            }
            return true;
        }*/

        public static HtmlParserCreaterBase SelectHtmlParserCreater(string link, string modulesPath = null)
        {
            return SelectParserObjectFromAvailableModules(link, null, modulesPath) as HtmlParserCreaterBase;
        }

        public static ExcelParserCreaterBase SelectExcelParserCreater(ExcelPage page, string modulesPath = null)
        {
            return SelectParserObjectFromAvailableModules(null, page, modulesPath) as ExcelParserCreaterBase;
        }

        private static IParserCreater SelectParserObjectFromAvailableModules(string link = null, ExcelPage page = null, string modulesPath = null)
        {
            if (modulesPath == null)
                modulesPath = System.Windows.Forms.Application.StartupPath + "\\Modules";

            string[] moduleNames = Directory.GetFiles(@modulesPath, "*.dll");

            Type baseCreatorType = null;
            if (link != null)
                baseCreatorType = typeof(HtmlParserCreaterBase);
            else if (page != null)
                baseCreatorType = typeof(ExcelParserCreaterBase);

            foreach (string module in moduleNames)
            {
                Assembly assembly = Assembly.LoadFile(module);
                Type[] typesOfAssembly = assembly.GetTypes();
                foreach (Type type in typesOfAssembly)
                    if (type.IsSubclassOf(baseCreatorType) & !type.IsAbstract
                        & type.GetCustomAttributes(typeof(SecondaryPriceAttribute), false).Length == 0)
                    {
                        if (link != null)
                        {
                            HtmlParserCreaterBase creater = Activator.CreateInstance(type) as HtmlParserCreaterBase;
                            if (creater.DetectIncomingLinkForParserClass(link))
                                return creater;
                        }
                        else if (page != null)
                        {
                            ExcelParserCreaterBase creater = Activator.CreateInstance(type) as ExcelParserCreaterBase;
                            if (creater.DetectIncomingPageForParserClass(page))
                                return creater;
                        }
                    }
            }
            return null;
        }
    }
}
