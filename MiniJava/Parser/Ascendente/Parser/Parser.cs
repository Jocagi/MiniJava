﻿using System.Collections.Generic;
using MiniJava.General;
using MiniJava.Lexer;
using MiniJava.Parser.Ascendente.TableGenerator

namespace MiniJava.Parser.Ascendente.Parser
{
    public class Parser
    {
        Stack<int> Pila = new Stack<int>();
        Stack<Token> Simbolo = new Stack<Token>();
        Queue<Token> Entrada = new Queue<Token>();
        Table tabla = new Table();
        Grammar gramatica = new Grammar();
        public Parser(List<Token> tokens, Table tab, Grammar g)
        {
            gramatica = g;
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
            int Enfoque = 0; //booleano que me dice si debo evaluar Entrada o Símbolo
                        //0 SI ES A ENTRADA Y 1 SI ES A SÍMBOLO 
            while (!Error & Aceptado)
            {
                int numEstado = Pila.Peek();
                State estado = tabla.states[numEstado];
                int movEstado = 0;
                TokenType tokenEvalua = new TokenType();
                if (Enfoque == 0)
                {
                    tokenEvalua = Entrada.Peek().tokenType;
                }
                else 
                {
                    tokenEvalua = Simbolo.Peek().tokenType;
                }
                foreach (var item in estado.actions)
                {
                    if (item.symbol == tokenEvalua)
                    {
                        accion = item.accion;
                        movEstado = item.estado;
                    }
                }
                if (accion == ActionType.Accept)
                {
                    Aceptado = true;

                }
                else if (accion == ActionType.Reduce)
                {
                    foreach (var item in gramatica.Productions)
                    {
                        if (item.ID == movEstado)
                        {
                            for (int i = 0; i < item.RightSide.Count; i++)
                            {
                                Simbolo.Pop();
                                Pila.Pop();                                
                            }
                            Token tokenNuevo = new Token(item.LeftSide);
                            Simbolo.Push(tokenNuevo);
                        }
                    }
                    Enfoque = 1;
                }
                else if (accion == ActionType.Shift)
                {
                    Enfoque = 0;
                    Pila.Push(movEstado);
                    Simbolo.Push(Entrada.Dequeue());
                }
                else if (accion == ActionType.Ir_A)
                {
                    Enfoque = 0;
                    Pila.Push(movEstado);
                }
                else
                {
                    Error = true;
                } //ERROR

            }

        }
    }
}
