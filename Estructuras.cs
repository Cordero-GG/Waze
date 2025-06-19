using System;
using System.Collections;
using System.Collections.Generic;

namespace Waze.Estructuras
{
    // Nodo para lista simple
    public class NodoDll<T>
    {
        public T dato;
        public NodoDll<T> siguiente;

        public NodoDll(T valor)
        {
            dato = valor;
            siguiente = null;
        }
    }

    // Lista enlazada simple genérica
    public class ListaSimple<T> : IEnumerable<T>
    {
        private NodoDll<T> primero;

        public ListaSimple()
        {
            primero = null;
        }

        public void Insertar(T valor) => AgregarFinal(valor);

        public void AgregarFinal(T valor)
        {
            NodoDll<T> nuevo = new NodoDll<T>(valor);
            if (primero == null)
                primero = nuevo;
            else
            {
                NodoDll<T> actual = primero;
                while (actual.siguiente != null)
                    actual = actual.siguiente;
                actual.siguiente = nuevo;
            }
        }

        // Devuelve una nueva ListaSimple con los elementos
        public ListaSimple<T> Recorrer()
        {
            ListaSimple<T> elementos = new();
            NodoDll<T> actual = primero;
            while (actual != null)
            {
                elementos.AgregarFinal(actual.dato);
                actual = actual.siguiente;
            }
            return elementos;
        }

        public void ReemplazaEn(int indice, T nuevoValor)
        {
            if (indice < 0) throw new ArgumentOutOfRangeException();
            NodoDll<T> actual = primero;
            int i = 0;
            while (actual != null)
            {
                if (i == indice)
                {
                    actual.dato = nuevoValor;
                    return;
                }
                actual = actual.siguiente;
                i++;
            }
            throw new ArgumentOutOfRangeException("Índice fuera de rango.");
        }

        public T ElementoEn(int indice)
        {
            if (indice < 0) throw new ArgumentOutOfRangeException("Índice negativo no válido.");
            NodoDll<T> actual = primero;
            int i = 0;
            while (actual != null)
            {
                if (i == indice)
                    return actual.dato;
                actual = actual.siguiente;
                i++;
            }
            throw new ArgumentOutOfRangeException("Índice fuera de rango.");
        }

        public int IndiceDe(T valor)
        {
            NodoDll<T> actual = primero;
            int i = 0;
            while (actual != null)
            {
                if (actual.dato.Equals(valor))
                    return i;
                actual = actual.siguiente;
                i++;
            }
            return -1;
        }

        public bool EstaVacia() => primero == null;

        public int Tamano()
        {
            int contador = 0;
            NodoDll<T> actual = primero;
            while (actual != null)
            {
                contador++;
                actual = actual.siguiente;
            }
            return contador;
        }

        public void Limpiar() => primero = null;

        public void EliminarEn(int indice)
        {
            if (indice < 0 || EstaVacia()) throw new ArgumentOutOfRangeException();
            if (indice == 0)
            {
                primero = primero.siguiente;
                return;
            }
            NodoDll<T> actual = primero;
            int i = 0;
            while (actual != null && i < indice - 1)
            {
                actual = actual.siguiente;
                i++;
            }
            if (actual == null || actual.siguiente == null)
                throw new ArgumentOutOfRangeException("Índice fuera de rango.");
            actual.siguiente = actual.siguiente.siguiente;
        }

        public bool EliminarElemento(T valor)
        {
            if (EstaVacia()) return false;
            if (primero.dato.Equals(valor))
            {
                primero = primero.siguiente;
                return true;
            }
            NodoDll<T> actual = primero;
            while (actual.siguiente != null)
            {
                if (actual.siguiente.dato.Equals(valor))
                {
                    actual.siguiente = actual.siguiente.siguiente;
                    return true;
                }
                actual = actual.siguiente;
            }
            return false;
        }

