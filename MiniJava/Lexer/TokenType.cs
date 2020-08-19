using System;
using System.Collections.Generic;
using System.Text;

namespace MiniJava.Lexer
{
    //Todo agregar todos los tipos de tokens
    public enum TokenType
    {
        Identifier,
        Token_Int,
        Token_Boolean,
        WhiteSpace,
        Token_reservedWord,
        Token_Comparison,
        Token_ComentInBlock,
        Token_Doubles,
        Token_Id
    }
}
