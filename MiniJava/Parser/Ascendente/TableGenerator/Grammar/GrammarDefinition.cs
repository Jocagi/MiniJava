using System.Collections.Generic;
using MiniJava.Lexer;
using MiniJava.Parser.Ascendente.TableGenerator.Gramatica;

namespace MiniJava.Parser.Ascendente.TableGenerator.Grammar
{
    /// <summary>
    /// Producciones y elementos First de la gramatica
    /// </summary>
    public class GrammarDefinition
    {
        //Definir producciones

        /*
            0: E' → E 
            1: E → T + E 
            2: E → T 
            3: T → id
        */

        public static List<Production> productions = new List<Production>
        {
            new Production(TokenType.NT_Start, new List<TokenType>{TokenType.NT_ExampleE}),
            new Production(TokenType.NT_ExampleE, new List<TokenType>{TokenType.NT_ExampleT, TokenType.Operator_mas, TokenType.NT_ExampleT}),
            new Production(TokenType.NT_ExampleT, new List<TokenType>{TokenType.Identifier})
        };

        //{
        //    new Production(TokenType.NT_ExampleE, new List<TokenType>{TokenType.NT_ExampleT, TokenType.Operator_mas, TokenType.NT_ExampleE}),
        //    new Production(TokenType.NT_ExampleE, new List<TokenType>{TokenType.NT_ExampleT}),
        //    new Production(TokenType.NT_ExampleT, new List<TokenType>{TokenType.Identifier})
        //};

        public static List<First> first = new List<First>
        {
            new First(TokenType.NT_Start, new List<TokenType>{TokenType.Identifier}),
            new First(TokenType.NT_ExampleE, new List<TokenType>{TokenType.Identifier}),
            new First(TokenType.NT_ExampleT, new List<TokenType>{TokenType.Identifier})
        };
    }
}
