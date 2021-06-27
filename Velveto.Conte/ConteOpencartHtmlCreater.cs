using MultyParser.Core;
using MultyParser.Core.Html;

namespace Velveto.Conte
{
    [SiteUrl("contesale.ru")]
    public class ConteOpencartHtmlCreater : HtmlParserCreaterBase
    {
        public override ParserBase GetParserObjectForProducts()
        {
            return new ConteOpencartTovarHtmlParser();
        }

        public override ParserBase GetParserObjectForOptions()
        {
            return new ConteOpencartOptionHtmlParser();
        }

        public override ParserBase GetParserObjectForFilters()
        {
            return new ConteOpencartFilterHtmlParser();
        }
    }
}