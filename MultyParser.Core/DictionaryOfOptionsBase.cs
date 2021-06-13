﻿using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using MultyParser.Core.Html;

namespace MultyParser.Core
{
    public abstract class DictionaryOfOptionsBase
    {
        private Dictionary<HtmlOption, List<string>> _members;
        public Dictionary<HtmlOption, List<string>> Members
        {
            get
            {
                if (_members == null)
                    _members = new Dictionary<HtmlOption, List<string>>();
                return _members;
            }
        }

        public void ReadOptionValuesFromDocument(IHtmlDocument document)
        {
            foreach (HtmlOption option in Members.Keys)
            {
                bool commonStringFound = false;
                if (option.CommonString && option.SingleSelector.Length > 0)
                {
                    IEnumerable<IElement> tags = document.QuerySelectorAll(option.SingleSelector)
                        .Where(p => p.TextContent.StartsWith(option.Name));
                    if (tags.Count() > 0)
                    {
                        commonStringFound = true;
                        List<string> values = option.ParseValuesFromString(tags.First().InnerHtml);
                        foreach (string val in values)
                        {
                            if (!Members[option].Contains(val))
                                Members[option].Add(val);
                        }
                    }
                }
                if (option.ListSelector.Length > 0 && (!option.CommonString || !commonStringFound))
                {
                    var listItems = document.QuerySelectorAll(option.ListSelector);
                    foreach (var item in listItems)
                    {
                        string val = item.TextContent.Trim();
                        if (option.TestRegular(val) && !Members[option].Contains(val))
                            Members[option].Add(val);
                    }
                }
            }
        }
    }
}