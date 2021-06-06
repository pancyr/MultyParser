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
            this._resultBookCreater = new OpencartOptionBookCreater();
        }

        public override string GetDefaultTemplate() => this.Templates["OPTI"];

        protected override List<Dictionary<int, string>> GatherOptionFromDictionary(Dictionary<HtmlOption, List<string>> options, out string pageName)
        {
            List<Dictionary<int, string>> result = new List<Dictionary<int, string>>();
            foreach (HtmlOption opt in options.Keys)
            {
                Dictionary<int, string> pairs = new Dictionary<int, string>();
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

        protected override List<Dictionary<int, string>> GatherOptionValueFromDictionary(Dictionary<HtmlOption, List<string>> options, out string pageName)
        {
            List<Dictionary<int, string>> result = new List<Dictionary<int, string>>();
            foreach (HtmlOption opt in options.Keys)
            {
                options[opt].Sort();
                ResultBookCreater.EntityPos = 1;
                foreach (string str in options[opt])
                {
                    Dictionary<int, string> pairs = new Dictionary<int, string>();
                    pairs.Add(1, (opt.ID + ResultBookCreater.EntityPos).ToString());
                    pairs.Add(2, opt.ID.ToString());
                    pairs.Add(4, ResultBookCreater.EntityPos.ToString());
                    pairs.Add(5, str);
                    pairs.Add(6, str);
                    result.Add(pairs);
                    ResultBookCreater.EntityPos++;
                }
            }
            pageName = OpencartOptionBookCreater.OPTION_VALUES_PAGE_NAME;
            return result;
        }
    }
}
