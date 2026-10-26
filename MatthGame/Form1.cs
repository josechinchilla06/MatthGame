using System;
using System.Drawing;
using System.Windows.Forms;

namespace MatthGame
{
    public partial class Form1 : Form
    {
        Juego juego;
        Registro registro = new Registro();
        string[] nombres = new string[2];

        Panel pnlInicio, pnlJuego, pnlResultados;
        TextBox txtJ1, txtJ2, txtRespuesta;
        RadioButton rbUno, rbDos;
        Label lblTurno, lblNivel, lblVidas, lblPregunta, lblMensaje, lblTitulo, lblResumen;
        ListBox lstRanking;
        Button btnResponder;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Juego de matematicas";
            this.ClientSize = new Size(420, 380);
            this.StartPosition = FormStartPosition.CenterScreen;

            CrearInicio();
            CrearJuego();
            CrearResultados();
            MostrarPanel(pnlInicio);
        }

        // ---------- ayudas para crear controles ----------
        Label NuevoLabel(Panel p, string texto, int x, int y, int ancho)
        {
            Label l = new Label();
            l.Text = texto;
            l.Left = x;
            l.Top = y;
            l.Width = ancho;
            p.Controls.Add(l);
            return l;
        }

        Button NuevoBoton(Panel p, string texto, int x, int y, int ancho)
        {
            Button b = new Button();
            b.Text = texto;
            b.Left = x;
            b.Top = y;
            b.Width = ancho;
            b.Height = 32;
            p.Controls.Add(b);
            return b;
        }

        Panel NuevoPanel()
        {
            Panel p = new Panel();
            p.Dock = DockStyle.Fill;
            this.Controls.Add(p);
            return p;
        }

        void MostrarPanel(Panel p)
        {
            pnlInicio.Visible = false;
            pnlJuego.Visible = false;
            pnlResultados.Visible = false;
            p.Visible = true;
        }

        // ---------- pantalla de inicio ----------
        void CrearInicio()
        {
            pnlInicio = NuevoPanel();

            Label t = NuevoLabel(pnlInicio, "Juego de matematicas", 20, 15, 360);
            t.Font = new Font("Arial", 16, FontStyle.Bold);
            t.Height = 30;

            rbUno = new RadioButton();
            rbUno.Text = "Un jugador";
            rbUno.Left = 30; rbUno.Top = 65;
            rbUno.Checked = true;
            rbUno.CheckedChanged += rbModo_CheckedChanged;
            pnlInicio.Controls.Add(rbUno);

            rbDos = new RadioButton();
            rbDos.Text = "Dos jugadores";
            rbDos.Left = 170; rbDos.Top = 65;
            pnlInicio.Controls.Add(rbDos);

            NuevoLabel(pnlInicio, "Jugador 1:", 30, 110, 80);
            txtJ1 = new TextBox();
            txtJ1.Left = 120; txtJ1.Top = 107; txtJ1.Width = 200;
            txtJ1.MaxLength = 15;
            pnlInicio.Controls.Add(txtJ1);

            NuevoLabel(pnlInicio, "Jugador 2:", 30, 150, 80);
            txtJ2 = new TextBox();
            txtJ2.Left = 120; txtJ2.Top = 147; txtJ2.Width = 200;
            txtJ2.MaxLength = 15;
            txtJ2.Enabled = false;
            pnlInicio.Controls.Add(txtJ2);

            Button jugar = NuevoBoton(pnlInicio, "Jugar", 30, 210, 130);
            jugar.Click += btnJugar_Click;

            Button ranking = NuevoBoton(pnlInicio, "Ver ranking", 180, 210, 130);
            ranking.Click += btnRanking_Click;
        }

        void rbModo_CheckedChanged(object sender, EventArgs e)
        {
            txtJ2.Enabled = rbDos.Checked;
            if (!rbDos.Checked) txtJ2.Clear();
        }

        void btnJugar_Click(object sender, EventArgs e)
        {
            string n1 = txtJ1.Text.Trim();
            string n2 = txtJ2.Text.Trim();

            if (n1 == "")
            {
                MessageBox.Show("Escribe el nombre del jugador 1");
                return;
            }
            if (rbDos.Checked && n2 == "")
            {
                MessageBox.Show("Escribe el nombre del jugador 2");
                return;
            }
            if (rbDos.Checked && n1.ToLower() == n2.ToLower())
            {
                MessageBox.Show("Los nombres tienen que ser distintos");
                return;
            }

            nombres[0] = n1;
            nombres[1] = n2;
            juego = new Juego(rbDos.Checked);

            lblMensaje.Text = "";
            MostrarPanel(pnlJuego);
            NuevaRonda();
        }

        void btnRanking_Click(object sender, EventArgs e)
        {
            lblTitulo.Text = "Tabla de clasificacion";
            lblResumen.Text = "";
            LlenarRanking();
            MostrarPanel(pnlResultados);
        }

