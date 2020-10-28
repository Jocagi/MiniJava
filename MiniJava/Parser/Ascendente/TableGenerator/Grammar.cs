using System;
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

        //test grammar
        public Grammar()
        {
            /*
               0: E' → E 
               1: E → T + E 
               2: E → T 
               3: T → id
             */

            //Definir producciones
            //this.Productions = new List<Production>
            //{
            //    new Production(TokenType.NT_ExampleE, new List<TokenType>{TokenType.NT_ExampleT, TokenType.Operator_mas, TokenType.NT_ExampleE}),
            //    new Production(TokenType.NT_ExampleE, new List<TokenType>{TokenType.NT_ExampleT}),
            //    new Production(TokenType.NT_ExampleT, new List<TokenType>{TokenType.Identifier})
            //};

            this.Productions = new List<Production>
            {
                new Production(TokenType.NT_ExampleE, new List<TokenType>{TokenType.NT_ExampleT, TokenType.Operator_mas, TokenType.NT_ExampleT}),
                new Production(TokenType.NT_ExampleT, new List<TokenType>{TokenType.Identifier})
            };

            //Agregar produccion inicial
            this.Productions.Insert
                (0, new Production(TokenType.NT_Start, Productions[0].LeftSide));
            
            //Agregar tokenes terminales
            setTerminals();
        }

        public Grammar(List<Production> productions)
        {
            //Definir producciones
            this.Productions = new List<Production>(productions);
            //Agregar produccion inicial
            this.Productions.Insert
                (0, new Production(TokenType.NT_Start, Productions[0].LeftSide));
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
