using System;
using System.Collections.Generic;
using System.Text;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.TableGenerator
{
    public class State
    {
        public int ID { get; set; }
        public List<LRItem> items { get; set; }

        public State(int id)
        {
            this.ID = id;
            this.items = new List<LRItem>();
        }
    }
}
