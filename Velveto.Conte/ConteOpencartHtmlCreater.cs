using MultyParser.Core;
using MultyParser.Core.Html;

namespace Velveto.Conte
{
    [SiteUrl("contesale.ru")]
    public class ConteOpencartHtmlCreater : HtmlParserCreaterBase
    {
        public override ParserBase GetParserObjectForProducts()
        {
            return new ConteProductOpencartHtmlParser();
        }

        public override ParserBase GetParserObjectForOptions()
        {
            return new ConteOptionOpencartHtmlParser();
        }
    }
}