using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
    class PlayerRandom : ILevel
    {
        public void Entrenar()
        {
            return;
        }

        public string Descripcion()
        {
            return this.Getlevel().ToString() ;
        }
        public double GetIA()
        {
            return 0.0;
        }

        public NivelJugador Getlevel()
        {
            return NivelJugador.RANDOM;
        }

        public Partida SiguienteMovimiento(Partida juego)
        {
            int filaRandom ;
            int columnaRandom;
            Random random = new Random();
            Ficha ficha;
            do
            {
                filaRandom = random.Next(0, juego.Rango);
                columnaRandom = random.Next(0, juego.Rango);
                ficha = juego.Valor(filaRandom, columnaRandom);

            } while (ficha != Ficha.NINGUNA);

            juego.Estado = Progreso.PROGRESO;
            if (juego.Turno == Turno.JUGADOR1)
            {
                juego.Tablero[filaRandom, columnaRandom] = Ficha.JUGADOR1;
                juego.Turno = Turno.JUGADOR2;
            }
            else
            {
                juego.Tablero[filaRandom, columnaRandom] = Ficha.JUGADOR2;
                juego.Turno = Turno.JUGADOR1;
            }

            
            return juego;
            }
    }
}
