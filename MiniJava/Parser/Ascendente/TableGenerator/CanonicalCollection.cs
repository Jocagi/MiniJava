using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

        }
    }
}
