using System.Collections.Generic;
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
                if (option.CommonString)
                {
                    IElement element = document.QuerySelectorAll(option.HtmlSelector)
                        .Where(p => p.TextContent.StartsWith(option.Name)).First();
                    if (element != null)
                    {
                        List<string> values = option.ParseValuesFromString(element.InnerHtml);
                        foreach (string val in values)
                        {
                            if (!Members[option].Contains(val))
                                Members[option].Add(val);
                        }
                    }
                }
                else
                {
                    var listItems = document.QuerySelectorAll(option.HtmlSelector);
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
