using MultyParser.Core.Excel;
using MultyParser.Core.ExcelReportBuilder;
using MultyParser.Core;
using System.Collections.Generic;

namespace MultyParser.Opencart
{
    public class OpencartFilterExcelReportBuilder : PropertyExcelReportBuilder
    {
        public const string PROPERTIES_PAGE_NAME = "FilterGroups";
        public const string VALUES_PAGE_NAME = "Filters";

        protected override ExcelBook CreateBookForResultData(string filePath = null)
        {
            Dictionary<string, Dictionary<int, object>> titles =
                new Dictionary<string, Dictionary<int, object>>();

            titles.Add(PROPERTIES_PAGE_NAME, new Dictionary<int, object>
            {
                [1] = "filter_group_id",
                [2] = "sort_order",
                [3] = "name(en-gb)",
                [4] = "name(ru-ru)"
            });
            titles.Add(VALUES_PAGE_NAME, new Dictionary<int, object>
            {
                [1] = "filter_id",
                [2] = "filter_group_id",
                [3] = "sort_order",
                [4] = "name(en-gb)",
                [5] = "name(ru-ru)"
            });
            ExcelBook result = CreateBookForResultData(titles, filePath);
            return result;
        }

        public override Dictionary<int, object> MakeLineForOption(TovarProperty option)
        {
            string name = option.DisplayName ?? option.Name;
            Dictionary<int, object> pairs = new Dictionary<int, object>();
            pairs.Add(1, option.ID.ToString());
            pairs.Add(2, option.SortOrder.ToString());
            pairs.Add(3, name);
            pairs.Add(4, name);
            return pairs;
        }

        public override Dictionary<int, object> MakeLineForOptionValue(TovarProperty option, string value)
        {
            Dictionary<int, object> pairs = new Dictionary<int, object>();
            pairs.Add(1, (option.ID + EntityPos).ToString());
            pairs.Add(2, option.ID.ToString());
            pairs.Add(3, EntityPos.ToString());
            pairs.Add(4, value);
            pairs.Add(5, value);
            return pairs;
        }
    }
}
