using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Waze.Estructuras;


namespace Waze
{
    public partial class MainWindow : Window
    {
        private const int GridRows = 6;
        private const int GridCols = 12;
        private double _cellSize = 0;

        private List<Ciudad> ciudades = new List<Ciudad>();
        private List<(Ciudad, Ciudad)> carreteras = new List<(Ciudad, Ciudad)>();

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            Loaded += MainWindow_Loaded;
            SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AdjustCanvasAndDrawGrid();
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustCanvasAndDrawGrid();
        }

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
        }

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

        private void AnimarCarro(Canvas canvas, Ciudad inicio, Ciudad fin, double cellSize)
        {
            // Elimina cualquier carro anterior
            var carros = canvas.Children.OfType<System.Windows.Controls.Image>()
                .Where(img => img.Source is System.Windows.Media.Imaging.BitmapImage bmp && bmp.UriSource.ToString().Contains("carro.png"))
                .ToList();
            foreach (var carro in carros)
                canvas.Children.Remove(carro);

            // Crea la imagen del carro
            double size = cellSize * 0.25;
            var carroImg = new System.Windows.Controls.Image
            {
                Width = size,
                Height = size,
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/carro.png"))
            };

            double x0 = inicio.X * cellSize + cellSize / 2 - size / 2;
            double y0 = inicio.Y * cellSize + cellSize / 2 - size / 2;
            double x1 = fin.X * cellSize + cellSize / 2 - size / 2;
            double y1 = fin.Y * cellSize + cellSize / 2 - size / 2;

            Canvas.SetLeft(carroImg, x0);
            Canvas.SetTop(carroImg, y0);
            canvas.Children.Add(carroImg);

            // Animación simple usando DispatcherTimer
            var steps = 50;
            int currentStep = 0;
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(15)
            };
            timer.Tick += (s, e) =>
            {
                currentStep++;
                double t = (double)currentStep / steps;
                double x = x0 + (x1 - x0) * t;
                double y = y0 + (y1 - y0) * t;
                Canvas.SetLeft(carroImg, x);
                Canvas.SetTop(carroImg, y);
                if (currentStep >= steps)
                {
                    timer.Stop();
                }
            };
            timer.Start();
        }


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

        private void RedrawCiudadesYCarreteras()
        {
            // Dibuja carreteras
            foreach (var carretera in carreteras)
            {
                Dibujador.DibujaCarretera(GridCanvas, carretera.Item1, carretera.Item2, _cellSize);
            }

            // Dibuja ciudades
            foreach (var ciudad in ciudades)
            {
                Dibujador.DibujaCiudad(GridCanvas, ciudad, _cellSize);
            }
        }

        private void BtnColocar_Click(object sender, RoutedEventArgs e)
        {
            string ciudad = InputCiudad.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(ciudad))
            {
                MessageBox.Show("Por favor, ingresa el nombre de la ciudad antes de colocar el nodo.");
                return;
            }

            if (ciudades.Any(c => c.Nombre.Equals(ciudad, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Ya existe una ciudad con ese nombre. Por favor, elige otro nombre.");
                return;
            }

            if (int.TryParse(InputX.Text, out int x) && int.TryParse(InputY.Text, out int y))
            {
                if (x >= 0 && x < GridCols && y >= 0 && y < GridRows)
                {
                    if (ciudades.Any(c => c.X == x && c.Y == y))
                    {
                        MessageBox.Show("Ya existe una ciudad en esa posición.");
                        return;
                    }

                    var nuevaCiudad = new Ciudad { Nombre = ciudad, X = x, Y = y };
                    ciudades.Add(nuevaCiudad);
                    ListBoxInicio.Items.Add(nuevaCiudad);
                    ListBoxFin.Items.Add(nuevaCiudad);

                    Dibujador.DibujaCiudad(GridCanvas, nuevaCiudad, _cellSize);
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

        private void BtnCrearCarretera_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxInicio.SelectedItem is Ciudad ciudadInicio && ListBoxFin.SelectedItem is Ciudad ciudadFin)
            {
                if (ciudadInicio == ciudadFin)
                {
                    MessageBox.Show("Selecciona dos ciudades diferentes.");
                    return;
                }

                bool existe = carreteras.Any(c =>
                    (c.Item1 == ciudadInicio && c.Item2 == ciudadFin) ||
                    (c.Item1 == ciudadFin && c.Item2 == ciudadInicio)
                );
                if (existe)
                {
                    MessageBox.Show("Ya existe una carretera entre estas dos ciudades.");
                    return;
                }

                carreteras.Add((ciudadInicio, ciudadFin));
                Dibujador.DibujaCarretera(GridCanvas, ciudadInicio, ciudadFin, _cellSize);
            }
            else
            {
                MessageBox.Show("Selecciona una ciudad de inicio y una de fin.");
            }
        }

        private void BtnCrearCarro_Click(object sender, RoutedEventArgs e)
        {
            CrearCarroEnCiudadAleatoria();
        }

        private void CrearCarroEnCiudadAleatoria()
        {
            var ciudadesConCarretera = carreteras
                .SelectMany(c => new[] { c.Item1, c.Item2 })
                .Distinct()
                .ToList();

            if (ciudadesConCarretera.Count == 0)
            {
                MessageBox.Show("No hay ciudades con carreteras para colocar un carro.");
                return;
            }

            var random = new Random();
            var ciudad = ciudadesConCarretera[random.Next(ciudadesConCarretera.Count)];

            double x = ciudad.X * _cellSize + _cellSize / 2;
            double y = ciudad.Y * _cellSize + _cellSize / 2;
            double size = _cellSize * 0.25;

            var carro = new System.Windows.Controls.Image
            {
                Width = size,
                Height = size,
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/carro.png"))
            };
            Canvas.SetLeft(carro, x - size / 2);
            Canvas.SetTop(carro, y - size / 2);
            GridCanvas.Children.Add(carro);
        }

        private void BtnViajar_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxInicio.SelectedItem is Ciudad ciudadInicio && ListBoxFin.SelectedItem is Ciudad ciudadFin)
            {
                if (ciudadInicio == ciudadFin)
                {
                    MessageBox.Show("Selecciona dos ciudades diferentes.");
                    return;
                }

                AnimarCarro(GridCanvas, ciudadInicio, ciudadFin, _cellSize);
            }
            else
            {
                MessageBox.Show("Selecciona una ciudad de inicio y una de fin.");
            }
        }
    }
}
