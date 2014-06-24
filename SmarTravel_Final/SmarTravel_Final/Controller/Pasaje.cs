using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class Pasaje
    {
        public int numero;
        public Cliente cliente;
        public ViajeDiario viajeDiario;
        public int precio;
        public string fechaVenta;
        public int asiento;
        public string estado;

        public Pasaje(){}

        public Pasaje(Cliente cliente, ViajeDiario viajeDiario, int precio, string fechaVenta, int asiento, string estado)
        {
            this.cliente = cliente;
            this.viajeDiario = viajeDiario;
            this.precio = precio;
            this.fechaVenta = fechaVenta;
            this.asiento = asiento;
            this.estado = estado;
        }
        
        public Pasaje(ViajeDiario viajeDiario, int precio, string fechaVenta, int asiento, string estado)
        {            
            this.viajeDiario = viajeDiario;
            this.precio = precio;
            this.fechaVenta = fechaVenta;
            this.asiento = asiento;
            this.estado = estado;
        }

        public Pasaje(int numero, Cliente cliente, ViajeDiario viajeDiario, int precio, string fechaVenta, int asiento, string estado)
        {
            this.numero = numero;
            this.cliente = cliente;
            this.viajeDiario = viajeDiario;
            this.precio = precio;
            this.fechaVenta = fechaVenta;
            this.asiento = asiento;
            this.estado = estado;
        }

        public Pasaje(int numero, ViajeDiario viajeDiario, int precio, string fechaVenta, int asiento, string estado)
        {
            this.numero = numero;
            this.viajeDiario = viajeDiario;
            this.precio = precio;
            this.fechaVenta = fechaVenta;
            this.asiento = asiento;
            this.estado = estado;
        }
    }
}
