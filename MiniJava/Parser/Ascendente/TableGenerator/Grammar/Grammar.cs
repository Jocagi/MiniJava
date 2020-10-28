using System;
using System.Collections.Generic;
using System.Linq;
using MiniJava.Lexer;
using MiniJava.Parser.Ascendente.TableGenerator.Gramatica;

namespace MiniJava.Parser.Ascendente.TableGenerator.Grammar
{
    public class Grammar
    {
        public List<Production> Productions { get; set; }
        public List<First> first { get; set; }
        private List<TokenType> Terminals { get; set; }
        private List<TokenType> NonTerminals { get; set; }

        //test grammar
        public Grammar()
        {
            //Definir producciones
            this.Productions = GrammarDefinition.productions;
            //Definir elementos First
            this.first = GrammarDefinition.first;
            //Agregar tokenes terminales
            setTerminals();
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
