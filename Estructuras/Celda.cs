namespace IPC2.Modelos
{
    public class Celda
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
        public int Estado { get; set; } // 0 -> sana, 1-> infectada (f) 

        public Celda(int f, int c, int estado)
        {
            Fila = f;
            Columna = c;
            Estado = estado;
        }
    }

}