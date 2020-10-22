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
            int totalStates = 1;
            List<Go_To> nextStates = new List<Go_To> { getFirstState() };

            while (nextStates.Count > 0)
            {
                //Variables de resultado de este estado
                State thisState = new State(actualState);
                List<LRItem> lrItems = new List<LRItem>();

                //Obtener Goto's que apuntan al estado que vamos a analizar ahora
                List<Go_To> itemsToAnalyze = new List<Go_To>(
                            ( nextStates.FindAll(x => x.NextStateID == actualState) ));
                //Remover Goto's anteriores de la lista original
                nextStates.RemoveAll(x => x.NextStateID == actualState);

                //Analizr todos los correspondientes
                foreach (var item in itemsToAnalyze)
                {
                    //Aumentar posicion analizada
                    LRItem kernel = item.LRItem;
                    kernel.Position++;
                    
                    //Token actual
                    TokenType token = kernel.Production.RightSide[kernel.Position];

                    //Verificar si es un terminal o un No Terminal
                    if (grammar.isNotTerminal(token))
                    {
                        //Si es No terminal obtener todos los derivados
                        //Todo
                    }
                    else
                    {
                        //Si es terminal, generar GOTO
                        totalStates++;
                        nextStates.Add(new Go_To(actualState, token, totalStates, kernel));
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
