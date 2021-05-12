using MultyParser.Core;
using MultyParser.Core.Html;

namespace CompressorShop.Berg
{
    [SiteUrl("berg-air.ru")]
    public class AbacOpencartHtmlCreater : HtmlParserCreaterBase
    {
        public override HtmlParserBase GetHtmlParserObject()
        {
            return new BergOpencartHtmlParser();
        }
    }
}
