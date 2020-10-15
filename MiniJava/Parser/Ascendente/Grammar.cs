using System;
using System.Collections.Generic;
using System.Text;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente
{
    public class Grammar
    {
        public List<Production> Productions { get; set; }
        private List<TokenType> Terminals { get; set; }
        private List<TokenType> NotTerminals { get; set; }

        public Grammar()
        {
        }

        public bool isTerminal(TokenType token)
        {
            return Terminals.Contains(token);
        }

        public bool isNotTerminal(TokenType token)
        {
            return !Terminals.Contains(token);
        }

        private void setTerminals()
        {
            this.Terminals = new List<TokenType>
            {
                TokenType.Token_void,
                TokenType.Token_int,
                TokenType.Token_double,
                TokenType.Token_boolean,
                TokenType.Token_string,
                TokenType.Token_class,
                TokenType.Token_const,
                TokenType.Token_interface,
                TokenType.Token_null,
                TokenType.Token_this,
                TokenType.Token_extends,
                TokenType.Token_implements,
                TokenType.Token_for,
                TokenType.Token_while,
                TokenType.Token_Print,
                TokenType.Token_if,
                TokenType.Token_else,
                TokenType.Token_return,
                TokenType.Token_break,
                TokenType.Token_New,
                TokenType.Token_System,
                TokenType.Token_out,
                TokenType.Token_println,    
                //IDENTIFICADORES
                TokenType.Identifier,        
                //CONSTANTES
                TokenType.Const_Int,
                TokenType.Const_bool,
                TokenType.Const_double,
                TokenType.Const_String,      
                //OPERADORES
                TokenType.Operator_mas,
                TokenType.Operator_menos,
                TokenType.Operator_asterisco,
                TokenType.Operator_div,
                TokenType.Operator_porcentaje,
                TokenType.Operator_menor,
                TokenType.Operator_menorIgual,
                TokenType.Operator_mayor,
                TokenType.Operator_mayorIgual,
                TokenType.Operator_igual,
                TokenType.Operator_comparacionIgual,
                TokenType.Operator_diferente,
                TokenType.Operator_dobleAnd,
                TokenType.Operator_dobleOr,
                TokenType.Operator_negacion,
                TokenType.Operator_puntoComa,
                TokenType.Operator_coma,
                TokenType.Operator_punto,
                TokenType.Operator_corcheteAbre,
                TokenType.Operator_corcheteCierra,
                TokenType.Operator_ParentesisAbre,
                TokenType.Operator_ParentesisCierra,
                TokenType.Operator_llaveAbre,
                TokenType.Operator_llaveCierra,
                TokenType.Operator_corchetes,
                TokenType.Operator_parentesis,
                TokenType.Operator_llaves,
                TokenType.Operator_dosPuntos,
                TokenType.Operator_puntosIgual
            };
        }
    }
}
