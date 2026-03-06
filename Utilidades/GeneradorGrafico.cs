using System.IO;
using System.Diagnostics;
using IPC2_Proyecto1_202400173.Modelos;

namespace IPC2_Proyecto1_202400173.Utilidades
{
    public class GeneradorGrafico
    {
        public void GenerarMatrizDot(ListaDobleFilas rejilla, int m, string nombre, int periodo)
        {
            // Creamos la carpeta si no existe
            string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "Graficas");
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

            string nombreLimpio = nombre.Replace(" ", "_");
            string dotPath = Path.Combine(carpeta, $"{nombreLimpio}_P{periodo}.dot");
            string pngPath = Path.Combine(carpeta, $"{nombreLimpio}_P{periodo}.png");

            using (StreamWriter sw = new StreamWriter(dotPath))
            {
                sw.WriteLine("digraph G {");
                sw.WriteLine("  bgcolor=\"#2D2D2D\"");
                sw.WriteLine("  node [shape=plaintext fontcolor=white fontname=\"Segoe UI, Arial\"]");
                
                sw.WriteLine($"  label=\"PATRÓN VIRAL: {nombreLimpio} \\n PERÍODO: {periodo}\"");
                sw.WriteLine("  labelloc=\"t\" fontsize=20");

                sw.WriteLine("  tabla [label=<");
                sw.WriteLine("    <TABLE BORDER=\"0\" CELLBORDER=\"1\" CELLSPACING=\"0\" COLOR=\"#555555\">");

                NodoFila? fActual = rejilla.Cabeza;
                while (fActual != null)
                {
                    sw.WriteLine("      <TR>");
                    NodoCelda? cActual = fActual.ListaColumnas.Cabeza;
                    while (cActual != null)
                    {
                        string color = (cActual.Estado == 1) ? "#00A8E8" : "#3C3C3C";
                        sw.WriteLine($"        <TD WIDTH=\"25\" HEIGHT=\"25\" BGCOLOR=\"{color}\" FIXEDSIZE=\"TRUE\"></TD>");
                        cActual = cActual.Siguiente;
                    }
                    sw.WriteLine("      </TR>");
                    fActual = fActual.Siguiente;
                }
                sw.WriteLine("    </TABLE>>];");
                sw.WriteLine("}");
            }

            EjecutarDot(dotPath, pngPath);
            
        }

        private void EjecutarDot(string dotPath, string pngPath)
        {
            try {
                ProcessStartInfo startInfo = new ProcessStartInfo("dot") {
                    Arguments = $"-Tpng \"{dotPath}\" -o \"{pngPath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process.Start(startInfo);
            } catch { /* Dot no instalado */ }
        }
    }
}