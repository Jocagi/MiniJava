using MiniJava.General;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.Parser
{
    public class Action
    {
        public TokenType symbol;
        public ActionType accion;
        public int estado;
        
        public Action()
        {

        }

        public Action(TokenType symbol, ActionType action, int state)
        {
            this.accion = action;
            this.symbol = symbol;
            this.estado = state;
        }
    }
}
