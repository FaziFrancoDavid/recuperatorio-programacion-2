using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace TrabajoRecuperatorio_FAZI
{
    internal class Program
    {
        static void Menu()
        {
            Console.WriteLine("╔══════════════════╗");
            Console.WriteLine("║ Elija una opción ║");
            Console.WriteLine("║ A) Agregar       ║");
            Console.WriteLine("║ B) Buscar        ║");
            Console.WriteLine("║ L) Listar        ║");
            Console.WriteLine("║ S) Salir         ║");
            Console.WriteLine("╚══════════════════╝");
        }

        static void Main(string[] args)
        {
            String Opcion;
            do
            {
                Menu();
                //Se solicita el ingreso de una opcion y se la pasa a mayusculas *
                Opcion = Console.ReadLine().ToUpper();
                while (Opcion != "A" && Opcion != "B" && Opcion != "L" && Opcion != "S")
                {
                    Console.WriteLine("\nIngrese una opcion valida. Presione cualquier tecla para volver a intentar.");
                    Console.ReadKey();
                    Console.Clear();
                    Menu();
                    Opcion = Console.ReadLine();
                }
                Console.Clear();
                switch (Opcion)
                {
                    case "A":
                        Agregar();
                        Console.Clear();
                        break;
                    case "B":
                        Buscar();
                        Console.Clear();
                        break;
                    case "L":
                        Listar();
                        Console.Clear();
                        break;
                    case "S":
                        break;
                }
            } while (Opcion != "S");
            Console.WriteLine("Programa terminado.Gracias!");
            Console.ReadKey();
        }

        //FUNCIONES
        static void Agregar()
        {
            FileStream Archivo;
            StreamWriter Grabar;
            long stockActual, stockMinimo;
            decimal precio;
            string codigo, descripcion, marca;
            //Pedimos el codigo del producto y lo validamos
            codigo = ValidarCodigo();
            Console.Clear();
            //Pedimos la descripcion del producto y lo validamos
            do
            {
                Console.Write("Ingrese la descripcion del producto: ");
                descripcion = Console.ReadLine();
            } while (ValidarLenght(descripcion, 15) == false);
            Console.Clear();

            //Pedimos el stock actual del producto y lo validamos
            Console.Write("Ingrese el stock actual: ");
            stockActual = ValidarLargo("stock actual");
            Console.Clear();

            //Pedimos el stock minimo del producto y lo validamos
            Console.Write("Ingrese el stock minimo: ");
            stockMinimo = ValidarLargo("stock minimo");
            Console.Clear();

            //Pedimos la marca del producto y lo validamos
            do
            {
                Console.Write("Ingrese la marca del producto: ");
                marca = Console.ReadLine();
            } while (ValidarLenght(marca, 10) == false);
            Console.Clear();

            //Pedimos el precio unitario del producto y lo validamos
            Console.Write("Ingrese el precio unitario del producto: ");
            precio = ValidarDecimal("precio unitario");
            Console.Clear();

            //Graba los datos ingresados en una linea dentro de ferreteria.txt
            Archivo = new FileStream("ferreteria.txt", FileMode.Append);
            Grabar = new StreamWriter(Archivo);
            Grabar.WriteLine($"{codigo};{descripcion};{stockActual};{stockMinimo};{marca};{precio};");
            Grabar.Close();
            Archivo.Close();
        }

        static void Buscar()
        {
            FileStream Archivo;
            StreamReader Leer;
            string cadena;
            string[] datos;
            int contador = 0;
            bool error = false;
            decimal sumaPrecios = 0;

            //Corroboramos si el archivo existe
            if (File.Exists("ferreteria.txt"))
            {
                Archivo = new FileStream("ferreteria.txt", FileMode.Open);
                Leer = new StreamReader(Archivo);
                string marca;
                //Se pide el ingreso de la marca a buscar en el archivo
                Console.Write("Ingrese la marca a buscar: ");
                marca = Console.ReadLine();
                Console.Clear();
                //Se dibuja la parte superior del recuadro
                recuadroArriba();
                //Mientras no se haya llegado al fin del archivo se leen las lineas
                while (Leer.EndOfStream == false)
                {
                    //En caso de que hayan lineas no validas dentro del archivo se trata de capturar el error
                    try
                    {
                        cadena = Leer.ReadLine();
                        datos = cadena.Split(';');
                        //Se corroboramos que la marca en la linea que se leyó coincida con la que se busca
                        if (datos[4].ToLower() == marca.ToLower())
                        {
                            //Se muestran los datos respetando los limites de caracteres y alineacion
                            Console.WriteLine($"│{datos[0],7}│{datos[1],-15}│{datos[2],5}│{datos[3],5}│{datos[4],-10}│{datos[5],8}│");
                            //Se lleva cuenta de las coincidencias
                            contador++;

                            sumaPrecios += decimal.Parse(datos[5]);
                        }
                        //Si se leyó la ultima linea de ferreteria.txt se dibuja la parte inferior del recuadro
                        if (Leer.EndOfStream == true)
                        {
                            recuadoAbajo();
                        }
                    }
                    catch
                    {
                        Console.Clear();
                        error = true;
                        Console.WriteLine("Error. Dentro del archivo ferreteria.txt hay lineas vacías o mal formateadas.\n\nPresione cualquier tecla para volver al menu.");
                        break;
                    }
                }

                //Si no hubo ningun error se muestra la cantidad de coincidencias encontradas
                if (error == false)
                {
                    decimal precioPromedio = sumaPrecios / contador;
                    Console.WriteLine($"El Precio promedio de los articulos de la lista es: {precioPromedio}");
                }
                Console.ReadKey();
                Leer.Close();
                Archivo.Close();
            }
            else
            //Si el archivo no existe tira error
            {
                Console.WriteLine("Error. El Archivo no existe.\nPresione cualquier tecla para volver al menu.");
                Console.ReadKey();
            }
        }

        static void Listar()
        {
            FileStream Archivo;
            StreamReader Leer;
            string cadena;
            string[] datos;
            int contador = 0;
            decimal sumaPrecios = 0; // Variable para sumar los precios

            // Comprueba si el archivo existe
            if (File.Exists("ferreteria.txt"))
            {
                Archivo = new FileStream("ferreteria.txt", FileMode.Open);
                Leer = new StreamReader(Archivo);

                // Se dibuja la parte superior del recuadro de ferreteria
                recuadroArriba();

                // Comprueba que no se haya llegado al final del archivo
                while (Leer.EndOfStream == false)
                {
                    // En caso de que hayan líneas no válidas dentro del archivo, se trata de capturar el error
                    try
                    {
                        // Se lleva cuenta de los productos existentes
                        contador++;

                        cadena = Leer.ReadLine();
                        datos = cadena.Split(';');

                        // Se suma el precio al acumulador
                        sumaPrecios += decimal.Parse(datos[5]);

                        // Se muestran los datos respetando los límites de caracteres y alineación
                        Console.WriteLine($"│{datos[0],7}│{datos[1],-15}│{datos[2],5}│{datos[3],5}│{datos[4],-10}│{datos[5],8}│");

                        // Si se leyó la última línea de ferreteria.txt se dibuja la parte inferior del recuadro
                        if (Leer.EndOfStream == true)
                        {
                            recuadoAbajo();
                        }
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Error. Dentro del archivo ferreteria.txt hay líneas vacías o mal formateadas.\n\nPresione cualquier tecla para volver al menú.");
                        break;
                    }
                }

                // Se calcula y muestra el precio promedio si hay al menos un elemento en la lista
                if (contador > 0)
                {
                    decimal precioPromedio = sumaPrecios / contador;
                    Console.WriteLine($"El Precio promedio de los articulos de la lista es: {precioPromedio}");
                }

                Console.WriteLine("Presione cualquier tecla para volver al menú.");
                Console.ReadKey();
                Leer.Close();
                Archivo.Close();
            }
            else
            {
                // Si el archivo no existe, muestra un error
                Console.WriteLine("Error. El Archivo no existe.\nPresione cualquier tecla para volver al menú.");
                Console.ReadKey();
            }
        }


        //VALIDACIONES PROCEDIMIENTOS.

        static bool ValidarLenght(string cadena, int largo)
        {
            //Verifica si la cadena proporcionada es mas larga que lo indicado en la invocacion de la función
            if (cadena.Length > largo)
            {
                Console.WriteLine($"Ingrese hasta {largo} caracteres. Presione cualquier tecla para volver a intentar.");
                Console.ReadKey();
                Console.Clear();
                return false;
            }
            else
            {
                return true;
            }
        }

        static string ValidarCodigo()
        {
            Console.Write("Ingrese un codigo donde los primeros tres caracteres son letras y el resto son digitos, sin guion ni otro caracter: \n");
            //Console.Write("Ingrese el codigo del producto: ");
            string aux1 = Console.ReadLine();
            string letras = "abcdefghijklmnñopqrstuvwxyz";
            string[] aux2;
            int numeros;
            string codigo;
            int total;
            //Corroboramos que se ingresen 6 caracteres
            while (aux1.Length != 6)
            {
                Console.Clear();
                Console.WriteLine("Error. vuelva a intentarlo.");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("Ingrese un codigo de 6 caracteres, sin guiones. Presione cualquier tecla para volver a intentar.");
                Console.ReadKey();
                Console.Clear();
                Console.Write("Ingrese el codigo del producto: ");
                aux1 = Console.ReadLine();
            }
            aux1 = aux1.ToLower();
            codigo = $"{aux1[0]}{aux1[1]}{aux1[2]}-{aux1[3]}{aux1[4]}{aux1[5]}";
            aux2 = codigo.Split('-');
            //Corroboramos que los tres primeros caracteres sean letras
            for (int i = 0; i < 3; i++)
            {
                total = 0;
                for (int n = 0; n < letras.Length; n++)
                {
                    if (aux2[0][i] == letras[n])
                    {
                        total++;
                    }
                }
                if (total == 0)
                {
                    Console.WriteLine("\nIngrese un codigo donde los primeros tres caracteres son letras y el resto son digitos. Por ejemplo: ABC123");
                    Console.ReadKey();
                    Console.Clear();
                    return ValidarCodigo();
                }
            }
            //Corroboramos que los ultimos tres caracteres sean digitos, sino vuelve a ejecutar la funcion
            if (int.TryParse(aux2[1], out numeros) == false)
            {
                Console.WriteLine("\nIngrese un codigo donde los primeros tres caracteres son letras y el resto son digitos. Por ejemplo: ABC-123 (sin el guion)");
                Console.ReadKey();
                Console.Clear();
                return ValidarCodigo();
            }
            //Si se cumplen todos los requerimientos se devuelve el codigo en mayusculas
            codigo = codigo.ToUpper();
            return codigo;
        }

        static long ValidarLargo(string valor)
        {
            long numero;
            //Compruea que el valor ingresado sea un entero, sino vuelve a pedir el ingreso
            while (long.TryParse(Console.ReadLine(), out numero) == false)
            {
                Console.WriteLine($"\nIngrese un {valor} valido. Presione cualquier tecla para volver a intentar.");
                Console.ReadKey();
                Console.Clear();
                Console.Write($"Ingrese el {valor} de nuevo: ");
            }
            //Corroboramos que se hayan ingresado 5 caracteres o menos, de no ser así se vuelve a ejecutar la unción
            if (ValidarLenght(numero.ToString(), 5) == false)
            {
                Console.Write($"Ingrese el {valor} de nuevo: ");
                return ValidarLargo(valor);
            }
            return numero;
        }


        static decimal ValidarDecimal(string valor)
        {
            decimal numero;
            //Compruea que el valor ingresado sea un numero decimal, sino vuelve a pedir el ingreso
            while (decimal.TryParse(Console.ReadLine(), out numero) == false)
            {
                Console.WriteLine($"\nIngrese un {valor} valido. Presione cualquier tecla para volver a intentar.");
                Console.ReadKey();
                Console.Clear();
                Console.Write($"Ingrese el {valor} de nuevo: ");
            }
            //Corroboramos que se hayan ingresado 8 caracteres o menos, de no ser así se vuelve a ejecutar la unción
            if (ValidarLenght(numero.ToString(), 8) == false)
            {
                Console.Write($"Ingrese el {valor} de nuevo: ");
                return ValidarDecimal(valor);
            }
            return numero;
        }

        //recuadro

        static void recuadroArriba()
        {
            //Dibuja la parte superior de la recuadro de la ferreteria
            Console.Write("┌");
            Console.Write("───────┬───────────────┬─────┬─────┬──────────┬────────");
            Console.WriteLine("┐");
            Console.WriteLine("│Codigo │Descripcion    │St.A.│St.M.│Marca     │P.U.    │");
            Console.Write("├");
            Console.Write("───────┼───────────────┼─────┼─────┼──────────┼────────");
            Console.WriteLine("┤");
        }
        static void recuadoAbajo()
        {
            //Dibuja la parte inferior de la recuadro de la ferreteria
            Console.Write("└");
            Console.Write("───────┴───────────────┴─────┴─────┴──────────┴────────");
            Console.WriteLine("┘");
        }
    }
}
