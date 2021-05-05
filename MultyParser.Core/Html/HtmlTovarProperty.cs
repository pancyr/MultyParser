using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Core.Html
{
    public class HtmlTovarProperty
    {
        public HtmlTovarProperty(string name, string group)
        {
            this.Name = name;
            this.Group = group;
        }

        public string Name { get; set; }
        public string Group { get; set; }
    }
}
