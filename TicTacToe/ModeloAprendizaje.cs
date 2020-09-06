using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TicTacToe
{


    public struct PosicionTablero
    {
        public int Ganadas { get; set; }
        public int Perdidas { get; set; }
        public int Tablas { get; set; }

        public int Totales { get; set; }

        public float Porcentaje { get; set; }

        public Ficha[,] Tablero { get; set; }


    }

    public static class ModeloAprendizaje
    {
        public static List<List<string>> partidas = new List<List<string>>();
        public static Dictionary<string, PosicionTablero> posiciones = new Dictionary<string, PosicionTablero>();
       // public static MLContext mlContext = new MLContext(seed: 0);

        public static string Tablero2String(Partida partida)
        {

            string keyDictionary = "";

            for (int fila = 0; fila < partida.Tablero.GetLength(0); fila++)
            {


                for (int columna = 0; columna < partida.Tablero.GetLength(1); columna++)
                {

                    keyDictionary += ((int)partida.Tablero[fila, columna]);
                }

            }
            return keyDictionary;

        }

        public static void AlmacenarModelo(Partida partida)
        {

            //Se comprueba que la partida no se ha jugado anteriormente, para no falsear porcentaje de acierto



            foreach (var movimientos in ModeloAprendizaje.partidas)
            {

                if (movimientos.Count == partida.Movimientos.Count)
                {
                    int indice = 0;
                    for (int i = 0; i < movimientos.Count; i++)
                    {
                        if (movimientos[i] != partida.Movimientos[i]) break;
                        indice++;
                    }
                    if (indice == (movimientos.Count - 1)) return;
                }

            }

            List<string> listaMovimientos = new List<string>();
            foreach (string cadena in partida.Movimientos)
            {

                listaMovimientos.Add(cadena);
                PosicionTablero valor = new PosicionTablero();
                if (posiciones.ContainsKey(cadena))
                {
                    valor = posiciones[cadena];
                    valor.Ganadas = (partida.Estado == Progreso.JUGADOR1GANA) ? posiciones[cadena].Ganadas + 1 : posiciones[cadena].Ganadas;
                    valor.Perdidas = (partida.Estado == Progreso.JUGADOR2GANA) ? posiciones[cadena].Perdidas + 1 : posiciones[cadena].Perdidas;
                    valor.Tablas = (partida.Estado == Progreso.TABLAS) ? posiciones[cadena].Tablas + 1 : posiciones[cadena].Tablas;
                    valor.Totales++;
                    valor.Porcentaje = ((float)valor.Ganadas+ (float)valor.Tablas/2.0f) / (float)valor.Totales;
                    valor.Tablero = partida.Tablero;
                    posiciones[cadena] = valor;

                }
                else
                {
                    valor.Ganadas = (partida.Estado == Progreso.JUGADOR1GANA) ? 1 : 0;
                    valor.Perdidas = (partida.Estado == Progreso.JUGADOR2GANA) ? 1 : 0;
                    valor.Tablas = (partida.Estado == Progreso.TABLAS) ? 1 : 0;
                    valor.Totales = 1;
                    valor.Tablero = partida.Tablero;
                    valor.Porcentaje = (float)valor.Ganadas / (float)valor.Totales;
                    posiciones.Add(cadena, valor);
                }
            }
            ModeloAprendizaje.partidas.Add(listaMovimientos);

        }


       
        public static List<InputData> Estructuras2List()
        {
            List<InputData> lista = new List<InputData>();
            foreach (var a in ModeloAprendizaje.posiciones)
            {
                InputData input = new InputData();
                input.Features = new float[9];

                input.Features[0] = ((int)a.Value.Tablero[0, 0]) / (float)2.0;
                input.Features[1] = ((int)a.Value.Tablero[0, 1]) / (float)2.0;
                input.Features[2] = ((int)a.Value.Tablero[0, 2]) / (float)2.0;
                input.Features[3] = ((int)a.Value.Tablero[1, 0]) / (float)2.0;
                input.Features[4] = ((int)a.Value.Tablero[1, 1]) / (float)2.0;
                input.Features[5] = ((int)a.Value.Tablero[1, 2]) / (float)2.0;
                input.Features[6] = ((int)a.Value.Tablero[2, 0]) / (float)2.0;
                input.Features[7] = ((int)a.Value.Tablero[2, 1]) / (float)2.0;
                input.Features[8] = ((int)a.Value.Tablero[2, 2]) / (float)2.0;
                input.Label = a.Value.Porcentaje;

                lista.Add(input);

            }
            return lista;

        }



public class InputData
        {
            [VectorType(9)]
            public float[] Features { get; set; }

public float Label { get; set; }

        }

        public class InputData3
        {
            [LoadColumn(0)]
            public float P0_0;

            [LoadColumn(1)]
            public float P0_1;
            [LoadColumn(2)]
            public float P0_2;
            [LoadColumn(3)]
            public float P1_0;
            [LoadColumn(4)]
            public float P1_1;
            [LoadColumn(5)]
            public float P1_2;
            [LoadColumn(6)]
            public float P2_0;
            [LoadColumn(7)]
            public float P2_1;
            [LoadColumn(8)]
            public float P2_2;
            [LoadColumn(9)]
            public float Label;

            

            
        }

        public class OutputData
        {
            [ColumnName("Score")]
            public float Porcentaje { get; set; }
        }

    }

}

