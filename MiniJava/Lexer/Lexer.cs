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

            tokenDescriptions.Add(new TokenDescription(TokenType.Test, "^test"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Test, "^[0-9]*x[0-9]*"));

            tokenDescriptions.Add(new TokenDescription(TokenType.Enter, "^\n")); //No tocar, este sirve para las filas
            tokenDescriptions.Add(new TokenDescription(TokenType.WhiteSpace, "^(\r|\t|\v|\f|\a| )"));

            //PALABRAS RESERVADAS
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_break ,  "^break" ));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_boolean, "^boolean"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_class, "^class"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_const, "^const"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_double, "^double"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_else, "^else"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_extends, "^extends"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_for, "^for"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_if, "^if"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_implements, "^implements"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_int, "^int"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_interface, "^interface"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_New, "^new"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_null, "^null"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_out, "^out"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_printlnt, "^printInt"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_return, "^return"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_string, "^string"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_System, "^System"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_this, "^this"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_void, "^void"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_while, "^while"));

            tokenDescriptions.Add(new TokenDescription(TokenType.Token_Comparison, "^=|^<|^>|^>=|^<="));
            tokenDescriptions.Add(new TokenDescription(TokenType.Int, "^0|^[1-9][0-9]*"));
            
            //tokenDescriptions.Add(new TokenDescription(TokenType.Token_Id, "([a-z]|[A-Z]|$)([a-z]|[A-Z]|$|[0-9])*"));

            //tokenDescriptions.Add(new TokenDescription(TokenType.Token_Doubles, "[0-9]+[.[0-9]+]?"));
            
        }

        /// <summary>
        /// Obtiene un listado de tokens para el codigo fuente, segun la gramatica definida
        /// </summary>
        public List<Token> getTokens(string sourceCode) 
        {
            var tokens = new List<Token>();
            int tokenDescCount = tokenDescriptions.Count;

            string text = sourceCode;
            
            //El texto se va reduciendo cada vez que hay un match, 
            //el ciclo termina cuando ya no haya texto por analizar.
            while (text.Length > 0)
            {
                //Evaluar cada una de las expresiones regulares con el texto que queda
                for (int i = 0; i < tokenDescCount; i++)
                {
                    var item = tokenDescriptions[i];
                    var token = getMatch(text, item);

                    //Si se ha econtrado una coincidencia, agregarlo a la lista de tokens y seguir
                    if (token.match)
                    {
                        //Calcular posicion del token
                        int row = 1;
                        int col1 = 1;
                        int col2;

                        int tokensCount = tokens.Count;

                        if (tokensCount > 0)
                        {
                            row = tokens[tokensCount - 1].location.row;
                            col1 = tokens[tokensCount - 1].location.lastCol + 1;

                            //Nueva fila
                            if (tokens[tokensCount - 1].tokenType == TokenType.Enter)
                            {
                                row += 1;
                                col1 = 1;
                            }
                        }
                        col2 = col1 + token.value.Length - 1;

                        TokenLocation location = new TokenLocation(row, col1, col2);

                        //Agregar token
                        tokens.Add(new Token(token, location));
                        text = token.remainingText;

                        break;
                    }

                    //Si se han evaluado todas las expresiones regulares y no hay resultado, es un simbolo no valido
                    //Error
                    else if (i == tokenDescCount - 1)
                    {
                        //Concatenar varios errores seguidos
                        if (tokens.Count > 0 && tokens[tokens.Count - 1].tokenType == TokenType.Error)
                        {
                            tokens[tokens.Count - 1].value += text[0];
                            tokens[tokens.Count - 1].location.lastCol++;
                        }
                        //Agregar el error
                        else
                        {
                            //Calcular posicion del token
                            int row = 1;
                            int col1 = 1;
                            int col2;

                            int tokensCount = tokens.Count;

                            if (tokensCount > 0)
                            {
                                row = tokens[tokensCount - 1].location.row;
                                col1 = tokens[tokensCount - 1].location.lastCol + 1;

                                //Nueva fila
                                if (tokens[tokensCount - 1].tokenType == TokenType.Enter)
                                {
                                    row += 1;
                                    col1 = 1;
                                }
                            }
                            col2 = col1;

                            TokenLocation location = new TokenLocation(row, col1, col2);

                            tokens.Add(new Token(TokenType.Error, text[0].ToString(), location));
                        }

                        //Avanzar un elemento en el texto
                        text = text.Substring(1);
                    }
                }
            }

            return tokens;
        }

        /// <summary>
        /// Compara el texto contra una expresion regular
        /// </summary>
        private MatchRegex getMatch(string text, TokenDescription description)
        {
            Regex regex = description.regexDefinition;
            TokenType token = description.tokenType;
            
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
