﻿using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using MultyParser.Core.Html;

namespace MultyParser.Core
{
    public class DictionaryOfProperty
    {
        private Dictionary<TovarProperty, List<string>> _members;
        public Dictionary<TovarProperty, List<string>> Members
        {
            get
            {
                if (_members == null)
                    _members = new Dictionary<TovarProperty, List<string>>();
                return _members;
            }
        }

        public void Add(TovarProperty key, List<string> value)
        {
            Members.Add(key, value);
        }

        public void ReadPropertyValuesFromDocument(IHtmlDocument document)
        {
            foreach (TovarProperty option in Members.Keys)
            {
                bool commonStringFound = false;
                if (option.CommonString && option.SingleSelector != null && option.SingleSelector.Length > 0)
                {
                    IEnumerable<IElement> tags = document.QuerySelectorAll(option.SingleSelector)
                        .Where(p => p.TextContent.StartsWith(option.Name));
                    if (tags.Count() > 0)
                    {
                        commonStringFound = true;
                        List<string> values = option.ParseValuesFromString(tags.First().TextContent);
                        foreach (string val in values)
                        {
                            if (!Members[option].Contains(val.ToLower()))
                                Members[option].Add(val.ToLower());
                        }
                    }
                }
                if (option.ListSelector != null && option.ListSelector.Length > 0 && (!option.CommonString || !commonStringFound))
                {
                    var listItems = document.QuerySelectorAll(option.ListSelector);
                    foreach (var item in listItems)
                    {
                        string val = item.TextContent.Trim().ToLower();
                        if (option.TestRegular(val) && !Members[option].Contains(val))
                            Members[option].Add(val);
                    }
                }
            }
        }
    }
}