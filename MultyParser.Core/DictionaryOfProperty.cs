using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using MultyParser.Core.Html;

namespace MultyParser.Core
{
    public class DictionaryOfProperty
    {
        public TovarProperty MainProperty { get; set; }

        public List<string> GetMainList() => Members[MainProperty];

        public string GetMainAsSingleString()
        {
            string result = "";
            List<string> values = GetMainList();
            foreach (string val in values)
                result += (result.Length > 0) ? ", " + val : val;
            return result;
        }

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
                List<string> listItems = option.ListFirst ?
                    option.FindListOfValuesFirst(document) : option.FindSingleValueFirst(document);

                if (listItems != null)
                {
                    foreach (var item in listItems)
                    {
                        string val = item.Trim().ToLower();
                        if (option.TestRegular(val) && !Members[option].Contains(val))
                            Members[option].Add(val);
                    }
                }
            }
        }
    }
}
