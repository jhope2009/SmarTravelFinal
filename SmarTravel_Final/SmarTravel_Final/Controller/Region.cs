using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class Region
    {
        public int id;
        public string nombre;

        public Region() { }

        public Region(string nombre)
        {
            this.id = -1;
            this.nombre = nombre;
        }

        public Region(int id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }
    }
}
