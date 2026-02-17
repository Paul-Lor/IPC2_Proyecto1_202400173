namespace IPC2.Estructuras
{
    public class ListaEnlazada<T>
    {
        public Nodo<T>? Cabeza { get; set; }
        private int _total;

        public ListaEnlazada()
        {
            Cabeza = null;
            _total = 0;
        }

        public void Insertar(T dato)
        {
            Nodo<T> nuevo = new Nodo<T>(dato);
            if (Cabeza == null)
            {
                Cabeza = nuevo;
            }
            else
            {
                Nodo<T> actual = Cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevo;
            }
            _total++;
        }

        public int Tamaño() => _total;
    }
}