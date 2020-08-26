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
        Token_if,          
        Token_else,        
        Token_return,      
        Token_break,       
        Token_New,         
        Token_System,      
        Token_out,         
        Token_printlnt,    
        //IDENTIFICADORES
        Identifier,        
        //CONSTANTES
        Const_Int,          //YA
        Const_bool,         //YA
        Const_double,
        Const_String,       //YA
        //OPERADORES
<<<<<<< HEAD
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
        //ESPACIOS
        WhiteSpace,
        JumpSpace,
        Tab,


=======
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
        //
>>>>>>> Branch-Jose
        Test,
        Error,
        Enter,

<<<<<<< HEAD
=======
        WhiteSpace,
        Token_Comparison,
        Token_ComentInBlock,
        Token_Doubles
>>>>>>> Branch-Jose
    }
}
