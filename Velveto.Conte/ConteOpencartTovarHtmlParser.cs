using System.Collections.Generic;
using System.Text.RegularExpressions;
using MultyParser.Core;
using MultyParser.Core.Html;
using MultyParser.Opencart;

namespace Velveto.Conte
{
    public class ConteOpencartTovarHtmlParser : OpencartTovarHtmlParserBase
    {
        public ConteOpencartTovarHtmlParser()
        {
            //Options = new VelvetoDictionaryOfOptions();
        }

        public override string GetSiteName() => "contesale.ru";

        protected override string GetBaseUrl() => "https://contesale.ru/";
        protected override string GetSelectorForList() => "a.product-title";
        protected override string GetSelectorForNextButton() => "a.ty-pagination__next";
        protected override string GetSelectorForTovarName() => "h1.ut2-pb__title";
        //protected override string GetSelectorForTovarDescription() => ".content-description>div";
        protected override string GetSelectorForPrice() => ".ty-price-num";
        protected override string GetSelectorForListOfImages() => "a.ty-previewer";
        protected override string GetSelectorForTableOfSpecifications() => "span.ty-control-group";
        protected override string GetTableCellName() => null;
        protected override string GetTableCellValue() => null;

        protected override string GetSelectorForAppendInfo() => ".content-description>div>*";

        //protected override string GetBrandName() => "компрессор";

        public override int GetVolumeSize() => 80;

        protected override string GetCodeOfTovarGroup() => "TIGHTS";

        private Dictionary<string, string> _departmentCategories;
        public override Dictionary<string, string> DepartmentCategories
        {
            get
            {
                if (_departmentCategories == null)
                    _departmentCategories = new Dictionary<string, string>
                    {
                        {"klassicheskie", "10,100,110"},
                        {"korrektiruyuschie", "10,100,120"},
                        {"fantaziynye", "10,100,130"},
                        {"azhurnye", "10,100,140"},
                        {"teplye", "10,100,150"},
                        {"svadebnye", "10,100,160"},
                        {"dlya-beremennyh", "10,100,170"}
                    };
                return _departmentCategories;
            }
        }

        protected override DictionaryOfProperty GetOptionsOfTovar() => ContePropertyManager.GetListForTovarOptions();
        protected override DictionaryOfProperty GetFiltersOfTovar() => ContePropertyManager.GetListForTovarFilters();

        protected override Dictionary<string, int> GetSpecifications()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("Плотность", 10);
            result.Add("Состав", 20);
            return result;
        }

        protected override Dictionary<string, int> GetForTransferToMainPage()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("Бренд", 13);
            return result;
        }
        protected override void ProcessOfAppendInfo(Tovar tovarObject, List<string> properties)
        {
            string description = "";
            Regex reg = new Regex(@"^\S*:");
            foreach (string prop in properties)
            {
                if (reg.IsMatch(prop))
                {
                    if (prop.StartsWith("Состав")) tovarObject.Specifications.Add("Состав", ExtractValue(prop));
                }
                else
                {
                    if (description.Length > 0)
                        description += " ";
                    description += prop.Trim();
                }
            }
            tovarObject.Description = description;
        }

        private string ExtractValue(string input)
        {
            Regex optionReg = new Regex(@":(.+)[\s\.,$]", RegexOptions.IgnoreCase);
            return optionReg.Match(input).Groups[1].ToString().Trim();
        }
    }
}