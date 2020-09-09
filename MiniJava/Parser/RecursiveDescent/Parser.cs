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

        public Parser(Queue<Token> tokens)
        {
            this.tokens = tokens;
            this.result = new ParserReport();
            this.lookahead = TokenType.Default;
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

        private bool Match(TokenType token)
        {
            if (lookahead == token)
            {
                lookahead = tokens.Count > 0 ? tokens.Dequeue().tokenType : TokenType.Default;
                return true;
            }
            else if (token == TokenType.Epsilon) 
            {
                return true;
            }

            expectedValue = token;
            return false;
        }
        private bool Match_Several_Times(TokenType token)//una o 0 veces lo toma como correcto
        {
            if (lookahead == token)
            {
                lookahead = tokens.Count > 0 ? tokens.Dequeue().tokenType : TokenType.Default;
                Match_Several_Times(token);
                return true;
            }
            return true;
        }

        private bool MatchType()
        {
            if (lookahead == TokenType.Token_boolean || lookahead == TokenType.Token_int || lookahead == TokenType.Token_string || lookahead == TokenType.Token_double)
            {
                lookahead = tokens.Count > 0 ? tokens.Dequeue().tokenType : TokenType.Default;
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
            bool firstTime = false;
            if (MatchType()) //VariableDECL
            {
                firstTime = true;
                if (Match_Several_Times(TokenType.Operator_corchetes) && Match(TokenType.Identifier) && Match(TokenType.Operator_puntoComa))
                {
                    return true;
                }
                else if (Match_Several_Times(TokenType.Operator_corchetes) && Match(TokenType.Identifier) && Match(TokenType.Operator_ParentesisAbre))
                {
                    firstTime = false;
                    esFunction = true;
                }
                else
                {
                    return false;
                }
            }
            if (!firstTime && (esFunction || (Match(TokenType.Token_void) && Match(TokenType.Operator_ParentesisAbre)))) //FunctionDECL
            {
                return true;
                //formals
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
            else if (Match(TokenType.Epsilon))
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
