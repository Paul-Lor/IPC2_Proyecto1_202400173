namespace IPC2_Proyecto1_202400173.Modelos
{
    public class NodoFila
    {
        private int _numeroFila;
        private ListaDobleCeldas _listaColumnas; // La lista de celdas de cada fila que se agregue (dependiendo del tamaño de la matriz)
        private NodoFila? _siguiente;
        private NodoFila? _anterior;

        public int NumeroFila
        {
            get => _numeroFila;
            private set => _numeroFila = value;
        }

        public ListaDobleCeldas ListaColumnas
        {
            get => _listaColumnas;
            set => _listaColumnas = value;
        }

        public NodoFila? Siguiente
        {
            get => _siguiente;
            set => _siguiente = value;
        }

        public NodoFila? Anterior
        {
            get => _anterior;
            set => _anterior = value;
        }

        // Constructor
        public NodoFila(int numeroFila)
        {
            _numeroFila = numeroFila;
            _listaColumnas = new ListaDobleCeldas(); // Cada fila nace con su propia lista de celdas
            _siguiente = null;
            _anterior = null;
        }
    }
}