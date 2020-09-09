using System;
using System.Collections.Generic;
using System.Text;

namespace MiniJava.Lexer
{
    //Todo agregar todos los tipos de tokens
    public enum TokenType
    {
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
        Token_Print,
        Token_if,          
        Token_else,        
        Token_return,      
        Token_break,       
        Token_New,         
        Token_System,      
        Token_out,         
        Token_println,    
        //IDENTIFICADORES
        Identifier,        
        //CONSTANTES
        Const_Int,          //YA
        Const_bool,         //YA
        Const_double,
        Const_String,       //YA
        //OPERADORES
        Operator_mas,                
        Operator_menos,              
        Operator_asterisco,          
        Operator_div,                
        Operator_porcentaje,         
        Operator_menor,              
        Operator_menorIgual,         
        Operator_mayor,              
        Operator_mayorIgual,         
        Operator_igual,              
        Operator_comparacionIgual,   
        Operator_diferente,          
        Operator_dobleAnd,           
        Operator_dobleOr,            
        Operator_negacion,           
        Operator_puntoComa,          
        Operator_coma,               
        Operator_punto,              
        Operator_corcheteAbre,       
        Operator_corcheteCierra,     
        Operator_ParentesisAbre,     
        Operator_ParentesisCierra,   
        Operator_llaveAbre,          
        Operator_llaveCierra,        
        Operator_corchetes,          
        Operator_parentesis,         
        Operator_llaves,     

        //COMENTARIOS
        Comments,
        Block_Comments,        

        //SPECIAL TOKENS
        Error,
        Error_Comment,
        Error_String,
        Error_Length,
        Enter,
        WhiteSpace,
        Error_EOFComment,
        Error_EOFstring,
        Error_nullString,
        Error_null,
        Error_UnpairedComment,
        Epsilon,
        Default

        }
}
