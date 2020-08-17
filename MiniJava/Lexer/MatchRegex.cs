using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MiniJava.Lexer
{
    public class MatchRegex
    {
        public string value;
        public TokenType tokenType;
        public string remainingText;
        public bool match;

        public MatchRegex(bool isMatch) 
        {
            this.match = isMatch;
        }

        public MatchRegex(string value, TokenType type, string remainingText) 
        {
            this.match = true;
            this.value = value;
            this.tokenType = type;
            this.remainingText = remainingText;
        }
    }
}
