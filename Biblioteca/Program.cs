using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

// Clase Libro: Contiene los atributos de un libro.
public class Libro
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string ISBN { get; set; }
    public bool Prestado { get; set; } = false; // Para indicar si el libro está prestado
}

// Clase Nodo: Contiene los atributos relacionados a un nodo para la estructura de datos y el constructor para inicializar el dato de tipo libro a un nodo.
public class Nodo
{
    public Libro Dato { get; set; }
    public Nodo Siguiente { get; set; }

    public Nodo(Libro dato)
    {
        Dato = dato;
    }
}

// Clase Usuario: Contiene los atributos relacionados a los usuarios.
public class Usuario
{
    public string Nombre { get; set; }
    public List<Libro> LibrosPrestados { get; set; } = new List<Libro>(); // Lista de los libros que se le han prestado al usuario.
}

public class Biblioteca
{
    static Nodo cabezaLista = null;
    static List<Usuario> usuarios = new List<Usuario>();
    static string archivoUsuarios = "registros_usuarios.json";

    //---Declaración de Métodos:

    // Método para agregar un libro
    static void AgregarLibro()
    {
        Libro nuevoLibro = new Libro();

        Console.WriteLine("Ingrese el título del libro:");
        nuevoLibro.Titulo = Console.ReadLine();
        Console.WriteLine("Ingrese el autor del libro:");
        nuevoLibro.Autor = Console.ReadLine();
        Console.WriteLine("Ingrese el ISBN del libro:");
        nuevoLibro.ISBN = Console.ReadLine();

        Nodo nuevoNodo = new Nodo(nuevoLibro);
        nuevoNodo.Siguiente = cabezaLista;
        cabezaLista = nuevoNodo;

        Console.WriteLine("Libro agregado correctamente.");
        Console.ReadKey();
    }

    // Método para Mostrar la lista de libros
    static void MostrarLibros()
    {
        Nodo nodoActual = cabezaLista;
        Console.WriteLine("Lista de libros:");
        Console.WriteLine("-----------------");
        int contador = 1;
        while (nodoActual != null)
        {
            Console.WriteLine($"{contador}. {nodoActual.Dato.Titulo} - {nodoActual.Dato.Autor} (ISBN: {nodoActual.Dato.ISBN})");
            contador++;
            nodoActual = nodoActual.Siguiente;
        }
        if (contador == 1)
        {
            Console.WriteLine("No hay libros registrados.");  
        }
        Console.ReadKey();
    }

    // Método para buscar un libro por su titulo
    static Nodo BuscarLibro(string tituloLibro)
    {
        Nodo nodoActual = cabezaLista;
        while (nodoActual != null)
        {
            if (nodoActual.Dato.Titulo.Equals(tituloLibro, StringComparison.OrdinalIgnoreCase))
            {
                return nodoActual; // Se encontró el libro
            }
            nodoActual = nodoActual.Siguiente;
        }
        return null;
    }

