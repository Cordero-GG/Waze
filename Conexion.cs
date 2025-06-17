using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Waze.Estructuras
{
    public class Conexion
    {
        public Punto PuntoA { get; }
        public Punto PuntoB { get; }
        public double TiempoBase { get; set; }
        public bool Bloqueada { get; set; }

        // Atributos que afectan el tiempo
        public double Trafico { get; set; }
        public double Trabajos { get; set; }
        public double DensidadCarros { get; set; }

        public double TiempoActual =>
            Bloqueada ? double.PositiveInfinity :
            TiempoBase * (1 + Trafico + Trabajos + DensidadCarros);

        public Conexion(Punto a, Punto b, double tiempoBase)
        {
            PuntoA = a;
            PuntoB = b;
            TiempoBase = tiempoBase;
            Bloqueada = false;
        }
    }
}

