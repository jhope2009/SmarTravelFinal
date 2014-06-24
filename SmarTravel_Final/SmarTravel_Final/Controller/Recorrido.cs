using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class Recorrido
    {
        public int id;
        public Parada parada;
        public List<Trayecto> trayectos;
        public Ciudad origen;
        public Ciudad destino;

        public Recorrido() { }

        public Recorrido(Parada parada, List<Trayecto> trayectos)
        {
            this.id = -1;
            this.parada = parada;
            this.trayectos = trayectos;
        }

        public Recorrido(int id, Parada parada, List<Trayecto> trayectos)
        {
            this.id = id;
            this.parada = parada;
            this.trayectos = trayectos;
        }
        public Recorrido(int id, Parada parada, Ciudad origen, Ciudad Destino)
        {
            this.id = id;
            this.parada = parada;
            this.origen = origen;
            this.destino = Destino;
        }
    }
}
