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

        public Parser(Queue<Token> tokens)
        {
            this.tokens = tokens;
            this.result = new ParserReport();
            this.lookahead = TokenType.Default;
            this.acertoToken = false;
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
        private void ERROR(TokenType expected)
        {
            result.addError(new ParserError(lookahead, expected));
            lookahead = tokens.Count > 0 ? tokens.Dequeue().tokenType : TokenType.Default;
        } 

        private void PROGRAM() 
        {
            if ( !(DECL() && DECLPlus()) )
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
            acertoToken = false;
            if (MatchType(true) && acertoToken && Match(TokenType.Identifier, false)) //VariableDECL
            {
                acertoToken = false;
                if ( Match(TokenType.Operator_puntoComa,true) && acertoToken)
                {
                    acertoToken = false;
                    return true;
                }
                else 
                {
                    esFunction = true;
                }
            }
            if (esFunction || Match(TokenType.Token_void,false) ) //FunctionDECL
            {
                //Formals
                if (Match(TokenType.Operator_ParentesisAbre,true) && acertoToken)
                {
                    acertoToken = false;
                    TokenType[] comaTipoId = { TokenType.Operator_coma, TokenType.Data_Type, TokenType.Identifier };
                    if (MatchType(true) && acertoToken)
                    {
                        acertoToken = false;
                        if (!(Match(TokenType.Identifier, false) && Match_Several_Times(comaTipoId) && Match(TokenType.Operator_ParentesisCierra, false)))
                        {
                            return false;
                        }
                    }
                    else if (!Match(TokenType.Operator_ParentesisCierra, false))
                    {
                        return false;
                    }
                }
                else if (!Match(TokenType.Operator_parentesis,false))
                {
                    return false;
                }
                   
                acertoToken = false;
                
                
                //funtionStmt

                return true;

            }
            return false;

        }

        private bool DECLPlus()
        {
            if (DECL())
            {
                DECLPlus();
                return true;
            }
            else if (Match(TokenType.Epsilon,true))
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
    }
}
