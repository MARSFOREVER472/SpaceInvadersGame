using System;
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

            // Le agregaremos algunos componentes a la ventana principal del juego.

            // Vamos a configurar el temporizador y algunos eventos.
            // Asimismo, también vamos a vincular el temporizador del despachador a un evento llamado "GameLoop".

            dispatcherTimer.Tick += GameLoop;

            // Con este temporizador se ejecutará cada 20 milisegundos.

            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);

            //  Inicializa el temporizador.

            dispatcherTimer.Start();

            // Se cargan las imágenes del jugador desde la carpeta de imágenes.

            aparienciaJugador.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player.png"));

            //  Asigna la nueva apariencia del jugador al rectángulo.

            player.Fill = aparienciaJugador;

            // Ejecuta la función de crear enemigos y decirle que cree 30 enemigos.

            crearEnemigos(30);

            // Ejecuta el Canvas principal (Cuadro del juego).

            myCanvas.Focus();
        }

        // Método especial para el despachador del temporizador.

        private void GameLoop(object sender, EventArgs e)
        {
            // Vamos a hacer un método de ejecución de prueba para el jugador.

            if (izquierda == true && Canvas.GetLeft(player) > 0) // Cuando el jugador se mueve hacia la izquierda.
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - 10); // El personaje puede mover la distancia dentro del cuadro del juego.
            }

            if (derecha == true && Canvas.GetLeft(player) + 80 < Application.Current.MainWindow.Width) // Cuando el jugador se mueve hacia la derecha.
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + 10); // El personaje puede mover la distancia dentro del cuadro del juego.
            }

            // Ejecución especial para las balas del enemigo.

            temporizadorBala -= 3; // Las balas del enemigo se lanzarán durante cada 3 segundos.

            if (temporizadorBala < 0)
            {
                crearBalasEnemigo(Canvas.GetLeft(player) + 20, 10); // Se posicionarán hacia el jugador cuando el enemigo se lanza una bala.

                temporizadorBala = limiteTemporizadorBala; // Será su tiempo límite al lanzar cada 3 segundos la bala.
            }
        }

        // Método que permite al usuario presionando una tecla.

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            // En un proyecto con WPF hay que tener en cuenta que el evento de presionar y soltar una tecla a la vez el tipo de variable tendría que ser "Key".

            // Cuando se presiona una tecla.

            if (e.Key == Key.Left) // Al presionar una tecla del lado izquierdo.
            {
                izquierda = true; // Va hacia la izquierda.
            }

            if (e.Key == Key.Right) // Al presionar una tecla del lado derecho.
            {
                derecha = true; // Va hacia la derecha.
            }
        }

        // Método que permite al usuario soltando una tecla.

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            // Cuando suelta una tecla.

            if (e.Key == Key.Left) // Cuando suelta la tecla del lado izquierdo.
            {
                izquierda = false; // No va hacia la izquierda.
            }

            if (e.Key == Key.Right) // Cuando suelta una tecla del lado derecho.
            {
                derecha = false; // No va hacia la derecha.
            }

            // El algoritmo sería algo muy especial para el jugador cuando suelta la tecla de espacio..

            if (e.Key == Key.Space)
            {
                Rectangle nuevaBala = new Rectangle // Se crearán las balas para el jugador con sus respectivos atributos..
                {
                    Tag = "bala", // Para las balas.
                    Height = 20, // 20 metros de altura para las balas.
                    Width = 5, // El ancho de las balas son de 5 cm.
                    Fill = Brushes.DarkRed, // Las balas se pintarán de color rojo oscuro.
                    Stroke = Brushes.DarkOrange // Cuando se colisionan con algo lo de las balas estarán de color naranjo oscuro.

                };

                // Ahora con todos estos atributos lo posicionaremos mediante un Canvas.

                Canvas.SetTop(nuevaBala, Canvas.GetTop(player) - nuevaBala.Height); // Posición vertical del jugador con las balas mediante Canvas.
                Canvas.SetLeft(nuevaBala, Canvas.GetLeft(player) + player.Width / 2); // Posición horizontal del jugador con las balas mediante Canvas.

                // Se añadirán las balas para el jugador.

                myCanvas.Children.Add(nuevaBala);
            }
        }

        // Ahora crearemos varios métodos adicionales para que el usuario pueda interactuar con el juego en general.

        private void crearBalasEnemigo(double x, double y) // Las balas del enemigo se definen mediante posiciones en x e y.
        {
            // Haremos lo mismo pero esta vez habrán algunas modificaciones para el enemigo.

            Rectangle balasEnemigo = new Rectangle // Se crearán los atributos para las balas del enemigo.
            {
                Tag = "balaEnemigo", // Nombre de la bala para el enemigo.
                Height = 40, // La altura de la bala del enemigo será más grande que la del jugador.
                Width = 15, // El ancho será lo mismo para el enemigo.
                Fill = Brushes.DarkOrange, // El color de éstas será de color naranjo oscuro.
                Stroke = Brushes.Black, // Cuando se colisionan, éstas cambiarán de color para las balas del enemigo.
                StrokeThickness = 5 // Intervalo de distancia al colisionar con las balas para el enemigo.
            };

            // Con esto posicionaremos las balas del enemigo mediante un Canvas de manera horizontal y vertical, según sea el caso.

            Canvas.SetTop(balasEnemigo, y); // Posición vertical de la bala del enemigo mediante Canvas.
            Canvas.SetLeft(balasEnemigo, x); // Posición horizontal de la bala del enemigo mediante Canvas.

            // Se crearán las balas para el enemigo.

            myCanvas.Children.Add(balasEnemigo);
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
