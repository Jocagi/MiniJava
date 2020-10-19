using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.Parser
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
