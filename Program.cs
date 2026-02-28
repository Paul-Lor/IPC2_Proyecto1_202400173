using System;
using IPC2_Proyecto1_202400173.Modelos;
using IPC2_Proyecto1_202400173.Servicios;
using IPC2_Proyecto1_202400173.Logica;
using IPC2_Proyecto1_202400173.Utilidades;

namespace IPC2_Proyecto1_202400173
{
    class Program
    {
        static ListaDoblePacientes listaPacientes = new ListaDoblePacientes();
        static LectorXML lector = new LectorXML();
        static Simulador simulador = new Simulador();
        static GeneradorGrafico graficador = new GeneradorGrafico();

        static void Main(string[] args)
        {
            bool continuar = true;
            while (continuar)
            {
                MostrarMenu();
                string? entrada = Console.ReadLine();
                if (int.TryParse(entrada, out int opcion))
                {
                    if (opcion == 6) { continuar = TerminarPrograma(); }
                    else { EjecutarOpcion(opcion); }
                }
            }
        }

        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("=== SISTEMA EPIDEMIOLÓGICO - 202400173 ===");
            Console.WriteLine("1. Cargar archivo XML");
            Console.WriteLine("2. Análisis Manual (Paso a paso)");
            Console.WriteLine("3. Simulación Automática (Diagnóstico)");
            Console.WriteLine("4. Generar archivo XML de salida");
            Console.WriteLine("5. Limpiar memoria");
            Console.WriteLine("6. Salir");
            Console.Write("Seleccione una opción: ");
        }

        static void EjecutarOpcion(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    FuncionCargarArchivo();
                    break;
                case 2:
                    FuncionAnalisisManual();
                    break;
                case 3:
                    FuncionSimulacionAutomatica();
                    break;
                case 5:
                    listaPacientes.Limpiar();
                    Console.WriteLine("Memoria limpia.");
                    break;
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static void FuncionCargarArchivo()
        {
            Console.Write("Ruta del archivo: ");
            string? ruta = Console.ReadLine();
            if (!string.IsNullOrEmpty(ruta)) lector.CargarPacientes(ruta, listaPacientes);
        }

        static void FuncionAnalisisManual()
        {
            Console.Write("Nombre del paciente: ");
            string? nombre = Console.ReadLine();
            Paciente? p = listaPacientes.BuscarPorNombre(nombre ?? "");

            if (p != null)
            {
                graficador.GenerarMatrizDot(p.RejillaActual, p.M, p.Nombre, 0);
                Console.WriteLine("Estado inicial generado en Graphviz.");
                
                Console.WriteLine("¿Desea avanzar un periodo? (s/n)");
                if (Console.ReadLine()?.ToLower() == "s")
                {
                    p.RejillaActual = simulador.GenerarSiguientePeriodo(p.RejillaActual, p.M);
                    Console.WriteLine("Periodo 1 calculado.");
                }
            }
            else Console.WriteLine("Paciente no encontrado.");
        }

        static void FuncionSimulacionAutomatica()
        {
            Console.Write("Nombre del paciente: ");
            string? nombre = Console.ReadLine();
            Paciente? p = listaPacientes.BuscarPorNombre(nombre ?? "");

            if (p != null)
            {
                ListaDobleFilas actual = p.RejillaActual;
                Console.WriteLine("Iniciando simulación automática...");
                // Lógica de detección de patrones...
            }
        }

        static bool TerminarPrograma()
        {
            Console.WriteLine("Saliendo...");
            return false;
        }
    }
}