﻿using System.Collections.Generic;
using MiniJava.General;
using MiniJava.Lexer;
using MiniJava.Parser.Ascendente.TableGenerator.Grammar;


namespace MiniJava.Parser.Ascendente.Parser
{
    public class Parser
    {
        Stack<int> Pila = new Stack<int>();
        Stack<Token> Simbolo = new Stack<Token>();
        Queue<Token> Entrada = new Queue<Token>();
        Table tabla = new Table();
        Grammar gramatica = new Grammar();
        public Parser(Queue<Token> tokens )
        {
            Token eof = new Token(TokenType.EOF);
           
            //gramatica = tab.grammar;
            Entrada = tokens;
            Entrada.Enqueue(eof);
            Pila.Push(0);
           // tabla = tab;
        }

        public List<string> Parserr()
        {
            List<string> errores = new List<string>();
            bool Error = false;
            bool Aceptado = false;
            ActionType accion = new ActionType();
            int Enfoque = 0; //booleano que me dice si debo evaluar Entrada o Símbolo
                             //0 SI ES A ENTRADA Y 1 SI ES A SÍMBOLO 
            bool clase = false;
            int opcion = 0;
            bool HacerPeek = true;
            while (!Error && !Aceptado && Entrada.Count > 0 && Pila.Count > 0)
            {
                int numEstado = Pila.Peek();
                State estado = tabla.states[numEstado];
                int movEstado = 0;
                TokenType tokenEvalua = new TokenType();
                if (Enfoque == 0 && HacerPeek)
                {
                    tokenEvalua = Entrada.Peek().tokenType;
                }
                else if (Enfoque == 1 && HacerPeek)
                {
                    tokenEvalua = Simbolo.Peek().tokenType;
                }
                HacerPeek = true;
                int cont = 0;
                #region Asigancion de opcion
                if (opcion != 3 && tokenEvalua == TokenType.Token_void)
                {
                    opcion = 1;
                }
                if (opcion == 4 && tokenEvalua == TokenType.Operator_ParentesisAbre)
                {
                    opcion = 1;
                }
                if ((opcion == 0 | opcion == 2) && (tokenEvalua == TokenType.Token_int | tokenEvalua == TokenType.Token_double | tokenEvalua == TokenType.Token_boolean | tokenEvalua == TokenType.Token_string | tokenEvalua == TokenType.Identifier))
                {
                    opcion = 4;
                }
                if ((opcion == 0 | opcion == 2) && tokenEvalua == TokenType.Token_interface)
                {
                    opcion = 3;
                }
                if (tokenEvalua == TokenType.Token_class)
                {
                    opcion = 2;
                    clase = true;
                }
                if (tokenEvalua == TokenType.Operator_llaveAbre && opcion == 1)
                {
                    opcion = 5;
                }
                if (tokenEvalua == TokenType.Token_static && (opcion == 0 | opcion == 2))
                {
                    opcion = 6;
                }

                #endregion
                List<Action> posibilidades = new List<Action>();
                foreach (var item in estado.actions)
                {
                    if (gramatica.isTerminal(item.symbol))
                    {
                        cont++;
                    }
                    if (item.symbol == tokenEvalua)
                    {
                        posibilidades.Add(item);
                    }
                }
                //ELEGIR EL MOVIMIENTO
                if (posibilidades.Count == 1)
                {
                    accion = posibilidades[0].accion;
                    movEstado = posibilidades[0].estado;
                }
                if (posibilidades.Count > 1)
                {
                    //accion = posibilidades[1].accion;
                    //movEstado = posibilidades[1].estado;
                    bool reduccion = false;
                    bool desplazamiento = false;
                    //bool irA = false;

                    //ELEGIR UNA OPCION
                    foreach (var item in posibilidades)
                    {
                        if (item.accion == ActionType.Reduce)
                        {
                            reduccion = true;
                        }
                        if (item.accion == ActionType.Shift)
                        {
                            desplazamiento = true;
                        }
                        if (item.accion == ActionType.Ir_A)
                        {
                            // irA = true;
                        }
                    }
                    //REDUCCION CON REDUCCION
                    if (reduccion & !desplazamiento)
                    {
                        accion = posibilidades[0].accion;
                        movEstado = posibilidades[0].estado;
                    }
                    // DESPLAZAMIENTO CON DESPLZAMIENTO
                    if (!reduccion & desplazamiento)
                    {
                        int prece = 0;
                        foreach (var item in posibilidades)
                        {
                            if (prece < item.precedencia)
                            {
                                prece = item.precedencia;
                                accion = item.accion;
                                movEstado = item.estado;
                            }
                        }
                    }
                    
                }
                //DESPUES DE ELEGIR EL MOVIMIENTO
                if (accion == ActionType.Accept)
                {
                    Aceptado = true;

                }
                else if (accion == ActionType.Reduce)
                {
                    var item1 = gramatica.findNumberProduction(movEstado);
                    if (!(item1.RightSide.Count == 1 && item1.RightSide[0] == TokenType.Epsilon))
                    {
                        for (int i = 0; i < item1.RightSide.Count; i++)
                        {
                            Simbolo.Pop();
                            Pila.Pop();
                        }
                    }
                    Token tokenNuevo = new Token(item1.LeftSide);
                    Simbolo.Push(tokenNuevo);

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
                //ERRORES :c
                else
                {

                    //Si el token que debia seguir se puede "adivinar"
                    if (cont == 1)
                    {
                        string error = "Falta token " + estado.actions[0].symbol.ToString() + " en la " + Entrada.Peek().location.row.ToString() + " fila y " + Entrada.Peek().location.firstCol.ToString() + " columna";
                        errores.Add(error);
                        Token tokenNuevo = new Token(estado.actions[0].symbol);
                        Simbolo.Push(tokenNuevo);
                        if (estado.actions[0].accion == ActionType.Accept)
                        {
                            Aceptado = true;

                        }
                        else if (estado.actions[0].accion == ActionType.Reduce)
                        {
                            var item1 = gramatica.findNumberProduction(movEstado);
                            if (!(item1.RightSide.Count == 1 && item1.RightSide[0] == TokenType.Epsilon))
                            {
                                for (int i = 0; i < item1.RightSide.Count; i++)
                                {
                                    Simbolo.Pop();
                                    Pila.Pop();
                                }
                            }
                            Token tokenNuevo1 = new Token(item1.LeftSide);
                            Simbolo.Push(tokenNuevo1);

                            Enfoque = 1;
                        }
                        else if (estado.actions[0].accion == ActionType.Shift)
                        {
                            Enfoque = 0;
                            Pila.Push(movEstado);
                            Simbolo.Push(Entrada.Dequeue());
                        }
                    }
                    else
                    {
                        //ERROR PRESENTE EN DECLARACION DE FUNCION
                        if (opcion == 1)
                        {
                            HacerPeek = false;
                            while (Entrada.Count > 0 && (tokenEvalua != TokenType.Token_void || tokenEvalua != TokenType.Token_class || tokenEvalua != TokenType.Token_interface || tokenEvalua != TokenType.Token_static || tokenEvalua != TokenType.Operator_llaveCierra || tokenEvalua != TokenType.Token_int || tokenEvalua != TokenType.Token_double || tokenEvalua != TokenType.Token_boolean || tokenEvalua != TokenType.Token_string || tokenEvalua != TokenType.Identifier))
                            {
                                Entrada.Dequeue();
                                tokenEvalua = Entrada.Peek().tokenType;
                                if (tokenEvalua == TokenType.Operator_llaveAbre)
                                {
                                    HacerPeek = true;
                                }
                            }
                            string error = "Error en declaracion de funcion  en la " + Entrada.Peek().location.row.ToString() + " fila y " + Entrada.Peek().location.firstCol.ToString() + " columna";
                            errores.Add(error);
                            Simbolo.Clear();
                            Pila.Clear();
                            Pila.Push(0);
                            opcion = 0;
                        }

                        //ERROR PRESENTE EN DECLARACION DE CLASE
                        if (opcion == 2)
                        {
                            while (Entrada.Count > 0 && (tokenEvalua != TokenType.Token_void || tokenEvalua != TokenType.Token_class || tokenEvalua != TokenType.Token_interface || tokenEvalua != TokenType.Token_static || tokenEvalua != TokenType.Operator_llaveCierra || tokenEvalua != TokenType.Token_int || tokenEvalua != TokenType.Token_double || tokenEvalua != TokenType.Token_boolean || tokenEvalua != TokenType.Token_string || tokenEvalua != TokenType.Identifier))
                            {
                                HacerPeek = false;
                                Entrada.Dequeue();
                                tokenEvalua = Entrada.Peek().tokenType;
                                if (tokenEvalua == TokenType.Operator_llaveAbre)
                                {
                                    HacerPeek = true;
                                    clase = false;
                                }
                            }
                            string error = "Error en declaracion de clase en la " + Entrada.Peek().location.row.ToString() + " fila y " + Entrada.Peek().location.firstCol.ToString() + " columna";
                            errores.Add(error);
                            Simbolo.Clear();
                            Pila.Clear();
                            Pila.Push(0);
                            opcion = 0;
                        }

                        //ERROR PRESENTE EN DECLARACION DE CLASE
                        if (tokenEvalua != TokenType.Operator_llaveAbre & clase == true)
                        {

                            string error = "Llave que cierra clase en la " + Entrada.Peek().location.row.ToString() + " fila y " + Entrada.Peek().location.firstCol.ToString() + " columna";
                            errores.Add(error);
                            Simbolo.Clear();
                            Pila.Clear();
                            Pila.Push(0);
                            opcion = 0;
                            clase = false;
                        }

                        //ERROR PRESENTE EN DECLARACION DE INTERFACE
                        if (opcion == 3)
                        {
                            while (Entrada.Count > 0 && (tokenEvalua != TokenType.Token_class || tokenEvalua != TokenType.Token_interface || tokenEvalua != TokenType.Token_static || tokenEvalua != TokenType.Operator_llaveCierra))
                            {
                                HacerPeek = false;
                                Entrada.Dequeue();
                                tokenEvalua = Entrada.Peek().tokenType;
                                if (tokenEvalua == TokenType.Operator_llaveAbre)
                                {
                                    HacerPeek = true;
                                    clase = false;
                                }
                            }
                            string error = "Error en declaracion de interface en la " + Entrada.Peek().location.row.ToString() + " fila y " + Entrada.Peek().location.firstCol.ToString() + " columna";
                            errores.Add(error);
                            Simbolo.Clear();
                            Pila.Clear();
                            Pila.Push(0);
                            opcion = 0;
                        }

                        //ERROR PRESENTE EN DECLARACION DE VARIABLE
                        if (opcion == 4)
                        {
                            HacerPeek = false;
                            while (Entrada.Count > 0 && (tokenEvalua != TokenType.Token_void || tokenEvalua != TokenType.Token_class || tokenEvalua != TokenType.Token_interface || tokenEvalua != TokenType.Token_static || tokenEvalua != TokenType.Operator_puntoComa || tokenEvalua != TokenType.Token_int || tokenEvalua != TokenType.Token_double || tokenEvalua != TokenType.Token_boolean || tokenEvalua != TokenType.Token_string || tokenEvalua != TokenType.Identifier))
                            {
                                Entrada.Dequeue();
                                tokenEvalua = Entrada.Peek().tokenType;
                                if (tokenEvalua == TokenType.Operator_puntoComa)
                                {
                                    HacerPeek = true;
                                }
                            }
                            string error = "Error en declaracion de variable  en la " + Entrada.Peek().location.row.ToString() + " fila y " + Entrada.Peek().location.firstCol.ToString() + " columna";
                            errores.Add(error);
                            Simbolo.Clear();
                            Pila.Clear();
                            Pila.Push(0);
                            opcion = 0;
                        }

                        //ERROR PRESENTE EN DECLARACION DE StamentBlock
                        if (opcion == 5)
                        {
                            HacerPeek = false;
                            while (Entrada.Count > 0 && (tokenEvalua != TokenType.Token_void || tokenEvalua != TokenType.Token_class || tokenEvalua != TokenType.Token_interface || tokenEvalua != TokenType.Token_static || tokenEvalua != TokenType.Operator_llaveCierra || tokenEvalua != TokenType.Token_int || tokenEvalua != TokenType.Token_double || tokenEvalua != TokenType.Token_boolean || tokenEvalua != TokenType.Token_string || tokenEvalua != TokenType.Identifier))
                            {
                                Entrada.Dequeue();
                                tokenEvalua = Entrada.Peek().tokenType;
                                if (tokenEvalua == TokenType.Operator_llaveCierra)
                                {
                                    HacerPeek = true;
                                }
                            }
                            string error = "Error en declaracion de un bloque de declaracion en la " + Entrada.Peek().location.row.ToString() + " fila y " + Entrada.Peek().location.firstCol.ToString() + " columna";
                            errores.Add(error);
                            Simbolo.Clear();
                            Pila.Clear();
                            Pila.Push(0);
                            opcion = 0;
                        }

                        //ERROR PRESENTE EN DECLARACION DE CONSTANTE
                        if (opcion == 6)
                        {
                            HacerPeek = false;
                            while (Entrada.Count > 0 && (tokenEvalua != TokenType.Token_class || tokenEvalua != TokenType.Token_interface || tokenEvalua != TokenType.Token_static || tokenEvalua != TokenType.Operator_puntoComa))
                            {
                                Entrada.Dequeue();
                                tokenEvalua = Entrada.Peek().tokenType;
                                if (tokenEvalua == TokenType.Operator_puntoComa)
                                {
                                    HacerPeek = true;
                                }
                            }
                            string error = "Error en declaracion de una constante de declaracion en la " + Entrada.Peek().location.row.ToString() + " fila y " + Entrada.Peek().location.firstCol.ToString() + " columna";
                            errores.Add(error);
                            Simbolo.Clear();
                            Pila.Clear();
                            Pila.Push(0);
                            opcion = 0;
                        }

                        //Error de simbolo no adecuado al inicio de un DECL
                        if (opcion == 0)
                        {
                            string error = "Error inicio de declaracion incorrecto en " + Entrada.Peek().location.row.ToString() + " fila y " + Entrada.Peek().location.firstCol.ToString() + " columna";
                            errores.Add(error);
                            Simbolo.Clear();
                            Pila.Clear();
                            Pila.Push(0);
                            opcion = 0;
                        }

                        if (Entrada.Count == 0)
                        {
                            Error = true;
                        }
                    }
                } //ERROR
            
            }
            if (!Aceptado)


            { 
                errores.Add("error");
            }

            return errores;
        }
    }
}
