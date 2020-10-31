using MiniJava.Lexer;
using System;
using System.Collections.Generic;
using System.Text;
using MiniJava.General;

namespace MiniJava.Parser.RecursiveDescent
{
    public class ParserError
    {
        public TokenType value;
        public TokenType expected;
        public TokenLocation location;
        public ParserError(TokenType value, TokenType expected) 
        {
            this.value = value;
            this.expected = expected;
        }
        public ParserError(TokenType value, TokenType expected, TokenLocation location)
        {
            this.value = value;
            this.expected = expected;
            this.location = location;
        }
    }
}
