using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class Persona
    {
        public string rut;
        public string nombre;
        public int edad;
        public string direccion;
        public Ciudad ciudad;
        public int fono;
        public string clave;
        //public string imagen;
        public string sexo;
        public string cargo;

        public Persona() { }

        public Persona(string rut, string nombre, int edad, string direccion, Ciudad ciudad, int fono, string clave, string sexo, string cargo)
        {
            this.rut = rut;
            this.nombre = nombre;
            this.edad = edad;
            this.direccion = direccion;
            this.ciudad = ciudad;
            this.fono = fono;
            this.clave = clave;
            this.sexo = sexo;
            this.cargo = cargo;
        }
    }
}
