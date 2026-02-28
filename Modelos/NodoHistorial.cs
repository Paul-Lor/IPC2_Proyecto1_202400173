namespace IPC2_Proyecto1_202400173.Modelos
{
    public class NodoHistorial
    {
        private int _periodo;
        private ListaDobleFilas _rejillaSnapshot;
        public NodoHistorial? Siguiente { get; set; }

        public int Periodo => _periodo;
        public ListaDobleFilas RejillaSnapshot => _rejillaSnapshot;

        public NodoHistorial(int periodo, ListaDobleFilas rejilla)
        {
            _periodo = periodo;
            _rejillaSnapshot = rejilla;
        }
    }
}