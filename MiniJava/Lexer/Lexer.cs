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

            //Espacios en blanco
            tokenDescriptions.Add(new TokenDescription(TokenType.Enter, "^\n")); 
            tokenDescriptions.Add(new TokenDescription(TokenType.WhiteSpace, "^(\r|\t|\b|\v|\f|\a| )"));

            //COMENTARIOS
            tokenDescriptions.Add(new TokenDescription(TokenType.Block_Comments, @"^(\/\*)((\*\/){0}|(.)|\n|\r)*(\*\/){1}"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Comments, @"^//(.*)"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Error, @"^\*/"));

            tokenDescriptions.Add(new TokenDescription(TokenType.Error_Comment, @"^(\/\*)((.)|\n|\r)*"));

            //PALABRAS RESERVADAS
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_break, @"^break(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_boolean, @"^boolean(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_class, @"^class(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_const, @"^const(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_double, @"^double(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_else, @"^else(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_extends, @"^extends(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_for, @"^for(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_if, @"^if(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_implements, @"^implements(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_int, @"^int(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_interface, @"^interface(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_New, @"^new(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_null, @"^null(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_out, @"^out(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_println, @"^printIn(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_return, @"^return(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_string, @"^string(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_System, @"^System(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_this, @"^this(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_void, @"^void(?![a-z]|[A-Z]|\$|[0-9])"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Token_while, @"^while(?![a-z]|[A-Z]|\$|[0-9])"));
            
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
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_punto, @"^\."));

            //CONSTANTES
            tokenDescriptions.Add(new TokenDescription(TokenType.Const_bool, "^true|^false"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Const_double, @"(([0-9]+)\.[0-9]*)(E(\+|-)?[0-9]+)?"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Const_Int, "(^0x|^0X)([0-9]|[A-F]|[a-f])*"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Const_Int, "^[0-9]+"));//decimales
            tokenDescriptions.Add(new TokenDescription(TokenType.Const_String, "\"(.*?)\""));

            //IDENTIFICADORES
            tokenDescriptions.Add(new TokenDescription(TokenType.Identifier, @"^([a-z]|[A-Z]|\$)(([a-z]|[A-Z]|\$|[0-9])){1,31}ica"));
            tokenDescriptions.Add(new TokenDescription(TokenType.Operator_punto, @"^\."));

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
                        //Prevenir ciclos infinitos
                        if (token.value == string.Empty)
                        {
                            throw new Exception($"Se ha encontrado un problema con el regex {token.tokenType}");
                        }

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

                        //Comments
                        if (token.tokenType == TokenType.Block_Comments)
                        {
                            string[] lines = token.value.Split('\n');
                            int countlines =  lines.Length-1;

                            row += countlines;

                            if (countlines > 1)
                            {
                                col1 = 1;
                                col2 = lines[countlines].Length;
                            }
                        }

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
