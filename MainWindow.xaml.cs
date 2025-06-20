using System;
using System.Windows;
using System.Windows.Controls;
using Waze.Estructuras;

namespace Waze
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Número de filas de la cuadrícula.
        /// </summary>
        private const int GridRows = 6;

        /// <summary>
        /// Número de columnas de la cuadrícula.
        /// </summary>
        private const int GridCols = 12;

        /// <summary>
        /// Tamaño de cada celda de la cuadrícula.
        /// </summary>
        private double _cellSize = 0;

        /// <summary>
        /// Lista de ciudades, usando la estructura ListaSimple de Estructuras.cs.
        /// </summary>
        private ListaSimple<Ciudad> ciudades = new ListaSimple<Ciudad>();

        /// <summary>
        /// Lista de carreteras (conexiones), usando la estructura ListaSimple de Estructuras.cs.
        /// </summary>
        private ListaSimple<Carretera> carreteras = new ListaSimple<Carretera>();

        /// <summary>
        /// Lista de carros visuales (opcional, para animaciones).
        /// </summary>
        private ListaSimple<CarroVisual> carros = new ListaSimple<CarroVisual>();

        /// <summary>
        /// Constructor de la ventana principal.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            Loaded += MainWindow_Loaded;
            SizeChanged += MainWindow_SizeChanged;
            SliderVelocidad.ValueChanged += SliderVelocidad_ValueChanged;
        }

        /// <summary>
        /// Evento que se ejecuta al cargar la ventana.
        /// Ajusta el canvas y dibuja la cuadrícula.
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AdjustCanvasAndDrawGrid();
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar el tamaño de la ventana.
        /// Ajusta el canvas y redibuja la cuadrícula.
        /// </summary>
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustCanvasAndDrawGrid();
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar el valor del slider de velocidad.
        /// Actualiza la etiqueta de velocidad.
        /// </summary>
        private void SliderVelocidad_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LabelVelocidad != null)
                LabelVelocidad.Text = $"Velocidad: {SliderVelocidad.Value:0}";
        }

        /// <summary>
        /// Ajusta el tamaño del canvas y dibuja la cuadrícula y los elementos.
        /// </summary>
        private void AdjustCanvasAndDrawGrid()
        {
            double availableWidth = ActualWidth * 0.8;
            double availableHeight = ActualHeight * 0.8;

            double cellWidth = availableWidth / (GridCols + 1);
            double cellHeight = availableHeight / (GridRows + 1);
            double cellSize = Math.Min(cellWidth, cellHeight);

            _cellSize = cellSize;

            CornerLabel.Width = cellSize;
            CornerLabel.Height = cellSize;

            HorizontalLabelsPanel.Width = cellSize * GridCols;
            HorizontalLabelsPanel.Height = cellSize;

            VerticalLabelsPanel.Width = cellSize;
            VerticalLabelsPanel.Height = cellSize * GridRows;

            GridCanvas.Width = cellSize * GridCols;
            GridCanvas.Height = cellSize * GridRows;

            DrawCoordinateLabels();
            DrawGridOnCanvas();
            RedrawCiudadesYCarreteras();
            RedrawCarros();
        }

        /// <summary>
        /// Dibuja las etiquetas de coordenadas en los paneles horizontales y verticales.
        /// </summary>
        private void DrawCoordinateLabels()
        {
            HorizontalLabelsPanel.Children.Clear();
            VerticalLabelsPanel.Children.Clear();

            for (int col = 0; col < GridCols; col++)
            {
                var label = new Label
                {
                    Content = col.ToString(),
                    Width = _cellSize,
                    Height = _cellSize,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Background = System.Windows.Media.Brushes.Transparent,
                    Foreground = System.Windows.Media.Brushes.Black,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(0)
                };
                HorizontalLabelsPanel.Children.Add(label);
            }

            for (int row = 0; row < GridRows; row++)
            {
                var label = new Label
                {
                    Content = row.ToString(),
                    Width = _cellSize,
                    Height = _cellSize,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Background = System.Windows.Media.Brushes.Transparent,
                    Foreground = System.Windows.Media.Brushes.Black,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(0)
                };
                VerticalLabelsPanel.Children.Add(label);
            }
        }

        /// <summary>
        /// Dibuja la cuadrícula en el canvas.
        /// </summary>
        private void DrawGridOnCanvas()
        {
            GridCanvas.Children.Clear();

            for (int i = 0; i <= GridCols; i++)
            {
                double x = i * _cellSize;
                var line = new System.Windows.Shapes.Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = GridRows * _cellSize,
                    Stroke = System.Windows.Media.Brushes.White,
                    StrokeThickness = 1
                };
                GridCanvas.Children.Add(line);
            }

            for (int i = 0; i <= GridRows; i++)
            {
                double y = i * _cellSize;
                var line = new System.Windows.Shapes.Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = GridCols * _cellSize,
                    Y2 = y,
                    Stroke = System.Windows.Media.Brushes.White,
                    StrokeThickness = 1
                };
                GridCanvas.Children.Add(line);
            }
        }

        /// <summary>
        /// Redibuja todas las ciudades y carreteras en el canvas.
        /// </summary>
        private void RedrawCiudadesYCarreteras()
        {
            // Dibuja carreteras
            foreach (var carretera in carreteras.Recorrer())
            {
                DibujarCarreteraConTiempo(GridCanvas, carretera, _cellSize);
            }

            // Dibuja ciudades
            foreach (var ciudad in ciudades.Recorrer())
            {
                DibujarCiudad(GridCanvas, ciudad, _cellSize);
            }
        }

        /// <summary>
        /// Redibuja todos los carros en el canvas.
        /// </summary>
        private void RedrawCarros()
        {
            // Elimina todos los carros del canvas y los vuelve a dibujar en su ciudad actual
            var imgs = GridCanvas.Children.OfType<Image>();
            foreach (var img in imgs)
                GridCanvas.Children.Remove(img);

            foreach (var carro in carros.Recorrer())
            {
                double x = carro.CiudadActual.X * _cellSize + _cellSize / 2;
                double y = carro.CiudadActual.Y * _cellSize + _cellSize / 2;
                double size = _cellSize * 0.25;

                carro.Imagen.Width = size;
                carro.Imagen.Height = size;
                Canvas.SetLeft(carro.Imagen, x - size / 2);
                Canvas.SetTop(carro.Imagen, y - size / 2);
                if (!GridCanvas.Children.Contains(carro.Imagen))
                    GridCanvas.Children.Add(carro.Imagen);
            }
        }

        /// <summary>
        /// Evento del botón para colocar una ciudad.
        /// Crea una ciudad y la agrega a la lista de ciudades si no existe.
        /// </summary>
        private void BtnColocar_Click(object sender, RoutedEventArgs e)
        {
            // Obtiene el nombre de la ciudad desde el TextBox.
            string nombreCiudad = InputCiudad.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(nombreCiudad))
            {
                MessageBox.Show("Por favor, ingresa el nombre de la ciudad antes de colocar el nodo.");
                return;
            }

            // Verifica que no exista una ciudad con el mismo nombre.
            foreach (var c in ciudades.Recorrer())
            {
                if (c.Nombre.Equals(nombreCiudad, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Ya existe una ciudad con ese nombre. Por favor, elige otro nombre.");
                    return;
                }
            }

            // Obtiene las coordenadas X e Y.
            if (int.TryParse(InputX.Text, out int x) && int.TryParse(InputY.Text, out int y))
            {
                // Verifica que las coordenadas estén dentro del rango permitido.
                if (x >= 0 && x < GridCols && y >= 0 && y < GridRows)
                {
                    // Verifica que no exista una ciudad en la misma posición.
                    foreach (var c in ciudades.Recorrer())
                    {
                        if (c.X == x && c.Y == y)
                        {
                            MessageBox.Show("Ya existe una ciudad en esa posición.");
                            return;
                        }
                    }

                    // Crea el objeto ciudad y lo agrega a la lista de ciudades.
                    var nuevaCiudad = new Ciudad { Nombre = nombreCiudad, X = x, Y = y };
                    ciudades.AgregarFinal(nuevaCiudad);

                    // Agrega la ciudad a los ListBox de selección.
                    ListBoxInicio.Items.Add(nuevaCiudad);
                    ListBoxFin.Items.Add(nuevaCiudad);

                    // Dibuja la ciudad en el canvas.
                    DibujarCiudad(GridCanvas, nuevaCiudad, _cellSize);
                }
                else
                {
                    MessageBox.Show("Coordenadas fuera de rango.");
                }
            }
            else
            {
                MessageBox.Show("Introduce valores numéricos válidos para X e Y.");
            }
        }

        /// <summary>
        /// Evento del botón para crear una carretera.
        /// Crea dos objetos Carretera (A->B y B->A) y los agrega a la lista de carreteras.
        /// </summary>
        private void BtnCrearCarretera_Click(object sender, RoutedEventArgs e)
        {
            // Verifica que se haya seleccionado una ciudad de inicio y una de fin.
            if (ListBoxInicio.SelectedItem is Ciudad ciudadInicio && ListBoxFin.SelectedItem is Ciudad ciudadFin)
            {
                // Verifica que las ciudades sean diferentes.
                if (ciudadInicio == ciudadFin)
                {
                    MessageBox.Show("Selecciona dos ciudades diferentes.");
                    return;
                }

                // Verifica que el tiempo de recorrido sea válido.
                if (!double.TryParse(InputTiempo.Text, out double tiempo) || tiempo <= 0)
                {
                    MessageBox.Show("Introduce un tiempo de recorrido válido (mayor a 0).");
                    return;
                }

                // Verifica si ya existe la carretera en ambas direcciones.
                bool existeIda = false, existeVuelta = false;
                foreach (var c in carreteras.Recorrer())
                {
                    if (c.Origen == ciudadInicio && c.Destino == ciudadFin)
                        existeIda = true;
                    if (c.Origen == ciudadFin && c.Destino == ciudadInicio)
                        existeVuelta = true;
                }

                // Crea y agrega la carretera de ida si no existe.
                if (!existeIda)
                {
                    var carreteraIda = new Carretera(ciudadInicio, ciudadFin, tiempo);
                    carreteras.AgregarFinal(carreteraIda);
                    DibujarCarreteraConTiempo(GridCanvas, carreteraIda, _cellSize);
                }

                // Crea y agrega la carretera de vuelta si no existe.
                if (!existeVuelta)
                {
                    var carreteraVuelta = new Carretera(ciudadFin, ciudadInicio, tiempo);
                    carreteras.AgregarFinal(carreteraVuelta);
                    DibujarCarreteraConTiempo(GridCanvas, carreteraVuelta, _cellSize);
                }
            }
            else
            {
                MessageBox.Show("Selecciona una ciudad de inicio y una de fin.");
            }
        }

        /// <summary>
        /// Evento del botón para crear un carro aleatorio.
        /// </summary>
        private void BtnCrearCarro_Click(object sender, RoutedEventArgs e)
        {
            CrearCarroEnCiudadAleatoria();
        }

        /// <summary>
        /// Crea un carro en una ciudad aleatoria que tenga al menos una carretera.
        /// </summary>
        private void CrearCarroEnCiudadAleatoria()
        {
            var ciudadesConCarretera = new ListaSimple<Ciudad>();
            foreach (var c in carreteras.Recorrer())
            {
                if (ciudadesConCarretera.IndiceDe(c.Origen) == -1)
                    ciudadesConCarretera.AgregarFinal(c.Origen);
                if (ciudadesConCarretera.IndiceDe(c.Destino) == -1)
                    ciudadesConCarretera.AgregarFinal(c.Destino);
            }

            if (ciudadesConCarretera.EstaVacia())
            {
                MessageBox.Show("No hay ciudades con carreteras para colocar un carro.");
                return;
            }

            var random = new Random();
            int idx = random.Next(0, ciudadesConCarretera.Tamano());
            var ciudad = ciudadesConCarretera.ElementoEn(idx);

            double x = ciudad.X * _cellSize + _cellSize / 2;
            double y = ciudad.Y * _cellSize + _cellSize / 2;
            double size = _cellSize * 0.25;

            var carroImg = new System.Windows.Controls.Image
            {
                Width = size,
                Height = size,
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/carro.png"))
            };
            Canvas.SetLeft(carroImg, x - size / 2);
            Canvas.SetTop(carroImg, y - size / 2);
            GridCanvas.Children.Add(carroImg);

            carros.AgregarFinal(new CarroVisual { CiudadActual = ciudad, Imagen = carroImg });
        }

        /// <summary>
        /// Evento del botón para viajar.
        /// </summary>
        private void BtnViajar_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxFin.SelectedItem is Ciudad ciudadFin)
            {
                double velocidad = SliderVelocidad.Value;
                int baseInterval = 30;
                int minInterval = 2;
                int interval = (int)(baseInterval + (100 - velocidad) * (baseInterval - minInterval) / 99.0);

                foreach (var carro in carros.Recorrer())
                {
                    if (carro.CiudadActual != ciudadFin)
                        AnimarCarro(carro, ciudadFin, _cellSize, interval);
                }
            }
            else
            {
                MessageBox.Show("Selecciona una ciudad de destino.");
            }
        }

        /// <summary>
        /// Dibuja una ciudad en el canvas.
        /// </summary>
        private void DibujarCiudad(Canvas canvas, Ciudad ciudad, double cellSize)
        {
            double x = ciudad.X * cellSize + cellSize / 2;
            double y = ciudad.Y * cellSize + cellSize / 2;
            double size = cellSize * 0.6;

            var ellipse = new System.Windows.Shapes.Ellipse
            {
                Width = size,
                Height = size,
                Fill = System.Windows.Media.Brushes.Yellow,
                Stroke = System.Windows.Media.Brushes.Black,
                StrokeThickness = 2
            };
            Canvas.SetLeft(ellipse, x - size / 2);
            Canvas.SetTop(ellipse, y - size / 2);
            canvas.Children.Add(ellipse);

            var label = new TextBlock
            {
                Text = ciudad.Nombre,
                Foreground = System.Windows.Media.Brushes.Black,
                FontWeight = FontWeights.Bold,
                FontSize = cellSize * 0.22,
                TextAlignment = TextAlignment.Center,
                Width = cellSize
            };
            Canvas.SetLeft(label, x - cellSize / 2);
            Canvas.SetTop(label, y + size / 2 - cellSize * 0.1);
            canvas.Children.Add(label);
        }

        /// <summary>
        /// Dibuja una carretera con el tiempo de recorrido en el canvas.
        /// </summary>
        private void DibujarCarreteraConTiempo(Canvas canvas, Carretera carretera, double cellSize)
        {
            double x1 = carretera.Origen.X * cellSize + cellSize / 2;
            double y1 = carretera.Origen.Y * cellSize + cellSize / 2;
            double x2 = carretera.Destino.X * cellSize + cellSize / 2;
            double y2 = carretera.Destino.Y * cellSize + cellSize / 2;

            var line = new System.Windows.Shapes.Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = System.Windows.Media.Brushes.Blue,
                StrokeThickness = 3
            };
            canvas.Children.Add(line);

            double textX = (x1 + x2) / 2;
            double textY = (y1 + y2) / 2;

            var tiempoLabel = new TextBlock
            {
                Text = carretera.Tiempo.ToString("0.##"),
                Foreground = System.Windows.Media.Brushes.Red,
                FontWeight = FontWeights.Bold,
                FontSize = cellSize * 0.3,
                Background = System.Windows.Media.Brushes.White,
                Opacity = 0.8,
                Padding = new Thickness(2)
            };
            Canvas.SetLeft(tiempoLabel, textX - cellSize * 0.15);
            Canvas.SetTop(tiempoLabel, textY - cellSize * 0.15);
            canvas.Children.Add(tiempoLabel);
        }

        /// <summary>
        /// Anima el movimiento de un carro de una ciudad a otra.
        /// </summary>
        private void AnimarCarro(CarroVisual carro, Ciudad destino, double cellSize, int interval)
        {
            var img = carro.Imagen;
            double size = img.Width;
            double x0 = carro.CiudadActual.X * cellSize + cellSize / 2 - size / 2;
            double y0 = carro.CiudadActual.Y * cellSize + cellSize / 2 - size / 2;
            double x1 = destino.X * cellSize + cellSize / 2 - size / 2;
            double y1 = destino.Y * cellSize + cellSize / 2 - size / 2;

            int steps = 50;
            int currentStep = 0;
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(interval)
            };
            timer.Tick += (s, e) =>
            {
                currentStep++;
                double t = (double)currentStep / steps;
                double x = x0 + (x1 - x0) * t;
                double y = y0 + (y1 - y0) * t;
                Canvas.SetLeft(img, x);
                Canvas.SetTop(img, y);
                if (currentStep >= steps)
                {
                    timer.Stop();
                    carro.CiudadActual = destino;
                }
            };
            timer.Start();
        }
    }
}
