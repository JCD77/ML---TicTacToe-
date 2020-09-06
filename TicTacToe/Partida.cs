using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TicTacToe
{



    public class Partida
    {
        public Ficha[,] Tablero { get; set; }
        public Progreso Estado { get; set; }
        public Turno Turno { get; set; }

        public ILevel Jugador1 { get; set; }
        public ILevel Jugador2 { get; set; }

        public Byte Rango { get; set; }

        public int Movimiento { get; set; }
        public List<string> Movimientos { get; set; }




        public Partida()
        {
            this.Estado = Progreso.INICIO;
            this.Turno = Turno.JUGADOR1;
            this.Movimiento = 0;
            this.Movimientos = new List<string>();

        }

        public Partida(byte rango , ILevel jugador1, ILevel jugador2) : base()
        {

            this.Tablero = new Ficha[rango, rango];
            this.Jugador1 = jugador1;
            this.Jugador2 = jugador2;
            this.Movimientos = new List<string>();
            this.Rango = rango;


        }

        internal Ficha Valor(int filaRandom, int columnaRandom)
        {
            if (filaRandom >= this.Rango || columnaRandom >= this.Rango || filaRandom < 0 || columnaRandom < 0) return Ficha.NINGUNA;
            return this.Tablero[filaRandom, columnaRandom];
        }



        public void Pintar()
        {
            for (int fila = 0; fila < this.Tablero.GetLength(0); fila++)
            {


                for (int columna = 0; columna < this.Tablero.GetLength(1); columna++)
                {

                    System.Console.Write("#{0}#", this.Tablero[fila, columna].ToString());
                }
                System.Console.WriteLine();
            }

            System.Console.WriteLine(); System.Console.WriteLine("");
        }

        public Turno? Ganador()
        {
            Turno? turno = null;
            bool ganador = false;

            for (int fila = 0; fila < this.Rango; fila++)
            {

                for (int columna = 0; columna < this.Rango; columna++)
                {
                    //HORIZONTAL
                    Ficha ficha = this.Valor(fila, columna);
                    if (ficha != Ficha.NINGUNA)
                    {
                        ganador = true;

                        for (int i = 0; i < this.Rango; i++)
                        {
                            if (this.Valor(fila, columna + i) != ficha)
                            {
                                ganador = false;
                                break;
                            }




                        }
                        if (ganador)
                        {
                            if (ficha == Ficha.JUGADOR2) return Turno.JUGADOR2;
                            else return Turno.JUGADOR1;

                        }

                        //VERTICAL
                        ganador = true;
                        for (int i = 0; i < this.Rango; i++)
                        {
                            if (this.Valor(fila + i, columna) != ficha)
                            {
                                ganador = false;
                                break;
                            }




                        }

                        if (ganador)
                        {
                            if (ficha == Ficha.JUGADOR2) return Turno.JUGADOR2;
                            else return Turno.JUGADOR1;

                        }



                        //DIAGONAL 1
                        ganador = true;
                        for (int i = 0; i < this.Rango; i++)
                        {
                            if (this.Valor(fila + i, columna + i) != ficha)
                            {
                                ganador = false;
                                break;
                            }




                        }

                        if (ganador)
                        {
                            if (ficha == Ficha.JUGADOR2) return Turno.JUGADOR2;
                            else return Turno.JUGADOR1;

                        }


                        //DIAGONAL 2
                        ganador = true;
                        for (int i = 0; i < this.Rango; i++)
                        {
                            if (this.Valor(fila - i, columna + i) != ficha)
                            {
                                ganador = false;
                                break;
                            }




                        }

                        if (ganador)
                        {
                            if (ficha == Ficha.JUGADOR2) return Turno.JUGADOR2;
                            else return Turno.JUGADOR1;

                        }


                    }
                }



            }
            return turno;
        }

        public Partida Jugar(bool pintar = true)
        {
            Turno? ganador;




            do
            {
                //turno 
                if (this.Turno == Turno.JUGADOR1) this.Jugador1.SiguienteMovimiento(this);
                else
                    this.Jugador2.SiguienteMovimiento(this);
                string movimiento = ModeloAprendizaje.Tablero2String(this);
                this.Movimientos.Add(movimiento);
              if (pintar)  this.Pintar();
                //comprobar ganador
                this.Movimiento++;
                ganador = Ganador();


            } while (this.Movimiento < (this.Rango * this.Rango) && ganador == null);

            if (ganador != null)
            {
                if (pintar)System.Console.WriteLine("Ganador: {0}", ganador);
                this.Estado = (ganador == Turno.JUGADOR2) ? Progreso.JUGADOR1GANA : Progreso.JUGADOR2GANA;
            }
            else if (pintar) System.Console.WriteLine("TABLAS", ganador);



          
            return this;
        }




        //Siempre ponemos que mueve el jugador 1
        public List<Ficha[,]> PosicionesSiguientes(Turno turno)
        {
            List<Ficha[,]> posibilidades = new List<Ficha[,]>();



            for (int fila = 0; fila < this.Tablero.GetLength(0); fila++)
            {


                for (int columna = 0; columna < this.Tablero.GetLength(1); columna++)
                {

                    if (this.Tablero[fila, columna] == Ficha.NINGUNA)
                    {
                        Ficha[,] clone = (Ficha[,])Tablero.Clone();
                        clone[fila, columna] =turno==Turno.JUGADOR1? Ficha.JUGADOR1:Ficha.JUGADOR2;
                        posibilidades.Add(clone);
                    }
                }

            }

            return posibilidades;

        }

        public Ficha[,] FlipTablero(Ficha[,] tablero)
        {
            Ficha[,] clone = (Ficha[,])tablero.Clone();



            for (int fila = 0; fila < tablero.GetLength(0); fila++)
            {


                for (int columna = 0; columna < tablero.GetLength(1); columna++)
                {

                    if (tablero[fila, columna] != Ficha.NINGUNA)
                    {

                        clone[fila, columna] = (clone[fila, columna] == Ficha.JUGADOR1) ? Ficha.JUGADOR2 : Ficha.JUGADOR1;

                    }
                }

            }

            return clone;
        }

       
    }
}
