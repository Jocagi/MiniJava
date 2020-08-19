using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MiniJava.Lexer
{
    public class Lexer
    {
        List<TokenDescription> tokenDescriptions;
        public Lexer() 
        {
            tokenDescriptions = new List<TokenDescription>();

            //tokenDescriptions.Add(new TokenDescription(TokenType.Identifier,  "" ));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_Comparison, "^=|^<|^>|^>=|^<="));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_Int, "^0|^[1-9][0-9]*"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_Boolean, "^boolean"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_Id, "([a-z]|[A-Z]|$)([a-z]|[A-Z]|$|[0-9])*"));
            //tokenDescriptions.Add(new TokenDescription(TokenType.WhiteSpace, "^\\r|^\\n|^\\t|^\\v|^\\f"));
            //tokenDescriptions.Add(new TokenDescription(TokenType.Token_Doubles, "[0-9]+[.[0-9]+]?"));
            
        }

        /// <summary>
        /// Obtiene un listado de tokens para el codigo fuente, segun la gramatica definida
        /// </summary>
        public List<Token> getTokens(string sourceCode) 
        {
            var tokens = new List<Token>();
            string text = sourceCode;
            
            //El texto se va reduciendo cada vez que hay un match, 
            //el ciclo termina cuando ya no haya texto por analizar.
            while (text.Length > 0)
            {
                //Evaluar cada una de las expresiones regulares con el texto que queda
                foreach (var item in tokenDescriptions)
                {
                    var token = getMatch(text, item.regexDefinition, item.tokenType);

                    //Si se ha econtrado una coincidencia, agregarlo a la lista de tokens y seguir
                    if (token.match) 
                    {
                        tokens.Add(new Token(token));
                        text = token.remainingText;

                        break;
                    }
                }
            }

            return tokens;
        }

        /// <summary>
        /// Compara el texto contra una expresion regular
        /// </summary>
        private MatchRegex getMatch(string text, Regex regex, TokenType token)
        {
            var match = regex.Match(text);

            if (match.Success)
            {
                string remainingText = "";

                //Si aun queda texto por evaluar
                if (match.Length < text.Length)
                {
                    //Se selecciona como indice inicial, la parte en la que ya no hay match
                    remainingText = text.Substring(match.Length);
                }

                return new MatchRegex(value: match.Value, type: token, remainingText: remainingText);
            }
            else 
            {
                //No hay match
                return new MatchRegex(false);
            }
        }
    }
}
