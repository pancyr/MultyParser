using System;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.Generic;

namespace MultyParser.Core
{
    public abstract class ParserBase
    {
        public ParserBase()
        {
            this.SeoTitlesStorage = new Dictionary<string, int>();
        }

        public DoWorkEventArgs WorkerArgs { get; set; }

        protected ResultBookCreaterBase _resultBookCreater;
        public ResultBookCreaterBase ResultBookCreater
        {
            get
            {
                return _resultBookCreater;
            }
        }

        protected Dictionary<string, int> SeoTitlesStorage { get; set; }

        protected string GetUniqueSeoTitle(string title)
        {
            if (SeoTitlesStorage.ContainsKey(title))
            {
                SeoTitlesStorage[title]++;
                return title + "-" + SeoTitlesStorage[title];
            }
            SeoTitlesStorage[title] = 1;
            return title;
        }

        protected string GetPrice(string input)
        {
            string result = Regex.Replace(input, @"[^\d,.]", "").Replace(".", ",");
            Regex regPrice = new Regex(@"\d+(,\d+)?");
            Match m = regPrice.Match(result);
            return (m.Success) ? m.ToString() : "";
        }


        protected abstract Dictionary<int, string> MakeDictionaryForAttributesPageRow(int tovarID, string groupName, string attributeName, string value);

        protected abstract string GetCodeOfTovarGroup();
        protected abstract Dictionary<string, int> GetSpecifications();

        protected virtual string GetBrandName() => null;    // после названия бренда может быть написана модель
        protected virtual string GetMainOption() => null;   // для выделения опции из названия

        public virtual int GetVolumeSize() => 0;         // если нужно изменить размер тома

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