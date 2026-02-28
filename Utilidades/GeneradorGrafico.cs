using IPC2_Proyecto1_202400173.Modelos;

namespace IPC2_Proyecto1_202400173.Utilidades
{
    public class GeneradorGrafico
    {
        public void GenerarMatrizDot(ListaDobleFilas rejilla, int m, string nombrePaciente, int periodo)
        {
            string nombreArchivo = $"{nombrePaciente}_Periodo_{periodo}.dot";
            using (StreamWriter sw = new StreamWriter(nombreArchivo))
            {
                sw.WriteLine("digraph G {");
                sw.WriteLine("  node [shape=plaintext]");
                sw.WriteLine("  label=\"Rejilla de Células - " + nombrePaciente + " (Periodo " + periodo + ")\"");
                sw.WriteLine("  tableNode [label=<");
                sw.WriteLine("    <TABLE BORDER=\"0\" CELLBORDER=\"1\" CELLSPACING=\"0\">");
                // Recorremos las filas usando tu ListaDobleFilas
                NodoFila? filaActual = rejilla.Cabeza;
                while (filaActual != null)
                {
                    sw.WriteLine("      <TR>");
                    // Recorremos las celdas de cada fila usando tu ListaDobleCeldas
                    NodoCelda? celdaActual = filaActual.ListaColumnas.Cabeza;
                    while (celdaActual != null)
                    {
                        // Si el estado es 1 (contagiada), usamos color azul como en la imagen
                        string color = (celdaActual.Estado == 1) ? "BGCOLOR=\"#4A7EBB\"" : "";
                        sw.WriteLine($"        <TD WIDTH=\"30\" HEIGHT=\"30\" {color}></TD>");
                        
                        celdaActual = celdaActual.Siguiente;
                    }
                    
                    sw.WriteLine("      </TR>");
                    filaActual = filaActual.Siguiente;
                }
                sw.WriteLine("    </TABLE>");
                sw.WriteLine("  >];");
                sw.WriteLine("}");
            }
            // dot -Tpng nombre.dot -o nombre.png
        }
    }
}