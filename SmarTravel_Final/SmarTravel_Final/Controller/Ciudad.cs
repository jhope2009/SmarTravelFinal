using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class Ciudad
    {
        public int id;
        public string nombre;
        public string region;
        public int numero;       

        public Ciudad(){}

        public Ciudad(string nombre, string region, int numero)
        {
            this.id = -1;
            this.nombre = nombre;
            this.region = region;
            this.numero = numero;
        }

        public Ciudad(int id, string nombre, string region, int numero)
        {
            this.id = id;
            this.nombre = nombre;
            this.region = region;
            this.numero = numero;
        }
    }
}
