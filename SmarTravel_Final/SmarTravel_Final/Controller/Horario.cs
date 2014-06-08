using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class Horario
    {
        public int id;
        public string salida;
        public string llegada;
        public Parada parada;

        public Horario() { }

        public Horario(int id, string salida, string llegada, Parada parada)
        {
            this.id = id;
            this.salida = salida;
            this.llegada = llegada;
            this.parada = parada;
        }

        public Horario(string salida, string llegada, Parada parada)
        {
            this.salida = salida;
            this.llegada = llegada;
            this.parada = parada;
        }
    }
}
