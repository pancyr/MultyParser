using System;
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
        protected const string PRODUCTS_PAGE_NAME = "Products";
        protected const string ATTRIBUTES_PAGE_NAME = "ProductAttributes";

        public OpencartHtmlParserBase()
        {
            this._resultBookCreater = new OpencartResultBookCreater();
        }

        protected override Dictionary<string, List<Dictionary<int, string>>> GatherValuesFromTovarObject(int tovarID, HtmlTovar tovarObject)
        {
            Dictionary<string, List<Dictionary<int, string>>> result
                = new Dictionary<string, List<Dictionary<int, string>>>();

            Dictionary<int, string> dataCommon = new Dictionary<int, string>();
            List<Dictionary<int, string>> dataAttributes = new List<Dictionary<int, string>>();

            string textName = tovarObject.Name;

            if (textName.Length > 0)
            {
                // заполняем массив данных для страницы Products
                dataCommon.Add(1, tovarID.ToString());
                dataCommon.Add(2, textName);
                dataCommon.Add(3, textName);
                dataCommon.Add(12, "1000");

                string photo_url = (tovarObject.MainPhotoUrl.StartsWith(@"//")) ?
                    tovarObject.MainPhotoUrl : GetBaseUrl() + tovarObject.MainPhotoUrl;

                dataCommon.Add(15, photo_url);
                dataCommon.Add(16, "yes");
                dataCommon.Add(18, "0");
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

                // заполнение страницы атрибутов

                TovarGroup commonTovarGroup = MultyParserApp.TovarGroups[this.GetCodeOfTovarGroup()];
                Dictionary<string, int> parserSpecifications = this.GetSpecifications();
                Dictionary<string, int> forTransfer = this.GetForTransferToMainPage();

                foreach (KeyValuePair<string, string> spec in tovarObject.Specifications)
                {
                    // Пытаемся найти соответствие спецификации со страницы сайта
                    // одной из тех, что подлежит занесению в файл импорта
                    if (parserSpecifications.ContainsKey(spec.Key))
                    {
                        // Если такое соответствие найдено,
                        // Делаем соответствующую запись на листе атрибутов

                        int attributeCode = parserSpecifications[spec.Key];
                        string attributeName = commonTovarGroup.Specifications[attributeCode];

                        Dictionary<int, string> values = new Dictionary<int, string>();
                        values.Add(1, tovarID.ToString());
                        values.Add(2, commonTovarGroup.DisplayName);
                        values.Add(3, attributeName);
                        values.Add(5, spec.Value);
                        dataAttributes.Add(values);
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

                result.Add(PRODUCTS_PAGE_NAME, new List<Dictionary<int, string>> { dataCommon });
                result.Add(ATTRIBUTES_PAGE_NAME, dataAttributes);
                return result;
            }
            return null;
        }
    }
}