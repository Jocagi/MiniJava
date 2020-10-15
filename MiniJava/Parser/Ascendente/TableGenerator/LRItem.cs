using System;
using System.Collections.Generic;
using System.Text;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente
{
    public class LRItem
    {
        public int State { get; set; }
        public TokenType Production { get; set; }
        public int Position { get; set; }
        public List<TokenType> lookahead { get; set; }

        public bool Equals(LRItem item)
        {
            return (Production == item.Production) && (Position == item.Position);
        }
	}
}
