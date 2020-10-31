using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniJava.General;

namespace MiniJava.Parser.Ascendente.TableGenerator.LR1
{
    public class TokenLAPair
    {
        public TokenType token { get; set; }
        public List<TokenType> lookahead { get; set; }

        public TokenLAPair(TokenType token, List<TokenType> lookahead)
        {
            this.token = token;
            this.lookahead = lookahead;
        }
        public bool isLookaheadEqual(List<TokenType> lookaheadCompare)
        {
            return
                lookahead.All(lookaheadCompare.Contains) &&
                lookahead.Count == lookaheadCompare.Count;
        }
    }
}
