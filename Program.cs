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
            Console.WriteLine("=============================================");
            Console.WriteLine("   SISTEMA EPIDEMIOLÓGICO - 202400173");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. Cargar archivo XML");
            Console.WriteLine("2. Análisis Manual (Paso a paso)");
            Console.WriteLine("3. Simulación Automática (Diagnóstico)");
            Console.WriteLine("4. Generar archivo XML de salida");
            Console.WriteLine("5. Limpiar memoria");
            Console.WriteLine("6. Salir");
            Console.WriteLine("---------------------------------------------");
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
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }

        static void FuncionCargarArchivo()
        {
            Console.Write("\nIngrese la ruta del archivo XML: ");
            string? ruta = Console.ReadLine();
            if (!string.IsNullOrEmpty(ruta)) 
            {
                lector.CargarPacientes(ruta, listaPacientes);
            }
        }

        static void FuncionAnalisisManual()
        {
            Console.Write("\nNombre del paciente: ");
            string? nom = Console.ReadLine();
            Paciente? p = listaPacientes.BuscarPorNombre(nom ?? "");

            if (p == null) { Console.WriteLine("Paciente no encontrado."); return; }

            int perActual = 0;
            string input = "";
            // Guardamos el estado original
            ListaDobleFilas inicial = p.RejillaActual; 

            while (input != "salir")
            {
                Console.Clear();
                int sanas, contagiadas;
                ContarEstados(p.RejillaActual, out sanas, out contagiadas);

                Console.WriteLine("========= MODO MANUAL =========");
                Console.WriteLine($"Paciente: {p.Nombre} | Período: {perActual}");
                Console.WriteLine($"Sanas: {sanas} | Contagiadas: {contagiadas}");
                
                // Generar gráfica en cada paso
                graficador.GenerarMatrizDot(p.RejillaActual, p.M, p.Nombre, perActual);

                Console.WriteLine("\n[ENTER] Avanzar Período | [salir] Volver al Menú");
                input = Console.ReadLine()?.ToLower() ?? "";
                
                if (input != "exit")
                {
                    p.RejillaActual = simulador.GenerarSiguientePeriodo(p.RejillaActual, p.M);
                    perActual++;
                }
            }
        }

        static void FuncionSimulacionAutomatica()
        {
            Console.Write("\nNombre del paciente para diagnóstico: ");
            string? nombre = Console.ReadLine();
            Paciente? p = listaPacientes.BuscarPorNombre(nombre ?? "");

            if (p == null) { Console.WriteLine("Paciente no encontrado."); return; }

            Console.WriteLine($"Analizando a {p.Nombre}...");

            // Historial para detección de ciclos
            NodoHistorial? cabezaHistorial = new NodoHistorial(0, p.RejillaActual);
            ListaDobleFilas actual = p.RejillaActual;
            
            bool detectado = false;
            int limite = 10000; 

            for (int n = 1; n <= limite; n++)
            {
                ListaDobleFilas siguiente = simulador.GenerarSiguientePeriodo(actual, p.M);
                
                // Comparar con el historial
                NodoHistorial? tempH = cabezaHistorial;
                while (tempH != null)
                {
                    if (simulador.SonIdenticas(siguiente, tempH.RejillaSnapshot, p.M))
                    {
                        int periodoDondeAparecio = tempH.Periodo;
                        int n1 = n - periodoDondeAparecio;

                        // Clasificación de resultados
                        string diagnostico;
                        if (periodoDondeAparecio == 0) // Repitió el inicial
                        {
                            diagnostico = (n == 1) ? "mortal" : "grave";
                            Console.WriteLine($"\nRESULTADO: {diagnostico.ToUpper()}");
                            Console.WriteLine($"Patrón inicial repetido en N={n}");
                            p.Resultado = (n == 1) ? "MORTAL" : "GRAVE";
                            p.N = n;
                            p.N1 = 0;
                        }
                        else // Ciclo posterior
                        {
                            diagnostico = (n1 == 1) ? "mortal" : "grave";
                            Console.WriteLine($"\nRESULTADO: {diagnostico.ToUpper()}");
                            Console.WriteLine($"Ciclo detectado: Apareció en {periodoDondeAparecio} y repite cada N1={n1}");
                            p.Resultado = (n1 == 1) ? "MORTAL" : "GRAVE";
                            p.N = periodoDondeAparecio;
                            p.N1 = n1;
                        }

                        // Guardar resultados en el objeto Paciente para el XML de salida (me falta)
                        // p.ResultadoFinal = diagnostico; p.N_Encontrado = periodoDondeAparecio; p.N1_Encontrado = n1;

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

            if (!detectado) Console.WriteLine("RESULTADO: LEVE. No se detectó ciclo en 10,000 períodos.");
        }

        static void FuncionGenerarSalida()
        {
            Console.Write("\nIngrese el nombre/ruta para el XML de salida: ");
            string? ruta = Console.ReadLine();
            if (!string.IsNullOrEmpty(ruta))
            {
                escritor.GenerarArchivoSalida(ruta, listaPacientes);
                Console.WriteLine("Archivo generado exitosamente.");
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