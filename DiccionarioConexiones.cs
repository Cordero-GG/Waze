// Diccionario simple usando solo estructuras propias
using Waze.Estructuras;

namespace DiccionarioDvid
{
    public class DiccionarioSimple<K, V>
    {
        private ListaSimple<K> claves = new();
        private ListaSimple<V> valores = new();

        public void AgregarOActualizar(K clave, V valor)
        {
            int index = IndiceDeClave(clave);
            if (index >= 0)
                valores.ReemplazaEn(index, valor);
            else
            {
                claves.AgregarFinal(clave);
                valores.AgregarFinal(valor);
            }
        }

        public bool ContieneClave(K clave) => IndiceDeClave(clave) >= 0;

        public V Obtener(K clave)
        {
            int index = IndiceDeClave(clave);
            if (index >= 0)
                return valores.ElementoEn(index);
            throw new Exception("Clave no encontrada");
        }

        public void Eliminar(K clave)
        {
            int index = IndiceDeClave(clave);
            if (index >= 0)
            {
                claves.EliminarEn(index);
                valores.EliminarEn(index);
            }
        }

        public ListaSimple<K> RecorrerClaves()
        {
            return claves.Recorrer();
        }

        private int IndiceDeClave(K clave)
        {
            int i = 0;
            foreach (var k in claves)
            {
                if (k.Equals(clave))
                    return i;
                i++;
            }
            return -1;
        }
    }
}
