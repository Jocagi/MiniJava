using System;
using System.Collections.Generic;
using System.Text;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente
{
    public class Production
    {
        public TokenType LeftSide { get; set; }
        public List<TokenType> RightSide { get; set; }
    }
}
