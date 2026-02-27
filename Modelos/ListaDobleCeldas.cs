namespace IPC2_Proyecto1_202400173.Modelos
{
    public class ListaDobleCeldas
    {
        private NodoCelda? _cabeza;
        private NodoCelda? _cola;
        private int _tamaño;

        public NodoCelda? Cabeza => _cabeza;
        public NodoCelda? Cola => _cola;
        public int Tamaño => _tamaño;

        public ListaDobleCeldas()
        {
            _cabeza = null;
            _cola = null;
            _tamaño = 0;
        }

        // Llenado secuencial
        public void InsertarAlFinal(int fila, int columna, int estado)
        {
            NodoCelda nuevo = new NodoCelda(fila, columna, estado);

            if (_cabeza == null)
            {
                _cabeza = nuevo;
                _cola = nuevo;
            }
            else
            {
                // Enlace doble: el actual último apunta al nuevo, y el nuevo apunta al anterior
                nuevo.Anterior = _cola;
                if (_cola != null) _cola.Siguiente = nuevo;
                _cola = nuevo;
            }
            _tamaño++;
        }

        // Método de búsqueda por columna (para la lógica de vecinos)
        public NodoCelda? BuscarCelda(int columna)
        {
            NodoCelda? actual = _cabeza;
            while (actual != null)
            {
                if (actual.Columna == columna)
                {
                    return actual;
                }
                actual = actual.Siguiente;
            }
            return null;
        }
    }
}