namespace Proyecto_1
{
    class Program
    {
        static void Main(string[] args)
        {
            int opcion = 0;
            while (opcion != 6)
            {
                MostrarMenu();
                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    ProcesarOpcion(opcion);
                }
            }
        }

        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("=== LABORATORIO EPIDEMIOLÓGICO DE GUATEMALA ===");
            Console.WriteLine("1. Cargar archivo XML");
            Console.WriteLine("2. Seleccionar Paciente y analizar");
            Console.WriteLine("3. Simulación Automática (Mortal/Grave/Leve)");
            Console.WriteLine("4. Generar archivo XML de salida");
            Console.WriteLine("5. Limpiar memoria");
            Console.WriteLine("6. Salir");
            Console.Write("Seleccione una opción: ");
        }

        static void ProcesarOpcion(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    // Cargar archivo XML (simulado)
                    Console.WriteLine("Cargando archivo XML...");
                    break;
                case 2:
                    // Seleccionar paciente y analizar
                    Console.WriteLine("Seleccionando paciente...");
                    break;
                case 3:
                    // Lógica de periodos
                    Console.WriteLine("Simulación automática...");
                    break;
                case 5:
                    // Limpiar memoria
                    Console.WriteLine("Limpiando memoria...");
                    break;
                case 6:
                    Console.WriteLine("Saliendo del programa...");
                    break;
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        
    }
}