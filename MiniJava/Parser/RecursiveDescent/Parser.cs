using MiniJava.Lexer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniJava.Parser.RecursiveDescent
{
    public class Parser
    {
        /// <summary>
        /// El analizador sitanctico recursivo recorre de forma recursiva la gramatica.
        /// Cada Simbolo NO TERMINAL es representado como una funcion y cada simbolo TERMINAL se evalua atraves del MATCH()
        /// El punto de partida es PROGRAM, si se hace backtracking hasta aqui se reporta el error.
        /// Se 'Concatenan' varias expresiones a traves de un AND para validar que todo este es el lugar correcto. Sin importar si es terminal o no terminal.
        /// </summary>

        private Queue<Token> tokens;
        private ParserReport result;
        private TokenType lookahead;
        private TokenType expectedValue;
        private bool acertoToken; // sirve para ver si  un token nullable cumplio o no
        private bool noMasSTMS; //no stms iniciado
        private bool LValue;

        public Parser(Queue<Token> tokens)
        {
            this.tokens = tokens;
            this.result = new ParserReport();
            this.lookahead = TokenType.Default;
            this.acertoToken = false;
            this.noMasSTMS = false;
            this.LValue = false;
        }

        public ParserReport getReport()
        {
            if (tokens.Count > 0)
            {
                lookahead = tokens.Dequeue().tokenType;
                //Start grammar path
                PROGRAM();
            }
            return result;
        }

        private bool Match(TokenType token, bool epsilon)// si epsilon es true, el token es nulable
        {
            acertoToken = false;
            if (lookahead == token)
            {
                lookahead = tokens.Count > 0 ? tokens.Dequeue().tokenType : TokenType.Default;
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
        private bool Match_Several_Times(TokenType[] tokensArray)//una o 0 veces lo toma como correcto
        {
            if (tokensArray.Length > 1 && lookahead == tokensArray[0])
            {
                foreach (var token in tokensArray)
                {
                    if (token == TokenType.Data_Type)
                    {
                        if (!MatchType(false))
                        {
                            return false;
                        }

                    }
                    else if (lookahead == token)
                    {
                        lookahead = tokens.Count > 0 ? tokens.Dequeue().tokenType : TokenType.Default;
                    }
                    else
                    {
                        return false;
                    }
                }
                Match_Several_Times(tokensArray);
                return true;
            }
            else if (lookahead == tokensArray[0])
            {
                lookahead = tokens.Count > 0 ? tokens.Dequeue().tokenType : TokenType.Default;
                Match_Several_Times(tokensArray);
                return true;
            }
            return true;
        }

        private bool MatchType(bool epsilon)
        {
            acertoToken = false;
            TokenType[] corchetes = { TokenType.Operator_corchetes };
            if (lookahead == TokenType.Token_boolean || lookahead == TokenType.Token_int || lookahead == TokenType.Token_string || lookahead == TokenType.Token_double)
            {
                lookahead = tokens.Count > 0 ? tokens.Dequeue().tokenType : TokenType.Default;
                acertoToken = true;
                Match_Several_Times(corchetes);
                return true;
            }
            if (epsilon)
            {
                return true;
            }
            expectedValue = TokenType.Data_Type;
            return false;
        }

        private bool MatchConstant(bool epsilon)
        {
            acertoToken = false;
            if (lookahead == TokenType.Const_Int|| lookahead == TokenType.Const_double || lookahead == TokenType.Const_bool|| lookahead == TokenType.Const_String || lookahead == TokenType.Token_null)
            {
                lookahead = tokens.Count > 0 ? tokens.Dequeue().tokenType : TokenType.Default;
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

        private bool MatchBoolSymbol(bool epsilon)
        {
            acertoToken = false;
            if (lookahead == TokenType.Operator_menor || lookahead == TokenType.Operator_menorIgual || lookahead == TokenType.Operator_mayor || lookahead == TokenType.Operator_mayorIgual)
            {
                lookahead = tokens.Count > 0 ? tokens.Dequeue().tokenType : TokenType.Default;
                acertoToken = true;
                return true;
            }
            if (epsilon)
            {
                return true;
            }
            expectedValue = TokenType.boolSymbol;
            return false;
        }

        private void ERROR(TokenType expected)
        {
            result.addError(new ParserError(lookahead, expected));
            lookahead = tokens.Count > 0 ? tokens.Dequeue().tokenType : TokenType.Default;
        }




        private void PROGRAM()
        {
            if (!(DECL() && DECLPlus()))
            {
                ERROR(expectedValue);
            }
            else if (tokens.Count > 0)
            {
                ERROR(expectedValue);
                PROGRAM();
            }
        }

        private bool DECL()
        {
            bool esFunction = false;
            //VariableDECL
            if (MatchType(true) && acertoToken && Match(TokenType.Identifier, false))
            {
                if (Match(TokenType.Operator_puntoComa, true) && acertoToken)
                {
                    return true;
                }
                else
                {
                    esFunction = true;
                }
            }
            //FunctionDECL
            if (esFunction || (Match(TokenType.Token_void, true) & Match(TokenType.Identifier, false)))
            {
                //Formals
                if (Match(TokenType.Operator_ParentesisAbre, true) && acertoToken)
                {
                    TokenType[] comaTipoId = { TokenType.Operator_coma, TokenType.Data_Type, TokenType.Identifier };
                    if (MatchType(true) && acertoToken)
                    {
                        if (!(Match(TokenType.Identifier, false) && Match_Several_Times(comaTipoId) && Match(TokenType.Operator_ParentesisCierra, false)))
                        {
                            acertoToken = false;
                            return false;
                        }
                    }
                    else if (!Match(TokenType.Operator_ParentesisCierra, false))
                    {
                        return false;
                    }
                }
                else if (!Match(TokenType.Operator_parentesis, true))
                {
                    return false;
                }
                noMasSTMS = false;
                while (!noMasSTMS)
                {
                    STMT(true);
                }
                return true;
            }
            return false;
        }
        private bool STMT(bool nullable)
        {
            //while stament 
            if (Match(TokenType.Token_while, true) && acertoToken)
            {
                noMasSTMS = false;//Entró al STMT
                if (!Match(TokenType.Operator_ParentesisAbre, false))
                {
                    return false;
                }
                if (!EXPR(false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                if (!STMT(false))
                {
                    return false;
                }
                return true;
            }
            //rerurn stament
            else if (Match(TokenType.Token_return, true) && acertoToken)
            {
                noMasSTMS = false;//Entró al STMT

                if (!EXPR(false))
                {
                    return false;
                }
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }
                return true;
            }
            //EXPR stament
            else if (EXPR(false))
            {
                noMasSTMS = false;//Entró al STMT
                if (!Match(TokenType.Operator_puntoComa, false))
                {
                    return false;
                }
                return true;
            }
            noMasSTMS = true;
            if (nullable)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool EXPR(bool nullable)
        {
            // LValue ExprP  
            bool lValue = false;
            bool eslValue = true;
            if (Match(TokenType.Identifier, true) && acertoToken)
            {
                if (Match(TokenType.Operator_punto, true) && acertoToken)
                {
                    if (!Match(TokenType.Identifier, false))
                    {
                        return false;
                    }
                }
                else if (Match(TokenType.Operator_corcheteAbre, true) && acertoToken)
                {
                    if (!EXPR(false))
                    {
                        return false;
                    }
                    if (!Match(TokenType.Operator_corcheteCierra, false))
                    {
                        return false;
                    }
                }
                lValue = true;
            }
            else if (Match(TokenType.Token_this, true) && acertoToken)
            {
                if (!Match(TokenType.Operator_punto, false))
                {
                    return false;
                }
                if (!Match(TokenType.Identifier, false))
                {
                    return false;
                }
                lValue = true;
            }
            //ExprP
            if (lValue)
            {
                
                Match(TokenType.Operator_puntosIgual, true);
                if (!acertoToken)
                {
                    Match(TokenType.Operator_igual, true);
                    if (!acertoToken)
                    {
                        eslValue = false;
                    }
                }
                if (eslValue)
                {
                    if (Match(TokenType.Token_New, true) && acertoToken)
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
                    if (!EXPR(false))
                    {
                        return false;
                    }
                    return true;
                }
            }
            //: Expr
            if (!lValue && Match(TokenType.Operator_dosPuntos, true)&& acertoToken)
            {
                if (!EXPR(false))
                {
                    return false;
                }
                return true;
            }
            //Operation
            if (lValue && !eslValue)
            {
                LValue = true;
            }
            if (OPERATION())
            {
                return true;
            }
            if (nullable)
            {
                return true;
            }
            return false;
        }


        private bool OPERATION()
        {
            //-OPERATION
            if (Match(TokenType.Operator_menos,true) && acertoToken )
            {
                if (!OPERATION())
                {
                    return false;
                }
                return true;
            }
            //(OPERATION)
            if (Match(TokenType.Operator_ParentesisAbre, true) && acertoToken)
            {
                if (!OPERATION())
                {
                    return false;
                }
                if (!Match(TokenType.Operator_ParentesisCierra, false))
                {
                    return false;
                }
                return true;
            }
            //OP1
            if (OP1())
            {
                return true;
            }
            return false;
        }
        private bool OPTerm()
        {
            //Constant
            if (MatchConstant(true) && acertoToken)
            {
                return true;
            }
            //lValue
            if (Match(TokenType.Identifier, true) && acertoToken)
            {
                if (Match(TokenType.Operator_punto, true) && acertoToken)
                {
                    if (!Match(TokenType.Identifier, false))
                    {
                        return false;
                    }
                }
                else if (Match(TokenType.Operator_corcheteAbre, true) && acertoToken)
                {
                    if (!EXPR(false))
                    {
                        return false;
                    }
                    if (!Match(TokenType.Operator_corcheteCierra, false))
                    {
                        return false;
                    }
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
            //(OPERATION)
            if (Match(TokenType.Operator_ParentesisAbre, true) && acertoToken)
            {
                if (!OPERATION())
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
        private bool OP1()
        {
            //OpTerm OP1_1
            if (LValue || OPTerm() )
            {
                LValue = false;
                if (!OP1_2())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool OP1_1()
        {
            // || OP1 
            if (Match(TokenType.Operator_dobleOr, true) && acertoToken)
            {
                if (!OP1())
                {
                    return false;
                }
                return true;
            }
            // && OP1 
            if (Match(TokenType.Operator_dobleAnd, true) && acertoToken)
            {
                if (!OP1())
                {
                    return false;
                }
                return true;
            }
            // == OP1
            if (Match(TokenType.Operator_comparacionIgual, true) && acertoToken)
            {
                if (!OP1())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool OP1_2()
        {
            //OP1_1
            if (OP1_1())
            {
                return true;
            }
            //OP2
            if (OP2())
            {
                return true;
            }
            return false;
        }
        private bool OP2()
        {
            //OpTerm BoolSymb OP2
            if (MatchBoolSymbol(false))
            { 
                if (!OP2_1())
                {
                    return false;
                }
                return true;
            }
            // OP3
            if (OP3())
            {
                return true;
            }
            return false;
        }
        private bool OP2_1()
        {
            if (OPTerm())
            {
                if (!OP2())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool OP3()
        {
            // OpTerm OP3_1
            if (OP3_1())
            {
                return true;
            }
            //OP4
            if (OP4())
            {
                return true;
            }
            return false;
        }
        private bool OP3_2()
        {
            if (OPTerm())
            {
                if (!OP3())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool OP3_1()
        {
            // * OP3 
            if (Match(TokenType.Operator_asterisco, true) && acertoToken)
            {
                if (!OP3())
                {
                    return false;
                }
                return true;
            }
            // / OP3
            if (Match(TokenType.Operator_div, true) && acertoToken)
            {
                if (!OP3())
                {
                    return false;
                }
                return true;
            }
            // % OP3
            if (Match(TokenType.Operator_porcentaje, true) && acertoToken)
            {
                if (!OP3())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool OP4()
        {
            if (OP4_1())
            {
                return true;
            }
            return false;
        }
        private bool OP4_1()
        {
            // + OP4
            if (Match(TokenType.Operator_mas, true)&& acertoToken)
            {
                if (!OPTerm())
                {
                    return false;
                }
                if (!OP4())
                {
                    return false;
                }
                return true;
            }
            // - OP4
            if (Match(TokenType.Operator_menos, true) && acertoToken)
            {
                if (!OPTerm())
                {
                    return false;
                }
                if (!OP4())
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool DECLPlus()
        {
            if (!(lookahead == TokenType.Default))
            {
                if (DECL())
                {
                    DECLPlus();
                    return true;
                }
            }
            
            else if (Match(TokenType.Epsilon,true))
            {
                return true;
            }
            
                return false;
            
        }
    }
}
