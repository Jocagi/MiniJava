using MiniJava.Lexer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniJava.Parser.RecursiveDescent
{
    public class Parser
    {
        /// <summary>
        /// El analizador sitanctico recursivo recorred e forma recursiva la gramatica.
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
            if (true) //VariableDECL
            {
                //Test -> reconoce int ID = <num>;

                if (Match(TokenType.Token_int) && Match(TokenType.Identifier)
                    && Match(TokenType.Operator_igual) && Match(TokenType.Const_Int)
                    && Match(TokenType.Operator_puntoComa))
                {
                    return true;
                }
                else if (Match(TokenType.Token_double) && Match(TokenType.Identifier)
                    && Match(TokenType.Operator_igual) && Match(TokenType.Const_double)
                    && Match(TokenType.Operator_puntoComa))
                {
                    return true;
                }

                return false;
            }
            else if (true) //FunctionDECL
            {

            }
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
