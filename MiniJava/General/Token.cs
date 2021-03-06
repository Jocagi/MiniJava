﻿using System;
using System.Collections.Generic;
using System.Text;
using MiniJava.General;

namespace MiniJava.Lexer
{
    public class Token
    {
        public TokenType tokenType;
        public string value;
        public TokenLocation location;

        public Token(TokenType tokenType) 
        {
            this.tokenType = tokenType;
            this.value = string.Empty;
        }
        public Token(TokenType tokenType, string value)
        {
            this.tokenType = tokenType;
            this.value = value;
        }
        public Token(TokenType tokenType, string value, TokenLocation location)
        {
            this.tokenType = tokenType;
            this.value = value;
            this.location = location;



        }
        public Token(MatchRegex match)
        {
            this.tokenType = match.tokenType;
            this.value = match.value;
        }
        public Token(MatchRegex match, TokenLocation location)
        {
            this.tokenType = match.tokenType;
            this.value = match.value;
            this.location = location;
        }
    }
}
