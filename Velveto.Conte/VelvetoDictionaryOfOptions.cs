using MultyParser.Core;
using MultyParser.Core.Html;
using System.Collections.Generic;

namespace Velveto.Conte
{
    public class VelvetoDictionaryOfOptions : DictionaryOfOptionsBase
    {
        public VelvetoDictionaryOfOptions()//(?!:)
        {
            Members.Add(new HtmlOption(1000, "Цвет", "radio", 1, ".content-description>div>*", ".ty-product-options__radio--label", @"[A-zА-яЁё]+(-[A-zА-яЁё]+)?\b(?!:)", true), new List<string>());
            Members.Add(new HtmlOption(2000, "Размер", "radio", 1, ".content-description>div>*", ".ty-product-options__radio--label", @"\d+(-\d+)?", true), new List<string>());
        }
    }
}
