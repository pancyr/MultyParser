using MultyParser.Core.Excel;
using MultyParser.Core.ExcelReportBuilder;
using MultyParser.Core.Html;
using System.Collections.Generic;

namespace MultyParser.Opencart
{
    public class OpencartOptionExcelReportBuilder : PropertyExcelReportBuilder
    {
        public const string PROPERTIES_PAGE_NAME = "Options";
        public const string VALUES_PAGE_NAME = "OptionValues";

        protected override ExcelBook CreateBookForResultData(string filePath = null)
        {
            Dictionary<string, Dictionary<int, object>> titles =
                new Dictionary<string, Dictionary<int, object>>();

            titles.Add(PROPERTIES_PAGE_NAME, new Dictionary<int, object>
            {
                [1] = "option_id",
                [2] = "type",
                [3] = "sort_order",
                [4] = "name(en-gb)",
                [5] = "name(ru-ru)"
            });
            titles.Add(VALUES_PAGE_NAME, new Dictionary<int, object>
            {
                [1] = "option_value_id",
                [2] = "option_id",
                [3] = "image",
                [4] = "sort_order",
                [5] = "name(en-gb)",
                [6] = "name(ru-ru)"
            });
            ExcelBook result = CreateBookForResultData(titles, filePath);
            return result;
        }

        public override Dictionary<int, object> MakeLineForOption(HtmlOption option)
        {
            Dictionary<int, object> pairs = new Dictionary<int, object>();
            pairs.Add(1, option.ID.ToString());
            pairs.Add(2, option.Type);
            pairs.Add(3, option.SortOrder.ToString());
            pairs.Add(4, option.Name);
            pairs.Add(5, option.Name);
            return pairs;
        }

        public override Dictionary<int, object> MakeLineForOptionValue(HtmlOption option, string value)
        {
            Dictionary<int, object> pairs = new Dictionary<int, object>();
            pairs.Add(1, (option.ID + EntityPos).ToString());
            pairs.Add(2, option.ID.ToString());
            pairs.Add(4, EntityPos.ToString());
            pairs.Add(5, value);
            pairs.Add(6, value);
            return pairs;
        }
    }
}