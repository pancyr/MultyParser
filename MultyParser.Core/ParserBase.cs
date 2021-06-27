using System;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;

using MultyParser.Configuration;
using MultyParser.Core.ExcelReportBuilder;

namespace MultyParser.Core
{
    public abstract class ParserBase
    {
        public const int PARSER_FOR_PRODUCTS = 1;
        public const int PARSER_FOR_OPTIONS = 2;
        public const int PARSER_FOR_FILTERS = 3;

        public ParserBase() { }

        public string DepartmentName { get; set; } // подраздел

        public int EntityID; // идентификатор сущности
        public string CurrentParsingTitle { get; set; }
        public int CurrentPageNum { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }

        protected Dictionary<string, string> _templates;
        public Dictionary<string, string> Templates
        {
            get
            {
                if (_templates == null)
                {
                    _templates = new Dictionary<string, string>();
                    Object[] attrs = this.GetType().GetCustomAttributes(typeof(TemplateSetAttribute), true);
                    if (attrs != null)
                    {
                        TemplateSetAttribute setOfClass = attrs[0] as TemplateSetAttribute;
                        foreach (TemplateSetConfigElement set in MultyParserConfigSection.Settings.TemplateSets)
                            if (set.System == setOfClass.Name)
                            {
                                string subDir = set.Dir;
                                if (!subDir.EndsWith("\\"))
                                    subDir += "\\";
                                foreach (TemplateConfigElement template in set.TemplateList)
                                    _templates.Add(template.Code, Path.GetFullPath(subDir + template.File));
                            }
                    }
                }
                return _templates;
            }
        }

        protected virtual string MakeMetaKeywords(string tovarName)
        {
            var keywordSet = MultyParserConfigSection.Settings.KeywordPatterns;
            string result = "";
            foreach (KeywordPatternConfigElement keyword in keywordSet)
            {
                if (result.Length > 0)
                    result += ", ";
                result += String.Format(keyword.Value, tovarName);
            }
            return result;
        }

        protected string GetPrice(string input)
        {
            string result = Regex.Replace(input, @"[^\d,.]", "").Replace(",", ".");
            Regex regPrice = new Regex(@"\d+(.\d+)?");
            Match m = regPrice.Match(result);
            return (m.Success) ? m.ToString() : "";
        }
        
        protected abstract string GetCodeOfTovarGroup();
        protected virtual string GetBrandName() => null;        // после названия бренда может быть написана модель
        protected virtual string GetMainOptionName() => null;   // для выделения опции из названия

        public abstract ExcelReportBuilderBase GetReportBuilderInstance();  // возвращает экземпляр построителя выходных документов
        public abstract string GetDefaultTemplate();                        // шаблон по умолчанию для импорта товаров
        public virtual int GetVolumeSize() => 0;            // если нужно изменить размер тома

        protected virtual List<int> GetListIntFromString(string input)
        {
            Regex optionReg = new Regex(String.Format(@"\d+"));
            MatchCollection ms = optionReg.Matches(input);
            if (ms != null)
            {
                List<int> result = new List<int>();
                foreach (Match m in ms)
                    result.Add(Int32.Parse(m.ToString()));
                return result;
            }
            return null;
        }

        protected virtual bool DivideByKeyword(string input, string keyword, out string first, out string second)
        {
            Regex optionReg = new Regex(String.Format("{1}{0}{1}(.*)(?:.*)", keyword, "[ ,:-=]*"), RegexOptions.IgnoreCase);
            Match m = optionReg.Match(input);
            if (m.Success)
            {
                first = input.Substring(0, input.Length - m.ToString().Length);
                second = m.Groups[1].ToString();
                return true;
            }
            first = second = null;
            return false;
        }

        #region Реализация события для индикатора обработки

        public delegate void SetProgressValueHandler(int NewValue);
        public event SetProgressValueHandler SetProgressValue;

        protected void OnSetProgressValue(int currentPos)
        {
            if (SetProgressValue != null)
                SetProgressValue(currentPos);
        }

        #endregion

    }
}