using System.Windows.Controls;
using Waze.Estructuras;

namespace Waze.Estructuras
{
    // Representa un carro visual en la interfaz, asociado a una ciudad y una imagen
    public class CarroVisual
    {
        public Ciudad CiudadActual { get; set; }
        public Image Imagen { get; set; }
    }
}
