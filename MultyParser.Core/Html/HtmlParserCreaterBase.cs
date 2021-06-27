using System;
using MultyParser.Core;

namespace MultyParser.Core.Html
{
    public abstract class HtmlParserCreaterBase : IParserCreater
    {
        public virtual ParserBase GetParserObjectForProducts() => null;
        public virtual ParserBase GetParserObjectForOptions() => null;
        public virtual ParserBase GetParserObjectForFilters() => null;

        public virtual bool DetectIncomingLinkForParserClass(string link)
        {
            Object[] attributes = this.GetType().GetCustomAttributes(typeof(SiteUrlAttribute), false);
            foreach (SiteUrlAttribute a in attributes)
            {
                if (link.Trim().ToUpper().Contains(a.Url.Trim().ToUpper()))
                    return true;
            }
            return false;
        }

    }
}
