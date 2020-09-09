using MiniJava.Lexer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniJava.Parser.RecursiveDescent
{
    public class ParserError
    {
        public TokenType value;
        public TokenType expected;
        public ParserError(TokenType value, TokenType expected) 
        {
            this.value = value;
            this.expected = expected;
        }
    }
}
