using System.Collections.Generic;
using MiniJava.Lexer;
using MiniJava.Parser.Ascendente.Parser;
using MiniJava.Parser.Ascendente.TableGenerator.Gramatica;
using MiniJava.Parser.Ascendente.TableGenerator.Grammar;

namespace MiniJava.Parser.Ascendente.TableGenerator.LR1
{
    public class CanonicalCollection
    {
        public List<State> States  {get; set;}
        private Grammar.Grammar grammar { get; set; }

        /// <summary>
        /// Crea la colección canónica a partir de la gramática suministrada
        /// </summary>
        public CanonicalCollection(Grammar.Grammar grammar)
        {
            this.States = new List<State>();
            this.grammar = grammar;
            getCanonicalCollection();
        }

        private void  getCanonicalCollection()
        {
            int actualState = 0;
            int totalStates = 0;
            // Variable de control para conocer los siguientes elementos a analizar
            List<Go_To> nextStates = new List<Go_To> { getFirstState() };
            // Recordar por donde han pasado los estados
            List<StatePointer> previouStates = new List<StatePointer>();

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
                        kernel.action = ActionType.Reduce;
                        lrItems.Add(kernel);
                    }
                    else
                    {
                        //Agregar al estado actual el primer elemento
                        LRItem nextLR = kernel.Copy();
                        nextLR.Position--;
                        lrItems.Add(nextLR);

                        //Token actual
                        TokenType token = kernel.Production.RightSide[kernel.Position - 1];

                        //Agregar goto del elemento actualmente analizado al siguiente estado
                        totalStates++;
                        nextStates.Add(new Go_To(actualState, token, totalStates, kernel));
                        gotoState.Add(token, totalStates);

                        //Verificar si es un terminal o un No Terminal
                        if (grammar.isNotTerminal(token))
                        {
                            //Si es No terminal obtener todos los derivados

                            //Obtener el first del elemento siguiente
                            List<TokenType>  firstNextItem = getFirstNextItem(kernel);
                            //Obtener derivados
                            List<LRItem> followUpItems = getFollowUpItems(token, firstNextItem, kernel.lookahead);

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

                                LRItem next = childItem.Copy();
                                next.Position++;

                                nextStates.Add(new Go_To(actualState, next.Production.LeftSide, nextState, next));
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
        private List<LRItem> getFollowUpItems(TokenType NonTerminalToken, List<TokenType> firstNextItem, List<TokenType> lookahead)
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
                    LRItem lritem;

                    //Agregar lookahead del elemento en cuestion
                    if (firstNextItem.Count > 0)
                    {
                        //Si existe un elemento a la derecha del simbolo, agregar first de ese elemento
                        lritem = new LRItem(item, 0, firstNextItem);
                    }
                    else
                    {
                        //Si no existe un elemento inmediatamente a la derecha del simbolo, agregar lookahead del padre
                        lritem = new LRItem(item, 0, lookahead);
                    }

                    //Agregar a lista de estados
                    followUpItems.Add(lritem);

                    //Si es un NO terminal, seguir obteniendo mas estados derivados
                    if (grammar.isNotTerminal(item.RightSide[0]) && 
                        !itemsAlreadyInState.Contains(item.RightSide[0]))
                    {
                        nextItems.Enqueue(item.RightSide[0]);
                        firstNextItem = getFirstNextItem(lritem);
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

        private List<TokenType> getFirstNextItem(LRItem item)
        {
            List<TokenType> result = new List<TokenType>();

            //Aumentar posicion analizada
            LRItem kernel = item.Copy();
            
            //Validar si no se ha llegado a la posicion final en la expresion
            if (kernel.Position < kernel.Production.RightSide.Count)
            {
                TokenType token = kernel.Production.RightSide[kernel.Position];
                if (grammar.isNotTerminal(token))
                {
                    result = grammar.first.Find
                        (x => x.tokenNT == kernel.Production.RightSide[kernel.Position])
                        ?.first;
                }
                else
                {
                    result.Add(token);
                }
            }

            return result;
        }
    }
}
