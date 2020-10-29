using System.Collections.Generic;
using MiniJava.General;
using MiniJava.Parser.Ascendente.TableGenerator.Grammar;
using MiniJava.Parser.Ascendente.TableGenerator.LR1;

namespace MiniJava.Parser.Ascendente.Parser
{
    public class Table
    {
        public List<State> states = new List<State>();

        public Table()
        {

        }
        public Table(CanonicalCollection collection)
        {
            foreach (var state in collection.States)
            {
                List<Action> actions = new List<Action>();

                foreach (var item in state.items)
                {
                    if (item.action == ActionType.Shift && collection.grammar.isTerminal(item.Production.RightSide[item.Position]))
                    {
                        actions.Add(new Action(item.Production.RightSide[item.Position], item.action, item.shiftTo));
                    }
                    else if (item.action == ActionType.Shift && collection.grammar.isNotTerminal(item.Production.RightSide[item.Position]))
                    {
                        actions.Add(new Action(item.Production.RightSide[item.Position], ActionType.Ir_A, item.shiftTo));
                    }
                    else if (item.action == ActionType.Accept)
                    {
                        actions.Add(new Action(TokenType.EOF, ActionType.Accept, 0));
                    }
                    else if (item.action == ActionType.Reduce)
                    {
                        int position = collection.grammar.findProductionNumber(item.Production);

                        foreach (var lookahead in item.lookahead)
                        {
                            actions.Add(new Action(lookahead, ActionType.Reduce, position));
                        }
                    }
                }

                this.states.Add(new State(actions));
            }
        }
    }
}
