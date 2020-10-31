using System.Collections.Generic;

namespace MiniJava.Parser.Ascendente.Parser
{
    public class State
    {
        public List<Action> actions = new List<Action>();

        public State()
        {

        }
        public State(List<Action> actions)
        {
            this.actions = actions;
        }
    }
}
