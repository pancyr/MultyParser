using MultyParser.Core;
using MultyParser.Core.Html;


namespace CompressorShop.Abac
{
    [SiteUrl("abac-air.com")]
    public class AbacOpencartHtmlCreater : HtmlParserCreaterBase
    {
        public override ParserBase GetParserObjectForProducts()
        {
            return new AbacOpencartHtmlParser();
        }
    }
}