        // Implementación de IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            NodoDll<T> actual = primero;
            while (actual != null)
            {
                yield return actual.dato;
                actual = actual.siguiente;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // Cola genérica
    public class Cola<T>
    {
        private NodoDll<T> frente;
        private NodoDll<T> fin;

        public Cola()
        {
            frente = null;
            fin = null;
        }

        public bool EstaVacia() => frente == null;

        public void Encolar(T valor)
        {
            NodoDll<T> nuevo = new NodoDll<T>(valor);
            if (EstaVacia())
            {
                frente = nuevo;
                fin = nuevo;
            }
            else
            {
                fin.siguiente = nuevo;
                fin = nuevo;
            }
        }

        public T Desencolar()
        {
            if (EstaVacia())
                throw new InvalidOperationException("La cola está vacía.");
            T valor = frente.dato;
            frente = frente.siguiente;
            if (frente == null)
                fin = null;
            return valor;
        }

        public T Consultar()
        {
            if (EstaVacia())
                throw new InvalidOperationException("La cola está vacía.");
            return frente.dato;
        }
    }

    // Pila genérica
    public class Pila<T>
    {
        private NodoDll<T> tope;

        public Pila()
        {
            tope = null;
        }

        public bool EstaVacia() => tope == null;

        public void Apilar(T valor)
        {
            NodoDll<T> nuevo = new NodoDll<T>(valor);
            nuevo.siguiente = tope;
            tope = nuevo;
        }

        public T Desapilar()
        {
            if (EstaVacia())
                throw new InvalidOperationException("La pila está vacía.");
            T valor = tope.dato;
            tope = tope.siguiente;
            return valor;
        }

        public T Consultar()
        {
            if (EstaVacia())
                throw new InvalidOperationException("La pila está vacía.");
            return tope.dato;
        }
    }

    // Nodo para lista doblemente enlazada
    public class NodoDllDoble<T>
    {
        public T dato;
        public NodoDllDoble<T> siguiente;
        public NodoDllDoble<T> anterior;

        public NodoDllDoble(T valor)
        {
            dato = valor;
            siguiente = null;
            anterior = null;
        }
    }

    // Lista doblemente enlazada genérica
    public class ListaDoble<T>
    {
        private NodoDllDoble<T> cabeza;
        private NodoDllDoble<T> cola;

        public ListaDoble()
        {
            cabeza = null;
            cola = null;
        }

        public bool EstaVacia() => cabeza == null;

        public void AgregarAlFinal(T valor)
        {
            NodoDllDoble<T> nuevo = new NodoDllDoble<T>(valor);
            if (EstaVacia())
            {
                cabeza = nuevo;
                cola = nuevo;
            }
            else
            {
                cola.siguiente = nuevo;
                nuevo.anterior = cola;
                cola = nuevo;
            }
        }

        public void AgregarAlInicio(T valor)
        {
            NodoDllDoble<T> nuevo = new NodoDllDoble<T>(valor);
            if (EstaVacia())
            {
                cabeza = nuevo;
                cola = nuevo;
            }
            else
            {
                nuevo.siguiente = cabeza;
                cabeza.anterior = nuevo;
                cabeza = nuevo;
            }
        }

        public bool Eliminar(T valor)
        {
            NodoDllDoble<T> actual = cabeza;
            while (actual != null)
            {
                if (actual.dato.Equals(valor))
                {
                    if (actual.anterior != null)
                        actual.anterior.siguiente = actual.siguiente;
                    else
                        cabeza = actual.siguiente;

                    if (actual.siguiente != null)
                        actual.siguiente.anterior = actual.anterior;
                    else
                        cola = actual.anterior;

                    return true;
                }
                actual = actual.siguiente;
            }
            return false;
        }

        public int Tamano()
        {
            int contador = 0;
            NodoDllDoble<T> actual = cabeza;
            while (actual != null)
            {
                contador++;
                actual = actual.siguiente;
            }
            return contador;
        }

        public T ElementoEn(int indice)
        {
            if (indice < 0)
                throw new ArgumentOutOfRangeException();
            NodoDllDoble<T> actual = cabeza;
            int i = 0;
            while (actual != null)
            {
                if (i == indice)
                    return actual.dato;
                actual = actual.siguiente;
                i++;
            }
            throw new ArgumentOutOfRangeException("Índice fuera de rango.");
        }

        public void Limpiar()
        {
            cabeza = null;
            cola = null;
        }
    }

    // Nodo para Array Dinámico
    public class NodoDllArray<T>
    {
        public T dato;
        public NodoDllArray<T> siguiente;

        public NodoDllArray(T valor)
        {
            dato = valor;
            siguiente = null;
        }
    }

    // Array Dinámico
    public class ArrayDinamico<T>
    {
        private NodoDllArray<T> primero;
        private int contador;

        public ArrayDinamico()
        {
            primero = null;
            contador = 0;
        }

        public int Tamano() => contador;

        public void Agregar(T valor)
        {
            NodoDllArray<T> nuevo = new NodoDllArray<T>(valor);
            if (primero == null)
                primero = nuevo;
            else
            {
                NodoDllArray<T> actual = primero;
                while (actual.siguiente != null)
                    actual = actual.siguiente;
                actual.siguiente = nuevo;
            }
            contador++;
        }

        public T ElementoEn(int indice)
        {
            if (indice < 0 || indice >= contador)
                throw new ArgumentOutOfRangeException();
            NodoDllArray<T> actual = primero;
            int i = 0;
            while (actual != null)
            {
                if (i == indice)
                    return actual.dato;
                actual = actual.siguiente;
                i++;
            }
            throw new ArgumentOutOfRangeException();
        }

        public void Limpiar()
        {
            primero = null;
            contador = 0;
        }
    }

    // Diccionario simple usando solo estructuras propias
    public class Diccionario<K, V>
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
                EliminarEn(claves, index);
                EliminarEn(valores, index);
            }
        }

