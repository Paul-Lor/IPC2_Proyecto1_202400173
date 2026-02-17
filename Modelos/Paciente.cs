namespace IPC2.Modelos
{
    using Estructuras;

    public class Paciente
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int Periodos { get; set; }
        public int DimensionM { get; set; }
        public Rejilla TejidoInicial { get; set; }

        public Paciente(string nombre, int edad, int periodos, int m)
        {
            Nombre = nombre;
            Edad = edad;
            Periodos = periodos;
            DimensionM = m;
            TejidoInicial = new Rejilla();
        }
    }
}