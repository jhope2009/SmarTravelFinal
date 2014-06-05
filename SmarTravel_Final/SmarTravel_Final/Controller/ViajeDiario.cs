using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class ViajeDiario
    {
        public int id;
        public Trayecto trayecto;
        public string fecha;
        public string bus;
        public string chofer;
        public string auxiliar;
        public int asientosDisponibles;
        
        public ViajeDiario() { }

        public ViajeDiario(Trayecto trayecto, string fecha, string bus, string chofer, string auxiliar, int asientosDisponibles)
        {
            this.id = -1;
            this.trayecto = trayecto;
            this.fecha = fecha;
            this.bus = bus;
            this.chofer = chofer;
            this.auxiliar = auxiliar;
            this.asientosDisponibles = asientosDisponibles;
        }

        public ViajeDiario(int id, Trayecto trayecto, string fecha, string bus, string chofer, string auxiliar, int asientosDisponibles)
        {
            this.id = id;
            this.trayecto = trayecto;
            this.fecha = fecha;
            this.bus = bus;
            this.chofer = chofer;
            this.auxiliar = auxiliar;
            this.asientosDisponibles = asientosDisponibles;
        }    
    }
}
