using MultyParser.Core;
using MultyParser.Core.Html;


namespace CompressorShop.Abac
{
    [SiteUrl("abac-air.com")]
    public class AbacOpencartHtmlCreater : HtmlParserCreaterBase
    {
        public override HtmlParserBase GetHtmlParserObject()
        {
            return new AbacOpencartHtmlParser();
        }
    }
}