using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyParser.Core
{
    public interface IParserCreater
    {
        ParserBase GetParserObjectForProducts();
        ParserBase GetParserObjectForOptions();
    }
}
