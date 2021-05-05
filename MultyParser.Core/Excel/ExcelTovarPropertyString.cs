using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Core.Excel
{
    public class ExcelTovarPropertyString : ExcelTovarPropertyBase
    {
        public ExcelTovarPropertyString(string name, string group, int column) : base(name, group, column) { }

        public ExcelTovarPropertyString(string name, string group, List<int> columns) : base(name, group, columns) { }

        public override string ParseVaue(string value) => value;
    }
}
