using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultyParser.Core;
using MultyParser.Core.Html;

namespace MultyParser.Opencart
{
    [TemplateSet("OPENCART")]
    public abstract class OpencartTovarHtmlParserBase : TovarHtmlParserBase
    {

        public OpencartTovarHtmlParserBase()
        {
            this._productBookCreater = new OpencartTovarExcelReportBuilder();
        }

        public override string GetDefaultTemplate() => this.Templates["PROD"];

        protected override string GetSizeUnit() => "мм";
        protected override string GetMassUnit() => "кг";

        protected override Dictionary<int, object> GatherCommonDataFromTovarObject(int tovarID, Tovar tovarObject, out string pageName)
        {
            
            pageName = OpencartTovarExcelReportBuilder.PRODUCTS_PAGE_NAME;

            if (tovarObject.Name.Length > 0)
            {
                string photiUrl = null;
                if (tovarObject.Photos.Count > 0)
                {
                    photiUrl = tovarObject.Photos[0];
                    tovarObject.Photos.RemoveAt(0);
                }
                string meta_Description = tovarObject.Name + " — лучшая цена, доставка по всей России!";
                return ProductBookCreater.MakeLineForProductPage(
                    tovarID, tovarObject.Name, DepartmentCategories[DepartmentName], 1000, "Conte", 
                    photiUrl, tovarObject.Price, GetMassUnit(), GetSizeUnit(), tovarObject.Description,
                    tovarObject.Name, meta_Description, MakeMetaKeywords(tovarObject.Name));
            }
            return null;
        }

        protected override List<Dictionary<int, object>> GatherAdditionalImagesFromTovarObject(int tovarID, Tovar tovarObject, out string pageName)
        {
            List<Dictionary<int, object>> result = new List<Dictionary<int, object>>();
            foreach (string photo in tovarObject.Photos)
                result.Add(ProductBookCreater.MakeLineForAdditionalImage(tovarID, photo));
            pageName = OpencartTovarExcelReportBuilder.IMAGES_PAGE_NAME;
            return result;
        }

        protected override List<Dictionary<int, object>> GatherAttributesFromTovarObject(
            int tovarID, Tovar tovarObject, TovarGroup tovarGroup, Dictionary<int, object> dataCommon,
            Dictionary<string, int> parserSpecifications, Dictionary<string, int> forTransfer, out string pageName)
        {
            List<Dictionary<int, object>> result = new List<Dictionary<int, object>>();

            foreach (KeyValuePair<string, string> spec in tovarObject.Specifications)
            {
                // Пытаемся найти соответствие спецификации со страницы сайта
                // одной из тех, что подлежит занесению в файл импорта
                if (parserSpecifications.ContainsKey(spec.Key))
                {
                    // Если такое соответствие найдено,
                    // Делаем запись на листе атрибутов

                    int attributeCode = parserSpecifications[spec.Key];
                    string attributeName = tovarGroup.Specifications[attributeCode];
                    result.Add(ProductBookCreater.MakeLineForAttribute(
                        tovarID, tovarGroup.DisplayName, attributeName, spec.Value));
                }
                else
                {
                    // Если не найдено, то надо ещё проверить
                    // может это должно быть перенесено в главную таблицу

                    foreach (string prop in forTransfer.Keys)
                    {
                        if (spec.Key.StartsWith(prop))
                        {
                            dataCommon.Add(forTransfer[prop], tovarObject.Specifications[spec.Key]);
                            break;
                        }
                    }
                }
            }

            pageName = OpencartTovarExcelReportBuilder.ATTRIBUTES_PAGE_NAME;
            return result;
        }

        protected override List<Dictionary<int, object>> GatherOptionFromTovarObject(int tovarID, string optionName, out string pageName)
        {
            pageName = OpencartTovarExcelReportBuilder.OPTIONS_PAGE_NAME;
            return new List<Dictionary<int, object>> { ProductBookCreater.MakeLineForOption(tovarID, optionName) };
        }

        protected override List<Dictionary<int, object>> GatherOptionValueFromTovarObject(int tovarID, string optionName, string optionValue, out string pageName)
        {
            pageName = OpencartTovarExcelReportBuilder.OPTION_VALUES_PAGE_NAME;
            return new List<Dictionary<int, object>> { ProductBookCreater.MakeLineForOptionValue(tovarID, optionName, optionValue) };
        }

        protected override Dictionary<int, object> GatherSeoKeywordsFromTovarObject(int tovarID, Tovar tovarObject, out string pageName)
        {
            pageName = OpencartTovarExcelReportBuilder.SEO_PAGE_NAME;
            return ProductBookCreater.MakeLineForSeoUrl(tovarID, tovarObject.Name);
        }
    }
}