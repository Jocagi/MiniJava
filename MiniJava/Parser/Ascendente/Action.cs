using System;
using System.Collections.Generic;
using System.Text;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente
{
    public class Action
    {
        public TokenType symbol;
        public bool esIR_A;
        public bool esRetroceso;
        public bool esDesplazamiento;

        public Action()
        {

        }
    }
}
