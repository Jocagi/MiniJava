using System.Collections.Generic;
using MiniJava.General;
using MiniJava.Parser.Ascendente.TableGenerator.Gramatica;

namespace MiniJava.Parser.Ascendente.TableGenerator.Grammar
{
    /// <summary>
    /// Producciones y elementos First de la gramatica
    /// </summary>
    public class GrammarDefinition
    {
        //Definir producciones

        public static List<Production> productions = new List<Production>
        {
            new Production(TokenType.NT_Start, new List<TokenType>{TokenType.NT_Program}),
            new Production(TokenType.NT_Program, new List<TokenType>{TokenType.NT_Decl}),

            new Production(TokenType.NT_Decl, new List<TokenType>{TokenType.NT_ConstDecl, TokenType.NT_Decl1}),
            new Production(TokenType.NT_Decl1, new List<TokenType>{TokenType.NT_Decl}),
            new Production(TokenType.NT_Decl1, new List<TokenType>{TokenType.Epsilon}),
            
            new Production(TokenType.NT_ConstDecl, new List<TokenType>{TokenType.Token_static, TokenType.NT_ConstType, TokenType.Identifier, TokenType.Operator_puntoComa}),
            new Production(TokenType.NT_ConstType, new List<TokenType>{TokenType.Const_Int}),
            new Production(TokenType.NT_ConstType, new List<TokenType>{TokenType.Const_double}),
            new Production(TokenType.NT_ConstType, new List<TokenType>{TokenType.Const_bool}),
            new Production(TokenType.NT_ConstType, new List<TokenType>{TokenType.Const_String}),
        };

        public static List<First> first = new List<First>
        {
            new First(TokenType.NT_Start, new List<TokenType>{TokenType.Token_static}),
            new First(TokenType.NT_Program, new List<TokenType>{TokenType.Token_static}),

            new First(TokenType.NT_Decl, new List<TokenType>{TokenType.Token_static}),
            new First(TokenType.NT_Decl1, new List<TokenType>{TokenType.Token_static}),

            new First(TokenType.NT_ConstDecl, new List<TokenType>{TokenType.Token_static}),
            new First(TokenType.NT_ConstType, new List<TokenType>{TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String})
        };
    }
}
