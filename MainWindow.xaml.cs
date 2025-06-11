using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Waze
{
    public partial class MainWindow : Window
    {
        private const int GridRows = 10;
        private const int GridCols = 15;
        private const double CellSizeMultiplier = 1.0125;

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

        private double _cellSize = 0;

        private void AdjustCanvasAndDrawGrid()
        {
            double availableWidth = ActualWidth * 0.6;
            double availableHeight = ActualHeight * 0.8;

            double cellWidth = availableWidth / GridCols;
            double cellHeight = availableHeight / GridRows;
            double cellSize = Math.Min(cellWidth, cellHeight);
            cellSize *= CellSizeMultiplier;

            double canvasWidth = cellSize * GridCols;
            double canvasHeight = cellSize * GridRows;


            GridCanvas.Width = canvasWidth;
            GridCanvas.Height = canvasHeight;

            _cellSize = cellSize;

            DrawGridOnCanvas(cellSize, canvasWidth, canvasHeight);
            DrawCoordinateLabels(cellSize, canvasWidth, canvasHeight);
        }

        private void DrawGridOnCanvas(double cellSize, double width, double height)
        {
            GridCanvas.Children.Clear();

            // Líneas verticales (incluye la última columna)
            for (int i = 0; i <= GridCols; i++)
            {
                double x = i * cellSize;
                var line = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = GridRows * cellSize,
                    Stroke = Brushes.White,
                    StrokeThickness = 1
                };
                GridCanvas.Children.Add(line);
            }
            

            // Líneas horizontales (incluye la última fila)
            for (int i = 0; i <= GridRows; i++)
            {
                double y = i * cellSize;
                var line = new Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = GridCols * cellSize,
                    Y2 = y,
                    Stroke = Brushes.White,
                    StrokeThickness = 1
                };
                GridCanvas.Children.Add(line);
            }
            
        }

        private void DrawCoordinateLabels(double cellSize, double canvasWidth, double canvasHeight)
        {
            HorizontalLabelsPanel.Children.Clear();
            VerticalLabelsPanel.Children.Clear();

            // Coordenadas verticales (izquierda)
            for (int row = 0; row <= GridRows; row++)
            {
                var label = new Label
                {
                    Content = row.ToString(),
                    Width = cellSize, // <-- Igual que la celda
                    Height = cellSize,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.Transparent,
                    Foreground = Brushes.Black,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(0)
                };
                VerticalLabelsPanel.Children.Add(label);
            }


            // Coordenadas verticales (izquierda)
            for (int row = 0; row <= GridRows; row++)
            {
                var label = new Label
                {
                    Content = row.ToString(),
                    Width = 32,
                    Height = cellSize,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.Transparent,
                    Foreground = Brushes.Black,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(0)
                };
                VerticalLabelsPanel.Children.Add(label);
            }
        }

        private void BtnColocar_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(InputX.Text, out int x) && int.TryParse(InputY.Text, out int y))
            {
                string ciudad = InputCiudad.Text?.Trim() ?? "";
                if (x >= 0 && x <= GridCols && y >= 0 && y <= GridRows)
                {
                    // Dibuja un punto en la celda (x, y)
                    double size = _cellSize * 0.5;
                    var ellipse = new Ellipse
                    {
                        Width = size,
                        Height = size,
                        Fill = Brushes.Red
                    };
                    Canvas.SetLeft(ellipse, x * _cellSize + (_cellSize - size) / 2);
                    Canvas.SetTop(ellipse, y * _cellSize + (_cellSize - size) / 2);
                    GridCanvas.Children.Add(ellipse);

                    // Dibuja el nombre de la ciudad debajo del punto
                    if (!string.IsNullOrEmpty(ciudad))
                    {
                        var label = new TextBlock
                        {
                            Text = ciudad,
                            Foreground = Brushes.Black,
                            FontWeight = FontWeights.Bold,
                            FontSize = _cellSize * 0.22,
                            TextAlignment = TextAlignment.Center
                        };
                        // Centrar el texto bajo el punto
                        double labelWidth = _cellSize;
                        double labelX = x * _cellSize + (_cellSize - labelWidth) / 2;
                        double labelY = y * _cellSize + _cellSize * 0.65;
                        label.Width = labelWidth;
                        Canvas.SetLeft(label, labelX);
                        Canvas.SetTop(label, labelY);
                        GridCanvas.Children.Add(label);
                    }
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
    }
}
