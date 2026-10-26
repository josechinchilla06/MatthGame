namespace MatthGame
{
    public class Registro
    {
        public string[] Nombres = new string[50];
        public int[] Puntajes = new int[50];
        public int Cantidad = 0;

        public bool Agregar(string nombre, int puntaje)
        {
            if (Cantidad >= 50) return false;
            Nombres[Cantidad] = nombre;
            Puntajes[Cantidad] = puntaje;
            Cantidad++;
            Ordenar();
            return true;
        }

        // burbuja, de mayor a menor
        public void Ordenar()
        {
            for (int i = 0; i < Cantidad - 1; i++)
            {
                for (int j = 0; j < Cantidad - 1 - i; j++)
                {
                    if (Puntajes[j] < Puntajes[j + 1])
                    {
                        int p = Puntajes[j];
                        Puntajes[j] = Puntajes[j + 1];
                        Puntajes[j + 1] = p;

                        string n = Nombres[j];
                        Nombres[j] = Nombres[j + 1];
                        Nombres[j + 1] = n;
                    }
                }
            }
        }

        // busqueda lineal, devuelve 0 si no esta
        public int MejorPuntaje(string nombre)
        {
            int mejor = 0;
            for (int i = 0; i < Cantidad; i++)
            {
                if (Nombres[i].ToLower() == nombre.ToLower() && Puntajes[i] > mejor)
                {
                    mejor = Puntajes[i];
                }
            }
            return mejor;
        }
    }
}