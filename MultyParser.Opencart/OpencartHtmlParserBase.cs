﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultyParser.Core;
using MultyParser.Core.Html;

namespace MultyParser.Opencart
{
    public abstract class OpencartHtmlParserBase : HtmlParserBase
    {

        public OpencartHtmlParserBase()
        {
            this._resultBookCreater = new OpencartResultBookCreater();
        }

        protected override string GetSizeUnit() => "мм";
        protected override string GetMassUnit() => "кг";

        protected override Dictionary<int, string> GatherCommonDataFromTovarObject(int tovarID, HtmlTovar tovarObject, out string pageName)
        {
            Dictionary<int, string> dataCommon = new Dictionary<int, string>();
            pageName = OpencartResultBookCreater.PRODUCTS_PAGE_NAME;
            string textName = tovarObject.Name;

            if (textName.Length > 0)
            {
                // заполняем массив данных для страницы Products
                dataCommon.Add(1, tovarID.ToString());
                dataCommon.Add(2, textName);
                dataCommon.Add(3, textName);
                dataCommon.Add(12, "1000");
                dataCommon.Add(13, tovarObject.Model);

                if (tovarObject.Photos.Count > 0)
                {
                    dataCommon.Add(15, tovarObject.Photos[0]);
                    tovarObject.Photos.RemoveAt(0);
                }

                dataCommon.Add(16, "yes");
                dataCommon.Add(17, tovarObject.Price);
                dataCommon.Add(18, "0");
                dataCommon.Add(23, GetMassUnit());
                dataCommon.Add(27, GetSizeUnit());
                dataCommon.Add(28, "true");

                // поле Seo_Title
                dataCommon.Add(33, textName);

                // поле Meta_Description
                dataCommon.Add(35, textName + " — купить от Газнефтесервис в Уфе. Гарантия до 5 лет. Сервис 24 часа в сутки.");

                // поле Meta_KeyWords
                dataCommon.Add(37, String.Format("{0}, {1}, купить в Уфе", textName, "" /*categName*/));
                dataCommon.Add(38, "5");
                dataCommon.Add(39, "0");

                // поле Seo_H1
                dataCommon.Add(43, String.Format("{0} купить в Уфе от Газнефтесервис по лучшей цене c гарантией", textName));

                dataCommon.Add(44, "1");
                dataCommon.Add(45, "true");
                dataCommon.Add(46, "1");

                return dataCommon;
            }
            return null;
        }

        protected override List<Dictionary<int, string>> GatherAdditionalImagesFromTovarObject(int tovarID, HtmlTovar tovarObject, out string pageName)
        {
            List<Dictionary<int, string>> result = new List<Dictionary<int, string>>();
            foreach (string photo in tovarObject.Photos)
            {
                Dictionary<int, string> pairs = new Dictionary<int, string>();
                pairs.Add(1, tovarID.ToString());
                pairs.Add(2, photo);
                pairs.Add(3, "0");
                result.Add(pairs);
            }
            pageName = OpencartResultBookCreater.IMAGES_PAGE_NAME;
            return result;
        }

        protected override Dictionary<int, string> MakeDictionaryForAttributesPageRow(int tovarID, string groupName, string attributeName, string value)
        {
            Dictionary<int, string> pairs = new Dictionary<int, string>();
            pairs.Add(1, tovarID.ToString());
            pairs.Add(2, groupName);
            pairs.Add(3, attributeName);
            pairs.Add(5, value);
            return pairs;
        }

        protected override List<Dictionary<int, string>> GatherAttributesFromTovarObject(
            int tovarID, HtmlTovar tovarObject, TovarGroup tovarGroup, Dictionary<int, string> dataCommon,
            Dictionary<string, int> parserSpecifications, Dictionary<string, int> forTransfer, out string pageName)
        {
            List<Dictionary<int, string>> result = new List<Dictionary<int, string>>();

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
                    result.Add(MakeDictionaryForAttributesPageRow(
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

            pageName = OpencartResultBookCreater.ATTRIBUTES_PAGE_NAME;
            return result;
        }

        protected override List<Dictionary<int, string>> GatherOptionFromTovarObject(int tovarID, out string pageName)
        {
            Dictionary<int, string> values = new Dictionary<int, string>();
            values.Add(1, tovarID.ToString());
            values.Add(2, this.GetMainOption());
            values.Add(4, "true");
            pageName = OpencartResultBookCreater.OPTIONS_PAGE_NAME;
            return new List<Dictionary<int, string>> { values };
        }

        protected override List<Dictionary<int, string>> GatherOptionValueFromTovarObject(int tovarID, HtmlTovar tovarObject, out string pageName)
        {
            Dictionary<int, string> values = new Dictionary<int, string>();
            values.Add(1, tovarID.ToString());
            values.Add(2, this.GetMainOption());
            values.Add(3, tovarObject.Option);
            values.Add(4, "50");
            values.Add(5, "false");
            values.Add(6, "0");
            values.Add(7, "+");
            values.Add(8, "0");
            values.Add(9, "+");
            values.Add(10, "0");
            values.Add(11, "+");
            pageName = OpencartResultBookCreater.OPTION_VALUES_PAGE_NAME;
            return new List<Dictionary<int, string>> { values };
        }

        protected override Dictionary<int, string> GatherSeoKeywordsFromTovarObject(int tovarID, HtmlTovar tovarObject, out string pageName)
        {
            string title = Transliteration.Front(tovarObject.Name).Replace(" ", "-");
            title = this.GetUniqueSeoTitle(title);
            Dictionary<int, string> pairs = new Dictionary<int, string>();
            pairs.Add(1, tovarID.ToString());
            pairs.Add(2, "0");
            pairs.Add(3, "en-" + title);
            pairs.Add(4, title);
            pageName = OpencartResultBookCreater.SEO_PAGE_NAME;
            return pairs;
        }
    }
}