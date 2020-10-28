using System.Collections.Generic;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.Parser
{
    public class Parser
    {
        Stack<int> Pila = new Stack<int>();
        Stack<Token> Simbolo = new Stack<Token>();
        Queue<Token> Entrada = new Queue<Token>();
        Table tabla = new Table();
        public Parser(List<Token> tokens, Table tab)
        {
            foreach (var item in tokens)
            {
                Entrada.Enqueue(item);
            }
            Pila.Push(0);
            tabla = tab;
        }

        public Parser()
        {
            bool Error = false;
            bool Aceptado = false;
            ActionType accion = new ActionType();
            while (!Error & Aceptado)
            {
                int numEstado = Pila.Peek();
                State estado = tabla.states[numEstado];
                
                foreach (var item in estado.actions)
                {
                    if (item.symbol == Entrada.Peek().tokenType)
                    {
                        accion = item.accion;
                    }
                }
                if (accion == ActionType.Accept)
                {

                }
                else if (accion == ActionType.Reduce)
                {

                }
                else if (accion == ActionType.Shift)
                {

                }
                else if (accion == ActionType.Ir_A)
                {

                }
                else
                {
                    //error
                }

            }

        }
    }
}
