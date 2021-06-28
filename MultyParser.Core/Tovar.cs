using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Core
{
    public class Tovar
    {
        public string Name { get; set; }
        public string Categories { get; set; }
        public string Group { get; set; }
        public string MainOptionValue { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public List<string> Photos { get; set; }

        public DictionaryOfProperty Options { get; set; }
        public DictionaryOfProperty Filters { get; set; }

        protected Dictionary<string, string> _specifications;
        public Dictionary<string, string> Specifications
        {
            get
            {
                if (_specifications == null)
                    _specifications = new Dictionary<string, string>();
                return _specifications;
            }
        }
    }
}
