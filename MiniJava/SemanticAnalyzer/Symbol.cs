using System;
using System.Collections.Generic;
using System.Text;

namespace MiniJava.SemanticAnalyzer
{
    public class Symbol
    {
        public string ID { get; set; }
        public int scope { get; set; }
        public string value { get; set; }
        public SymbolType type { get; set; }
    }
}
