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
    public abstract class OpencartPropertyHtmlParserBase : PropertyHtmlParserBase
    {
        protected override List<Dictionary<int, object>> GatherOptionFromDictionary(Dictionary<HtmlOption, List<string>> options, out string pageName)
        {
            List<Dictionary<int, object>> result = new List<Dictionary<int, object>>();
            foreach (HtmlOption opt in options.Keys)
                result.Add(OptionReportBuilder.MakeLineForOption(opt));
            pageName = GetNameOfPageProperties();
            return result;
        }

        protected override List<Dictionary<int, object>> GatherOptionValueFromDictionary(Dictionary<HtmlOption, List<string>> options, out string pageName)
        {
            List<Dictionary<int, object>> result = new List<Dictionary<int, object>>();
            foreach (HtmlOption opt in options.Keys)
            {
                options[opt].Sort();
                OptionReportBuilder.EntityPos = 1;
                foreach (string str in options[opt])
                {
                    
                    result.Add(OptionReportBuilder.MakeLineForOptionValue(opt, str));
                    OptionReportBuilder.EntityPos++;
                }
            }
            pageName = GetNameOfPageValues();
            return result;
        }
    }
}
