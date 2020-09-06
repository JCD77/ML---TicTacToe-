using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
    class PlayerManual : ILevel
    {
      
            public string Descripcion()
            {
                return this.Getlevel().ToString();
            }
       

        public void Entrenar()
        {
            return;
        }

        public double GetIA()
        {
            return 0.0;
        }

        public NivelJugador Getlevel()
        {
            return NivelJugador.MANUAL;
        }

        public Partida SiguienteMovimiento(Partida juego)
        {
            int fila=-1 ;
            int columna=-1;
            Random random = new Random();
            Ficha ficha=Ficha.NINGUNA;
            do
            {
                Console.Write("\nIndique Posicion [fila,columna]");
                string entrada = Console.ReadLine();
                var split = entrada.Split(",");
                if (int.TryParse(split[0], out fila) && int.TryParse(split[1], out columna))
                {

                   
                    ficha = juego.Valor(fila, columna);
                }

            } while (ficha != Ficha.NINGUNA);

            juego.Estado = Progreso.PROGRESO;
            if (juego.Turno == Turno.JUGADOR1)
            {
                juego.Tablero[fila, columna] = Ficha.JUGADOR1;
                juego.Turno = Turno.JUGADOR2;
            }
            else
            {
                juego.Tablero[fila, columna] = Ficha.JUGADOR2;
                juego.Turno = Turno.JUGADOR1;
            }

            
            return juego;
            }
    }
}
