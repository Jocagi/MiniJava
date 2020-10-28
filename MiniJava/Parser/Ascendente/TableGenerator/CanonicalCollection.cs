using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MiniJava.Lexer;
using MiniJava.Parser.Ascendente.Parser;

namespace MiniJava.Parser.Ascendente.TableGenerator
{
    public class CanonicalCollection
    {
        public List<State> States  {get; set;}
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
            int totalStates = 0;
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

                //Analizar todos los correspondientes
                foreach (var item in itemsToAnalyze)
                {
                    //Diccionario para validar a que estado dirigirse {Token, NoEstado}
                    Dictionary<TokenType, int> gotoState = new Dictionary<TokenType, int>();

                    //Aumentar posicion analizada
                    LRItem kernel = item.LRItem.Copy();
                    kernel.Position++;
                    
                    //Validar si se ha llegado a la posicion final en la expresion
                    if (kernel.Position > kernel.Production.RightSide.Count)
                    {
                        //Agregar al siguiente estado
                        kernel.action = ActionType.Reduce;
                        lrItems.Add(kernel);
                    }
                    else
                    {
                        lrItems.Add(kernel);

                        //Token actual
                        TokenType token = kernel.Production.RightSide[kernel.Position - 1];

                        //Agregar goto del elemento actualmente analizado
                        totalStates++;
                        nextStates.Add(new Go_To(actualState, token, totalStates, kernel));
                        gotoState.Add(token, totalStates);

                        //Verificar si es un terminal o un No Terminal
                        if (grammar.isNotTerminal(token))
                        {
                            //Si es No terminal obtener todos los derivados
                            List<LRItem> followUpItems = getFollowUpItems(token, kernel.lookahead);

                            foreach (var childItem in followUpItems)
                            {
                                int nextState;

                                if (gotoState.ContainsKey(childItem.Production.RightSide[0]))
                                {
                                    nextState = gotoState[childItem.Production.RightSide[0]];
                                }
                                else
                                {
                                    totalStates++;
                                    nextState = totalStates;
                                    gotoState.Add(childItem.Production.RightSide[0], nextState);
                                }

                                lrItems.Add(childItem);
                                nextStates.Add(new Go_To(actualState, childItem.Production.LeftSide, nextState, childItem));
                            }
                        }
                    }
                }

                actualState++;
                thisState.items = lrItems;
                this.States.Add(thisState);
            }
        }

        /// <summary>
        /// Primer estado a analizar
        /// </summary>
        private Go_To getFirstState()
        {
            return new Go_To(-1, TokenType.NT_Start, grammar.Productions[0], 0);
        }

        /// <summary>
        /// Al encontrar un NO TERMINAL, es necesario analizar nuevas producciones.
        /// Se devuelven todas las que correspondan.
        /// </summary>
        private List<LRItem> getFollowUpItems(TokenType NonTerminalToken, List<TokenType> lookahead)
        {
            List<LRItem> followUpItems = new List<LRItem>();
            Queue<TokenType> nextItems = new Queue<TokenType>();
            List<TokenType> itemsAlreadyInState = new List<TokenType>();

            nextItems.Enqueue(NonTerminalToken);

            while (nextItems.Count > 0)
            {
                List<Production> childProductions = getSingleFollowUpItem(nextItems.Dequeue());

                foreach (var item in childProductions)
                {
                    followUpItems.Add(new LRItem(item, 0, lookahead));

                    //Si es un NO terminal, seguir obteniendo mas estados derivados
                    if (grammar.isNotTerminal(item.RightSide[0]) && 
                        !itemsAlreadyInState.Contains(item.RightSide[0]))
                    {
                        nextItems.Enqueue(item.RightSide[0]);
                        itemsAlreadyInState.Add(item.RightSide[0]);
                    }
                }
            }

            return followUpItems;
        }

        private List<Production> getSingleFollowUpItem(TokenType NonTerminalToken)
        {
            List<Production> followUpItems = new List<Production>();

            foreach (var item in grammar.Productions)
            {
                if (item.LeftSide == NonTerminalToken)
                {
                    followUpItems.Add(item);
                }
            }

            return followUpItems;
        }

    }
}
