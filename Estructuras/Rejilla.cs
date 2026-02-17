namespace IPC2.Modelos
{
    using Estructuras;
    
    public class Rejilla
    {
        // Una lista donde cada Nodo contiene una ListaEnlazada de Celdas
        public ListaEnlazada<ListaEnlazada<Celda>> Filas { get; set; }

        public Rejilla()
        {
            Filas = new ListaEnlazada<ListaEnlazada<Celda>>();
        }
    }

}