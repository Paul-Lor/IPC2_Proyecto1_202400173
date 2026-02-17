namespace IPC2.Estructuras
{
    public class Nodo<T>
    {
        public T Dato { get; set; }

        public Nodo<T>? Siguiente { get; set; }

        public Nodo(T dato)
        {
            this.Dato = dato;
            this.Siguiente = null;
        }
    }
}