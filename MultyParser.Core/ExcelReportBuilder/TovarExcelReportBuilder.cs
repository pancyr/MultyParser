using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Core.ExcelReportBuilder
{
    public abstract class TovarExcelReportBuilder : ExcelReportBuilderBase
    {
        public abstract Dictionary<int, object> MakeLineForProductPage(
            int tovarID, string tovarName, string groups, int quantity, string brand,
            string mainPhoto, string price, string massUnit, string sizeUnit,
            string description, string metaTitle, string metaDescription, string metaKeywords);

        public abstract Dictionary<int, object> MakeLineForAdditionalImage(int tovarID, string imagePath);
        public abstract Dictionary<int, object> MakeLineForOption(int tovarID, string optionName);
        public abstract Dictionary<int, object> MakeLineForOptionValue(int tovarID, string optionName, string optionValue);
        public abstract Dictionary<int, object> MakeLineForAttribute(int tovarID, string groupName, string attributeName, string value);
        public abstract Dictionary<int, object> MakeLineForFilterValue(int tovarID, string groupName, string filterValue);
        public abstract Dictionary<int, object> MakeLineForSeoUrl(int tovarID, string productName);

        private Dictionary<string, int> _seoTitlesStorage;
        public Dictionary<string, int> SeoTitlesStorage
        {
            get
            {
                if (_seoTitlesStorage == null)
                    _seoTitlesStorage = new Dictionary<string, int>();
                return _seoTitlesStorage;
            }
        }

        public string GetUniqueSeoTitle(string title)
        {
            if (SeoTitlesStorage.ContainsKey(title))
            {
                SeoTitlesStorage[title]++;
                return title + "-" + SeoTitlesStorage[title];
            }
            SeoTitlesStorage[title] = 1;
            return title;
        }
    }
}
