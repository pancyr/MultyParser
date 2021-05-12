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
        public static string CurrentParsingTitle { get; set; }
        public static int CurrentPageNum { get; set; }
        public static int TotalPages { get; set; }
        public static int TotalRows { get; set; }

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

        public static DoWorkEventArgs WorkerArgs { get; set; }

        public static void InitConfiguration(MultyParserConfigSection section)
        {
            TovarGroups = new Dictionary<string, TovarGroup>();
            foreach (TovarGroupConfigElement group in section.TovarGroups)
                TovarGroups.Add(group.Code, new TovarGroup(group));
        }

        public static bool DoParsingOfWebSite(string siteUrl, string reportTemplate, ParserBase.SetProgressValueHandler handler = null)
        {
            CurrentPageNum = 0;
            HtmlParserBase parser = MultyParserApp.SelectHtmlParser(siteUrl, System.Windows.Forms.Application.StartupPath + "\\Modules");
            if (parser != null)
            {
                CurrentParsingTitle = "Сайт: " + parser.GetSiteName();
                TotalPages = parser.CountPages(siteUrl);
                string templateFile = (reportTemplate.Length > 0) ? reportTemplate : null;
                string dep = parser.GetDepartmentByUrl(siteUrl);
                parser.ResultBookCreater.Init(dep, templateFile, parser.GetSiteName());

                int volumeSize = parser.GetVolumeSize();
                if (volumeSize != 0)
                    parser.ResultBookCreater.VolumeSize = volumeSize;

                if (handler != null)
                    parser.SetProgressValue += handler;

                // Достаём из конфига текущее значение идентификатора товара
                System.Configuration.Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                int tovarIdent = Int32.Parse(currentConfig.AppSettings.Settings["identValue"].Value);

                parser.DoParsingOfIncomingHtml(ref tovarIdent, siteUrl, WorkerArgs); // запускаем парсинг веб-страницы

                // Сохраняем новое значение идентификатора
                currentConfig.AppSettings.Settings["identValue"].Value = tovarIdent.ToString();
                currentConfig.Save();
                ConfigurationManager.RefreshSection("appSettings");

                if (WorkerArgs.Cancel)
                    return false;
            }
            return true;
        }

        public static bool DoParsingOfExcelBook(ExcelBook book, string reportTemplate, ParserBase.SetProgressValueHandler handler = null)
        {
            CurrentPageNum = 0;
            foreach (string key in book.Pages.Keys)
            {
                ExcelParserBase parser = MultyParserApp.SelectExcelParser(book.Pages[key], System.Windows.Forms.Application.StartupPath + "\\Modules");
                if (parser != null)
                {
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
        }

        public static HtmlParserBase SelectHtmlParser(string link, string modulesPath = null)
        {
            return SelectParserObjectFromAvailableModules(link, null, modulesPath) as HtmlParserBase;
        }

        public static ExcelParserBase SelectExcelParser(ExcelPage page, string modulesPath = null)
        {
            return SelectParserObjectFromAvailableModules(null, page, modulesPath) as ExcelParserBase;
        }

        private static ParserBase SelectParserObjectFromAvailableModules(string link = null, ExcelPage page = null, string modulesPath = null)
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
                            HtmlParserCreaterBase creator = Activator.CreateInstance(type) as HtmlParserCreaterBase;
                            if (creator.DetectIncomingLinkForParserClass(link))
                                return creator.GetHtmlParserObject();
                        }
                        else if (page != null)
                        {
                            ExcelParserCreaterBase creator = Activator.CreateInstance(type) as ExcelParserCreaterBase;
                            if (creator.DetectIncomingPageForParserClass(page))
                                return creator.GetExcelParserObject();
                        }
                    }
            }
            return null;
        }
    }
}
