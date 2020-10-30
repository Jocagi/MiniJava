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
            //Inicio
            new Production(TokenType.NT_Start, new List<TokenType>{TokenType.NT_Program}),
            new Production(TokenType.NT_Program, new List<TokenType>{TokenType.NT_Decl}),
            //Declaraciones
            new Production(TokenType.NT_Decl, new List<TokenType>{TokenType.NT_InterfaceDecl, TokenType.NT_Decl1}),
            new Production(TokenType.NT_Decl, new List<TokenType>{TokenType.NT_VariableDecl, TokenType.NT_Decl1}),
            new Production(TokenType.NT_Decl, new List<TokenType>{TokenType.NT_ConstDecl, TokenType.NT_Decl1}),
            new Production(TokenType.NT_Decl1, new List<TokenType>{TokenType.NT_Decl}),
            new Production(TokenType.NT_Decl1, new List<TokenType>{TokenType.Epsilon}),
            //Declaracion de constante
            new Production(TokenType.NT_ConstDecl, new List<TokenType>{TokenType.Token_static, TokenType.NT_ConstType, TokenType.Identifier, TokenType.Operator_puntoComa}),
            new Production(TokenType.NT_ConstType, new List<TokenType>{TokenType.Const_Int}),
            new Production(TokenType.NT_ConstType, new List<TokenType>{TokenType.Const_double}),
            new Production(TokenType.NT_ConstType, new List<TokenType>{TokenType.Const_bool}),
            new Production(TokenType.NT_ConstType, new List<TokenType>{TokenType.Const_String}),
            //Declaracion de variable
            new Production(TokenType.NT_VariableDecl, new List<TokenType>{TokenType.NT_Variable, TokenType.Operator_puntoComa}),
            new Production(TokenType.NT_Variable, new List<TokenType>{TokenType.NT_Type,TokenType.NT_TypeArray,TokenType.Identifier}),
            new Production(TokenType.NT_Type, new List<TokenType>{TokenType.Const_Int}),
            new Production(TokenType.NT_Type, new List<TokenType>{TokenType.Const_double}),
            new Production(TokenType.NT_Type, new List<TokenType>{TokenType.Const_bool}),
            new Production(TokenType.NT_Type, new List<TokenType>{TokenType.Const_String}),
            new Production(TokenType.NT_Type, new List<TokenType>{TokenType.Identifier}),
            new Production(TokenType.NT_TypeArray, new List<TokenType>{TokenType.Operator_corchetes, TokenType.NT_TypeArray}),
            new Production(TokenType.NT_TypeArray, new List<TokenType>{TokenType.Epsilon}),
            //Declaracion de interfaz
            new Production(TokenType.NT_InterfaceDecl, new List<TokenType>{TokenType.Token_interface, TokenType.Identifier, TokenType.Operator_llaveAbre, TokenType.NT_Prototype, TokenType.Operator_llaveCierra}),
            new Production(TokenType.NT_Prototype, new List<TokenType>{TokenType.NT_Type, TokenType.NT_TypeArray, TokenType.Identifier, TokenType.Operator_ParentesisAbre, TokenType.NT_Formals, TokenType.Operator_ParentesisCierra, TokenType.Operator_puntoComa, TokenType.NT_Prototype}),
            new Production(TokenType.NT_Prototype, new List<TokenType>{TokenType.Token_void, TokenType.Identifier, TokenType.Operator_ParentesisAbre, TokenType.NT_Formals, TokenType.Operator_ParentesisCierra, TokenType.Operator_puntoComa, TokenType.NT_Prototype}),
            new Production(TokenType.NT_Prototype, new List<TokenType>{TokenType.Epsilon}),
            //Formals
            new Production(TokenType.NT_Formals, new List<TokenType>{TokenType.NT_Variable, TokenType.Operator_coma, TokenType.NT_Formals}),
            new Production(TokenType.NT_Formals, new List<TokenType>{TokenType.NT_Variable}),
        };

        public static List<First> first = new List<First>
        {
            //Inicio
            new First(TokenType.NT_Start, new List<TokenType>{TokenType.Token_static,TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Identifier, TokenType.Token_interface}),
            new First(TokenType.NT_Program, new List<TokenType>{TokenType.Token_static,TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Identifier,TokenType.Token_interface}),
            //Declaraciones
            new First(TokenType.NT_Decl, new List<TokenType>{TokenType.Token_static,TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Identifier, TokenType.Token_interface}),
            new First(TokenType.NT_Decl1, new List<TokenType>{TokenType.Epsilon,TokenType.Token_static,TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Identifier,TokenType.Token_interface}),
            //Declaracion de contantes
            new First(TokenType.NT_ConstDecl, new List<TokenType>{TokenType.Token_static}),
            new First(TokenType.NT_ConstType, new List<TokenType>{TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String}),
            //Declaracion de variables
            new First(TokenType.NT_VariableDecl, new List<TokenType>{TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Identifier}),
            new First(TokenType.NT_Variable, new List<TokenType>{TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Identifier}),
            new First(TokenType.NT_Type, new List<TokenType>{TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Identifier}),
            new First(TokenType.NT_TypeArray, new List<TokenType>{TokenType.Epsilon, TokenType.Operator_corchetes}),
            //Declaracion de interfaz
            new First(TokenType.NT_InterfaceDecl, new List<TokenType>{TokenType.Token_interface}),
            new First(TokenType.NT_Prototype, new List<TokenType>{TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Identifier, TokenType.Token_void, TokenType.Epsilon}),
            //Formals
            new First(TokenType.NT_Formals, new List<TokenType>{TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Identifier}),
        };
    }
}
