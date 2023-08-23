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

            // Haremos que el jugador colisione con algo para efectos posteriores.

            Rect playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

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

            // Sprint final...

            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "bala")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);

                    // Faltaba un cambio más al programa...

                    if (Canvas.GetTop(x) < 10)
                    {
                        elementosAEliminar.Add(x);
                    }

                    // Haremos que la bala pueda colisionarse por sí sola con el enemigo o el jugador.

                    Rect bala = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                }

                // Agregaremos animaciones para cada enemigo.

                if (x is Rectangle && (string)x.Tag == "enemigo")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) + velocidadEnemigo);

                    // Trataremos de que los enemigos se puedan desplazar por toda la ventana del juego moviéndose de izquierda a derecha.

                    if (Canvas.GetLeft(x) > 820)
                    {
                        Canvas.SetLeft(x, -80); // De izquierda a derecha.
                        Canvas.SetTop(x, Canvas.GetTop(x) + (x.Height + 10)); // De arriba hacia abajo.
                    }
                }
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
            // Ahora sí vamos a hacer que los enemigos estén en el juego automáticamente.

            // Se crea un entero local llamado "left" y se establecerá inicialmente en 0.

            int left = 0;

            // Guarda el valor límite de los enemigos como el valor total de ellos mismos.

            totalEnemigos = limite;

            // Este es el bucle "for" que creará todos los enemigos de este juego.
            // Si el límite de los enemigos se establece en 10, este bucle se ejecutará 10 veces, si se establece en 20, luego 20 veces, y así sucesivamente.

            for (int i = 0; i < limite; i++)
            {
                // Con cada ciclo...
                // ... se creará un nuevo pincel de imagen de apariencia del enemigo para usarlo con el rectángulo del mismo.

                ImageBrush aparienciaEnemigo = new ImageBrush();

                // 1.- Se crea un nuevo rectángulo llamado "newEnemy".
                // 2.- Dentro de este rectángulo se establecerán las propiedades para etiquetar al enemigo con 45 * 45 de altura y de ancho, respectivamente, y vincular la apariencia del enemigo como relleno.

                Rectangle newEnemy = new Rectangle
                {
                    Tag = "enemigo",
                    Height = 45,
                    Width = 45,
                    Fill = aparienciaEnemigo,
                };

                // Se establece la ubicación inicial para los invasores espaciales.

                Canvas.SetTop(newEnemy, 10); // Posición vertical del enemigo dentro del Canvas.
                Canvas.SetLeft(newEnemy, left); // Posición horizontal del enemigo dentro del Canvas.

                // Se añadirá a cada enemigo en la escena.

                myCanvas.Children.Add(newEnemy);

                // Se cambiará a -60 la posición del enemigo de manera horizontal.

                left -= 60;

                // Se incrementa en 1 al entero de la imagen del enemigo.

                imagenesEnemigo++;

                // 1.- Si el número entero de imágenes del enemigo supera a 8 imágenes.
                // 2.- Luego volvemos a establecer el entero en 1.

                if (imagenesEnemigo > 8)
                {
                    imagenesEnemigo = 1;
                }

                // 1.- La declaración "switch" a continuación se verificará el entero de imágenes del enemigo.
                // 2.- A cada número se le asignará una nueva apariencia al enemigo.
                // 3.- Con esta declaración se ejecutará a lo largo del bucle y nos ayudará a hacer uso de las imágenes del invasor espacial que importamos anteriormente.
                // 4.- Con esto buscará en qué número está en el entero de las imágenes del enemigo y luego se asignará a esa imagen a esa clase y luego se romperá el bucle.

                switch (imagenesEnemigo)
                {
                    case 1:
                        aparienciaEnemigo.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader1.gif"));
                        // Si el número de imágenes del enemigo aparece como 1, podemos cambiar la fuente de la imagen al archivo GIF del invasor 1.
                        break;

                    case 2:
                        aparienciaEnemigo.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader2.gif"));
                        // Si el número de imágenes del enemigo aparece como 2, podemos cambiar la fuente de la imagen al archivo GIF del invasor 2.
                        break;

                    case 3:
                        aparienciaEnemigo.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader3.gif"));
                        // Si el número de imágenes del enemigo aparece como 3, podemos cambiar la fuente de la imagen al archivo GIF del invasor 3.
                        break;

                    case 4:
                        aparienciaEnemigo.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader4.gif"));
                        // Si el número de imágenes del enemigo aparece como 4, podemos cambiar la fuente de la imagen al archivo GIF del invasor 4.
                        break;

                    case 5:
                        aparienciaEnemigo.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader5.gif"));
                        // Si el número de imágenes del enemigo aparece como 5, podemos cambiar la fuente de la imagen al archivo GIF del invasor 5.
                        break;

                    case 6:
                        aparienciaEnemigo.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader6.gif"));
                        // Si el número de imágenes del enemigo aparece como 6, podemos cambiar la fuente de la imagen al archivo GIF del invasor 6.
                        break;

                    case 7:
                        aparienciaEnemigo.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader7.gif"));
                        // Si el número de imágenes del enemigo aparece como 7, podemos cambiar la fuente de la imagen al archivo GIF del invasor 7.
                        break;

                    case 8:
                        aparienciaEnemigo.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/invader8.gif"));
                        // Si el número de imágenes del enemigo aparece como 8, podemos cambiar la fuente de la imagen al archivo GIF del invasor 8.
                        break;
                }

            }
        }

        private void mostrarFinDelJuego(string mensaje) // El fin del juego se desplegará mediante una ventana de mensaje.
        {
            // EN INSTANTES...
        }


    }
}
