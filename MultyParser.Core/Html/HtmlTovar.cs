using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Core.Html
{
    public class HtmlTovar
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MainPhotoUrl { get; set; }

        protected List<string> _photos;
        public List<string> Photos
        {
            get
            {
                if (_photos == null)
                    _photos = new List<string>();
                return _photos;
            }
        }

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
