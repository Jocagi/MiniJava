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
            new Production(TokenType.NT_Decl, new List<TokenType>{TokenType.NT_FunctionDecl, TokenType.NT_Decl1}),
            new Production(TokenType.NT_Decl, new List<TokenType>{TokenType.NT_ClassDecl, TokenType.NT_Decl1}),
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
            //Declaracion de funcion
            new Production(TokenType.NT_FunctionDecl, new List<TokenType>{TokenType.NT_Type, TokenType.NT_TypeArray, TokenType.Identifier, TokenType.Operator_ParentesisAbre, TokenType.NT_Formals, TokenType.Operator_ParentesisCierra, TokenType.NT_StmtBlock}),
            new Production(TokenType.NT_FunctionDecl, new List<TokenType>{TokenType.Token_void, TokenType.Identifier, TokenType.Operator_ParentesisAbre, TokenType.NT_Formals, TokenType.Operator_ParentesisCierra, TokenType.NT_StmtBlock}),
            //Stmt
            new Production(TokenType.NT_StmtBlock, new List<TokenType> {TokenType.Operator_llaveAbre, TokenType.NT_StmtBlock1, TokenType.NT_StmtBlock2, TokenType.Operator_llaveCierra}),
            new Production(TokenType.NT_StmtBlock1, new List<TokenType>{TokenType.NT_VariableDecl, TokenType.NT_StmtBlock1}),
            new Production(TokenType.NT_StmtBlock1, new List<TokenType>{TokenType.Epsilon}),
            new Production(TokenType.NT_StmtBlock2, new List<TokenType>{TokenType.NT_Stmt, TokenType.NT_StmtBlock2}),
            new Production(TokenType.NT_StmtBlock2, new List<TokenType>{TokenType.Epsilon}),
            new Production(TokenType.NT_Stmt, new List<TokenType>{TokenType.Operator_puntoComa}),
            new Production(TokenType.NT_Stmt, new List<TokenType>{TokenType.NT_Expr,TokenType.Operator_puntoComa}),
            new Production(TokenType.NT_Stmt, new List<TokenType> {TokenType.NT_IfStmt}),
            new Production(TokenType.NT_Stmt, new List<TokenType> {TokenType.NT_WhileStmt}),
            new Production(TokenType.NT_Stmt, new List<TokenType> {TokenType.NT_ForStmt}),
            new Production(TokenType.NT_Stmt, new List<TokenType> {TokenType.NT_BreakStmt}),
            new Production(TokenType.NT_Stmt, new List<TokenType>{TokenType.NT_ReturnStmt}),
            new Production(TokenType.NT_Stmt, new List<TokenType>{TokenType.NT_PrintStmt}),
            new Production(TokenType.NT_Stmt, new List<TokenType>{TokenType.NT_StmtBlock}),
            //If Stmt
            new Production(TokenType.NT_IfStmt, new List<TokenType>{TokenType.Token_if, TokenType.Operator_ParentesisAbre, TokenType.NT_Expr, TokenType.Operator_ParentesisCierra, TokenType.NT_Stmt, TokenType.NT_ElseStmt}),
            new Production(TokenType.NT_ElseStmt, new List<TokenType>{TokenType.NT_ElseStmt, TokenType.NT_Stmt}),
            new Production(TokenType.NT_ElseStmt, new List<TokenType>{TokenType.Epsilon}),
            //While Stmt
            new Production(TokenType.NT_WhileStmt, new List<TokenType>{TokenType.Token_while, TokenType.Operator_ParentesisAbre, TokenType.NT_Expr, TokenType.Operator_ParentesisCierra, TokenType.NT_Stmt}),
            //For Stmt
            new Production(TokenType.NT_ForStmt, new List<TokenType>{TokenType.Token_for, TokenType.Operator_ParentesisAbre, TokenType.NT_Expr, TokenType.Operator_puntoComa, TokenType.NT_Expr, TokenType.Operator_puntoComa, TokenType.NT_Expr, TokenType.Operator_puntoComa, TokenType.Operator_ParentesisCierra, TokenType.NT_Stmt}),
            //Return Stmt
            new Production(TokenType.NT_ReturnStmt, new List<TokenType>{TokenType.Token_return, TokenType.NT_Expr, TokenType.Operator_puntoComa}),
            //Break Stmt
            new Production(TokenType.NT_BreakStmt, new List<TokenType> {TokenType.NT_BreakStmt, TokenType.Operator_puntoComa}),
            //Print Stmt
            new Production(TokenType.NT_PrintStmt, new List<TokenType> {TokenType.Token_System, TokenType.Operator_punto, TokenType.Token_out, TokenType.Operator_punto, TokenType.Token_println, TokenType.Operator_ParentesisAbre, TokenType.NT_PrintStmt2, TokenType.Operator_llaveCierra, TokenType.Operator_puntoComa}),
            new Production(TokenType.NT_PrintStmt2, new List<TokenType>{TokenType.NT_Expr, TokenType.NT_PrintStmt3}),
            new Production(TokenType.NT_PrintStmt3, new List<TokenType>{TokenType.Operator_coma, TokenType.NT_Expr, TokenType.NT_PrintStmt3}),
            new Production(TokenType.NT_PrintStmt3, new List<TokenType>{TokenType.Epsilon}),
            //Expr
            new Production(TokenType.NT_Expr, new List<TokenType>{TokenType.NT_LValue, TokenType.Operator_igual, TokenType.NT_RValue}),
            new Production(TokenType.NT_Expr, new List<TokenType> {TokenType.NT_ExprLogi}),
            new Production(TokenType.NT_Expr, new List<TokenType> {TokenType.Operator_menos, TokenType.NT_Expr}),
            new Production(TokenType.NT_Expr, new List<TokenType> {TokenType.Operator_negacion, TokenType.NT_Expr}),
            new Production(TokenType.NT_LValue, new List<TokenType> {TokenType.Identifier}),
            new Production(TokenType.NT_LValue, new List<TokenType> {TokenType.Identifier, TokenType.Operator_punto, TokenType.Identifier}),
            new Production(TokenType.NT_LValue, new List<TokenType> {TokenType.Token_this, TokenType.Operator_punto, TokenType.Identifier}),
            new Production(TokenType.NT_RValue, new List<TokenType> {TokenType.NT_Expr}),
            new Production(TokenType.NT_RValue, new List<TokenType> {TokenType.Token_New, TokenType.Operator_ParentesisAbre, TokenType.Identifier, TokenType.Operator_llaveCierra}),
           
            new Production(TokenType.NT_ExprLogi, new List<TokenType> {TokenType.NT_ExprDiv}),
            new Production(TokenType.NT_ExprLogi, new List<TokenType> {TokenType.NT_Expr, TokenType.Operator_mayor, TokenType.NT_ExprLogi}),
            new Production(TokenType.NT_ExprLogi, new List<TokenType> {TokenType.NT_Expr, TokenType.Operator_mayorIgual, TokenType.NT_ExprLogi}),
            new Production(TokenType.NT_ExprLogi, new List<TokenType> {TokenType.NT_Expr, TokenType.Operator_diferente, TokenType.NT_ExprLogi}),
            new Production(TokenType.NT_ExprLogi, new List<TokenType> {TokenType.NT_Expr, TokenType.Operator_dobleOr, TokenType.NT_ExprLogi}),
            
            new Production(TokenType.NT_ExprDiv, new List<TokenType> {TokenType.NT_ExprMin}),
            new Production(TokenType.NT_ExprDiv, new List<TokenType> {TokenType.NT_Expr, TokenType.Operator_porcentaje, TokenType.NT_ExprMin}),
            new Production(TokenType.NT_ExprDiv, new List<TokenType> {TokenType.NT_Expr, TokenType.Operator_div, TokenType.NT_ExprMin}),
            
            new Production(TokenType.NT_ExprMin, new List<TokenType> {TokenType.NT_Factor, TokenType.Operator_menos, TokenType.NT_ExprMin}),
            new Production(TokenType.NT_ExprMin, new List<TokenType> {TokenType.NT_Factor}),
            
            new Production(TokenType.NT_Factor, new List<TokenType> {TokenType.NT_Constant}),
            new Production(TokenType.NT_Factor, new List<TokenType> {TokenType.NT_LValue}),
            new Production(TokenType.NT_Factor, new List<TokenType> {TokenType.Operator_ParentesisAbre, TokenType.NT_Expr, TokenType.Operator_ParentesisCierra}),
            
            new Production(TokenType.NT_Constant, new List<TokenType> {TokenType.Const_Int}),
            new Production(TokenType.NT_Constant, new List<TokenType> {TokenType.Const_double}),
            new Production(TokenType.NT_Constant, new List<TokenType> {TokenType.Const_bool}),
            new Production(TokenType.NT_Constant, new List<TokenType> {TokenType.Const_String}),
            new Production(TokenType.NT_Constant, new List<TokenType> {TokenType.Token_null}),
            //Class declaration
            new Production(TokenType.NT_ClassDecl, new List<TokenType> {TokenType.Token_class, TokenType.Identifier, TokenType.NT_ClassDecl1, TokenType.NT_ClassDecl2, TokenType.Operator_llaveAbre, TokenType.NT_Field, TokenType.Operator_llaveCierra}),
            new Production(TokenType.NT_ClassDecl1, new List<TokenType> {TokenType.Token_extends, TokenType.Identifier}),
            new Production(TokenType.NT_ClassDecl1, new List<TokenType> {TokenType.Epsilon}),
            new Production(TokenType.NT_ClassDecl2, new List<TokenType> {TokenType.Token_implements, TokenType.Identifier, TokenType.NT_ClassDecl3}),
            new Production(TokenType.NT_ClassDecl2, new List<TokenType> {TokenType.Epsilon}),
            new Production(TokenType.NT_ClassDecl3, new List<TokenType> {TokenType.Operator_coma, TokenType.Identifier, TokenType.NT_ClassDecl3}),
            new Production(TokenType.NT_ClassDecl3, new List<TokenType> {TokenType.Epsilon}),
            new Production(TokenType.NT_Field, new List<TokenType> {TokenType.NT_VariableDecl, TokenType.NT_Field}),
            new Production(TokenType.NT_Field, new List<TokenType> {TokenType.NT_FunctionDecl, TokenType.NT_Field}),
            new Production(TokenType.NT_Field, new List<TokenType> {TokenType.NT_ConstDecl, TokenType.NT_Field}),
            new Production(TokenType.NT_Field, new List<TokenType> {TokenType.Epsilon})
        };

        public static List<First> first = new List<First>
        {
            //Inicio
            new First(TokenType.NT_Start, new List<TokenType>{TokenType.Token_static,TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_void, TokenType.Identifier, TokenType.Token_interface, TokenType.Token_class}),
            new First(TokenType.NT_Program, new List<TokenType>{TokenType.Token_static,TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_void, TokenType.Identifier,TokenType.Token_interface, TokenType.Token_class}),
            //Declaraciones
            new First(TokenType.NT_Decl, new List<TokenType>{TokenType.Token_static,TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_void, TokenType.Identifier, TokenType.Token_interface, TokenType.Token_class}),
            new First(TokenType.NT_Decl1, new List<TokenType>{TokenType.Epsilon,TokenType.Token_static,TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_void, TokenType.Identifier, TokenType.Token_interface, TokenType.Token_class}),
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
            //Declaracion de funcion
            new First(TokenType.NT_FunctionDecl, new List<TokenType>{TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Identifier, TokenType.Token_void}),
            //Stmt
            new First(TokenType.NT_StmtBlock, new List<TokenType>{TokenType.Operator_llaveAbre}),
            new First(TokenType.NT_StmtBlock1, new List<TokenType>{TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Identifier, TokenType.Epsilon}),
            new First(TokenType.NT_StmtBlock2, new List<TokenType>{TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_null, TokenType.Operator_ParentesisAbre, TokenType.Identifier, TokenType.Token_this, TokenType.Operator_menos, TokenType.Operator_diferente, TokenType.Operator_puntoComa, TokenType.Token_if, TokenType.Token_while, TokenType.Token_for, TokenType.Token_break, TokenType.Token_return, TokenType.Token_System, TokenType.Operator_llaveAbre, TokenType.Epsilon}),
            new First(TokenType.NT_Stmt, new List<TokenType>{TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_null, TokenType.Operator_ParentesisAbre, TokenType.Identifier, TokenType.Token_this, TokenType.Operator_menos, TokenType.Operator_diferente, TokenType.Operator_puntoComa, TokenType.Token_if, TokenType.Token_while, TokenType.Token_for, TokenType.Token_break, TokenType.Token_return, TokenType.Token_System, TokenType.Operator_llaveAbre}),
            //If Stmt
            new First(TokenType.NT_IfStmt, new List<TokenType> {TokenType.Token_if}),
            new First(TokenType.NT_ElseStmt, new List<TokenType> {TokenType.NT_ElseStmt, TokenType.Epsilon}),
            //While Stmt
            new First(TokenType.NT_WhileStmt, new List<TokenType> {TokenType.Token_while}),
            //For Stmt
            new First(TokenType.NT_ForStmt, new List<TokenType> {TokenType.Token_for}),
            //Break Stmt
            new First(TokenType.NT_BreakStmt, new List<TokenType> {TokenType.Token_break}),
            //Return Stmt
            new First(TokenType.NT_ReturnStmt, new List<TokenType> {TokenType.Token_return}),
            //Print
            new First(TokenType.NT_PrintStmt, new List<TokenType> {TokenType.Token_System}),
            new First(TokenType.NT_PrintStmt2, new List<TokenType> {TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_null, TokenType.Operator_ParentesisAbre, TokenType.Identifier, TokenType.Token_this, TokenType.Operator_menos, TokenType.Operator_diferente}),
            new First(TokenType.NT_PrintStmt3, new List<TokenType> {TokenType.Operator_coma, TokenType.Epsilon}),
            //Expr
            new First(TokenType.NT_Expr, new List<TokenType> {TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_null, TokenType.Operator_ParentesisAbre, TokenType.Identifier, TokenType.Token_this, TokenType.Operator_menos, TokenType.Operator_diferente}),
            new First(TokenType.NT_LValue, new List<TokenType> {TokenType.Identifier, TokenType.Token_this}),
            new First(TokenType.NT_RValue, new List<TokenType> {TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_null, TokenType.Operator_ParentesisAbre, TokenType.Identifier, TokenType.Token_this, TokenType.Operator_menos, TokenType.Operator_diferente, TokenType.Token_New}),
            new First(TokenType.NT_ExprLogi, new List<TokenType> {TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_null, TokenType.Operator_ParentesisAbre, TokenType.Identifier, TokenType.Token_this, TokenType.Operator_menos, TokenType.Operator_diferente}),
            new First(TokenType.NT_ExprDiv, new List<TokenType> {TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_null, TokenType.Operator_ParentesisAbre, TokenType.Identifier, TokenType.Token_this, TokenType.Operator_menos, TokenType.Operator_diferente}),
            new First(TokenType.NT_ExprMin, new List<TokenType> {TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_null, TokenType.Operator_ParentesisAbre, TokenType.Identifier, TokenType.Token_this}),
            new First(TokenType.NT_Factor, new List<TokenType> {TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_null, TokenType.Operator_ParentesisAbre, TokenType.Identifier, TokenType.Token_this}),
            new First(TokenType.NT_Constant, new List<TokenType> {TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_null}),
            //Declaracion de clase
            new First(TokenType.NT_ClassDecl, new List<TokenType> {TokenType.Token_class}),
            new First(TokenType.NT_ClassDecl1, new List<TokenType> {TokenType.Token_extends, TokenType.Epsilon}),
            new First(TokenType.NT_ClassDecl2, new List<TokenType> {TokenType.Token_implements, TokenType.Epsilon}),
            new First(TokenType.NT_ClassDecl3, new List<TokenType> {TokenType.Operator_coma, TokenType.Epsilon}),
            new First(TokenType.NT_Field, new List<TokenType>{TokenType.Token_static,TokenType.Const_Int, TokenType.Const_double, TokenType.Const_bool, TokenType.Const_String, TokenType.Token_void, TokenType.Identifier, TokenType.Token_interface, TokenType.Epsilon})
        };
    }
}