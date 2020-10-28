using System.Collections.Generic;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.TableGenerator.Gramatica
{
    public class First
    {
        public TokenType tokenNT { get; set; }
        public List<TokenType> first { get; set; }

        public First(TokenType token, List<TokenType> first)
        {
            this.tokenNT = token;
            this.first = first;
        }
    }
}
