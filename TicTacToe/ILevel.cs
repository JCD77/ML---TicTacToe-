using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
  public  interface ILevel
    {
        public Partida SiguienteMovimiento(Partida juego);

        public NivelJugador Getlevel();

        public double GetIA();

        public void Entrenar();

        public string Descripcion();
    }
}
