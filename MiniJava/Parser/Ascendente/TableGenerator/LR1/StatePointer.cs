using System.Collections.Generic;
using System.Linq;
using MiniJava.General;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.TableGenerator.LR1
{
    public class StatePointer
    {
        public TokenType symbol { get; set; }
        public List<TokenType> lookahead { get; set; }
        public int state { get; set; }

        public StatePointer(TokenType symbol, List<TokenType> lookahead, int state)
        {
            this.symbol = symbol;
            this.lookahead = lookahead;
            this.state = state;
        }

        public bool isLookaheadEqual(List<TokenType> lookaheadCompare)
        {
            return 
                lookahead.All(lookaheadCompare.Contains) && 
                lookahead.Count == lookaheadCompare.Count;
        }
    }
}
