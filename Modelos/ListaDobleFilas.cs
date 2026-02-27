namespace IPC2_Proyecto1_202400173.Modelos
{
    public class ListaDobleFilas
    {
        private NodoFila? _cabeza;
        private NodoFila? _cola;
        private int _totalFilas;

        public NodoFila? Cabeza => _cabeza;
        public NodoFila? Cola => _cola;
        public int TotalFilas => _totalFilas;

        public ListaDobleFilas()
        {
            _cabeza = null;
            _cola = null;
            _totalFilas = 0;
        }

        // Llenado secuencial
        public void InsertarFila(int numero)
        {
            NodoFila nuevo = new NodoFila(numero);
            if (_cabeza == null)
            {
                _cabeza = nuevo;
                _cola = nuevo;
            }
            else
            {
                nuevo.Anterior = _cola;
                if (_cola != null) _cola.Siguiente = nuevo;
                _cola = nuevo;
            }
            _totalFilas++;
        }

        // Buscar fila
        public NodoFila? BuscarFila(int numero)
        {
            NodoFila? actual = _cabeza;
            while (actual != null)
            {
                if (actual.NumeroFila == numero) return actual;
                actual = actual.Siguiente;
            }
            return null;
        }
    }
}