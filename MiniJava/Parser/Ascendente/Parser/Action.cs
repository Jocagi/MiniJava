using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.Parser
{
    public class Action
    {
        public TokenType symbol;
        public ActionType accion;
        public int estado;
        public int precedencia;
        public int asociatividad;
        public Action()
        {

        }
    }
}
