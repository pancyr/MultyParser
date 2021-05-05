using MultyParser.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Core
{
    public class TovarGroup
    {
        public TovarGroup() { }

        public TovarGroup(TovarGroupConfigElement group)
        {
            this.DisplayName = group.DisplayName;

            this.Specifications = new Dictionary<int, string>();
            foreach (TovarSpecificationConfigElement spec in group.AttributeList)
            {
                this.Specifications.Add(spec.ID, spec.Name);
            }
        }

        public string DisplayName { get; set; }

        public Dictionary<int, string> Specifications { get; set; }
    }
}
