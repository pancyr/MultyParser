using MultyParser.Core;
using MultyParser.Core.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Opencart
{
    public class OpencartOptionBookCreater : ResultBookCreaterBase
    {
        public const string OPTIONS_PAGE_NAME = "Options";
        public const string OPTION_VALUES_PAGE_NAME = "OptionValues";

        protected override ExcelBook CreateBookForResultData(string filePath = null)
        {
            Dictionary<string, Dictionary<int, string>> titles =
                new Dictionary<string, Dictionary<int, string>>();

            titles.Add(OPTIONS_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "option_id",
                [2] = "type",
                [3] = "sort_order",
                [4] = "name(en-gb)",
                [5] = "name(ru-ru)"
            });
            titles.Add(OPTION_VALUES_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "option_value_id",
                [2] = "option_id",
                [3] = "image",
                [4] = "sort_order",
                [5] = "name(en-gb)",
                [6] = "name(ru-ru)"
            });
            ExcelBook result = CreateBookForResultData(titles, filePath);

            /*result.Pages[PRODUCTS_PAGE_NAME].SetRowHeight(1, 30);
            result.Pages[PRODUCTS_PAGE_NAME].CentrateRow(1);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(4);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(19);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(20);
            result.Pages[PRODUCTS_PAGE_NAME].MakeColumnStringFormat(21);*/
            return result;
        }
    }
}