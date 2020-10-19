using System.Collections.Generic;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.TableGenerator
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
