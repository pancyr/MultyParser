using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MultyParser.Core;
using MultyParser.Core.Html;
using MultyParser.Opencart;

//https://berg-air.ru/catalog/vintovye-kompressory/

namespace CompressorShop.Berg
{
    [SiteUrl("berg-air.ru")]
    public class BergOpencartHtmlParser : OpencartTovarHtmlParserBase
    {
        public override string GetSiteName() => "berg-air.ru";

        protected override string GetBaseUrl() => "https://berg-air.ru";
        protected override string GetSelectorForList() => ".catalog-section-item-base>.catalog-section-item-name>a";
        protected override string GetSelectorForNextButton() => ".system-pagenavigation-item-next>a";
        protected override string GetSelectorForTovarName() => "h1.intec-header";
        protected override string GetSelectorForTovarDescription() => "div.catalog-element-section-description";
        protected override string GetSelectorForPrice() => ".catalog-element-price-base";
        protected override string GetSelectorForListOfImages() => "a.catalog-element-gallery-picture";
        protected override string GetSelectorForTableOfSpecifications() => "div.catalog-element-section-property";
        protected override string GetTableCellName() => "div.catalog-element-section-property-name";
        protected override string GetTableCellValue() => "div.catalog-element-section-property-value";

        protected override string GetBrandName() => "Berg";
        protected override string GetMainOptionName() => "Давление";

        public override int GetVolumeSize() => 20;

        protected override string GetCodeOfTovarGroup() => "COMPRESS";

        protected override Dictionary<string, int> GetSpecifications()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("Производительность, м³/мин", 10);
            result.Add("Давление, бар", 20);
            result.Add("Мощность, кВт", 30);
            result.Add("Питание, В", 40);
            result.Add("Тип привода", 50);
            result.Add("Шум, дБ", 60);
            return result;
        }

        protected override Dictionary<string, int> GetForTransferToMainPage()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("Вес, кг", 22);
            return result;
        }

        protected override List<Dictionary<int, object>> GatherAttributesFromTovarObject(
            int tovarID, Tovar tovarObject, TovarGroup tovarGroup, Dictionary<int, object> dataCommon, 
            Dictionary<string, int> parserSpecifications, Dictionary<string, int> forTransfer, out string pageName)
        {
            // получаем список атрибутов из базового класса
            List<Dictionary<int, object>> result = base.GatherAttributesFromTovarObject
                (tovarID, tovarObject, tovarGroup, dataCommon, parserSpecifications, forTransfer, out pageName);
            
            // переводим кубометры в литры
            decimal performance = Decimal.Parse(result[0][5].ToString().Replace(".", ","));
            result[0][5] = (performance * 1000).ToString("N0");

            // извлекаем метрические данные
            string size = tovarObject.Specifications["Габариты, ДхШхВ, мм"];
            List<int> values = this.GetListIntFromString(size);
            if (values != null)
            {
                dataCommon.Add(24, values[0].ToString());
                dataCommon.Add(25, values[1].ToString());
                dataCommon.Add(26, values[2].ToString());
            }

            // вносим недостающий пункт в список атрибутов
            string compressType = null;
            if (tovarObject.Name.ToUpper().IndexOf("ВИНТ") > -1)
                compressType = "Винтовой";
            else if (tovarObject.Name.ToUpper().IndexOf("СПИР") > -1)
                compressType = "Спиральный";
            if (compressType != null)
                result.Add(ProductBookCreater.MakeLineForAttribute(
                    tovarID, tovarGroup.DisplayName, "Вид компрессора", compressType));

            return result;
        }
    }
}