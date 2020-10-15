﻿using System;
using System.Collections.Generic;
using System.Text;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente
{
    public class Grammar
    {
        public List<Production> Productions { get; set; }
        private List<TokenType> Terminals { get; set; }
        private List<TokenType> NotTerminals { get; set; }

        public Grammar()
        {
        }

        public bool isTerminal(TokenType token)
        {
            return Terminals.Contains(token);
        }

        public bool isNotTerminal(TokenType token)
        {
            return !Terminals.Contains(token);
        }
        private void setTerminals()
        {
            this.Terminals = new List<TokenType> { TokenType.Operator_coma };
        }
    }
}
