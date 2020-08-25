using System;
using System.Collections.Generic;
using System.Text;

namespace MiniJava.Lexer
{
    //Todo agregar todos los tipos de tokens
    public enum TokenType
    {
        Test,
        Error,

        Token_Id,
        Token_Comparison,

        //PALABRAS RESERVADAS
        Token_void,        
        Token_int,         
        Token_double,      
        Token_boolean,     
        Token_string,      
        Token_class,       
        Token_const,       
        Token_interface,   
        Token_null,        
        Token_this,        
        Token_extends,     
        Token_implements,  
        Token_for,         
        Token_while,       
        Token_if,          
        Token_else,        
        Token_return,      
        Token_break,       
        Token_New,         
        Token_System,    
        Token_out,       
        Token_printlnt,  

        Int,
        WhiteSpace,
        Enter
        //Token_Comparison,
        //Token_ComentInBlock,
        //Token_Doubles,
        //Token_Id
    }
}
