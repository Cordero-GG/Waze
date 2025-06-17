using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waze.Estructuras
{
    public class Grafo
    {
        public Dictionary<Punto, List<Conexion>> Adyacencias { get; } = new();
        public List<Conexion> Conexiones { get; } = new();

        public void AgregarPunto(Punto punto)
        {
            if (!Adyacencias.ContainsKey(punto))
                Adyacencias[punto] = new List<Conexion>();
        }

        public void AgregarConexion(Punto a, Punto b, double tiempoBase)
        {
            var conexion = new Conexion(a, b, tiempoBase);
            Conexiones.Add(conexion);
            Adyacencias[a].Add(conexion);
            Adyacencias[b].Add(conexion); // Doble sentido
        }

        public void EliminarConexion(Punto a, Punto b)
        {
            var conexion = Conexiones.FirstOrDefault(c =>
                (c.PuntoA == a && c.PuntoB == b) || (c.PuntoA == b && c.PuntoB == a));
            if (conexion != null)
            {
                Conexiones.Remove(conexion);
                Adyacencias[a].Remove(conexion);
                Adyacencias[b].Remove(conexion);
            }
        }

        public void EliminarPunto(Punto punto)
        {
            if (Adyacencias.ContainsKey(punto))
            {
                // Elimina todas las conexiones asociadas
                foreach (var conexion in Adyacencias[punto].ToList())
                {
                    EliminarConexion(conexion.PuntoA, conexion.PuntoB);
                }
                Adyacencias.Remove(punto);
            }
        }
    }
}
