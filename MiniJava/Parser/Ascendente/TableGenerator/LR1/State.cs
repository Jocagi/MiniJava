using System.Collections.Generic;

namespace MiniJava.Parser.Ascendente.TableGenerator.LR1
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