    // Método para editar un libro
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
            Console.ReadKey();
        }
    }

    // Método para eliminar un libro
    static void EliminarLibro(string tituloABuscar)
    {
        Nodo nodoAnterior = null;
        Nodo nodoActual = cabezaLista;

        while (nodoActual != null)
        {
            if (nodoActual.Dato.Titulo.Equals(tituloABuscar, StringComparison.OrdinalIgnoreCase))
            {
                // Si es el primer nodo
                if (nodoAnterior == null)
                {
                    cabezaLista = nodoActual.Siguiente;
                }
                else
                {
                    nodoAnterior.Siguiente = nodoActual.Siguiente;
                }
                Console.WriteLine("Libro eliminado correctamente.");
                Console.ReadKey();
                return;
            }
            nodoAnterior = nodoActual;
            nodoActual = nodoActual.Siguiente;
        }
        Console.WriteLine("Libro no encontrado.");
        Console.ReadKey();
    }

    // Método para agregar usuarios
    static void AgregarUsuario()
    {
        Usuario nuevoUsuario = new Usuario();

        Console.WriteLine("Ingrese el nombre del usuario:");
        nuevoUsuario.Nombre = Console.ReadLine();

        usuarios.Add(nuevoUsuario);
        Console.WriteLine("Usuario agregado correctamente.");
        GuardarUsuarios();
        Console.ReadKey();
    }

    // Método para Mostrar Usuarios
    static void MostrarUsuarios()
    {
        Console.WriteLine("Lista de usuarios:");
        Console.WriteLine("-----------------");
        int contador = 1;
        foreach (var usuario in usuarios)
        {
            Console.WriteLine($"{contador}. {usuario.Nombre}");
            contador++;
        }
        if (contador == 1)
        {
            Console.WriteLine("No hay usuarios registrados.");
        }
        Console.ReadKey();
    }

    static void PrestarLibro(string tituloLibro, string nombreUsuario)
    {
        // Buscar el libro por título
        Nodo nodoLibro = BuscarLibro(tituloLibro);
        if (nodoLibro == null)
        {
            Console.WriteLine("El libro no se encuentra en la biblioteca.");
            Console.ReadKey();
            return;
        }

        // Buscar el usuario por nombre
        Usuario usuarioEncontrado = usuarios.FirstOrDefault(u => u.Nombre.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase));
        if (usuarioEncontrado == null)
        {
            Console.WriteLine("El usuario no está registrado.");
            Console.ReadKey();
            return;
        }

        // Verificar si el libro ya está prestado a otro usuario
        if (nodoLibro.Dato.Prestado)
        {
            Console.WriteLine("El libro ya está prestado.");
            Console.ReadKey();
            return;
        }

        // Agregar el libro a la lista de libros prestados del usuario y marcarlo como prestado
        usuarioEncontrado.LibrosPrestados.Add(nodoLibro.Dato);
        nodoLibro.Dato.Prestado = true;

        Console.WriteLine("El libro ha sido prestado a {0}.", nombreUsuario);
        GuardarUsuarios();
        Console.ReadKey();
    }

    // Método para devolver un libro a la biblioteca
    static void DevolverLibro(string tituloLibro, string nombreUsuario)
    {
        // Buscar el usuario por nombre
        Usuario usuarioEncontrado = usuarios.FirstOrDefault(u => u.Nombre.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase));
        if (usuarioEncontrado == null)
        {
            Console.WriteLine("El usuario no está registrado.");
            Console.ReadKey();
            return;
        }

        // Buscar el libro en la lista de libros prestados del usuario
        Libro libroDevuelto = usuarioEncontrado.LibrosPrestados.FirstOrDefault(l => l.Titulo.Equals(tituloLibro, StringComparison.OrdinalIgnoreCase));
        if (libroDevuelto == null)
        {
            Console.WriteLine("El libro no está en la lista de libros prestados del usuario.");
            Console.ReadKey();
            return;
        }

        // Marcar el libro como no prestado y removerlo de la lista de libros prestados del usuario
        libroDevuelto.Prestado = false;
        usuarioEncontrado.LibrosPrestados.Remove(libroDevuelto);

        Console.WriteLine("El libro ha sido devuelto correctamente.");
        GuardarUsuarios();
        Console.ReadKey();
    }

    static void ExportarRegistros()
    {
        string jsonUsuarios = JsonConvert.SerializeObject(usuarios, Formatting.Indented);

        File.WriteAllText(archivoUsuarios, jsonUsuarios);
        Console.WriteLine("Registros exportados correctamente.");
        Console.ReadKey();
    }

    static void GuardarUsuarios()
    {
        string jsonUsuarios = JsonConvert.SerializeObject(usuarios, Formatting.Indented);

        File.WriteAllText(archivoUsuarios, jsonUsuarios);
    }

    static void CargarUsuarios()
    {
        if (File.Exists(archivoUsuarios))
        {
            string jsonUsuarios = File.ReadAllText(archivoUsuarios);
            usuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonUsuarios);
        }
    }

    // Menú principal
    static void Main(string[] args)
    {
        CargarUsuarios();

        int opcion;
        do
        {
            Console.Clear();
            Console.WriteLine("1. Agregar libro");
            Console.WriteLine("2. Editar libro");
            Console.WriteLine("3. Eliminar libro");
            Console.WriteLine("4. Agregar usuario");
            Console.WriteLine("5. Prestar libro");
            Console.WriteLine("6. Devolver libro"); // Nueva opción
            Console.WriteLine("7. Exportar registros a JSON");
            Console.WriteLine("8. Mostrar lista de libros");
            Console.WriteLine("9. Mostrar lista de usuarios");
            Console.WriteLine("10. Salir");
            Console.Write("Seleccione una opción: ");
            opcion = int.Parse(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    Console.Clear();
                    AgregarLibro();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Ingrese el título del libro a editar:");
                    string tituloEditar = Console.ReadLine();
                    Console.WriteLine("Ingrese el nuevo título:");
                    string nuevoTitulo = Console.ReadLine();
                    Console.WriteLine("Ingrese el nuevo autor:");
                    string nuevoAutor = Console.ReadLine();
                    Console.WriteLine("Ingrese el nuevo ISBN:");
                    string nuevoISBN = Console.ReadLine();
                    EditarLibro(tituloEditar, nuevoTitulo, nuevoAutor, nuevoISBN);
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("Ingrese el título del libro a eliminar:");
                    string tituloEliminar = Console.ReadLine();
                    EliminarLibro(tituloEliminar);
                    break;
                case 4:
                    Console.Clear();
                    AgregarUsuario();
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("Ingrese el título del libro que desea prestar:");
                    string tituloLibro = Console.ReadLine();
                    Console.WriteLine("Ingrese el nombre del usuario:");
                    string nombreUsuario = Console.ReadLine();
                    PrestarLibro(tituloLibro, nombreUsuario);
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("Ingrese el título del libro que desea devolver:");
                    string tituloDevolver = Console.ReadLine();
                    Console.WriteLine("Ingrese el nombre del usuario:");
                    string nombreDevolverUsuario = Console.ReadLine();
                    DevolverLibro(tituloDevolver, nombreDevolverUsuario);
                    break;
                case 7:
                    Console.Clear();
                    ExportarRegistros();
                    break;
                case 8:
                    Console.Clear();
                    MostrarLibros();
                    break;
                case 9:
                    Console.Clear();
                    MostrarUsuarios();
                    break;
                case 10:
                    Console.Clear();
                    Console.WriteLine("Saliendo del programa...");
                    break;
                default:
                    Console.WriteLine("Opción no válida, intente nuevamente.");
                    Console.ReadKey();
                    break;
            }
        } while (opcion != 10);
    }
}



