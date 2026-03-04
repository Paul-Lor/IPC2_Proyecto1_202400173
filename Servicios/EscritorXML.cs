using System.Xml;
using IPC2_Proyecto1_202400173.Modelos;

namespace IPC2_Proyecto1_202400173.Servicios
{
    public class EscritorXML
    {
        public void GenerarArchivoSalida(string ruta, ListaDoblePacientes lista)
        {
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
            string rutaCompleta = ruta.EndsWith(".xml") ? ruta : $"{ruta}.xml";
            using (XmlWriter writer = XmlWriter.Create(rutaCompleta, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("pacientes"); // <pacientes>

                Paciente? actual = lista.Cabeza;
                while (actual != null)
                {
                    writer.WriteStartElement("paciente"); // <paciente>
                    
                    writer.WriteStartElement("datospersonales");
                    writer.WriteElementString("nombre", actual.Nombre);
                    writer.WriteElementString("edad", actual.Edad.ToString());
                    writer.WriteEndElement(); // </datospersonales>

                    writer.WriteElementString("periodos", actual.PeriodosMaximos.ToString());
                    writer.WriteElementString("m", actual.M.ToString());
                    writer.WriteElementString("resultado", actual.Resultado);

                    // Solo escribir n y n1 si el resultado no es leve
                    if (actual.Resultado != "LEVE")
                    {
                        writer.WriteElementString("n", actual.N.ToString());
                        if (actual.N1 > 0)
                        {
                            writer.WriteElementString("n1", actual.N1.ToString());
                        }
                    }

                    writer.WriteEndElement(); // </paciente>
                    actual = actual.Siguiente;
                }

                writer.WriteEndElement(); // </pacientes>
                writer.WriteEndDocument();
            }
        }
    }
}