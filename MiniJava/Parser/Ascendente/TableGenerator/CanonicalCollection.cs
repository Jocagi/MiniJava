using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.TableGenerator
{
    public class CanonicalCollection
    {
        private List<State> States = new List<State>();
        private Grammar grammar { get; set; }

        /// <summary>
        /// Crea la colección canónica a partir de la gramática suministrada
        /// </summary>
        public CanonicalCollection(Grammar grammar)
        {
            this.grammar = grammar;
            getCanonicalCollection();
        }

        private void  getCanonicalCollection()
        {
            int actualState = 0;
            List<Go_To> nextStates = new List<Go_To>();

            nextStates.Add(getFirstState());

            while (nextStates.Count > 0)
            {
                

            }
        }

        private Go_To getFirstState()
        {
            return new Go_To(-1, TokenType.NT_Start, 0);
        }
    }
}
