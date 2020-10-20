using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MiniJava.Lexer;

namespace MiniJava.Parser.Ascendente.TableGenerator
{
    public class CanonicalCollection
    {
        private List<State> States  {get; set;}
        private Grammar grammar { get; set; }

        /// <summary>
        /// Crea la colección canónica a partir de la gramática suministrada
        /// </summary>
        public CanonicalCollection(Grammar grammar)
        {
            this.States = new List<State>();
            this.grammar = grammar;
            getCanonicalCollection();
        }

        private void  getCanonicalCollection()
        {
            int actualState = 0;
            List<Go_To> nextStates = new List<Go_To> { getFirstState() };

            while (nextStates.Count > 0)
            {
                //Variables de resultado de este estado
                State thisState = new State(actualState);
                List<LRItem> lrItems = new List<LRItem>();

                //Obtener Goto's que apuntan al estado que vamos a analizar ahora
                List<Go_To> itemsToAnalyze = new List<Go_To>(
                            ( nextStates.FindAll(x => x.NextStateID == actualState) ));
                //Remover items de la lista original
                nextStates.RemoveAll(x => x.NextStateID == actualState);

                foreach (var item in itemsToAnalyze)
                {
                    bool itemIsNotTerminal = true;

                    //Analizar cada kernel hasta llegar a un No terminal o que se terminen las posibles producciones
                    while (itemIsNotTerminal)
                    {
                        //Aumentar posicion a recorrer
                        LRItem closure = item.LRItem;
                        closure.Position++;

                        //lrItems.Add(  );
                    }
                }
            }
        }

        /// <summary>
        /// Primer estado a analizar
        /// </summary>
        private Go_To getFirstState()
        {
            return new Go_To(-1, TokenType.NT_Start, 0);
        }

        /// <summary>
        /// Al encontrar un NO TERMINAL, es necesario analizar nuevas producciones.
        /// Se devuelven todas las que correspondan.
        /// </summary>
        private List<LRItem> getFollowUpItems(TokenType NonTerminalToken)
        {
            List<LRItem> followUpItems = new List<LRItem>();

            //TODO

            return followUpItems;
        }
    }
}
