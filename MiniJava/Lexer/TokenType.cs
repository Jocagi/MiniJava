using System;
using System.Collections.Generic;
using System.Text;

namespace MiniJava.Lexer
{
    //Todo agregar todos los tipos de tokens
    public enum TokenType
    {
        //PALABRAS RESERVADAS
        Token_void,       //YA
        Token_int,        //YA
        Token_double,     //YA
        Token_boolean,    //YA
        Token_string,     //YA
        Token_class,      //YA
        Token_const,      //YA
        Token_interface,  //YA
        Token_null,       //YA
        Token_this,       //YA
        Token_extends,    //YA
        Token_implements, //YA
        Token_for,        //YA
        Token_while,      //YA
        Token_if,         //YA
        Token_else,       //YA
        Token_return,     //YA
        Token_break,      //YA
        Token_New,        //YA
        Token_System,     //YA
        Token_out,        //YA
        Token_printlnt,   //YA
        //IDENTIFICADORES
        Identifier,       //YA
        //CONSTANTES
        Const_Int,
        Const_bool,
        Const_double,
        Const_String,
        //OPERADORES
        Operator_mas,               //YA
        Operator_menos,             //YA
        Operator_asterisco,         //YA
        Operator_div,               //YA
        Operator_porcentaje,        //YA
        Operator_menor,             //YA
        Operator_menorIgual,        //YA
        Operator_mayor,             //YA
        Operator_mayorIgual,        //YA
        Operator_igual,             //YA
        Operator_comparacionIgual,  //YA
        Operator_diferente,         //YA
        Operator_dobleAnd,          //YA
        Operator_dobleOr,           //YA
        Operator_negacion,          //YA
        Operator_puntoComa,         //YA
        Operator_coma,              //YA
        Operator_punto,             //YA
        Operator_corcheteAbre,      //YA
        Operator_corcheteCierra,    //YA
        Operator_ParentesisAbre,    //YA
        Operator_ParentesisCierra,  //YA
        Operator_llaveAbre,         //YA
        Operator_llaveCierra,       //YA
        Operator_corchetes,         //YA
        Operator_parentesis,        //YA
        Operator_llaves,            //YA
        //COMENTARIOS
        Comments,
        Block_Comments,
        //
        Test,
        Error,

        WhiteSpace,
        Token_Comparison,
        Token_ComentInBlock,
        Token_Doubles

    }
}
