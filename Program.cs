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
        static EscritorXML escritor = new EscritorXML();
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
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
            ╔══════════════════════════════════════════════════════╗
            ║        SISTEMA DE ANÁLISIS EPIDEMIOLÓGICO            ║
            ║             Registro: 202400173                      ║
            ╚══════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("\n  [:v] MENÚ PRINCIPAL:");
            Console.WriteLine("  ────────────────────────────────────────");
            
            Console.WriteLine("  [1] Cargar Pacientes (XML)");
            Console.WriteLine("  [2] Análisis Manual (Paso a paso)");
            Console.WriteLine("  [3] Simulación Automática (Diagnóstico)");
            Console.WriteLine("  [4] Generar Reporte de Salida (XML)");
            Console.WriteLine("  [5] Limpiar Memoria");
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  [6] Salir del Programa");
            Console.ResetColor();
            
            Console.WriteLine("  ────────────────────────────────────────");
            Console.Write("  > Seleccione una opción: ");
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
                case 4:
                    FuncionGenerarSalida();
                    break;
                case 5:
                    listaPacientes.Limpiar();
                    Console.WriteLine("Memoria limpia satisfactoriamente.");
                    break;
            }
            if (opcion != 6)
            {
                PresionarParaContinuar();
                Console.ReadKey();
            }
        }

        static void PresionarParaContinuar()
        {
            Console.WriteLine("\n-----------------------------------------");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Presione cualquier tecla para volver al menú...");
            Console.ResetColor();
        }

        static void FuncionCargarArchivo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nIniciando lectura de archivo...");
            Console.ResetColor();
            Console.Write("\nIngrese la ruta del archivo XML: ");
            string? ruta = Console.ReadLine();
            if (!string.IsNullOrEmpty(ruta)) 
            {
                lector.CargarPacientes(ruta, listaPacientes);
            }
        }

        static void FuncionAnalisisManual()
        {
            Console.Write("\nIngrese el nombre del paciente: ");
            string? nom = Console.ReadLine();
            Paciente? p = listaPacientes.BuscarPorNombre(nom ?? "");

            if (p == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" [X] Error: Paciente no encontrado.");
                Console.ResetColor();
                return;
            }



            // Rejilla inicial para tener una referencia base (N)
            ListaDobleFilas rejillaOriginal = p.RejillaActual; 
            ListaDobleFilas? rejillaAnterior = null; // Para detectar Mortal (N1=1)
            int perActual = 0;
            string input = "";

            while (input != "salir")
            {
                Console.Clear();
                int sanas, contagiadas;
                ContarEstados(p.RejillaActual, out sanas, out contagiadas);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╔══════════════════════════════════════════════════════╗");
                Console.WriteLine("║                LABORATORIO DE ANÁLISIS               ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════╝");
                Console.ResetColor();

                Console.WriteLine($"  Paciente: {p.Nombre.PadRight(20)} Periodo: {perActual}");
                Console.WriteLine($"  Sanas: {sanas} | Contagiadas: {contagiadas}");
                Console.WriteLine("  ──────────────────────────────────────────────────────");


                // --- Detectar Alertas por periodo ---
                if (perActual > 0 && rejillaAnterior != null)
                {
                    // 1. Detección de MORTAL (N1 = 1): ¿es igual al periodo anterior?
                    if (simulador.SonIdenticas(p.RejillaActual, rejillaAnterior, p.M))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"  DIAGNÓSTICO: MORTAL DETECTADO (N1 = 1)");
                        Console.WriteLine($"     La enfermedad se ha estabilizado en el periodo {perActual - 1}.");
                        Console.ResetColor();
                        p.Resultado = "MORTAL";
                        p.N = perActual - 1; 
                        p.N1 = 1;
                    }
                    // 2. Detección de GRAVE (Regresó al inicial):
                    else if (simulador.SonIdenticas(p.RejillaActual, rejillaOriginal, p.M))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"  DIAGNÓSTICO: GRAVE / OSCILANTE");
                        Console.WriteLine($"     La muestra regresó al estado inicial (N = {perActual}).");
                        Console.ResetColor();
                        p.Resultado = "GRAVE";
                        p.N = 0;
                        p.N1= perActual;
                    }
                }

                // --- Generar gráfica con graphviz ---
                graficador.GenerarMatrizDot(p.RejillaActual, p.M, p.Nombre, perActual);
                Console.WriteLine("\n  [Generando visualización .dot ...]");
                
                // --- Manejo de entrada ---
                Console.WriteLine("\n  ──────────────────────────────────────────────────────");
                Console.WriteLine("  [ ENTER ] -> Siguiente periodo");
                Console.WriteLine("  [ SALIR ] -> Finalizar análisis");
                Console.Write("\n  Acción: ");
                
                input = Console.ReadLine()?.ToLower() ?? "";

                if (input != "salir")
                {
                    // Avanzar al siguiente periodo pero guardando el estado actual para comparación
                    rejillaAnterior = p.RejillaActual;
                    p.RejillaActual = simulador.GenerarSiguientePeriodo(p.RejillaActual, p.M);
                    perActual++;
                }
            }
            p.RejillaActual = rejillaOriginal; // Restaurar estado original al finalizar
        }

        static void FuncionSimulacionAutomatica()
        {
            Console.Write("\nNombre del paciente para diagnóstico: ");
            string? nombre = Console.ReadLine();
            Paciente? p = listaPacientes.BuscarPorNombre(nombre ?? "");

            if (p == null) { 
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Paciente no encontrado.");
                Console.ResetColor(); 
                return; 
            }

            Console.CursorVisible = false;
            Console.WriteLine($"\nIniciando secuenciación genómica: {p.Nombre}");
            
            char[] spinner = { '|', '/', '-', '\\' }; // Spinner para animación
            int spinnerIndex = 0;

            // Historial para detección de ciclos
            NodoHistorial? cabezaHistorial = new NodoHistorial(0, p.RejillaActual);
            ListaDobleFilas actual = p.RejillaActual;
            
            bool detectado = false;
            int limite = p.PeriodosMaximos; 

            for (int n = 1; n <= limite; n++)
            {

                Console.Write($"\r    [{spinner[spinnerIndex]}] Analizando periodo: {n} ... ");
                spinnerIndex = (spinnerIndex + 1) % spinner.Length;

                Thread.Sleep(500); // Pausa de medio segundo

                ListaDobleFilas siguiente = simulador.GenerarSiguientePeriodo(actual, p.M);
                
                // Comparar con el historial
                NodoHistorial? tempH = cabezaHistorial;
                while (tempH != null)
                {
                    if (simulador.SonIdenticas(siguiente, tempH.RejillaSnapshot, p.M))
                    {
                        int periodoDondeAparecio = tempH.Periodo;
                        int n1 = n - periodoDondeAparecio;

                        if (periodoDondeAparecio == 0) // Repitió el inicial
                        {
                            p.Resultado = (n == 1) ? "MORTAL" : "GRAVE";
                            p.N = n;
                            p.N1 = 0;
                        }
                        else // Ciclo posterior
                        {
                            p.Resultado = (n1 == 1) ? "MORTAL" : "GRAVE";
                            p.N = periodoDondeAparecio;
                            p.N1 = n1;
                        }

                        Console.WriteLine("\n\n  ┌──────────────────────────────────────────┐");
                        Console.ForegroundColor = p.Resultado == "mortal" ? ConsoleColor.Red : ConsoleColor.Yellow;
                        Console.WriteLine($"  │  DIAGNÓSTICO: {p.Resultado.ToUpper().PadRight(26)} │");
                        Console.ResetColor();
                        Console.WriteLine($"  │  N  encontrado: {p.N.ToString().PadRight(24)} │");
                        Console.WriteLine($"  │  N1 encontrado: {p.N1.ToString().PadRight(24)} │");
                        Console.WriteLine("  └──────────────────────────────────────────┘");
                        detectado = true;
                        break;
                    }
                    tempH = tempH.Siguiente;
                }

                if (detectado) break;

                // Agregar al historial (Insertar al inicio para optimizar búsqueda reciente)
                NodoHistorial nuevoH = new NodoHistorial(n, siguiente);
                nuevoH.Siguiente = cabezaHistorial;
                cabezaHistorial = nuevoH;
                actual = siguiente;
            }

            if (!detectado)
            {
                p.Resultado = "LEVE";
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n\nResultado: LEVE. El virus se disipó o es inofensivo.");
                Console.ResetColor();
            }
            Console.CursorVisible = true; 
        }

        static void FuncionGenerarSalida()
        {
            Console.Write("\nIngrese el nombre/ruta para el XML de salida: ");
            string? ruta = Console.ReadLine();
            if (!string.IsNullOrEmpty(ruta))
            {
                escritor.GenerarArchivoSalida(ruta, listaPacientes);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Archivo generado exitosamente.");
                Console.ResetColor();
            }
        }

        static void ContarEstados(ListaDobleFilas rejilla, out int sanas, out int contagiadas) 
        {
            sanas = 0; contagiadas = 0;
            NodoFila? f = rejilla.Cabeza;
            while (f != null) {
                NodoCelda? c = f.ListaColumnas.Cabeza;
                while (c != null) {
                    if (c.Estado == 1) contagiadas++; else sanas++;
                    c = c.Siguiente;
                }
                f = f.Siguiente;
            }
        }

        static bool TerminarPrograma()
        {
            Console.WriteLine("¿Desea cerrar la aplicación? (s/n)");
            return Console.ReadLine()?.ToLower() != "s";
        }
    }
}