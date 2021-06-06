using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MultyParser.Core.Html
{
    public class HtmlOption
    {
        public HtmlOption() { }

        public HtmlOption(int id, string name, string type, int sortOrder, string htmlSelector, string regPath = null)
        {
            this.ID = id;
            this.Name = name;
            this.Type = type;
            this.SortOrder = sortOrder;
            this.HtmlSelector = htmlSelector;
            this.RegPath = regPath;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int SortOrder { get; set; }
        public string HtmlSelector { get; set; }
        public string RegPath { get; set; }

        public bool TestRegular(string input)
        {
            if (RegPath == null)
                return true;
            Regex reg = new Regex(RegPath);
            return reg.IsMatch(input);
        }
    }
}
