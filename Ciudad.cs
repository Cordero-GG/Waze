using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waze.Estructuras
{
    public class Ciudad
    {
        public string Nombre { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString() => Nombre;

        // Agregar comparación de ciudades
        public override bool Equals(object obj)
        {
            if (obj is Ciudad otra)
                return Nombre == otra.Nombre && X == otra.X && Y == otra.Y;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, X, Y);
        }
    }
}