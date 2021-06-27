using MultyParser.Core;
using MultyParser.Core.Excel;
using MultyParser.Core.ExcelReportBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Opencart
{
    public class OpencartTovarExcelReportBuilder : TovarExcelReportBuilder
    {
        public const string PRODUCTS_PAGE_NAME = "Products";
        public const string IMAGES_PAGE_NAME = "AdditionalImages";
        public const string OPTIONS_PAGE_NAME = "ProductOptions";
        public const string OPTION_VALUES_PAGE_NAME = "ProductOptionValues";
        public const string ATTRIBUTES_PAGE_NAME = "ProductAttributes";
        public const string SEO_PAGE_NAME = "ProductSEOKeywords";

        protected override ExcelBook CreateBookForResultData(string filePath = null)
        {
            Dictionary<string, Dictionary<int, object>> titles =
                new Dictionary<string, Dictionary<int, object>>();

            titles.Add(PRODUCTS_PAGE_NAME, new Dictionary<int, object>
            {
                [1] = "product_id",
                [2] = "name(en-gb)",
                [3] = "name(ru-ru)",
                [4] = "categories",
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
            titles.Add(IMAGES_PAGE_NAME, new Dictionary<int, object>
            {
                [1] = "product_id",
                [2] = "image",
                [3] = "sort_order"
            });
            titles.Add(OPTIONS_PAGE_NAME, new Dictionary<int, object>
            {
                [1] = "product_id",
                [2] = "option",
                [3] = "default_option_value",
                [4] = "required"
            });
            titles.Add(OPTION_VALUES_PAGE_NAME, new Dictionary<int, object>
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
            titles.Add(ATTRIBUTES_PAGE_NAME, new Dictionary<int, object>
            {
                [1] = "product_id",
                [2] = "attribute_group",
                [3] = "attribute",
                [4] = "text (en-gb)",
                [5] = "text (ru-ru)"
            });
            titles.Add(SEO_PAGE_NAME, new Dictionary<int, object>
            {
                [1] = "product_id",
                [2] = "store_id",
                [3] = "keyword(en-gb)",
                [4] = "keyword(ru-ru)"
            });
            ExcelBook result = CreateBookForResultData(titles, filePath);

            result.Pages[PRODUCTS_PAGE_NAME].SetRowHeight(1, 30);
            result.Pages[PRODUCTS_PAGE_NAME].CentrateRow(1);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat
                (new int[]{ 4, 19, 20, 21, 28, 45});
            result.Pages[OPTIONS_PAGE_NAME].MakeColumnStringFormat(4);
            result.Pages[OPTION_VALUES_PAGE_NAME].MakeColumnStringFormat(5);
            return result;
        }

        public override Dictionary<int, object> MakeLineForProductPage(
            int tovarID, string tovarName, string groups, int quantity, string brand,
            string mainPhoto, string price, string massUnit, string sizeUnit,
            string description, string metaTitle, string metaDescription, string metaKeywords)
        {
            Dictionary<int, object> dataCommon = new Dictionary<int, object>();

            // заполняем массив данных для страницы Products
            dataCommon.Add(1, tovarID);
            dataCommon.Add(2, tovarName);
            dataCommon.Add(3, tovarName);
            dataCommon.Add(4, groups);
            dataCommon.Add(12, 1000);
            //dataCommon.Add(13, tovarObject.Model);
            dataCommon.Add(14, "Conte");
            dataCommon.Add(15, mainPhoto);


            dataCommon.Add(16, "yes");
            dataCommon.Add(17, price);
            dataCommon.Add(18, 0);
            dataCommon.Add(23, massUnit);
            dataCommon.Add(27, sizeUnit);
            dataCommon.Add(28, Boolean.TrueString);
            dataCommon.Add(29, 9);
            dataCommon.Add(30, description);
            dataCommon.Add(31, description);

            // поле meta_Title
            dataCommon.Add(32, metaTitle);
            dataCommon.Add(33, metaTitle);

            // поле meta_Description
            
            dataCommon.Add(34, metaDescription);
            dataCommon.Add(35, metaDescription);

            // поле meta_KeyWords
            dataCommon.Add(36, metaKeywords);
            dataCommon.Add(37, metaKeywords);
            dataCommon.Add(38, 7);
            dataCommon.Add(39, 0);

            // поле Seo_H1
            dataCommon.Add(42, metaDescription);
            dataCommon.Add(43, metaDescription);
            dataCommon.Add(44, 1);
            dataCommon.Add(45, Boolean.TrueString);
            dataCommon.Add(46, 1);

            return dataCommon;
        }

        public override Dictionary<int, object> MakeLineForAdditionalImage(int tovarID, string imagePath)
        {
            Dictionary<int, object> result = new Dictionary<int, object>();
            result.Add(1, tovarID);
            result.Add(2, imagePath);
            result.Add(3, 0);
            return result;
        }

        public override Dictionary<int, object> MakeLineForOption(int tovarID, string optionName)
        {
            Dictionary<int, object> result = new Dictionary<int, object>();
            result.Add(1, tovarID.ToString());
            result.Add(2, optionName);
            result.Add(4, Boolean.TrueString);
            return result;
        }

        public override Dictionary<int, object> MakeLineForOptionValue(int tovarID, string optionName, string optionValue)
        {
            Dictionary<int, object> result = new Dictionary<int, object>();
            result.Add(1, tovarID.ToString());
            result.Add(2, optionName);
            result.Add(3, optionValue);
            result.Add(4, 50);
            result.Add(5, Boolean.FalseString);
            result.Add(6, 0);
            result.Add(7, "+");
            result.Add(8, 0);
            result.Add(9, "+");
            result.Add(10, 0);
            result.Add(11, "+");
            return result;
        }

        public override Dictionary<int, object> MakeLineForAttribute(int tovarID, string groupName, string attributeName, string value)
        {
            Dictionary<int, object> result = new Dictionary<int, object>();
            result.Add(1, tovarID.ToString());
            result.Add(2, groupName);
            result.Add(3, attributeName);
            result.Add(5, value);
            return result;
        }

        public override Dictionary<int, object> MakeLineForSeoUrl(int tovarID, string productName)
        {
            string title = Transliteration.Front(productName).Replace(" ", "-");
            title = this.GetUniqueSeoTitle(title);
            Dictionary<int, object> pairs = new Dictionary<int, object>();
            pairs.Add(1, tovarID.ToString());
            pairs.Add(2, 0);
            pairs.Add(3, "en-" + title);
            pairs.Add(4, title);
            return pairs;
        }
    }
}
