using System.Collections.Generic;
using MiniJava.General;
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
            new Production(TokenType.NT_Start, new List<TokenType>{TokenType.NT_ExampleS}),
            new Production(TokenType.NT_ExampleS, new List<TokenType>{TokenType.Identifier}),
            new Production(TokenType.NT_ExampleS, new List<TokenType>{TokenType.NT_ExampleV, TokenType.Operator_puntosIgual, TokenType.NT_ExampleE}),
            new Production(TokenType.NT_ExampleV, new List<TokenType>{TokenType.Identifier}),
            new Production(TokenType.NT_ExampleE, new List<TokenType>{TokenType.NT_ExampleV}),
            new Production(TokenType.NT_ExampleE, new List<TokenType>{TokenType.Const_Int})
        };

        public static List<First> first = new List<First>
        {
            new First(TokenType.NT_Start, new List<TokenType>{TokenType.Identifier}),
            new First(TokenType.NT_ExampleS, new List<TokenType>{TokenType.Identifier}),
            new First(TokenType.NT_ExampleV, new List<TokenType>{TokenType.Identifier}),
            new First(TokenType.NT_ExampleE, new List<TokenType>{TokenType.Identifier, TokenType.Const_Int})
        };
    }
}
