namespace IPC2_Proyecto1_202400173.Modelos
{
    public class NodoCelda
    {
        private int _fila;
        private int _columna;
        private int _estado; // 0: sana, 1: contagiada
        private NodoCelda? _siguiente;
        private NodoCelda? _anterior;

        public int Fila 
        { 
            get => _fila; 
            private set => _fila = value; 
        }

        public int Columna 
        { 
            get => _columna; 
            private set => _columna = value;
        }

        public int Estado 
        { 
            get => _estado; 
            set 
            {
                // Validación: Solo permitimos 0 o 1 según el problema 
                if (value == 0 || value == 1)
                    _estado = value;
            } 
        }

        public NodoCelda? Siguiente 
        { 
            get => _siguiente; 
            set => _siguiente = value; 
        }

        public NodoCelda? Anterior 
        { 
            get => _anterior; 
            set => _anterior = value; 
        }

        public NodoCelda(int fila, int columna, int estado)
        {
            _fila = fila;
            _columna = columna;
            Estado = estado; // Usamos la propiedad para validar el estado inicial
            _siguiente = null;
            _anterior = null;
        }
    }
}