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
    }
}
