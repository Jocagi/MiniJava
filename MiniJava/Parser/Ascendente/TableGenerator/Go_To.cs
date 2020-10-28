using System;
using System.Collections.Generic;
using System.Text;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.TableGenerator
{
    public class Go_To
    {
        public int StateID { get; set; }
        public int NextStateID { get; set; }
        public TokenType Token { get; set; }
        public LRItem LRItem { get; set; }

        public Go_To(int ID, TokenType token, int nextState, LRItem item)
        {
            this.StateID = ID;
            this.Token = token;
            this.NextStateID = nextState;
            this.LRItem = item;
        }

        public Go_To(int ID, TokenType token, Production prod, int nextState)
        {
            this.StateID = ID;
            this.Token = token;
            this.NextStateID = nextState;
            this.LRItem = new LRItem(prod,0);
        }

        public bool isEqual(Go_To _goto)
        {
            return this.Token == _goto.Token;
        }
    }
}
