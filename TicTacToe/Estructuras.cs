using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TicTacToe
{
    class Estructuras
    {

    }

    public enum Progreso { INICIO,PROGRESO, TABLAS, JUGADOR1GANA, JUGADOR2GANA};
    public enum Turno { JUGADOR1,JUGADOR2};
    public enum NivelJugador { MANUAL, RANDOM, LEVELIA};

    public enum Ficha { NINGUNA, JUGADOR1, JUGADOR2}


    public struct movimiento
    {

        int ganadas;
        int perdidas;
        int tablas;

    }

    public struct scoreMovimiento
    {
       public  int posicion;
        public int score;
        public int probabilidadMinima;
        public  int probabilidadMaxima;


    }
    public static class Helper
    {
        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

    }

}

   

