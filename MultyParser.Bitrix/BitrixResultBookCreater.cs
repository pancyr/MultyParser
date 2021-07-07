using MultyParser.Core;
using MultyParser.Core.Excel;
using MultyParser.Core.ExcelReportBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Bitrix
{
    public class BitrixResultBookCreater : TovarExcelReportBuilder
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
                [4] = "categories"
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
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(19);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(20);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(21);
            return result;
        }

        public override void SetTovarTitle(string title, Dictionary<int, object> data)
        {
            // поле meta_Title
            data.Add(32, title);
            data.Add(33, title);
        }

        public override Dictionary<int, object> MakeLineForProductPage(int tovarID, string tovarName, string groups, int quantity, string brand, string mainPhoto, string price, string massUnit, string sizeUnit, string description, string metaTitle, string metaDescription, string metaKeywords)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<int, object> MakeLineForAdditionalImage(int tovarID, string imagePath)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<int, object> MakeLineForOption(int tovarID, string optionName)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<int, object> MakeLineForOptionValue(int tovarID, string optionName, string optionValue)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<int, object> MakeLineForAttribute(int tovarID, string groupName, string attributeName, string value)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<int, object> MakeLineForFilterValue(int tovarID, string groupName, string filterValue)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<int, object> MakeLineForSeoUrl(int tovarID, string productName)
        {
            throw new NotImplementedException();
        }
    }
}
