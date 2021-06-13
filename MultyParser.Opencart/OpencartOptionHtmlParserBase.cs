using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultyParser.Core;
using MultyParser.Core.Html;

namespace MultyParser.Opencart
{
    [TemplateSet("OPENCART")]
    public abstract class OpencartOptionHtmlParserBase : OptionHtmlParserBase
    {

        public OpencartOptionHtmlParserBase()
        {
            this._optionBookCreater = new OpencartOptionBookCreater();
        }

        public override string GetDefaultTemplate() => this.Templates["OPTI"];

        protected override List<Dictionary<int, object>> GatherOptionFromDictionary(Dictionary<HtmlOption, List<string>> options, out string pageName)
        {
            List<Dictionary<int, object>> result = new List<Dictionary<int, object>>();
            foreach (HtmlOption opt in options.Keys)
            {
                Dictionary<int, object> pairs = new Dictionary<int, object>();
                pairs.Add(1, opt.ID.ToString());
                pairs.Add(2, opt.Type);
                pairs.Add(3, opt.SortOrder.ToString());
                pairs.Add(4, opt.Name);
                pairs.Add(5, opt.Name);
                result.Add(pairs);
            }
            pageName = OpencartOptionBookCreater.OPTIONS_PAGE_NAME;
            return result;
        }

        protected override List<Dictionary<int, object>> GatherOptionValueFromDictionary(Dictionary<HtmlOption, List<string>> options, out string pageName)
        {
            List<Dictionary<int, object>> result = new List<Dictionary<int, object>>();
            foreach (HtmlOption opt in options.Keys)
            {
                options[opt].Sort();
                OptionBookCreater.EntityPos = 1;
                foreach (string str in options[opt])
                {
                    Dictionary<int, object> pairs = new Dictionary<int, object>();
                    pairs.Add(1, (opt.ID + OptionBookCreater.EntityPos).ToString());
                    pairs.Add(2, opt.ID.ToString());
                    pairs.Add(4, OptionBookCreater.EntityPos.ToString());
                    pairs.Add(5, str);
                    pairs.Add(6, str);
                    result.Add(pairs);
                    OptionBookCreater.EntityPos++;
                }
            }
            pageName = OpencartOptionBookCreater.OPTION_VALUES_PAGE_NAME;
            return result;
        }
    }
}
