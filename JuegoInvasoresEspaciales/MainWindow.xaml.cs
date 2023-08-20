﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading; // Para el despachador del temporizador.

namespace JuegoInvasoresEspaciales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Algunas de las variables a declarar:

        // Con ir a la izquierda y a la derecha, los valores booleanos se establecen en falso.

        bool izquierda, derecha;

        // Los elementos de esta lista a eliminar se utilizarán como un recolector de basura.

        List<Rectangle> elementosAEliminar = new List<Rectangle>();

        // Estas imágenes de enemigos de tipo int nos ayudarán a cambiar las imágenes de un enemigo.

        int imagenesEnemigo = 0;

        // Este es el temporizador de bala para el enemigo.

        int temporizadorBala;

        // Este es el límite del temporizador de bala para el enemigo.

        int limiteTemporizadorBala = 90;

        // Guarda el número total de enemigos.

        int totalEnemigos;

        // Se crea una nueva instancia para la clase del despachador del temporizador.

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        // Clase de pincel de imagen que usaremos como la imagen del jugador llamada Apariencia del jugador.

        ImageBrush aparienciaJugador = new ImageBrush();

        // Velocidad del enemigo por defecto.

        int velocidadEnemigo = 6;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Método que permite al usuario presionando una tecla.

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            // EN INSTANTES...
        }

        // Método que permite al usuario soltando una tecla.

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            // EN INSTANTES...
        }

        // Ahora crearemos varios métodos adicionales para que el usuario pueda interactuar con el juego en general.

        private void crearBalasEnemigo(double x, double y) // Las balas del enemigo se definen mediante posiciones en x e y.
        {
            // EN INSTANTES...
        }

        private void crearEnemigos(int limite) // Tiene que tener una cantidad máxima de enemigos mediante un límite.
        {
            // EN INSTANTES...
        }
        
        private void mostrarFinDelJuego(string mensaje) // El fin del juego se desplegará mediante una ventana de mensaje.
        {
            // EN INSTANTES...
        }


    }
}
