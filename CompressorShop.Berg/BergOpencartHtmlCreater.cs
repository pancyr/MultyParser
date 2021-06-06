using MultyParser.Core;
using MultyParser.Core.Html;

namespace CompressorShop.Berg
{
    [SiteUrl("berg-air.ru")]
    public class BergOpencartHtmlCreater : HtmlParserCreaterBase
    {
        public override ParserBase GetParserObjectForProducts()
        {
            return new BergOpencartHtmlParser();
        }
    }
}
