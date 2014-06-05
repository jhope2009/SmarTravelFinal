using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class Viaje
    {
        public int id;
        public int recorrido;
        public string identificador;
        public string desde;
        public string hasta;
        public List<ViajeDiario> viajesDiarios;

        public Viaje() { }

        public Viaje(int recorrido, string identificador, string desde, string hasta, List<ViajeDiario> viajesDiarios)
        {
            this.id = -1;
            this.recorrido = recorrido;
            this.identificador = identificador;
            this.desde = desde;
            this.hasta = hasta;
            this.viajesDiarios = viajesDiarios;
        }

        public Viaje(int id, int recorrido, string identificador, string desde, string hasta, List<ViajeDiario> viajesDiarios)
        {
            this.id = id;
            this.recorrido = recorrido;
            this.identificador = identificador;
            this.desde = desde;
            this.hasta = hasta;
            this.viajesDiarios = viajesDiarios;
        }
    }
}
