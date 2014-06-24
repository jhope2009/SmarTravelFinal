using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class Parada
    {
        public int id;
        public Ciudad ciudad;
        public int recorrido;
        public Parada siguiente;

        public Parada() 
        {
            this.id = -1;
        }

        public Parada(Ciudad ciudad, Parada siguiente)
        {
            this.id = -1;
            this.ciudad = ciudad;
            this.siguiente = siguiente;            
        }

        public Parada(int id, Ciudad ciudad, Parada siguiente)
        {
            this.id = id;
            this.ciudad = ciudad;
            this.siguiente = siguiente;
        }
        public Parada(int id, int Recorrido, Ciudad ciudad, Parada siguiente)
        {
            this.id = id;
            this.recorrido = Recorrido;
            this.ciudad = ciudad;
            this.siguiente = siguiente;
        }       
    }
}
