using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using MiniJava.General;
using MiniJava.Lexer;
using MiniJava.SemanticAnalyzer;

namespace MiniJava.Parser.Descendente
{
    public class Parser
    {
        /// <summary>
        /// El analizador sitanctico recursivo recorre de forma recursiva la gramatica.
        /// Cada Simbolo NO TERMINAL es representado como una funcion y cada simbolo TERMINAL se evalua atraves del MATCH()
        /// El punto de partida es PROGRAM, si se hace backtracking hasta aqui se reporta el error.
        /// Se 'Concatenan' varias expresiones a traves de un AND para validar que cada token este es el lugar correcto. Sin importar si es terminal o no terminal.
        /// </summary>
        
        //Variables Parser
        private Queue<Token> tokens;
        private ParserReport result;
        private TokenType lookahead;
        private TokenType expectedValue;
        private TokenLocation actualLocation;
        private bool acertoToken; // sirve para ver si un token nullable cumplio o no
        private bool repetirDECLerror;
        //Variables Analizador semantico
        private List<List<Symbol>> tablas = new List<List<Symbol>>();
        private List<Symbol> tablaSimbolos = new List<Symbol>();
        private string mathOperation = "";
        private int actualScope = 0;

        //ANALIZADOR SINTACTICO
        public Parser(Queue<Token> tokens)
        {
            this.tokens = tokens;
            this.result = new ParserReport();
            this.lookahead = TokenType.Default;
            this.acertoToken = false;
            this.repetirDECLerror = false;
        }
        public ParserReport getReport()
        {
            if (tokens.Count > 0)
            {
                Dequeue();
                //Start grammar path
                PROGRAM();
            }
            return result;
        }
        private void Dequeue()
        {
            if (tokens.Count > 0)
            {
                Token t = tokens.Dequeue();
                lookahead = t.tokenType;
                actualLocation = t.location;
            }
            else
            {
                lookahead = TokenType.Default;
            }
        }
        private bool Match(TokenType token, bool epsilon)// si epsilon es true, el token es nulable
        {
            acertoToken = false;
            if (lookahead == token)
            {
                Dequeue();
                acertoToken = true;
                return true;
            }
            else if (token == TokenType.Epsilon || epsilon)
            {
                return true;
            }

            expectedValue = token;
            return false;
        }       
        private bool MatchConstant(bool epsilon)
        {
            acertoToken = false;
            if (lookahead == TokenType.Const_Int || lookahead == TokenType.Const_double || lookahead == TokenType.Const_bool || 
                lookahead == TokenType.Const_String || lookahead == TokenType.Token_null)
            {
                Dequeue();
                acertoToken = true;
                return true;
            }
            if (epsilon)
            {
                return true;
            }
            expectedValue = TokenType.Constant;
            return false;
        }
        private void ERROR(TokenType expected, ErrorType errorType)
        {
            if ( lookahead != TokenType.Default )
            {
                result.addError(new ParserError(lookahead, expected, actualLocation, errorType));
                int errorRow = actualLocation.row;
                if (repetirDECLerror)
                {
                    repetirDECLerror = false;
                    //Saltar a la siguiente linea
                    while (errorRow == actualLocation.row && lookahead != TokenType.Default)
                    {
                        Dequeue();
                    }
                }
                else
                {
                    repetirDECLerror = true;
                }
            }            
        }
        private void PROGRAM()
        {
            if (!Decl())
            {
                ERROR(expectedValue, ErrorType.semantico);
            }
            if (tokens.Count > 0)
            {
                PROGRAM();
            }
        }     
        private bool Constant()
        {
            if (MatchConstant(true) && !acertoToken)
            {
                return false;
            }
            return true;
        }
        private bool Lvalue1()
        {
            // + OP1
            if (Match(TokenType.Operator_punto, true) && acertoToken)
            {
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                if (!Lvalue1())
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool Lvalue()
        {
            if (Match(TokenType.Identifier, true) && acertoToken)
            {
                if (!Lvalue1())
                {
                    return false;
                }
                return true;
            }
            if (Match(TokenType.Token_this, true) && acertoToken)
            {
                if (!Match(TokenType.Operator_punto, false))
                {
                    return false;
                }
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool Factor()
        {
            if (Constant())
            {
                return true;
            }
            if (Lvalue())
            {
                return true;
            }
            if (Match(TokenType.Operator_ParentesisAbre, true) & acertoToken)
            {
                if (!Expr())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                return true;
            }
            if (Match(TokenType.Token_New, true) & acertoToken)
            {
                if (!Match(TokenType.Operator_ParentesisAbre, false))
                {
                    return false;
                }
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool Operacion()
        {
            acertoToken = false;
            if (lookahead == TokenType.Operator_igual || lookahead == TokenType.Operator_mayor ||
                lookahead == TokenType.Operator_mayorIgual || lookahead == TokenType.Operator_diferente || 
                lookahead == TokenType.Operator_dobleOr || lookahead == TokenType.Operator_porcentaje || 
                lookahead == TokenType.Operator_div || lookahead == TokenType.Operator_menos)
            {
                Dequeue();
                acertoToken = true;
                return true;
            }
            expectedValue = TokenType.Operator_menor;
            return false;
        }
        private bool A(bool epsilon)
        {
            acertoToken = false;
            if (lookahead == TokenType.Operator_negacion || lookahead == TokenType.Operator_menos)
            {
                Dequeue();
                acertoToken = true;
                return true;
            }
            if (epsilon)
            {
                return true;
            }
            expectedValue = TokenType.Operator_menor;
            return false;
        }
        private bool Expr()
        {
            //OpTerm BoolSymb OP1
            if (A(true))
            {
                if (!Factor())
                {
                    return false;
                }
                if (!Expr1())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool Expr1()
        {
            //OpTerm BoolSymb OP1
            if (Operacion())
            {               
                if (!Expr())
                {
                    return false;
                }
                return true;
            }

            return true;
        }
        private bool PrintStmt3()
        {
            if (Match(TokenType.Operator_coma, true) && acertoToken)
            {
                if (!Expr())
                {
                    return false;
                }
                if (!PrintStmt3())
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool PrintStmt2()
        {
            if (Expr())
            {
                if (!PrintStmt3())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool PrintStmt()
        {
            if (Match(TokenType.Token_System, true) && acertoToken)
            {
                if (!Match(TokenType.Operator_punto, false))
                {
                    return false;
                }
                if (!Match(TokenType.Token_out, false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_punto, false))
                {
                    return false;
                }
                if (!Match(TokenType.Token_println, false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisAbre, false))
                {
                    return false;
                }
                if (!PrintStmt2())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool BreakStmt()
        {
            if (Match(TokenType.Token_break, true) && acertoToken)
            {
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool ReturnStmt()
        {
            if (Match(TokenType.Token_return, true) && acertoToken)
            {
                if (!Expr())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool ForStmt()
        {
            if (Match(TokenType.Token_for, true) && acertoToken)
            {
                if (!Match(TokenType.Operator_ParentesisAbre, false))
                {
                    return false;
                }
                if (!Expr())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }
                if (!Expr())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }
                if (!Expr())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                if (!Stmt())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool WhileStmt()
        {
            if (Match(TokenType.Token_while, true) && acertoToken)
            {
                if (!Match(TokenType.Operator_ParentesisAbre, false))
                {
                    return false;
                }
                if (!Expr())
                {
                    return false;
                }

                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                if (!Stmt())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool ElseStmt()
        {
            if (Match(TokenType.Token_else, true) & acertoToken)
            {
                if (!Stmt())
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool IfStmt()
        {
            if (Match(TokenType.Token_if, true) && acertoToken)
            {
                if (!Match(TokenType.Operator_ParentesisAbre, false))
                {
                    return false;
                }
                if (!Expr())
                {
                    return false;
                }

                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                if (!Stmt())
                {
                    return false;
                }
                if (!ElseStmt())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool Actuals()
        {
            if (Match(TokenType.Operator_coma, true) && acertoToken)
            {
                if (!Expr())
                {
                    return false;
                }
                if (!Actuals())
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool CallStmt()
        {
            if (Match(TokenType.Operator_ParentesisAbre, true) && acertoToken)
            {
                if (!Expr())
                {
                    return false;
                }
                if (!Actuals())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool Stmt0()
        {
            if (CallStmt())
            {
                return true;
            }
            if (Expr1())
            {
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool Stmt()
        {
            if (Match(TokenType.Operator_puntoComa, true) & acertoToken)
            {
                return true;
            }
            if (IfStmt())
            {
                return true;
            }
            if (WhileStmt())
            {
                return true;
            }
            if (ForStmt())
            {
                return true;
            }
            if (BreakStmt())
            {
                return true;
            }
            if (ReturnStmt())
            {
                return true;
            }
            if (PrintStmt())
            {
                return true;
            }
            if (StmtBlock())
            {
                return true;
            }
            if (Lvalue())
            {
                if (!Stmt0())
                {
                    return false;
                }
                return true;
            }
            if (Expr())
            {
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool StmtBlock2()
        {
            if (Stmt())
            {
                if (!StmtBlock2())
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool StmtBlock1()
        {
            if (VariableDecl())
            {
                if (!StmtBlock1())
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool StmtBlock()
        {
            if (Match(TokenType.Operator_llaveAbre, true) && acertoToken)
            {
                if (!StmtBlock1())
                {
                    return false;
                }
                if (!StmtBlock2())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_llaveCierra, false) )
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool Prototype()
        {
            if (Match(TokenType.Token_void, true) && acertoToken)
            {
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisAbre, false))
                {
                    return false;
                }
                if (!Formals())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }
                if (!Prototype())
                {
                    return false;
                }
                return true;
            }
            if (Type())
            {
                if (!TypeArray())
                {
                    return false;
                }
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisAbre, false))
                {
                    return false;
                }
                if (!Formals())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }
                if (!Prototype())
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool InterfaceDecl()
        {
            if (Match(TokenType.Token_interface, true) && acertoToken)
            {
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_llaveAbre, false))
                {
                    return false;
                }
                if (!Prototype())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_llaveCierra, false))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool Field2()
        {
            if (Match(TokenType.Operator_puntoComa, true) && acertoToken)
            {
                if (!Field())
                {
                    return false;
                }
                return true;
            }
            if (FunctionDecl())
            {
                if (!Field())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool Field()
        {
            if (Variable())
            {
                if (!Field2())
                {
                    return false;
                }
                return true;
            }
            if (FunctionDecl1())
            {
                if (!Field())
                {
                    return false;
                }
                return true;
            }
            
            if (ConstDecl())
            {
                if (!Field())
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool ClassDecl3()
        {
            if (Match(TokenType.Operator_coma, true) && acertoToken)
            {
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }                
                if (!ClassDecl3())
                {
                    return false;
                }                
                return true;
            }
            return true;
        }
        private bool ClassDecl2()
        {
            if (Match(TokenType.Token_implements, true) && acertoToken)
            {
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                if (!ClassDecl3())
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool ClassDecl1()
        {
            if (Match(TokenType.Token_extends, true) && acertoToken)
            {
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool ClassDecl()
        {
            if (Match(TokenType.Token_class, true) && acertoToken)
            {
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                if (!ClassDecl1())
                {
                    return false;
                }
                if (!ClassDecl2())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_llaveAbre, false))
                {
                    return false;
                }
                if (!Field())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_llaveCierra, false))
                {
                    return false;
                }                
                return true;
            }
            return false;
        }
        private bool Formals1()
        {
            if (Match(TokenType.Operator_coma, true) && acertoToken)
            {
                if (!Formals())
                {
                    return false;
                }
                return true;
            }
            return true;
            
        }
        private bool Formals()
        {
            if (Variable())
            {
                if (!Formals1())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool FunctionDecl()
        {    
            if (Match(TokenType.Operator_ParentesisAbre, true) && acertoToken)
            { 
                if (!Formals())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                if (!StmtBlock())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool FunctionDecl1()
        {            
            if (Match(TokenType.Token_void, true) && acertoToken)
            {
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisAbre, false))
                {
                    return false;
                }
                if (!Formals())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                if (!StmtBlock())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool TypeArray()
        {
            if (Match(TokenType.Operator_corchetes, true) && acertoToken)
            {
                if (!TypeArray())
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool Type()
        {
            if (ConstType(true))
            {
                return true;
            }
            if (Match(TokenType.Identifier, true) && acertoToken)
            {
                return true;
            }
            return false;
        }
        private bool ConstType(bool epsilon)
        {
            acertoToken = false;
            if (lookahead == TokenType.Token_boolean || lookahead == TokenType.Token_int || 
                lookahead == TokenType.Token_string || lookahead == TokenType.Token_double)
            {
                Dequeue();
                acertoToken = true;
                return true;
            }
            if (epsilon)
            {
                return true;
            }
            expectedValue = TokenType.Constant;
            return false;
        }
        private bool ConstDecl()
        {
            if (Match(TokenType.Token_static, true) && acertoToken)
            {
                if (!ConstType(false))
                {
                    return false;
                }
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }               
                return true;
            }
            return false;
        }
        private bool Variable()
        {
            if (Type())
            {
                if (!TypeArray())
                {
                    return false;
                }
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }                
                return true;
            }
            return false;
        }
        private bool VariableDecl()
        {
            if (Variable())
            {
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        bool DeclB = false;
        private bool Decl2()
        {
            if (Match(TokenType.Operator_puntoComa, true) && acertoToken)
            {
                if (!Decl1())
                {
                    return false;
                }
                return true;
            }
            if (FunctionDecl())
            {
                if (!Decl1())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool Decl1()
        {
            if (DeclB && !Decl())
            {
                return false;
            }
            if (DeclB && Decl())
            {
                return true;
            }
            if (!DeclB )
            {
                return true;
            }
            return true;            
        }
        private bool Decl()
        {
            DeclB = false;
            if (lookahead == TokenType.Token_class || lookahead == TokenType.Token_interface ||
                lookahead == TokenType.Constant || lookahead == TokenType.Token_void || lookahead == TokenType.Identifier||
                lookahead == TokenType.Token_int || lookahead == TokenType.Token_double || lookahead == TokenType.Token_boolean||
                lookahead == TokenType.Token_string)
            {
                DeclB = true;
            }                
            if (FunctionDecl1())
            {
                DeclB = true;
                if (!Decl1())
                {
                    return false;
                }
                return true;
            }
            if (ClassDecl())
            {
                DeclB = true;
                if (!Decl1())
                {
                    return false;
                }
                return true;
            }
            if (InterfaceDecl())
            {
                DeclB = true;
                if (!Decl1())
                {
                    return false;
                }
                return true;
            }
            if (Variable())
            {
                DeclB = true;                
                if (!Decl2())
                {
                    return false;
                }
                return true;
            }
            if (ConstDecl())
            {
                DeclB = true;
                if (!Decl1())
                {
                    return false;
                }
                return true;
            }                        
            return false;
        }

        //ANALIZADOR SEMANTICO
        private void addToSymbolTable(TokenType dataType, SymbolType symbolType, Token token)
        {
            //Evaluar declaracion repetida
            if (tablaSimbolos.All(x => x.scope== actualScope && x.ID != token.value))
            {
                Symbol newSymbol = new Symbol(token.value, this.actualScope, "0", dataType, symbolType);
                tablaSimbolos.Add(newSymbol);
            }
            else
            {
                result.addError(new ParserError(lookahead, "Identificador ya existe", actualLocation, ErrorType.semantico));
            }
        }
        private void updateValueInSymbolTable(Token token, string value)
        {
            //Evaluar si existe el simbolo
            if (tablaSimbolos.Any(x => (x.ID == token.value) && (x.scope == actualScope || x.scope < actualScope)))
            {
                Symbol actualSymbol = tablaSimbolos.FindLast(x => x.scope == actualScope && x.ID == token.value);
                actualSymbol.value = value;
            }
            else
            {
                result.addError(new ParserError(lookahead, "Identificador no declarado", actualLocation, ErrorType.semantico));
            }
        }
        private string evaluateExpression(string mathExpression)
        {
            try
            {
                mathExpression = "-100 > (5.3 - 2)";
                return new DataTable().Compute(mathExpression, null).ToString();
            }
            catch (Exception e)
            {
                result.addError(new ParserError(lookahead, "Error en operacion", actualLocation, ErrorType.semantico));
                return "Error";
            }
        }
        private string getValueFromSymbolTable(Token token)
        {
            //Evaluar si existe el simbolo
            if (tablaSimbolos.Any(x => (x.ID == token.value) && (x.scope == actualScope || x.scope < actualScope)))
            {
                Symbol actualSymbol = tablaSimbolos.FindLast(x => x.scope == actualScope && x.ID == token.value);
                return actualSymbol.value;
            }
            else
            {
                result.addError(new ParserError(lookahead, "Identificador no declarado", actualLocation, ErrorType.semantico));
                return "Error";
            }
        }
    }
}
