using MultyParser.Core;
using MultyParser.Core.Html;
using System.Collections.Generic;

namespace Velveto.Conte
{
    public class VelvetoDictionaryOfOptions : DictionaryOfOptionsBase
    {
        public VelvetoDictionaryOfOptions()
        {
            Members.Add(new HtmlOption(1000, "Цвета", "radio", 1, ".content-description>div>*", @"\w+", true), new List<string>());
            Members.Add(new HtmlOption(2000, "Размеры", "radio", 1, ".content-description>div>*", @"\d+", true), new List<string>());
        }
    }
}
