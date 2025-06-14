using System;
using System.Collections.Generic;

namespace DLLForUnityStandart
{

    // Clase NodoDll: representa cada elemento de la lista genérica
    public class NodoDll<T> // NodoDll genérico
    {
        public T dato; // Dato del NodoDll
        public NodoDll<T> siguiente; // Referencia al siguiente NodoDll

        public NodoDll(T valor) // Constructor del NodoDll
        {
            dato = valor;     // Asigna el valor al dato
            siguiente = null; // Inicializa el siguiente NodoDll como null
        }
    }

    // Clase ListaSimple: lista enlazada simple genérica
    public class ListaSimple<T>
    {
        private NodoDll<T> primero;

        public ListaSimple()
        {
            primero = null;
        }

        // Inserta al final por defecto
        public void Insertar(T valor)
        {
            AgregarFinal(valor);
        }

        // Inserta un NodoDll al final de la lista
        public void AgregarFinal(T valor)
        {
            NodoDll<T> nuevo = new NodoDll<T>(valor);

            if (primero == null)
            {
                primero = nuevo;
            }
            else
            {
                NodoDll<T> actual = primero;
                while (actual.siguiente != null)
                {
                    actual = actual.siguiente;
                }
                actual.siguiente = nuevo;
            }
        }

        // Devuelve todos los elementos como lista (recorrido)
        public List<T> Recorrer()
        {
            List<T> elementos = new List<T>();
            NodoDll<T> actual = primero;
            while (actual != null)
            {
                elementos.Add(actual.dato);
                actual = actual.siguiente;
            }
            return elementos;
        }

        // Retorna el elemento en el índice especificado

        public void ReemplazaEn(int indice, T nuevoValor)
        {
            if (indice < 0)
                throw new ArgumentOutOfRangeException();

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
            if (indice < 0)
                throw new ArgumentOutOfRangeException("Índice negativo no válido.");

            NodoDll<T> actual = primero;
            int i = 0;
            while (actual != null)
            {
                if (i == indice)
                    return actual.dato; // Aquí devuelve el objeto Nodo completo
                actual = actual.siguiente;
                i++;
            }

            throw new ArgumentOutOfRangeException("Índice fuera de rango.");
        }



        // Devuelve el índice del primer elemento igual a 'valor'
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

        // Verifica si está vacía
        public bool EstaVacia()
        {
            return primero == null;
        }

        // Devuelve el tamaño
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

        // Limpia la lista
        public void Limpiar()
        {
            primero = null;
        }

        // Elimina NodoDll en índice específico
        public void EliminarEn(int indice)
        {
            if (indice < 0 || EstaVacia())
                throw new ArgumentOutOfRangeException();

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

        // Elimina el primer NodoDll con el valor especificado
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
    }


    // Lista ejecutable cm colas 
    public class Cola<T>
    {
        private NodoDll<T> frente;
        private NodoDll<T> fin;

        public Cola()
        {
            frente = null;
            fin = null;
        }

        public bool EstaVacia()
        {
            return frente == null;
        }

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

    // lista ejecutable cm pilas

    public class Pila<T>
    {
        private NodoDll<T> tope;

        public Pila()
        {
            tope = null;
        }

        public bool EstaVacia()
        {
            return tope == null;
        }

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


    // NodoDll para lista doblemente enlazada
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

        public bool EstaVacia()
        {
            return cabeza == null;
        }

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


    // NodoDll para Array Dinámico
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
    // Clase Array Dinámico
    public class ArrayDinamico<T>
    {
        private NodoDllArray<T> primero;
        private int contador;

        public ArrayDinamico()
        {
            primero = null;
            contador = 0;
        }

        public int Tamano()
        {
            return contador;
        }

        public void Agregar(T valor)
        {
            NodoDllArray<T> nuevo = new NodoDllArray<T>(valor);
            if (primero == null)
            {
                primero = nuevo;
            }
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

    // NodoDll pera Diccionario
    public class NodoDllDiccionario<K, V>
    {
        public K clave;
        public V valor;
        public NodoDllDiccionario<K, V> siguiente;

        public NodoDllDiccionario(K c, V v)
        {
            clave = c;
            valor = v;
            siguiente = null;
        }
    }
    // Clase Diccionario


    public class Diccionario<K, V>
    {
        private List<K> claves = new List<K>();
        private List<V> valores = new List<V>();

        public void AgregarOActualizar(K clave, V valor)
        {
            int index = claves.IndexOf(clave);
            if (index >= 0)
            {
                valores[index] = valor;
            }
            else
            {
                claves.Add(clave);
                valores.Add(valor);
            }
        }

        public bool ContieneClave(K clave)
        {
            return claves.Contains(clave);
        }

        public V Obtener(K clave)
        {
            int index = claves.IndexOf(clave);
            if (index >= 0)
                return valores[index];
            throw new KeyNotFoundException();
        }

        public void Eliminar(K clave)
        {
            int index = claves.IndexOf(clave);
            if (index >= 0)
            {
                claves.RemoveAt(index);
                valores.RemoveAt(index);
            }
        }

        public IEnumerable<K> RecorrerClaves()
        {
            foreach (K clave in claves)
            {
                yield return clave;
            }
        }
    }


}
