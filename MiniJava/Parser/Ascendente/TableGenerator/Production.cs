using System.Collections.Generic;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.TableGenerator
{
    public class Production
    {
        public TokenType LeftSide { get; set; }
        public List<TokenType> RightSide { get; set; }

        public Production(TokenType left, List<TokenType> right)
        {
            this.LeftSide = left;
            this.RightSide = new List<TokenType>(right);
        }

        public Production(TokenType left, TokenType right)
        {
            this.LeftSide = left;
            this.RightSide = new List<TokenType>{ right };
        }
    }
}
