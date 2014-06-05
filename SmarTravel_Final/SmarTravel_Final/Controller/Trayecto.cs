using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class Trayecto
    {
        public int id;
        public Ciudad origen;
        public Ciudad destino;
        public int precio;
        
        public Trayecto() { }

        public Trayecto(Ciudad origen, Ciudad destino, int precio)
        {
            this.id = -1;            
            this.origen = origen;
            this.destino = destino;
            this.precio = precio;
        }

        public Trayecto(int id, Ciudad origen, Ciudad destino, int precio)
        {
            this.id = id;
            this.origen = origen;
            this.destino = destino;
            this.precio = precio;
        }
    }
}
