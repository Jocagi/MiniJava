using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using MiniJava.General;

namespace MiniJava.Lexer
{
    class TokenDescription
    {
        public TokenType tokenType;
        public Regex regexDefinition;

        public TokenDescription(TokenType tokenType, string regex) 
        {
            this.tokenType = tokenType;
            this.regexDefinition = new Regex(regex);
        }
    }
}
