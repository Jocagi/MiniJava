using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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
        private Token lookaheadValue;
        private Token actualToken;
        private TokenType expectedValue;
        private TokenLocation actualLocation;
        private bool acertoToken; // sirve para ver si un token nullable cumplio o no
        private bool repetirDECLerror;
        //Variables Analizador semantico
        private List<List<Symbol>> tablas = new List<List<Symbol>>();
        private List<Symbol> tablaSimbolos = new List<Symbol>();
        private string mathOperation;
        private Stack<string> scopes = new Stack<string>();
        private string actualScope = "";
        private Token actualSymbol;
        private List<TokenType> actualParameters = new List<TokenType>();
        private TokenType actualDataType;
        private SymbolType actualSymbolType = SymbolType.variable;
        private string actualIdentifier = "";
        private string actualSymbolName = "";
        private bool mathExpressionFound = false;
        private bool inicioParentesis = false;
        private List<TokenType> tiposOperacion = new List<TokenType>();

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
                //Tablas de simbolos
                tablas.Add(tablaSimbolos);
                result.TablaSimbolos = tablas;
            }
            return result;
        }
        private void Dequeue()
        {
            if (tokens.Count > 0)
            {
                actualToken = lookaheadValue;
                Token t = tokens.Dequeue();
                lookahead = t.tokenType;
                actualLocation = t.location;
                lookaheadValue = t;
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
        private void ERROR(TokenType expected)
        {
            if ( lookahead != TokenType.Default )
            {
                result.addError(new ParserError(lookahead, expected, actualLocation));
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
            actualScope = "PROGRAM";
            scopes.Push(actualScope);
            if (!Decl())
            {
                ERROR(expectedValue);
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
                actualIdentifier = actualToken.value;
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
                actualIdentifier = actualToken.value;
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
                inicioParentesis = true;
                mathOperation += "(";
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
                mathOperation = "New";
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
                //Analisis Semantico
                if (lookahead == TokenType.Operator_igual)
                {
                    //Recordar el valor del lado izquierdo "x" (x = 1)
                    actualSymbolName = actualIdentifier;
                }
                else
                {
                    //Valor en operacion matematica (Semantico)
                    mathOperation += getMathValueFromToken(actualToken);
                }

                Dequeue();
                
                //Simbolo en operacion matematica (Semantico)
                switch (actualToken.tokenType)
                {
                    //Validar '='
                    case TokenType.Operator_igual:
                        break;
                    //Validar 'OR'
                    case TokenType.Operator_dobleOr:
                        mathOperation += " OR ";
                        break;
                    //Validar 'AND'
                    case TokenType.Operator_dobleAnd:
                        mathOperation += " AND ";
                        break;
                    //Validar '!='
                    case TokenType.Operator_diferente:
                        mathOperation += " <> ";
                        break;
                    default:
                        mathOperation += actualToken.value;
                        break;
                }

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
                mathOperation += lookahead == TokenType.Operator_negacion? " NOT " : "";
                mathOperation += lookahead == TokenType.Operator_menos ? "-" : "";

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
            mathExpressionFound = false;

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
                if (!mathExpressionFound)
                {
                    //Valor en operacion matematica (Semantico)
                    mathOperation += getMathValueFromToken(actualToken);
                    mathOperation += inicioParentesis? ")" : "";
                    string answer = evaluateExpression();
                    updateValueInSymbolTable(actualSymbolName, answer);
                    actualSymbolName = "";
                    mathExpressionFound = true;
                    inicioParentesis = false;
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
                entrostmt = true;
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
                entrostmt = true;
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
                entrostmt = true;
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
                actualScope = "for_" + Convert.ToString(scopes.LongCount());
                scopes.Push(actualScope);
                entrostmt = true;
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
                entrostmt = true;
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
            if (Match(TokenType.Token_else, true) && acertoToken)
            {
                entrostmt = true;
                if (!Stmt())
                {
                    ERROR(expectedValue);
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
                entrostmt = true;
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
        bool entrostmt;
        private bool Stmt()
        {
            entrostmt = false;
            if (Match(TokenType.Operator_puntoComa, true) & acertoToken)
            {
                entrostmt = true;
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
                entrostmt = true;
                if (!Stmt0())
                {
                    return false;
                }
                return true;
            }
            if (Expr())
            {
                entrostmt = true;
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
            if (entrostmt)
            {
                return false;
            }
            return true;
        }
        private bool StmtBlock1()
        {
            if (Type())
            {
                if (Operacion())
                {
                    if (!Expr())
                    {
                        return false;
                    }
                    return true;
                }
                if (VariableDecl())
                {
                    if (!StmtBlock1())
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }           
            return true;
        }
        private bool StmtBlock3()
        {
            ConstDecl();
            
            return true;
        }
        private bool StmtBlock()
        {
            if (Match(TokenType.Operator_llaveAbre, true) && acertoToken)
            {
                entrostmt = true;
                if (!StmtBlock1())
                {
                    return false;
                }
                if (!StmtBlock3())
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
            Token nombreFunc;
            Token tipo;
            if (Match(TokenType.Token_void, true) && acertoToken)
            {
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                nombreFunc = actualToken;
                if (!Match(TokenType.Operator_ParentesisAbre, false))
                {
                    return false;
                }
                if (!Formals())
                {
                    return false;
                }
                addToSymbolTableFunct(TokenType.Token_void, SymbolType.prototype, nombreFunc, actualParameters);
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
                tipo = actualToken;
                if (!TypeArray())
                {
                    return false;
                }
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                nombreFunc = actualToken;
                if (!Match(TokenType.Operator_ParentesisAbre, false))
                {
                    return false;
                }
                if (!Formals())
                {
                    return false;
                }
                addToSymbolTableFunct(tipo.tokenType, SymbolType.prototype , nombreFunc, actualParameters);
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
                addToSymbolTable(TokenType.Token_interface, SymbolType._interface, actualToken);
                actualScope = actualToken.value;
                scopes.Push(actualScope);
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
                scopes.Pop();
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
            if (Type())
            {
                typeF = actualToken;
                if (!Variable())
                {
                    return false;
                }
                
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
                addToSymbolTable(TokenType.Token_class, SymbolType.classe, actualToken);
                actualScope = actualToken.value;
                scopes.Push(actualScope);
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
                scopes.Pop();
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
            if (Type())
            {
                actualParameters.Add(actualToken.tokenType);
                actualSymbolType = SymbolType.parameter;
                if (!Variable())
                {
                    return false;
                }
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
            actualSymbolType = SymbolType.function;
            actualScope = nombreF.value;
            scopes.Push(actualScope);
            if (Match(TokenType.Operator_ParentesisAbre, true) && acertoToken)
            { 
                if (!Formals())
                {
                    return false;
                }
                Token No = new Token(TokenType.NT_FunctionDecl);
                No.value = actualScope;
                addToSymbolTableFunct(typeF.tokenType, SymbolType.function, No, actualParameters);
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                if (!StmtBlock())
                {
                    return false;
                }
                scopes.Pop();
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
                actualScope = actualToken.value;
                scopes.Push(actualScope);
                Token nombreFunc = actualToken;
                if (!Match(TokenType.Operator_ParentesisAbre, false))
                {
                    return false;
                }
                if (!Formals())
                {
                    return false;
                }
                nombreFunc.value = actualScope;
                addToSymbolTableFunct(TokenType.Token_void, SymbolType.function, nombreFunc, actualParameters);
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                if (!StmtBlock())
                {
                    return false;
                }
                scopes.Pop();
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
            if (Match(TokenType.Identifier, true) && acertoToken)
            {
                nombreF = actualToken;
                actualDataType = actualToken.tokenType;
                return true;
            }
            if (ConstType(false))
            {
                actualDataType = actualToken.tokenType;
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
                actualDataType = actualToken.tokenType;
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

                addToSymbolTable(actualDataType, SymbolType.constant, actualToken);
                actualSymbolType = SymbolType.variable;

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
            actualSymbolType = actualSymbolType == SymbolType.parameter? 
                SymbolType.parameter : SymbolType.variable;

            if (TypeArray())
            {
                if (!Match(TokenType.Identifier, false))
                {
                    actualSymbolType = SymbolType.variable;
                    return false;
                }
                nombreF = actualToken;
                if (lookahead != TokenType.Operator_ParentesisAbre)
                {
                    addToSymbolTable(actualDataType, actualSymbolType, actualToken);
                }                
                actualSymbolType = SymbolType.variable;
                return true;
            }
            actualSymbolType = SymbolType.variable;
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
        Token nombreF;
        Token typeF;
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
            DeclB =           (lookahead == TokenType.Token_class || lookahead == TokenType.Token_interface ||
                              lookahead == TokenType.Constant || lookahead == TokenType.Token_void || lookahead == TokenType.Identifier||
                              lookahead == TokenType.Token_int || lookahead == TokenType.Token_double || lookahead == TokenType.Token_boolean||
                              lookahead == TokenType.Token_string);
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
            if (Type())
            {
                typeF = actualToken;
                DeclB = true;
                if (!Variable())
                {
                    return false;
                }
                
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
            if (tablaSimbolos.Count == 0)
            {
                Symbol newSymbol = new Symbol(token.value, this.actualScope, "0", symbolType, dataType);
                newSymbol.scopesB = scopes;
                tablaSimbolos.Add(newSymbol);
                tablas.Add(new List<Symbol>(tablaSimbolos.ToArray()));
            }
            //Evaluar declaracion repetida
            else if (tablaSimbolos.All(x => x.ID != token.value))
            {
                Symbol newSymbol = new Symbol(token.value, this.actualScope, "0", symbolType, dataType);
                newSymbol.scopesB = scopes;
                tablaSimbolos.Add(newSymbol);
                tablas.Add(new List<Symbol>(tablaSimbolos.ToArray()));
            }
            else if (tablaSimbolos.All(x => scopes.Contains(actualScope) && x.ID != token.value))
            {
                Symbol newSymbol = new Symbol(token.value, this.actualScope, "0", symbolType, dataType);
                newSymbol.scopesB = scopes;
                tablaSimbolos.Add(newSymbol);
                tablas.Add(new List<Symbol>(tablaSimbolos.ToArray()));
            }
            else
            {
                result.addError(new ParserError(lookahead, "Identificador ya existe", actualLocation, ErrorType.semantico));
            }
        }
        private void addToSymbolTableFunct(TokenType dataType, SymbolType symbolType, Token token, List<TokenType> parameters)
        {
            //Evaluar declaracion repetida
            if (!tablaSimbolos.Any(x => x.scope == actualScope && x.ID == token.value))
            {
                scopes.Pop();
                Symbol newSymbol = new Symbol(token.value, scopes.Peek(), "0", dataType, symbolType,parameters);
                scopes.Push(actualScope);
                newSymbol.scopesB = scopes;
                tablaSimbolos.Add(newSymbol);
                actualParameters = new List<TokenType>();
                tablas.Add(new List<Symbol>(tablaSimbolos.ToArray()));
            }
            else
            {
                result.addError(new ParserError(lookahead, "Identificador ya existe", actualLocation, ErrorType.semantico));
            }
        }
        private void updateValueInSymbolTable(string token, string value)
        {
            if (token != "")
            {
                //Evaluar si existe el simbolo
                if (token.Contains('.'))
                {
                    token = token.Substring(0, token.IndexOf(".", StringComparison.Ordinal));
                }
                if (tablaSimbolos.Any(x => (x.ID == token)))
                {
                    List<string> actualScopes = scopes.ToList();
                    List<Symbol> symbols = tablaSimbolos.FindAll(x => (x.ID == token));

                    //Evaluar en cada ambito
                    for (int i = actualScopes.Count - 1; i >= 0; i--)
                    {
                        if (symbols.Any(x => x.scope == actualScopes[i]))
                        {
                            Symbol symbol = symbols.FindLast(x => x.scope == actualScopes[i]);
                            TokenType dataType = getDataType(value);

                            if (symbol.dataType == dataType)
                            {
                                symbol.value = value;
                            }
                            else if (symbol.dataType == TokenType.Token_double && dataType == TokenType.Token_int)
                            {
                                symbol.value = value;
                            }
                            else
                            {
                                result.addError(new ParserError(lookahead, $"No se puede convertir de {dataType} a {symbol.dataType}", actualLocation, ErrorType.semantico));
                                symbol.value = "Error";
                            }

                            tablas.Add(new List<Symbol>(tablaSimbolos.ToArray()));
                        }
                    }
                }
                else
                {
                    result.addError(new ParserError(lookahead, "Identificador no declarado", actualLocation, ErrorType.semantico));
                }
            }
        }
        private string evaluateExpression()
        {
            string answer = "";
            if (dataTypesAreCorrect())
            {
                try
                {
                    if (tiposOperacion.Count(x => x == TokenType.Const_String || x == TokenType.Token_string) != 0)
                    {
                        answer = $"{mathOperation}";
                    }
                    else
                    {
                        answer = new DataTable().Compute(mathOperation, null).ToString();
                    }
                }
                catch
                {
                    if (!mathOperation.Contains("New"))
                    {
                        result.addError(new ParserError(lookahead, "Error en operacion", actualLocation, ErrorType.semantico));
                        answer = "Error";
                    }
                    else
                    {
                        answer = "0";
                    }
                }
                mathOperation = "";
            }
            else
            {
                answer = "Error";
            }
            tiposOperacion = new List<TokenType>();
            return answer;
        }
        private bool dataTypesAreCorrect()
        {
            int strings = tiposOperacion.Count(x => x == TokenType.Const_String || x == TokenType.Token_string);
            int ints = tiposOperacion.Count(x => x == TokenType.Const_Int || x == TokenType.Token_int);
            int doubles = tiposOperacion.Count(x => x == TokenType.Const_double || x == TokenType.Token_double);

            if (strings > 0 && ints > 0)
            {
                result.addError(new ParserError(lookahead, "Error: No se puede operar 'int' y 'string'", actualLocation, ErrorType.semantico));
                return false;
            }
            else if (strings > 0 && doubles > 0)
            {
                result.addError(new ParserError(lookahead, "Error: No se puede operar 'double' y 'string'", actualLocation, ErrorType.semantico));
                return false;
            }
            return true;
        }
        private TokenType getDataType(string value)
        {
            string intRegex = "^[0-9]+";
            string doubleRegex = @"^(([0-9]+)\.[0-9]*)(E(\+|-)?[0-9]+)?";
            string boolRegex = @"(^True|^False)(?![a-z]|[A-Z]|\$|[0-9])";
            string stringRegex = "^\"(.*?)\"";

            Regex regex = new Regex(intRegex);
            var match = regex.Match(value);
            if (match.Success)
            {
                return TokenType.Token_int;
            }
            regex = new Regex(doubleRegex);
            match = regex.Match(value);
            if (match.Success)
            {
                return TokenType.Token_double;
            }
            regex = new Regex(boolRegex);
            match = regex.Match(value);
            if (match.Success)
            {
                return TokenType.Token_boolean;
            }
            regex = new Regex(stringRegex);
            match = regex.Match(value);
            if (match.Success)
            {
                return TokenType.Token_string;
            }

            return TokenType.Default;
        }
        private string getValueFromSymbolTable(Token token)
        {
            //Evaluar si existe el simbolo
            if (tablaSimbolos.Any(x => (x.ID == token.value)))
            {
                List<Symbol> symbols = tablaSimbolos.FindAll(x => (x.ID == token.value));
                List<string> actualScopes = scopes.ToList();

                for (int i = actualScopes.Count - 1; i >= 0; i--)
                {
                    if (symbols.Any(x => x.scope == actualScopes[i]))
                    {
                        Symbol actualSymbol = symbols.FindLast(x => x.scope == actualScopes[i]);
                        tiposOperacion.Add(actualSymbol.dataType);
                        return actualSymbol.value;
                    }       
                }

                result.addError(new ParserError(lookahead, "Identificador no accesible", actualLocation, ErrorType.semantico));
                return "Error";
            }
            else
            {
                result.addError(new ParserError(lookahead, "Identificador no declarado", actualLocation, ErrorType.semantico));
                return "Error";
            }
        }
        private string getMathValueFromToken(Token value)
        {
            string numericalValue; 

            if (mathOperation != "New")
            {
                if (value.tokenType == TokenType.Identifier)
                {
                    numericalValue = getValueFromSymbolTable(value);
                }
                else
                {
                    numericalValue = value.value;
                    tiposOperacion.Add(value.tokenType);
                }
            }
            else
            {
                mathOperation = "0";
                numericalValue = "";
            }

            return numericalValue;
        }
    }
}