namespace IPC2_Proyecto1_202400173.Modelos
{
    public class Paciente
    {
        private string _nombre;
        private int _edad;
        private int _m; // Tamaño de la rejilla (M x M)
        private int _periodosMaximos; // N periodos a evaluar
        private ListaDobleFilas _rejillaActual;
        
        // Propiedades públicas
        public string Nombre => _nombre;
        public int Edad => _edad;
        public int M => _m;
        public int PeriodosMaximos => _periodosMaximos;
        
        public ListaDobleFilas RejillaActual
        {
            get => _rejillaActual;
            set => _rejillaActual = value;
        }

        // Nodo para la lista global de pacientes (porque también debe ser lista doble)
        public Paciente? Siguiente { get; set; }
        public Paciente? Anterior { get; set; }

        public Paciente(string nombre, int edad, int m, int periodos, ListaDobleFilas rejilla)
        {
            _nombre = nombre;
            _edad = edad;
            _m = m;
            _periodosMaximos = periodos;
            _rejillaActual = rejilla;
            this.Siguiente = null;
            this.Anterior = null;
        }
    }
}