        public ListaSimple<K> RecorrerClaves()
        {
            return claves.Recorrer();
        }

        private int IndiceDeClave(K clave)
        {
            NodoDll<K> actual = claves.GetType()
                .GetField("primero", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(claves) as NodoDll<K>;
            int i = 0;
            while (actual != null)
            {
                if (actual.dato.Equals(clave))
                    return i;
                actual = actual.siguiente;
                i++;
            }
            return -1;
        }

        private void EliminarEn<T>(ListaSimple<T> lista, int indice)
        {
            lista.EliminarEn(indice);
        }
    }

    // -------------------------------------------------------------------------
    // ESTRUCTURA DE LISTA DE ADYACENCIA PARA GRAFOS
    // -------------------------------------------------------------------------
    /// <summary>
    /// Representa una lista de adyacencia para un grafo.
    /// Cada nodo tiene una lista de aristas salientes.
    /// </summary>
    /// <typeparam name="TNodo">Tipo del nodo (por ejemplo, Ciudad)</typeparam>
    /// <typeparam name="TArista">Tipo de la arista (por ejemplo, Carretera)</typeparam>
    public class ListaAdyacencia<TNodo, TArista>
    {
        /// <summary>
        /// Nodo interno para la lista de adyacencia.
        /// Relaciona un nodo con su lista de aristas salientes.
        /// </summary>
        public class NodoAdy
        {
            public TNodo Nodo; // Nodo principal
            public ListaSimple<TArista> Aristas; // Lista de aristas salientes

            public NodoAdy(TNodo nodo)
            {
                Nodo = nodo;
                Aristas = new ListaSimple<TArista>();
            }
        }

        // Lista de nodos de adyacencia
        private ListaSimple<NodoAdy> nodos;

        /// <summary>
        /// Constructor: crea una lista de adyacencia vacía.
        /// </summary>
        public ListaAdyacencia()
        {
            nodos = new ListaSimple<NodoAdy>();
        }

        /// <summary>
        /// Agrega un nodo al grafo (si no existe).
        /// </summary>
        public void AgregarNodo(TNodo nodo)
        {
            if (BuscarNodoAdy(nodo) == null)
                nodos.AgregarFinal(new NodoAdy(nodo));
        }

        /// <summary>
        /// Agrega una arista saliente a un nodo.
        /// </summary>
        public void AgregarArista(TNodo origen, TArista arista)
        {
            var nodoAdy = BuscarNodoAdy(origen);
            if (nodoAdy == null)
            {
                nodoAdy = new NodoAdy(origen);
                nodos.AgregarFinal(nodoAdy);
            }
            nodoAdy.Aristas.AgregarFinal(arista);
        }

        /// <summary>
        /// Devuelve la lista de aristas salientes de un nodo.
        /// </summary>
        public ListaSimple<TArista> ObtenerAristas(TNodo nodo)
        {
            var nodoAdy = BuscarNodoAdy(nodo);
            return nodoAdy != null ? nodoAdy.Aristas : new ListaSimple<TArista>();
        }

        /// <summary>
        /// Devuelve todos los nodos del grafo.
        /// </summary>
        public ListaSimple<TNodo> ObtenerNodos()
        {
            var lista = new ListaSimple<TNodo>();
            foreach (var nodoAdy in nodos.Recorrer().Recorrer())
                lista.AgregarFinal(nodoAdy.Nodo);
            return lista;
        }

        /// <summary>
        /// Busca el nodo de adyacencia correspondiente a un nodo.
        /// </summary>
        private NodoAdy BuscarNodoAdy(TNodo nodo)
        {
            foreach (var n in nodos.Recorrer().Recorrer())
                if (n.Nodo.Equals(nodo))
                    return n;
            return null;
        }

        /// <summary>
        /// Limpia toda la estructura de adyacencia.
        /// </summary>
        public void Limpiar()
        {
            nodos.Limpiar();
        }
    }
}
