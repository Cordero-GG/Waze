using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Waze.Estructuras
{
    public class Carro
    {
        public Punto Origen { get; set; }
        public Punto Destino { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime? HoraLlegada { get; set; }
        public string Id { get; set; } // Para identificar el carro en la UI

        public Carro(Punto origen, Punto destino, string id)
        {
            Origen = origen;
            Destino = destino;
            Id = id;
            HoraInicio = DateTime.Now;
        }
    }
}

