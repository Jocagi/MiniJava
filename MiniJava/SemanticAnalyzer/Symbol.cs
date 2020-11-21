using System;
using System.Collections.Generic;
using System.Text;
using MiniJava.General;

namespace MiniJava.SemanticAnalyzer
{
    public class Symbol
    {
        public string ID { get; set; }
        public int scope { get; set; }
        public string value { get; set; }
        public TokenType dataType { get; set; }
        public SymbolType type { get; set; }

        public Symbol(string Name, int scope, string value, TokenType dataType, SymbolType symbolType)
        {
            this.ID = Name;
            this.scope = scope;
            this.value = value;
            this.dataType = dataType;
            this.type = symbolType;
        }
    }
}
