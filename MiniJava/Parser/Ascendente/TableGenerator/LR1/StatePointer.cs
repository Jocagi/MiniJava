using System.Collections.Generic;
using System.Linq;
using MiniJava.General;
using MiniJava.Lexer;
using MiniJava.Parser.Ascendente.TableGenerator.Grammar;

namespace MiniJava.Parser.Ascendente.TableGenerator.LR1
{
    public class StatePointer
    {
        public TokenType symbol { get; set; }
        public Production production { get; set; }
        public List<TokenType> lookahead { get; set; }
        public int state { get; set; }

        public StatePointer(TokenType symbol, List<TokenType> lookahead, int state, Production production)
        {
            this.symbol = symbol;
            this.lookahead = lookahead;
            this.state = state;
            this.production = production;
        }

        public bool isLookaheadEqual(List<TokenType> lookaheadCompare)
        {
            return 
                lookahead.All(lookaheadCompare.Contains) && 
                lookahead.Count == lookaheadCompare.Count;
        }
        public bool isProductionEqual(Production productionCompare)
        {
            return
                production.RightSide.All(productionCompare.RightSide.Contains) &&
                production.RightSide.Count == productionCompare.RightSide.Count &&
                production.LeftSide == productionCompare.LeftSide;
        }
    }
}
