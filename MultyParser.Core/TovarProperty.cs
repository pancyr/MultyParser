using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace MultyParser.Core
{
    public class TovarProperty
    {
        public TovarProperty() { }

        public TovarProperty(int id, string name, string type, int sortOrder, string regPath, string listSelector, string singleSelector = null, bool listFirst = false, string displayName = null)
        {
            this.ID = id;
            this.Name = name;
            this.Type = type;
            this.SortOrder = sortOrder;
            this.RegPath = regPath;
            this.ListSelector = listSelector;
            this.SingleSelector = singleSelector;
            this.ListFirst = listFirst;
            this.DisplayName = displayName;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int SortOrder { get; set; }
        public string RegPath { get; set; }
        public string ListSelector { get; set; }
        public string SingleSelector { get; set; }
        public bool ListFirst { get; set; }
        public string DisplayName { get; set; }

        public bool TestRegular(string input)
        {
            if (RegPath == null)
                return true;
            Regex reg = new Regex(RegPath);
            return reg.IsMatch(input);
        }

        public List<string> Separate(string common)
        {
            MatchCollection matches = new Regex(RegPath).Matches(common);
            List<string> result = new List<string>();
            foreach (Match m in matches)
                result.Add(m.ToString());
            return result;
        }

        public List<string> FindListOfValuesFirst(IHtmlDocument document)
        {
            List<string> listItems = null;

            if (ListSelector != null && ListSelector.Length > 0)
                listItems = document.QuerySelectorAll(ListSelector).Select(s => s.TextContent).ToList();

            if (listItems.Count > 0)
                return listItems;
            else if (SingleSelector != null && SingleSelector.Length > 0)
            {
                IEnumerable<string> values = document.QuerySelectorAll(SingleSelector)
                    .Where(p => p.TextContent.StartsWith(Name))
                    .Select(s => s.TextContent);
                if (values.Count() > 0)
                    listItems = Separate(values.First());
            }

            return listItems;
        }

        public List<string> FindSingleValueFirst(IHtmlDocument document)
        {
            List<string> listItems = null;
            string singleValue = null;
            if (SingleSelector != null && SingleSelector.Length > 0)
            {
                IEnumerable<string> values = document.QuerySelectorAll(SingleSelector)
                    .Where(p => p.TextContent.StartsWith(Name))
                    .Select(s => s.TextContent);
                if (values.Count() > 0)
                    singleValue = values.First();
            }

            if (singleValue != null)
                listItems = Separate(singleValue);
            else if (ListSelector != null && ListSelector.Length > 0)
                listItems = document.QuerySelectorAll(ListSelector).Select(s => s.TextContent).ToList();

            return listItems;
        }
    }
}
