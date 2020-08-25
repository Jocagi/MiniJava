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
        //
        Identifier,
        Int,
        WhiteSpace,
        Token_Comparison,
        Token_ComentInBlock,
        Token_Doubles,
        Token_Id
    }
}
