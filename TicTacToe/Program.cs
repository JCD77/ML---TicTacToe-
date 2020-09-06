using System;
using System.Collections.Generic;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {

            ILevel jugador1 = new PlayerRandom();
            ILevel jugador2 = new PlayerRandom();
            Partida partida;
            int numeroPartidasModelo = 50;
            Console.WriteLine("Generando  {0} Partidas como modelo", numeroPartidasModelo);

            do
            {

                partida = new Partida(rango: 3, jugador1, jugador2);
                partida.Jugar(false);
                ModeloAprendizaje.AlmacenarModelo(partida);



            } while (ModeloAprendizaje.partidas.Count < numeroPartidasModelo);



            Console.WriteLine("Empezando partidas");





            int auxResultadoJugador1 = 0;
            int auxResultadoJugador2 = 0;
            int auxtablas = 0;
            //Partida 

            jugador2 = new PlayerIA(0.90);
            //  jugador1 = 
        //    jugador2 = new PlayerIA(0.90);
            jugador1 = new PlayerIA(0.25,true);
            for (int j = 0; j < 2000; j++)
            {
                partida = new Partida(rango: 3, jugador1, jugador2);

                partida.Jugar(false);
                switch (partida.Estado)
                {
                    case Progreso.JUGADOR1GANA: auxResultadoJugador2++; break;
                    case Progreso.JUGADOR2GANA: auxResultadoJugador1++; break;
                    default: auxtablas++; break;


                }
            }
            System.Console.WriteLine("********************************************************************");
            System.Console.WriteLine("jugador1-{0}:{1}", partida.Jugador1.Descripcion(), auxResultadoJugador1);
            System.Console.WriteLine("jugador2-{0}:{1}", partida.Jugador2.Descripcion(), auxResultadoJugador2);
            System.Console.WriteLine("Empate:{0}", auxtablas);
            System.Console.WriteLine("********************************************************************");


            ILevel[] jugadores = new ILevel[1];
            jugadores[0] = (ILevel)new PlayerIA(0.90);
            //   jugadores[1] = (ILevel)new PlayerIA(0.25);
            //    jugadores[2] = (ILevel)new PlayerIA(0.50);
            //    jugadores[3] = (ILevel)new PlayerIA(0.75);
            //    jugadores[4] = (ILevel)new PlayerIA(0.90);
            ILevel[] jugadores2 = new ILevel[1];
             //   jugadores2[0] = (ILevel)new PlayerRandom();
            //    jugadores2[1] = (ILevel)new PlayerIA(0.25);
            //    jugadores2[2] = (ILevel)new PlayerIA(0.50);
            //    jugadores2[3] = (ILevel)new PlayerIA(0.75);
            //     jugadores2[4] = (ILevel)new PlayerIA(0.90);
            jugadores2[0] = (ILevel)new PlayerIA(0.25);


            int numeroPartidas = 2000;
            foreach (var j1 in jugadores)
            {
                foreach (var j2 in jugadores2)
                {
                    int resultadoJugador1 = 0;
                    int resultadoJugador2 = 0;
                    int tablas = 0;
                    for (int j = 0; j < numeroPartidas; j++)
                    {
                        var partida1 = new Partida(rango: 3, j1, j2);

                        partida1.Jugar(false);
                        switch (partida1.Estado)
                        {
                            case Progreso.JUGADOR1GANA: resultadoJugador2++; break;
                            case Progreso.JUGADOR2GANA: resultadoJugador1++; break;
                            default: tablas++; break;


                        }
                    }
                    System.Console.WriteLine("**************************-Partidas Jugadas:{0}-****************************************", numeroPartidas);
                    System.Console.WriteLine("jugador1-{0}:{1}", j1.Descripcion(), resultadoJugador1);
                    System.Console.WriteLine("jugador2-{0}:{1}", j2.Descripcion(), resultadoJugador2);
                    System.Console.WriteLine("Empate:{0}", tablas);
                    System.Console.WriteLine("***************************************************************************************");

                }

            }
        }





        




    }
}
