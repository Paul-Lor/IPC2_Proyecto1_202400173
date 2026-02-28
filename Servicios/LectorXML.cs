using System.Xml;
using IPC2_Proyecto1_202400173.Modelos;

namespace IPC2_Proyecto1_202400173.Servicios
{
    public class LectorXML
    {
        public void CargarPacientes(string ruta, ListaDoblePacientes listaGlobal)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ruta);

                XmlNodeList? nodosPacientes = doc.SelectNodes("//paciente");
                if (nodosPacientes == null) return;

                foreach (XmlNode nodo in nodosPacientes)
                {
                    // Extracción de datos básicos
                    string nombre = nodo.SelectSingleNode("datospersonales/nombre")?.InnerText ?? "Sin Nombre";
                    int edad = int.Parse(nodo.SelectSingleNode("datospersonales/edad")?.InnerText ?? "0");
                    int m = int.Parse(nodo.SelectSingleNode("m")?.InnerText ?? "10");
                    int periodos = int.Parse(nodo.SelectSingleNode("periodos")?.InnerText ?? "0");

                    // 1. Crear rejilla base (M x M) totalmente sana (0)
                    ListaDobleFilas rejillaNueva = GenerarRejillaVacia(m);

                    // 2. Cargar celdas contagiadas (1) desde el XML
                    XmlNodeList? celdasInfectadas = nodo.SelectNodes("rejilla/celda");
                    if (celdasInfectadas != null)
                    {
                        foreach (XmlNode celdaNode in celdasInfectadas)
                        {
                            int f = int.Parse(celdaNode.Attributes?["f"]?.Value ?? "0");
                            int c = int.Parse(celdaNode.Attributes?["c"]?.Value ?? "0");

                            // Buscamos el nodo exacto y cambiamos su estado a 1 (contagiada)
                            NodoFila? filaEncontrada = rejillaNueva.BuscarFila(f);
                            if (filaEncontrada != null)
                            {
                                NodoCelda? celdaEncontrada = filaEncontrada.ListaColumnas.BuscarCelda(c);
                                if (celdaEncontrada != null)
                                {
                                    celdaEncontrada.Estado = 1;
                                }
                            }
                        }
                    }

                    // Crear objeto Paciente y agregarlo a la lista global
                    Paciente nuevoPaciente = new Paciente(nombre, edad, m, periodos, rejillaNueva);
                    listaGlobal.Insertar(nuevoPaciente);
                }
                Console.WriteLine("Archivo cargado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el archivo: {ex.Message}");
            }
        }

        private ListaDobleFilas GenerarRejillaVacia(int m)
        {
            ListaDobleFilas rejilla = new ListaDobleFilas();
            for (int i = 1; i <= m; i++)
            {
                rejilla.InsertarFila(i);
                NodoFila? filaActual = rejilla.BuscarFila(i);
                for (int j = 1; j <= m; j++)
                {
                    // Llenamos preventivamente todas las celdas como sanas (0)
                    filaActual?.ListaColumnas.InsertarAlFinal(i, j, 0);
                }
            }
            return rejilla;
        }
    }
}