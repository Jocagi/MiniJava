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
            //PALABRAS RESERVADAS
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_break, "^break"));
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
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_New, "^New"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_null, "^null"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_out, "^out"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_printlnt, "^printInt"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_return, "^return"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_string, "^string"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_System, "^System"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_this, "^this"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_void, "^void"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_while, "^while"));
            //OPERADORES
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_menorIgual, "^<="));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_mayorIgual, "^>="));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_comparacionIgual, "^=="));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_diferente, "^!="));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_mayor, "^>"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_menor, "^<"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_igual, "^="));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_dobleAnd, "^&&"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_puntoComa, "^;"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_porcentaje, "^%"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_negacion, "^!"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_menos, "^-"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_coma, "^,"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_asterisco, @"^\*"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_div, "^/"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_mas, @"^\+"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_dobleOr, @"^\|\|"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_corchetes, @"^\[\]"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_corcheteAbre, @"^\["));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_corcheteCierra, @"^\]"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_llaves, @"^\{\}"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_llaveAbre, @"^\{"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_llaveCierra, @"^\}"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_parentesis, @"^\(\)"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_ParentesisAbre, @"^\("));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_ParentesisCierra, @"^\)"));
            //ESPACIOS
            tokenDescriptions.Add(new TokenDescription(TokenType.WhiteSpace, @" "));
            //CONSTANTES
            tokenDescriptions.Add(new TokenDescription(TokenType.Const_bool, "^true|^false"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Const_Int, "^0|^[1-9][0-9]*"));//decimales
            tokenDescriptions.Add(new TokenDescription(TokenType.Const_Int, "(^0x|^0X)([0-9]|[A-F][a-f])*"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Test, "^test"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Test, "^[0-9]*x[0-9]*"));


            //IDENTIFICADORES
            tokenDescriptions.Add(new TokenDescription(TokenType.Identifier, "([a-z]|[A-Z]|$)([a-z]|[A-Z]|$|[0-9])*"));

            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_punto, "^."));
        }

        /// <summary>
        /// Obtiene un listado de tokens para el codigo fuente, segun la gramatica definida
        /// </summary>
        public List<Token> getTokens(string sourceCode) 
        {
            var tokens = new List<Token>();
            string text = sourceCode;
            int tokenDescCount = tokenDescriptions.Count;

            //El texto se va reduciendo cada vez que hay un match, 
            //el ciclo termina cuando ya no haya texto por analizar.
            while (text.Length > 0)
            {
                //Evaluar cada una de las expresiones regulares con el texto que queda
                for (int i = 0; i < tokenDescCount; i++)
                {
                    var item = tokenDescriptions[i];
                    var token = getMatch(text, item.regexDefinition, item.tokenType);

                    //Si se ha econtrado una coincidencia, agregarlo a la lista de tokens y seguir
                    if (token.match)
                    {
                        tokens.Add(new Token(token));
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
                        }
                        //Agregar el error
                        else
                        {
                            tokens.Add(new Token(TokenType.Error, text[0].ToString()));
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
