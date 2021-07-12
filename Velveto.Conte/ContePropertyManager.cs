using System.Collections.Generic;
using MultyParser.Core;

namespace Velveto.Conte
{
    public class ContePropertyManager
    {
        public static DictionaryOfProperty GetListForTovarOptions()
        {
            DictionaryOfProperty result = new DictionaryOfProperty();
            TovarProperty colors = new TovarProperty(1000, "Цвет", "radio", 1, ParserBase.REGULAR_COLORS,
                "label.ty-product-options__radio--label",  ".content-description>div>*", true);
            result.MainProperty = colors;
            result.Add(colors, new List<string>());
            result.Add(new TovarProperty(2000, "Размер", "radio", 1, ParserBase.REGULAR_SIZES, ".ty-product-options__radio--label", ".content-description>div>*"), new List<string>());
            return result;
        }

        public static DictionaryOfProperty GetListForTovarFilters()
        {
            DictionaryOfProperty result = GetListForTovarOptions();
            result.Add(new TovarProperty(3000, "Плотность", "radio", 1, @"\d+ ден", "span.ty-control-group>*"), new List<string>());
            result.Add(new TovarProperty(4000, "Бренд", "radio", 1, @"[A-z]\b(?!:)", "span.ty-control-group>*"), new List<string>());
            return result;
        }
    }
}
