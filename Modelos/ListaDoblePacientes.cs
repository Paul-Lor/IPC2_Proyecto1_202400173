namespace IPC2_Proyecto1_202400173.Modelos
{
    public class ListaDoblePacientes
    {
        private Paciente? _cabeza;
        private Paciente? _cola;

        public Paciente? Cabeza => _cabeza;
        public Paciente? Cola => _cola;

        public void Insertar(Paciente nuevo)
        {
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
        }

        public Paciente? BuscarPorNombre(string nombre)
        {
            Paciente? actual = _cabeza;
            while (actual != null)
            {
                if (actual.Nombre.Equals(nombre, System.StringComparison.OrdinalIgnoreCase))
                    return actual;
                actual = actual.Siguiente;
            }
            return null;
        }

        public void Limpiar()
        {
            _cabeza = null;
            _cola = null;
        }
    }
}