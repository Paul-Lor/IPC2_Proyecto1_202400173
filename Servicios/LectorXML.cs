using System.Xml;

namespace IPC2_Proyecto1_202400173.Servicios
{
    public class LectorXML
    {
        public void CargarPacientes(string rutaArchivo, ListaDoblePacientes listaGlobal)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(rutaArchivo);

            // Obtenemos todos los nodos de paciente
            XmlNodeList? pacientes = doc.SelectNodes("//paciente");
            if (pacientes == null) return;

            foreach (XmlNode nodoPaciente in pacientes)
            {
                string nombre = nodoPaciente.SelectSingleNode("datospersonales/nombre")?.InnerText ?? "Sin nombre";
                int m = int.Parse(nodoPaciente.SelectSingleNode("m")?.InnerText ?? "10");
                
                // 1. Crear la estructura base (todas sanas)
                ListaDobleFilas rejillaInicial = GenerarRejillaVacia(m);

                // 2. Marcar las contagiadas según el XML
                XmlNodeList? celdasEnfermas = nodoPaciente.SelectNodes("rejilla/celda");
                if (celdasEnfermas != null)
                {
                    foreach (XmlNode celda in celdasEnfermas)
                    {
                        int f = int.Parse(celda.Attributes?["f"]?.Value ?? "0");
                        int c = int.Parse(celda.Attributes?["c"]?.Value ?? "0");

                        // Buscamos el nodo en nuestra lista de listas y cambiamos estado
                        NodoFila? filaNodo = rejillaInicial.BuscarFila(f);
                        NodoCelda? celdaNodo = filaNodo?.ListaColumnas.BuscarCelda(c);
                        
                        if (celdaNodo != null)
                        {
                            celdaNodo.Estado = 1; // Marcamos como contagiada
                        }
                    }
                }
                
                // Guardar el paciente en la lista global
            }
        }

        private ListaDobleFilas GenerarRejillaVacia(int m)
        {
            ListaDobleFilas nuevaRejilla = new ListaDobleFilas();
            for (int i = 1; i <= m; i++)
            {
                nuevaRejilla.InsertarFila(i);
                NodoFila? filaActual = nuevaRejilla.BuscarFila(i);
                
                for (int j = 1; j <= m; j++)
                {
                    filaActual?.ListaColumnas.InsertarAlFinal(i, j, 0); // Todas inician sanas (0)
                }
            }
            return nuevaRejilla;
        }
    }
}