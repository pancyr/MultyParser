using System.Collections.Generic;
using MultyParser.Core;

namespace Velveto.Conte
{
    public class ContePropertyManager
    {
        public static DictionaryOfProperty GetListForTovarOptions()
        {
            DictionaryOfProperty result = new DictionaryOfProperty();
            result.Add(new TovarProperty(1000, "Цвет", "radio", 1, ".content-description>div>*", ".ty-product-options__radio--label", @"^[A-zА-яЁё]+((-|\. |\.)[A-zА-яЁё]+)*\b(?!:)", true), new List<string>());
            result.Add(new TovarProperty(2000, "Размер", "radio", 1, ".content-description>div>*", ".ty-product-options__radio--label", @"\d+(-\d+)?", true), new List<string>());
            return result;
        }

        public static DictionaryOfProperty GetListForTovarFilters()
        {
            DictionaryOfProperty result = GetListForTovarOptions();
            result.Add(new TovarProperty(3000, "Плотность", "radio", 1, null, "span.ty-control-group>*", @"\d+ ден", true), new List<string>());
            result.Add(new TovarProperty(4000, "Бренд", "radio", 1, null, "span.ty-control-group>*", @"[A-z]\b(?!:)", true, "Коллекция"), new List<string>());
            return result;
        }
    }
}
