using System.Collections.Generic;
using MultyParser.Core;
using MultyParser.Core.Html;
using MultyParser.Opencart;

namespace CompressorShop.Abac
{
    public class AbacOpencartHtmlParser : OpencartHtmlParserBase
    {
        public override string GetSiteName() => "www.abac-air.com";

        protected override string GetBaseUrl() => "http://www.abac-air.com";
        protected override string GetSelectorForList() => "td.details>a";
        protected override string GetSelectorForNextButton() => "li.bx-pag-next>a";
        protected override string GetSelectorForTovarName() => "div>h1";
        protected override string GetSelectorForTovarDescription() => "div.emarket-detail-area-container>p";
        protected override string GetSelectorForPrice() => ".item_current_price";
        protected override string GetSelectorForListOfImages() => "div#emarket_big_photo>a";
        protected override string GetSelectorForTableOfSpecifications() =>
            ".emarket-detail-area-container>table.emarket-props-table>tbody>tr";
        protected override string GetTableCellName() => "td.emarket-props-name";
        protected override string GetTableCellValue() => "td.emarket-props-data";

        protected override string GetBrandName() => "компрессор";

        protected override string GetCodeOfTovarGroup() => "COMPRESS";

        protected override Dictionary<string, int> GetSpecifications()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("Производительность, л/мин", 10);
            result.Add("Давление, бар", 20);
            result.Add("Мощность, кВт", 30);
            result.Add("Питание", 40);
            result.Add("Тип привода", 50);
            result.Add("Уровень шума, дБ(А)", 60);
            result.Add("Вид компрессора", 70);
            return result;
        }

        protected override Dictionary<string, int> GetForTransferToMainPage()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("Вес", 22);
            result.Add("Длина", 24);
            result.Add("Ширина", 25);
            result.Add("Высота", 26);
            return result;
        }
    }
}
