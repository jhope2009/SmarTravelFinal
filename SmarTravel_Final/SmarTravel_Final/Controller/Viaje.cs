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
        public string fechaDesde;
        public string fechaHasta;        
        public List<ViajeDiario> viajesDiarios;
        public List<Horario> horarios;

        public Viaje() { }

        public Viaje(int recorrido, string identificador, string desde, string hasta, List<ViajeDiario> viajesDiarios, List<Horario> horarios)
        {
            this.id = -1;
            this.recorrido = recorrido;
            this.identificador = identificador;
            this.fechaDesde = desde;
            this.fechaHasta = hasta;
            this.viajesDiarios = viajesDiarios;
            this.horarios = horarios;
        }

        public Viaje(int id, int recorrido, string identificador, string desde, string hasta, List<ViajeDiario> viajesDiarios, List<Horario> horarios)
        {
            this.id = id;
            this.recorrido = recorrido;
            this.identificador = identificador;
            this.fechaDesde = desde;
            this.fechaHasta = hasta;
            this.viajesDiarios = viajesDiarios;
            this.horarios = horarios;
        }
    }
}
