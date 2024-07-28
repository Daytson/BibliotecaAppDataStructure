
using System.Runtime.InteropServices.ObjectiveC;

namespace BibliotecaApp
{
    //---Declaración de clases:

    //Clase Libro que contendrá los atributos y la informacion general de un libro.
    public class Libro
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
    }

    //Clase Nodo que se utilizará para la estructura de datos de la aplicacion.
    public class Nodo
    {
        public Libro Dato { get; set; }
        public Nodo Siguiente { get; set; }

        public Nodo(Libro dato)
        {
            Dato = dato;
        }
    }

    //Fin de la decalaración de clases---.


    public class Biblioteca
    {
        static Nodo cabezaLista = null;

        //---Declaración de Métodos:

        static void AgregarLibro()
        {
            Libro nuevoLibro = new Libro();

            nuevoLibro.Titulo = Console.ReadLine();
            nuevoLibro.Autor = Console.ReadLine();
            nuevoLibro.ISBN = Console.ReadLine();

            Nodo nuevoNodo = new Nodo(nuevoLibro);
            nuevoNodo.Siguiente = cabezaLista;

            cabezaLista = nuevoNodo;

        }

        static Nodo BuscarLibro(string tituloLibro)
        {
            Nodo nodoActual = cabezaLista;
            while (nodoActual != null)
            {
                if (nodoActual.Dato.Titulo == tituloLibro)
                {
                    return nodoActual; //se encontró el libro
                }
                nodoActual = nodoActual.Siguiente;
            }
            return null;
        }

        static void EditarLibro(string tituloABuscar, string nuevoTitulo, string nuevoAutor, string nuevoISBN)
        {
            Nodo nodoEncontrado = BuscarLibro(tituloABuscar);
            if (nodoEncontrado != null)
            {
                nodoEncontrado.Dato.Titulo = nuevoTitulo;
                nodoEncontrado.Dato.Autor = nuevoAutor;
                nodoEncontrado.Dato.ISBN = nuevoISBN;
                Console.WriteLine("Libro modificado correctamente.");
            }
            else
            {
                Console.WriteLine("Libro no encontrado.");
            }
        }

        static void EliminarLibro(string tituloABuscar)
        {
            Nodo nodoAnterior = null;
            Nodo nodoActual = cabezaLista;

            while (nodoActual != null)
            {
                if (nodoActual.Dato.Titulo == tituloABuscar)
                {
                    //Si es el primer nodo
                    if (nodoAnterior == null)
                    {
                        cabezaLista = nodoActual.Siguiente;
                    }
                    else
                    {
                        nodoAnterior.Siguiente = nodoActual.Siguiente;
                    }
                    Console.WriteLine("Libro eliminado correctamente.");
                    return;
                }
                nodoAnterior = nodoActual;
                nodoActual = nodoActual.Siguiente;
            }
            Console.WriteLine("Libro no encontrado.");
        }

    }
}