using MultyParser.Core;
using MultyParser.Core.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Bitrix
{
    public class BitrixResultBookCreater : ResultBookCreaterBase
    {
        public const string PRODUCTS_PAGE_NAME = "Products";
        public const string IMAGES_PAGE_NAME = "AdditionalImages";
        public const string OPTIONS_PAGE_NAME = "ProductOptions";
        public const string OPTION_VALUES_PAGE_NAME = "ProductOptionValues";
        public const string ATTRIBUTES_PAGE_NAME = "ProductAttributes";
        public const string SEO_PAGE_NAME = "ProductSEOKeywords";

        protected override ExcelBook CreateBookForResultData(string filePath = null)
        {
            Dictionary<string, Dictionary<int, string>> titles =
                new Dictionary<string, Dictionary<int, string>>();

            titles.Add(PRODUCTS_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "product_id",
                [2] = "name(en-gb)",
                [3] = "name(ru-ru)",
                [4] = "categories"
            });
            titles.Add(IMAGES_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "product_id",
                [2] = "image",
                [3] = "sort_order"
            });
            titles.Add(OPTIONS_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "product_id",
                [2] = "option",
                [3] = "default_option_value",
                [4] = "required"
            });
            titles.Add(OPTION_VALUES_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "product_id",
                [2] = "option",
                [3] = "option_value",
                [4] = "quantity",
                [5] = "subtract",
                [6] = "price",
                [7] = "price_prefix",
                [8] = "points",
                [9] = "points_prefix",
                [10] = "weight",
                [11] = "weight_prefix"
            });
            titles.Add(ATTRIBUTES_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "product_id",
                [2] = "attribute_group",
                [3] = "attribute",
                [4] = "text (en-gb)",
                [5] = "text (ru-ru)"
            });
            titles.Add(SEO_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "product_id",
                [2] = "store_id",
                [3] = "keyword(en-gb)",
                [4] = "keyword(ru-ru)"
            });
            ExcelBook result = CreateBookForResultData(titles, filePath);

            result.Pages[PRODUCTS_PAGE_NAME].SetRowHeight(1, 30);
            result.Pages[PRODUCTS_PAGE_NAME].CentrateRow(1);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(19);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(20);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(21);
            return result;
        }
    }
}
