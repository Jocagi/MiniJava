using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MiniJava.General;
using MiniJava.Lexer;
using MiniJava.Parser.Ascendente.Parser;

namespace MiniJava.Parser.Ascendente.TableGenerator.LR1
{
    public class CanonicalCollection
    {
        public List<State> States  { get; set; }
        public Grammar.Grammar grammar { get; set; }

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
                    
                    //Validar si se ha llegado a la posicion final en la expresion o si es epsilon
                    if (kernel.Position > kernel.Production.RightSide.Count || 
                        kernel.Production.RightSide[kernel.Position - 1] == TokenType.Epsilon)
                    {
                        kernel.action = kernel.Production.LeftSide == TokenType.NT_Start?
                            ActionType.Accept : ActionType.Reduce;
                        kernel.shiftTo = 0;
                        lrItems.Add(kernel);
                    }
                    else
                    {
                        //Token actual
                        TokenType token = kernel.Production.RightSide[kernel.Position - 1];
                        List<TokenType> lookAheadNextItem = getLookaheadNextItem(getFirstNextItem(kernel), kernel.lookahead);

                        //Verificar si se ha visto el simbolo anteriormente
                        if (previouStates.Any(x => 
                            x.symbol == token &&
                            x.isLookaheadEqual(lookAheadNextItem)))
                        {
                            //No hacer nada, solo indicar elemento econtrado
                            LRItem nextLR = kernel.Copy();
                            nextLR.Position--;
                            
                            var state = previouStates.Find(x =>
                                x.symbol == token &&
                                x.isLookaheadEqual(lookAheadNextItem))?.state;
                            if (state != null)
                                nextLR.shiftTo = (int)++totalStates; 

                            lrItems.Add(nextLR);

                            //Agregar goto del elemento actualmente analizado al siguiente estado
                            nextStates.Add(new Go_To(actualState, token, totalStates, kernel));
                            gotoState.Add(token, totalStates);

                            //Obtener el first del elemento siguiente
                            List<TokenType> firstNextItem = getFirstNextItem(kernel);
                            //Obtener derivados
                            List<LRItem> followUpItems = getFollowUpItems(token, firstNextItem, kernel.lookahead);

                            foreach (var childItem in followUpItems)
                            {
                                //Recuperar valor del simbolo anterior
                                var value = previouStates.Find(x =>
                                    x.isProductionEqual(childItem.Production)  &&
                                    x.isLookaheadEqual(childItem.lookahead))?.state;
                                if (value != null)
                                {
                                    childItem.shiftTo = (int) value;
                                }
                                else
                                {
                                    if (!gotoState.ContainsKey(childItem.Production.RightSide[0]))
                                    {
                                        childItem.shiftTo = ++totalStates;
                                        //Agregar goto del elemento actualmente analizado al siguiente estado
                                        nextStates.Add(new Go_To(actualState, childItem.Production.RightSide[0], totalStates, childItem));
                                        gotoState.Add(childItem.Production.RightSide[0], totalStates);
                                    }
                                }

                                lrItems.Add(childItem);
                            }
                        }
                        else
                        {
                            totalStates++;

                            //Agregar al estado actual el primer elemento
                            LRItem nextLR = kernel.Copy();
                            nextLR.Position--;
                            nextLR.shiftTo = totalStates;
                            lrItems.Add(nextLR);

                            //Agregar goto del elemento actualmente analizado al siguiente estado
                            nextStates.Add(new Go_To(actualState, token, totalStates, kernel));
                            gotoState.Add(token, totalStates);

                            //Verificar si es un terminal o un No Terminal
                            if (grammar.isNotTerminal(token))
                            {
                                previouStates.Add(new StatePointer(token, lookAheadNextItem, totalStates + 1, kernel.Production));

                                //Si es No terminal obtener todos los derivados

                                //Obtener el first del elemento siguiente
                                List<TokenType> firstNextItem = getFirstNextItem(kernel);
                                //Obtener derivados
                                List<LRItem> followUpItems = getFollowUpItems(token, firstNextItem, kernel.lookahead);

                                foreach (var childItem in followUpItems)
                                {
                                    List<TokenType> lookAheadNextChildItem = 
                                        getLookaheadNextItem(getFirstNextItem(childItem), childItem.lookahead);

                                    //Verificar si se ha visto el simbolo anteriormente
                                    if (previouStates.Any(x =>
                                        x.symbol == childItem.Production.RightSide[0] &&
                                        x.isLookaheadEqual(lookAheadNextChildItem)))
                                    {
                                        var state = previouStates.Find(x =>
                                            x.symbol == childItem.Production.RightSide[0] &&
                                            x.isLookaheadEqual(lookAheadNextChildItem))?.state;
                                        if (state != null)
                                            childItem.shiftTo = (int)state;

                                        lrItems.Add(childItem);
                                    }
                                    else
                                    {
                                        int nextState;

                                        if (childItem.action == ActionType.Shift)
                                        {
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

                                            LRItem next = childItem.Copy();
                                            next.Position++;

                                            nextStates.Add(new Go_To(actualState, next.Production.LeftSide, nextState, next));

                                            if (grammar.isNotTerminal(next.Production.LeftSide))
                                            {
                                                previouStates.Add(new StatePointer(next.Production.LeftSide, next.lookahead, nextState, next.Production));
                                            }

                                            childItem.shiftTo = nextState;
                                        }

                                        lrItems.Add(childItem);
                                    }
                                }
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

        private List<TokenType> getLookaheadNextItem(List<TokenType> firstNextItem, List<TokenType> lookaheadActualItem)
        {
            //Agregar lookahead del elemento en cuestion
            if (firstNextItem.Count > 0)
            {
                //Si el elemento a la derecha es nullable, combianr ambos
                if (firstNextItem.Contains(TokenType.Epsilon))
                {
                    List<TokenType> result = firstNextItem.Select(item => item).ToList();
                    result.RemoveAll(x => x == TokenType.Epsilon);

                    return result.Concat(lookaheadActualItem).ToList();
                }
                //Si existe un elemento a la derecha del simbolo, agregar first de ese elemento
                return firstNextItem;
            }
            else
            {
                //Si no existe un elemento inmediatamente a la derecha del simbolo, agregar lookahead del padre
                return lookaheadActualItem;
            }
        }

        /// <summary>
        /// Al encontrar un NO TERMINAL, es necesario analizar nuevas producciones.
        /// Se devuelven todas las que correspondan.
        /// </summary>
        private List<LRItem> getFollowUpItems(TokenType NonTerminalToken, List<TokenType> firstNextItem, List<TokenType> lookahead)
        {
            List<LRItem> followUpItems = new List<LRItem>();
            Queue<TokenLAPair> nextItems = new Queue<TokenLAPair>();
            List<TokenLAPair> itemsAlreadyInState = new List<TokenLAPair>();

            nextItems.Enqueue( new TokenLAPair(NonTerminalToken, getLookaheadNextItem(firstNextItem, lookahead)));

            while (nextItems.Count > 0)
            {
                List<LRItem> childProductions = 
                    getSingleFollowUpItem(nextItems.Dequeue());

                foreach (var lritem in childProductions)
                {
                    var item = lritem.Copy().Production;

                    if (item.RightSide[0] == TokenType.Epsilon)
                    {
                        lritem.action = ActionType.Reduce;
                        lritem.Position = 0;
                        followUpItems.Add(lritem);
                    }
                    else
                    {
                        //Agregar a lista de estados
                        followUpItems.Add(lritem);

                        //Si es un NO terminal, seguir obteniendo mas estados derivados
                        if (grammar.isNotTerminal(item.RightSide[0]))
                        {
                            LRItem copyLR = lritem.Copy();
                            copyLR.Position++; //obtener lookahead correcto

                            if (!itemsAlreadyInState.Any(x =>
                                x.token == item.RightSide[0] && x.isLookaheadEqual(copyLR.lookahead)))
                            {
                                nextItems.Enqueue(new TokenLAPair
                                    (item.RightSide[0], getLookaheadNextItem(getFirstNextItem(copyLR), copyLR.lookahead)));
                                itemsAlreadyInState.Add(new TokenLAPair(item.RightSide[0], copyLR.lookahead));
                            }
                        }
                    }
                }
            }
            return followUpItems;
        }

        private List<LRItem> getSingleFollowUpItem(TokenLAPair value)
        {
            List<LRItem> followUpItems = new List<LRItem>();

            foreach (var item in grammar.Productions)
            {
                if (item.LeftSide == value.token)
                {
                    followUpItems.Add(new LRItem(item, 0, value.lookahead));
                }
            }

            return followUpItems;
        }

        private List<TokenType> getFirstNextItem(LRItem item)
        {
            List<TokenType> result = new List<TokenType>();

            //Aumentar posicion analizada
            LRItem kernel = item.Copy();
            
            Inicio:
            //Validar si no se ha llegado a la posicion final en la expresion
            if (kernel.Position < kernel.Production.RightSide.Count)
            {
                TokenType token = kernel.Production.RightSide[kernel.Position];

                if (token != TokenType.Epsilon)
                {
                    if (grammar.isNotTerminal(token))
                    {
                        result = result.Concat( grammar.first.Find
                                (x => x.tokenNT == kernel.Production.RightSide[kernel.Position])
                            ?.first ?? throw new InvalidOperationException()).ToList();

                        if (result.Contains(TokenType.Epsilon))
                        {
                            result.RemoveAll(x => x == TokenType.Epsilon);
                            kernel.Position++;
                            goto Inicio;
                        }
                    }
                    else
                    {
                        result.Add(token);
                    }
                }
            }

            return result;
        }
    }
}