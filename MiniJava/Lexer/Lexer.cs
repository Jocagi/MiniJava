using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MiniJava.Lexer
{
    public class Lexer
    {
        private readonly List<TokenDescription> tokenDescriptions;
        
        public Lexer()
        {
            tokenDescriptions = new List<TokenDescription>
            {
                new TokenDescription(TokenType.Error_null, "^\0"),
                //Espacios en blanco
                new TokenDescription(TokenType.Enter, "^\n"),
                new TokenDescription(TokenType.WhiteSpace, "^(\r|\t|\b|\v|\f|\a| )"),
                //COMENTARIOS
                new TokenDescription(TokenType.Block_Comments, @"^(\/\*)((\*\/){0}|(.)|\n|\r|\0)*(\*\/){1}"),
                new TokenDescription(TokenType.Comments, @"^//(.|\0)*"),
                //ERRORES COMENTARIOS
                new TokenDescription(TokenType.Error_EOFComment, @"^/\*(.*)\Z"),
                new TokenDescription(TokenType.Error_UnpairedComment, @"^\*/"),
                new TokenDescription(TokenType.Error_Comment, @"^(\/\*)((.)|\n|\r)*"),
                //PALABRAS RESERVADAS
                new TokenDescription(TokenType.Token_break, @"^break(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_boolean, @"^boolean(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_class, @"^class(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_static, @"^static(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_double, @"^double(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_else, @"^else(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_extends, @"^extends(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_for, @"^for(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_if, @"^if(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_implements, @"^implements(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_int, @"^int(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_interface, @"^interface(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_New, @"^New(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_null, @"^null(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_out, @"^out(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_println, @"^println(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_Print, @"^Print(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_return, @"^return(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_string, @"^string(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_System, @"^System(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_this, @"^this(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_void, @"^void(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Token_while, @"^while(?![a-z]|[A-Z]|\$|[0-9])"),
                //OPERADORES
                new TokenDescription(TokenType.Operator_menorIgual, "^<="),
                new TokenDescription(TokenType.Operator_mayorIgual, "^>="),
                new TokenDescription(TokenType.Operator_comparacionIgual, "^=="),
                new TokenDescription(TokenType.Operator_diferente, "^!="),
                new TokenDescription(TokenType.Operator_mayor, "^>"),
                new TokenDescription(TokenType.Operator_menor, "^<"),
                new TokenDescription(TokenType.Operator_puntosIgual, "^:="),
                new TokenDescription(TokenType.Operator_dosPuntos, "^:"),
                new TokenDescription(TokenType.Operator_igual, "^="),
                new TokenDescription(TokenType.Operator_dobleAnd, "^&&"),
                new TokenDescription(TokenType.Operator_puntoComa, "^;"),
                new TokenDescription(TokenType.Operator_porcentaje, "^%"),
                new TokenDescription(TokenType.Operator_negacion, "^!"),
                new TokenDescription(TokenType.Operator_menos, "^-"),
                new TokenDescription(TokenType.Operator_coma, "^,"),
                new TokenDescription(TokenType.Operator_asterisco, @"^\*"),
                new TokenDescription(TokenType.Operator_div, "^/"),
                new TokenDescription(TokenType.Operator_mas, @"^\+"),
                new TokenDescription(TokenType.Operator_dobleOr, @"^\|\|"),
                new TokenDescription(TokenType.Operator_corchetes, @"^\[\]"),
                new TokenDescription(TokenType.Operator_corcheteAbre, @"^\["),
                new TokenDescription(TokenType.Operator_corcheteCierra, @"^\]"),
                new TokenDescription(TokenType.Operator_llaves, @"^\{\}"),
                new TokenDescription(TokenType.Operator_llaveAbre, @"^\{"),
                new TokenDescription(TokenType.Operator_llaveCierra, @"^\}"),
                new TokenDescription(TokenType.Operator_parentesis, @"^\(\)"),
                new TokenDescription(TokenType.Operator_ParentesisAbre, @"^\("),
                new TokenDescription(TokenType.Operator_ParentesisCierra, @"^\)"),
                new TokenDescription(TokenType.Operator_punto, @"^\."),
                new TokenDescription(TokenType.Error_nullString, "^\"((\0.*)|(.*\0)|(.*\0.*))\""),
                //CONSTANTES
                new TokenDescription(TokenType.Const_bool, @"(^true|^false)(?![a-z]|[A-Z]|\$|[0-9])"),
                new TokenDescription(TokenType.Const_double, @"^(([0-9]+)\.[0-9]*)(E(\+|-)?[0-9]+)?"),
                new TokenDescription(TokenType.Const_Int, "(^0x|^0X)([0-9]|[A-F]|[a-f])*"),
                new TokenDescription(TokenType.Const_Int, "^[0-9]+"), //decimales
                new TokenDescription(TokenType.Const_String, "^\"(.*?)\""),
                //ERRORES
                new TokenDescription(TokenType.Error_EOFstring, "^\"(\0|.)*" + @"\Z"),
                new TokenDescription(TokenType.Error_String, "^(\"){1}(.)(.(?!\"))*"),
                //IDENTIFICADORES
                new TokenDescription(TokenType.Identifier, @"^([a-z]|[A-Z]|\$)(([a-z]|[A-Z]|\$|[0-9])){0,30}")
            };
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


                        //ID with lenght > 31
                        if (token.tokenType == TokenType.Identifier && tokensCount > 0 &&
                            (tokens[tokensCount - 1].tokenType == TokenType.Identifier ||
                            tokens[tokensCount - 1].tokenType == TokenType.Error_Length))
                        {
                            token.tokenType = TokenType.Error_Length;
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
                            tokens[^1].value += text[0];
                            tokens[^1].location.lastCol++;
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

            tokens.RemoveAll(x => x.tokenType == TokenType.WhiteSpace);
            tokens.RemoveAll(x => x.tokenType == TokenType.Enter);
            tokens.RemoveAll(x => x.tokenType == TokenType.Comments);
            tokens.RemoveAll(x => x.tokenType == TokenType.Block_Comments);

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

        public Queue<Token> ListToQueue(List<Token> values) 
        {
            List<Token> tokens = new List<Token>(values);
            tokens.RemoveAll(x => 
                   x.tokenType == TokenType.WhiteSpace
                || x.tokenType == TokenType.Enter 
                || x.tokenType == TokenType.Comments 
                || x.tokenType == TokenType.Block_Comments 
                || x.tokenType == TokenType.Error 
                || x.tokenType == TokenType.Error_Comment 
                || x.tokenType == TokenType.Error_EOFComment 
                || x.tokenType == TokenType.Error_EOFstring 
                || x.tokenType == TokenType.Error_Length 
                || x.tokenType == TokenType.Error_null 
                || x.tokenType == TokenType.Error_nullString 
                || x.tokenType == TokenType.Error_String 
                || x.tokenType == TokenType.Error_UnpairedComment
                );

            return new Queue<Token>(tokens);
        }
    }
}
