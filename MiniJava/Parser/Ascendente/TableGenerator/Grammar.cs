﻿using System;
using System.Collections.Generic;
using System.Linq;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.TableGenerator
{
    public class Grammar
    {
        public List<Production> Productions { get; set; }
        private List<TokenType> Terminals { get; set; }
        private List<TokenType> NonTerminals { get; set; }

        public Grammar(List<Production> productions)
        {
            //Definir producciones
            this.Productions = new List<Production>(productions);
            //Agregar produccion inicial
            this.Productions.Insert
                (0, new Production(TokenType.NT_Start, Productions[0].LeftSide));
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