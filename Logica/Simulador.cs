using IPC2_Proyecto1_202400173.Modelos;

namespace IPC2_Proyecto1_202400173.Logica
{
    public class Simulador
    {
        private int coordenadas;

        // Compara dos rejillas completas para detectar repetición de patrones
        public bool SonIdenticas(ListaDobleFilas r1, ListaDobleFilas r2, int m)
        {
            for (int f = 1; f <= m; f++)
            {
                NodoFila? fila1 = r1.BuscarFila(f);
                NodoFila? fila2 = r2.BuscarFila(f);

                // Si alguna fila no existe en una de las dos
                if (fila1 == null || fila2 == null) return false;

                for (int c = 1; c <= m; c++)
                {
                    NodoCelda? celda1 = fila1.ListaColumnas.BuscarCelda(c);
                    NodoCelda? celda2 = fila2.ListaColumnas.BuscarCelda(c);

                    if (celda1 == null || celda2 == null) return false;

                    // Comparamos el estado (0 o 1)
                    if (celda1.Estado != celda2.Estado)
                    {
                        return false; // A la primera diferencia, ya no son iguales
                    }
                }
            }
            return true; // Si terminó todos los ciclos, son idénticas
        }

        // Genera el estado de la rejilla para el siguiente periodo
        public ListaDobleFilas GenerarSiguientePeriodo(ListaDobleFilas actual, int m)
        {
            ListaDobleFilas nuevaRejilla = InicializarRejillaVacia(m);

            for (int f = 1; f <= m; f++)
            {
                NodoFila? filaActual = actual.BuscarFila(f);
                NodoFila? filaNueva = nuevaRejilla.BuscarFila(f);

                for (int c = 1; c <= m; c++)
                {
                    int vecinos = ContarVecinosContagiados(actual, f, c);
                    NodoCelda? celdaOriginal = filaActual?.ListaColumnas.BuscarCelda(c);
                    NodoCelda? celdaNueva = filaNueva?.ListaColumnas.BuscarCelda(c);

                    if (celdaOriginal != null && celdaNueva != null)
                    {
                        // Regla 1: Célula contagiada sobrevive con 2 o 3 vecinos (ENUNCIADO)
                        if (celdaOriginal.Estado == 1)
                        {
                            celdaNueva.Estado = (vecinos == 2 || vecinos == 3) ? 1 : 0;
                        }
                        // Regla 2: Célula sana se contagia con exactamente 3 vecinos (ENUNCIADO)
                        else
                        {
                            celdaNueva.Estado = (vecinos == 3) ? 1 : 0;
                        }
                    }
                }
            }
            return nuevaRejilla;
        }

        private int ContarVecinosContagiados(ListaDobleFilas rejilla, int f, int c)
        {
            int contador = 0;
            // Evaluamos los 8 vecinos alrededor de la celda (f,c)
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue; // No contarse a sí misma

                    NodoFila? filaVecino = rejilla.BuscarFila(f + i);
                    NodoCelda? celdaVecino = filaVecino?.ListaColumnas.BuscarCelda(c + j);

                    if (celdaVecino != null && celdaVecino.Estado == 1)
                    {
                        contador++;
                    }
                }
            }
            return contador;
        }
        
        private int InsertarCelula(ListaDobleFilas r1, int f, int c, int estado, int m)
        {
            if (f < 1 || f > m || c < 1 || c > m)
            {
                return 0; 
            }
            NodoFila? fila = r1.BuscarFila(f);
            if (fila == null)
            {
                return 0; 
            }
            NodoCelda? celda = fila.ListaColumnas.BuscarCelda(c);
            if (celda == null)
            {
                return 0; 
            }
            celda.Estado = estado;
            return 1; 
        }
            
        }
        private ListaDobleFilas InicializarRejillaVacia(int m)
        {
            ListaDobleFilas r = new ListaDobleFilas();
            for (int i = 1; i <= m; i++)
            {
                r.InsertarFila(i);
                NodoFila? f = r.BuscarFila(i);
                for (int j = 1; j <= m; j++)
                {
                    f?.ListaColumnas.InsertarAlFinal(i, j, 0);
                }
            }
            return r;
        }
    }
}