using System;

namespace Waze.Estructuras
{
    // Representa la lógica de un carro en el sistema (no visual)
    public class Carro
    {
        public Punto Origen { get; set; }
        public Punto Destino { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime? HoraLlegada { get; set; }
        public string Id { get; set; }
        public Ciudad CiudadActual { get; set; }
        public ListaSimple<Carretera> Ruta { get; set; }

        public Carro (Punto origen, Punto destino, string id, ListaSimple<Carretera> ruta)
        {
            Origen = origen;
            Destino = destino;
            Id = id;
            HoraInicio = DateTime.Now;
            HoraLlegada = null;
            Ruta = new ListaSimple<Carretera>();

        }
    }
}
