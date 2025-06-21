// Dijkstrag.cs implementado
using System;
using Waze.Estructuras;
using DiccionarioDvid;

namespace DijktragN
{
    public class Dijkstrag
    {
        public ListaSimple<Carretera> EncontrarRutaMasCorta(
            Ciudad inicio,
            Ciudad destino,
            ListaSimple<Ciudad> todasLasCiudades,
            DiccionarioSimple<Ciudad, ListaSimple<Carretera>> conexiones)
        {
            // Inicializar estructuras
            DiccionarioSimple<Ciudad, double> distancias = new DiccionarioSimple<Ciudad, double>();
            DiccionarioSimple<Ciudad, Carretera> camino = new DiccionarioSimple<Ciudad, Carretera>();
            DiccionarioSimple<Ciudad, bool> visitados = new DiccionarioSimple<Ciudad, bool>();
            ListaSimple<Ciudad> porVisitar = new ListaSimple<Ciudad>();

            // Configurar valores iniciales
            foreach (Ciudad ciudad in todasLasCiudades.Recorrer().Recorrer())
            {
                distancias.AgregarOActualizar(ciudad, double.MaxValue);
                camino.AgregarOActualizar(ciudad, null);
                visitados.AgregarOActualizar(ciudad, false);
                porVisitar.AgregarFinal(ciudad);
            }
            distancias.AgregarOActualizar(inicio, 0);

            // Algoritmo principal
            while (porVisitar.Tamano() > 0)
            {
                // Encontrar ciudad con menor distancia
                Ciudad actual = null;
                double minDist = double.MaxValue;
                foreach (Ciudad ciudad in porVisitar.Recorrer().Recorrer())
                {
                    double dist = distancias.Obtener(ciudad);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        actual = ciudad;
                    }
                }

                if (actual == null || actual.Equals(destino)) break;

                porVisitar.EliminarElemento(actual);
                visitados.AgregarOActualizar(actual, true);

                // Procesar vecinos
                if (conexiones.ContieneClave(actual))
                {
                    foreach (Carretera carretera in conexiones.Obtener(actual).Recorrer().Recorrer())
                    {
                        Ciudad vecino = carretera.Destino;
                        if (!visitados.Obtener(vecino))
                        {
                            double nuevaDist = distancias.Obtener(actual) + carretera.Tiempo;
                            if (nuevaDist < distancias.Obtener(vecino))
                            {
                                distancias.AgregarOActualizar(vecino, nuevaDist);
                                camino.AgregarOActualizar(vecino, carretera);
                            }
                        }
                    }
                }
            }

            // Reconstruir ruta
            ListaSimple<Carretera> ruta = new ListaSimple<Carretera>();
            if (distancias.Obtener(destino) < double.MaxValue)
            {
                Ciudad actual = destino;
                while (!actual.Equals(inicio))
                {
                    Carretera carretera = camino.Obtener(actual);
                    ruta.AgregarAlPrincipio(carretera);
                    actual = carretera.Origen;
                }
            }
            return ruta;
        }
    }
}