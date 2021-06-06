using System.Collections.Generic;
using MultyParser.Core;
using MultyParser.Core.Html;
using MultyParser.Opencart;

namespace Velveto.Conte
{
    public class ConteProductOpencartHtmlParser : OpencartProductHtmlParserBase
    {
        public override string GetSiteName() => "contesale.ru";

        protected override string GetBaseUrl() => "https://contesale.ru/";
        protected override string GetSelectorForList() => "a.product-title";
        protected override string GetSelectorForNextButton() => "a.ty-pagination__next";
        protected override string GetSelectorForTovarName() => "h1.ut2-pb__title";
        protected override string GetSelectorForTovarDescription() => ".content-description>div";
        protected override string GetSelectorForPrice() => ".ty-price-num";
        protected override string GetSelectorForListOfImages() => "a.ty-previewer";
        protected override string GetSelectorForTableOfSpecifications() => "span.ty-control-group";
        protected override string GetTableCellName() => null;
        protected override string GetTableCellValue() => null;

        //protected override string GetBrandName() => "компрессор";

        public override int GetVolumeSize() => 80;

        protected override string GetCodeOfTovarGroup() => "TIGHTS";

        protected override Dictionary<string, int> GetSpecifications()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            /*result.Add("Производительность, л/мин", 10);
            result.Add("Давление, бар", 20);
            result.Add("Мощность, кВт", 30);
            result.Add("Питание", 40);
            result.Add("Тип привода", 50);
            result.Add("Уровень шума, дБ(А)", 60);
            result.Add("Вид компрессора", 70);*/
            return result;
        }

        protected override Dictionary<string, int> GetForTransferToMainPage()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            /*result.Add("Вес", 22);
            result.Add("Длина", 24);
            result.Add("Ширина", 25);
            result.Add("Высота", 26);*/
            return result;
        }
    }
}