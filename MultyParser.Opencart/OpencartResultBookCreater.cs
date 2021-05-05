using MultyParser.Core;
using MultyParser.Core.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Opencart
{
    public class OpencartResultBookCreater : ResultBookCreaterBase
    {
        private const string PRODUCTS_PAGE_NAME = "Products";
        private const string ATTRIBUTES_PAGE_NAME = "ProductAttributes";

        protected override ExcelBook CreateBookForResultData(string filePath = null)
        {
            Dictionary<string, Dictionary<int, string>> titles =
                new Dictionary<string, Dictionary<int, string>>();

            /*titles.Add(CATEGORY_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "category_id",
                [2] = "category_name"
            });*/
            titles.Add(PRODUCTS_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "product_id",
                [2] = "name(en-gb)",
                [3] = "name(ru-ru)",
                [4] = "categories",
                //[4] = "main_categories",
                [5] = "sku",
                [6] = "upc",
                [7] = "ean",
                [8] = "jan",
                [9] = "isbn",
                [10] = "mpn",
                [11] = "location",
                [12] = "quantity",
                [13] = "model",
                [14] = "manufacturer",
                [15] = "image_name",
                [16] = "requires shipping",
                [17] = "price",
                [18] = "points",
                [19] = "date_added",
                [20] = "date_modified",
                [21] = "date_available",
                [22] = "weight",
                [23] = "unit",
                [24] = "length",
                [25] = "width",
                [26] = "height",
                [27] = "length_unit",
                [28] = "status",
                [29] = "tax_class_id",
                //[30] = "viewed",
                //[31] = "language_id",
                //[32] = "seo_keyword",
                [30] = "description (en-gb)",
                [31] = "description (ru-ru)",
                [32] = "meta_title (en-gb)",
                [33] = "meta_title (ru-ru)",
                [34] = "meta_description (en-gb)",
                [35] = "meta_description (ru-ru)",
                [36] = "meta_keywords (en-gb)",
                [37] = "meta_keywords (ru-ru)",
                [38] = "stock_status_id",
                [39] = "store_ids",
                [40] = "layout",
                [41] = "related_ids",
                [42] = "tags (en-gb)",
                [43] = "tags (ru-ru)",
                [44] = "sort_order",
                [45] = "subtract",
                [46] = "minimum"
            });
            titles.Add(ATTRIBUTES_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "product_id",
                [2] = "attribute_group",
                [3] = "attribute",
                [4] = "text (en-gb)",
                [5] = "text (ru-ru)"
            });
            ExcelBook result = CreateBookForResultData(titles, filePath);

            result.Pages[PRODUCTS_PAGE_NAME].SetRowHeight(1, 30);
            result.Pages[PRODUCTS_PAGE_NAME].CentrateRow(1);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(19);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(20);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(21);

            /*result.Pages[ATTRIBUTES_PAGE_NAME].SetRowHeight(1, 30);
            result.Pages[ATTRIBUTES_PAGE_NAME].CentrateRow(1);
            result.Pages[ATTRIBUTES_PAGE_NAME].MakeColumnStringFormat(4);
            result.Pages[ATTRIBUTES_PAGE_NAME].MakeColumnStringFormat(5);*/
            return result;
        }
    }
}
