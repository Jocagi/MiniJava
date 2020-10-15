using System;
using System.Collections.Generic;
using System.Linq;
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
            //Convertir enumerable en lista
            this.Terminals = Enum.GetValues(typeof(TokenType)).Cast<TokenType>().ToList();
            //Remover todos los items mayores al ultimo terminal (No terminales)
            this.Terminals.RemoveAll(x => x > TokenType.Operator_puntosIgual);
        }
    }
}
