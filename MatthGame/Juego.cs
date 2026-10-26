using System;

namespace MatthGame
{
    public class Juego
    {
        Random azar = new Random();

        public bool DosJugadores;
        public int Turno = 0;
        public int Respuesta;
        public int[] Vidas = new int[2];
        public int[] Aciertos = new int[2];
        public int[] Puntos = new int[2];

        public Juego(bool dos)
        {
            DosJugadores = dos;
            Vidas[0] = 3;
            if (dos) Vidas[1] = 3;
        }

        public int Nivel
        {
            get
            {
                int n = (Aciertos[0] + Aciertos[1]) / 5 + 1;
                if (n > 5) n = 5;
                return n;
            }
        }

        public bool Terminado
        {
            get
            {
                if (Vidas[0] > 0) return false;
                if (DosJugadores && Vidas[1] > 0) return false;
                return true;
            }
        }

        public string NuevaPregunta()
        {
            int max = 10 * Nivel;
            int a = azar.Next(1, max + 1);
            int b = azar.Next(1, max + 1);

            int cantOps = Nivel + 1;
            if (cantOps > 4) cantOps = 4;
            int op = azar.Next(0, cantOps);
            string simbolo = "+";

            if (op == 0)
            {
                Respuesta = a + b;
            }
            else if (op == 1)
            {
                if (b > a)
                {
                    int temp = a;
                    a = b;
                    b = temp;
                }
                Respuesta = a - b;
                simbolo = "-";
            }
            else if (op == 2)
            {
                a = azar.Next(2, 6 + Nivel);
                b = azar.Next(2, 6 + Nivel);
                Respuesta = a * b;
                simbolo = "x";
            }
            else
            {
                b = azar.Next(2, 5 + Nivel);
                Respuesta = azar.Next(1, 10 + Nivel);
                a = b * Respuesta;
                simbolo = "/";
            }

            return a + " " + simbolo + " " + b + " = ?";
        }

        public bool Responder(int valor)
        {
            bool bien = (valor == Respuesta);

            if (bien)
            {
                Puntos[Turno] += 10 * Nivel;
                Aciertos[Turno]++;
            }
            else
            {
                Vidas[Turno]--;
            }

            CambiarTurno();
            return bien;
        }

        void CambiarTurno()
        {
            if (!DosJugadores) return;
            int otro = 1 - Turno;

            if (Vidas[otro] > 0) Turno = otro;
        }


        public int Ganador()
        {
            if (!DosJugadores) return 0;
            if (Puntos[0] > Puntos[1]) return 0;
            if (Puntos[1] > Puntos[0]) return 1;
            return -1;
        }
    }
}