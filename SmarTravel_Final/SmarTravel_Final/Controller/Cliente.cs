using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class Cliente: Persona
    {
        public int tarjeta;
        public int descuento;

        public Cliente() { }

        public Cliente(string rut, string nombre, int edad, string direccion, Ciudad ciudad, int fono, string clave, string sexo, string cargo, int tarjeta, int descuento)
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
            this.tarjeta = tarjeta;
            this.descuento = descuento;
        }
    }
}
