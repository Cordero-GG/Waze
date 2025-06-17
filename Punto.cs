using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Waze.Estructuras
{
    public class Punto
    {
        public string Nombre { get; set; }
        public (int X, int Y) Coordenadas { get; set; }

        public Punto(string nombre, int x, int y)
        {
            Nombre = nombre;
            Coordenadas = (x, y);
        }

        public override string ToString() => Nombre;
    }
}

