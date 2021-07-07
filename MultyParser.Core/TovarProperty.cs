﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MultyParser.Core
{
    public class TovarProperty
    {
        public TovarProperty() { }

        public TovarProperty(int id, string name, string type, int sortOrder, string regPath, string listSelector, string singleSelector = null, string separators = null, string displayName = null)
        {
            this.ID = id;
            this.Name = name;
            this.Type = type;
            this.SortOrder = sortOrder;
            this.RegPath = regPath;
            this.ListSelector = listSelector;
            this.SingleSelector = singleSelector;
            this.Separators = separators;
            this.DisplayName = displayName;
        }

        //public const string REG_TEMPLATE = @"({0})[\s\.,;-_$]+";

        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int SortOrder { get; set; }
        public string RegPath { get; set; }
        public string ListSelector { get; set; }
        public string SingleSelector { get; set; }
        public string Separators { get; set; }
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
            string regular = String.Format(@"[^{0}]+\b(?!:)", Separators);
            MatchCollection matches = new Regex(regular).Matches(common);
            List<string> result = new List<string>();
            foreach (Match m in matches)
                result.Add(m.ToString());
            return result;
        }
    }
}
