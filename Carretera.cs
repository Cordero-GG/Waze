using System;

namespace Waze.Estructuras
{
    public class Carretera
    {
        public Ciudad Origen { get; set; }
        public Ciudad Destino { get; set; }
        public double Tiempo { get; set; }

        public Carretera(Ciudad origen, Ciudad destino, double tiempo)
        {
            Origen = origen;
            Destino = destino;
            Tiempo = tiempo;
        }
    }
}