        // ---------- pantalla de juego ----------
        void CrearJuego()
        {
            pnlJuego = NuevoPanel();

            lblTurno = NuevoLabel(pnlJuego, "", 20, 15, 380);
            lblTurno.Font = new Font("Arial", 12, FontStyle.Bold);
            lblNivel = NuevoLabel(pnlJuego, "", 20, 50, 180);
            lblVidas = NuevoLabel(pnlJuego, "", 220, 50, 180);

            lblPregunta = NuevoLabel(pnlJuego, "", 20, 100, 380);
            lblPregunta.Font = new Font("Arial", 22, FontStyle.Bold);
            lblPregunta.Height = 45;
            lblPregunta.TextAlign = ContentAlignment.MiddleCenter;

            txtRespuesta = new TextBox();
            txtRespuesta.Left = 140; txtRespuesta.Top = 170; txtRespuesta.Width = 130;
            txtRespuesta.MaxLength = 6;
            txtRespuesta.KeyDown += txtRespuesta_KeyDown;
            pnlJuego.Controls.Add(txtRespuesta);

            btnResponder = NuevoBoton(pnlJuego, "Responder", 140, 210, 130);
            btnResponder.Click += btnResponder_Click;

            lblMensaje = NuevoLabel(pnlJuego, "", 20, 265, 380);
            lblMensaje.TextAlign = ContentAlignment.MiddleCenter;
        }

        void txtRespuesta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnResponder.PerformClick();
            }
        }

        void NuevaRonda()
        {
            lblPregunta.Text = juego.NuevaPregunta();
            lblTurno.Text = "Turno de: " + nombres[juego.Turno];
            lblNivel.Text = "Nivel: " + juego.Nivel;
            lblVidas.Text = "Vidas: " + juego.Vidas[juego.Turno];
            txtRespuesta.Clear();
            txtRespuesta.Focus();
        }

        void btnResponder_Click(object sender, EventArgs e)
        {
            int valor;
            // evita que se caiga si esta vacio o no es numero
            if (!int.TryParse(txtRespuesta.Text.Trim(), out valor))
            {
                MessageBox.Show("Escribe un numero entero");
                txtRespuesta.Clear();
                txtRespuesta.Focus();
                return;
            }

            int correcta = juego.Respuesta;
            bool bien = juego.Responder(valor);

            if (bien)
            {
                lblMensaje.ForeColor = Color.Green;
                lblMensaje.Text = "Correcto!";
            }
            else
            {
                lblMensaje.ForeColor = Color.Red;
                lblMensaje.Text = "Incorrecto, era " + correcta;
            }

            if (juego.Terminado)
                Terminar();
            else
                NuevaRonda();
        }

        // ---------- pantalla de resultados ----------
        void CrearResultados()
        {
            pnlResultados = NuevoPanel();

            lblTitulo = NuevoLabel(pnlResultados, "", 20, 10, 380);
            lblTitulo.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitulo.Height = 28;

            lblResumen = NuevoLabel(pnlResultados, "", 20, 45, 380);
            lblResumen.Height = 70;

            lstRanking = new ListBox();
            lstRanking.Left = 20; lstRanking.Top = 120;
            lstRanking.Width = 380; lstRanking.Height = 190;
            pnlResultados.Controls.Add(lstRanking);

            Button menu = NuevoBoton(pnlResultados, "Volver al menu", 20, 325, 150);
            menu.Click += btnMenu_Click;
        }

        void btnMenu_Click(object sender, EventArgs e)
        {
            MostrarPanel(pnlInicio);
        }

        void Terminar()
        {
            registro.Agregar(nombres[0], juego.Puntos[0]);
            if (juego.DosJugadores)
                registro.Agregar(nombres[1], juego.Puntos[1]);

            lblTitulo.Text = "Fin de la partida";

            string texto = nombres[0] + ": " + juego.Puntos[0] + " puntos (" + juego.Aciertos[0] + " aciertos)";
            if (juego.DosJugadores)
            {
                texto += "\n" + nombres[1] + ": " + juego.Puntos[1] + " puntos (" + juego.Aciertos[1] + " aciertos)";
                int g = juego.Ganador();
                if (g == -1)
                    texto += "\nEmpate!";
                else
                    texto += "\nGana " + nombres[g];
            }
            else
            {
                texto += "\nTu mejor puntaje: " + registro.MejorPuntaje(nombres[0]);
            }
            lblResumen.Text = texto;

            LlenarRanking();
            MostrarPanel(pnlResultados);
        }

        void LlenarRanking()
        {
            lstRanking.Items.Clear();
            if (registro.Cantidad == 0)
            {
                lstRanking.Items.Add("Todavia no hay partidas");
                return;
            }
            for (int i = 0; i < registro.Cantidad; i++)
            {
                lstRanking.Items.Add((i + 1) + ". " + registro.Nombres[i] + " - " + registro.Puntajes[i]);
            }
        }
    }
}