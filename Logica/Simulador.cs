using IPC2_Proyecto1_202400173.Modelos;

namespace IPC2_Proyecto1_202400173.Logica
{
    public class Simulador
    {
        // Método principal para avanzar un período
        public ListaDobleFilas GenerarSiguientePeriodo(ListaDobleFilas rejillaActual, int m)
        {
            // Creamos una nueva rejilla vacía para el siguiente periodo
            ListaDobleFilas nuevaRejilla = GenerarRejillaBase(m);

            for (int f = 1; f <= m; f++)
            {
                NodoFila? filaActual = rejillaActual.BuscarFila(f);
                NodoFila? filaNueva = nuevaRejilla.BuscarFila(f);

                for (int c = 1; c <= m; c++)
                {
                    NodoCelda? celdaActual = filaActual?.ListaColumnas.BuscarCelda(c);
                    int vecinosContagiados = ContarVecinos(rejillaActual, f, c);
                    
                    int nuevoEstado = 0;

                    if (celdaActual != null)
                    {
                        // REGLA 1: Célula contagiada (Estado 1)
                        if (celdaActual.Estado == 1)
                        {
                            if (vecinosContagiados == 2 || vecinosContagiados == 3)
                                nuevoEstado = 1; // Sigue contagiada
                            else
                                nuevoEstado = 0; // Sana
                        }
                        // REGLA 2: Célula sana (Estado 0)
                        else
                        {
                            if (vecinosContagiados == 3)
                                nuevoEstado = 1; // Se contagia
                        }
                    }

                    // Guardamos el resultado en la nueva rejilla
                    NodoCelda? celdaNueva = filaNueva?.ListaColumnas.BuscarCelda(c);
                    if (celdaNueva != null) celdaNueva.Estado = nuevoEstado;
                }
            }
            return nuevaRejilla;
        }

        private int ContarVecinos(ListaDobleFilas rejilla, int f, int c)
        {
            int contador = 0;
            // Coordenadas de los 8 vecinos
            int[] df = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dc = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int nf = f + df[i];
                int nc = c + dc[i];

                // Buscamos el nodo vecino en la lista de listas
                NodoFila? filaVecino = rejilla.BuscarFila(nf);
                NodoCelda? celdaVecino = filaVecino?.ListaColumnas.BuscarCelda(nc);

                if (celdaVecino != null && celdaVecino.Estado == 1)
                {
                    contador++;
                }
            }
            return contador;
        }

        // Método auxiliar para crear la estructura M x M en blanco
        private ListaDobleFilas GenerarRejillaBase(int m)
        {
            ListaDobleFilas baseR = new ListaDobleFilas();
            for (int i = 1; i <= m; i++)
            {
                baseR.InsertarFila(i);
                NodoFila? f = baseR.BuscarFila(i);
                for (int j = 1; j <= m; j++)
                {
                    f?.ListaColumnas.InsertarAlFinal(i, j, 0);
                }
            }
            return baseR;
        }

        public bool SonIguales(ListaDobleFilas r1, ListaDobleFilas r2, int m)
        {
            for (int f = 1; f <= m; f++)
            {
                NodoFila? fila1 = r1.BuscarFila(f);
                NodoFila? fila2 = r2.BuscarFila(f);

                for (int c = 1; c <= m; c++)
                {
                    NodoCelda? c1 = fila1?.ListaColumnas.BuscarCelda(c);
                    NodoCelda? c2 = fila2?.ListaColumnas.BuscarCelda(c);

                    // Si los estados son diferentes en cualquier punto, las rejillas no son iguales
                    if (c1?.Estado != c2?.Estado) return false;
                }
            }
            return true;
        }
    }
}