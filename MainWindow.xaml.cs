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

        private double _cellSize = 0;

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
            // Calcula el tamaño máximo disponible para la cuadrícula
            double availableWidth = ActualWidth * 0.6;
            double availableHeight = ActualHeight * 0.8;

            double cellWidth = availableWidth / (GridCols + 1); // +1 para labels
            double cellHeight = availableHeight / (GridRows + 1); // +1 para labels
            double cellSize = Math.Min(cellWidth, cellHeight);
            cellSize *= CellSizeMultiplier;

            _cellSize = cellSize;

            // Ajusta el tamaño de los labels y el canvas
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
        }

        private void DrawCoordinateLabels()
        {
            HorizontalLabelsPanel.Children.Clear();
            VerticalLabelsPanel.Children.Clear();

            // Labels horizontales (superior)
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
                    Background = Brushes.Transparent,
                    Foreground = Brushes.Black,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(0)
                };
                HorizontalLabelsPanel.Children.Add(label);
            }

            // Labels verticales (izquierda)
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
                    Background = Brushes.Transparent,
                    Foreground = Brushes.Black,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(0)
                };
                VerticalLabelsPanel.Children.Add(label);
            }
        }

        private void DrawGridOnCanvas()
        {
            GridCanvas.Children.Clear();

            // Líneas verticales
            for (int i = 0; i <= GridCols; i++)
            {
                double x = i * _cellSize;
                var line = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = GridRows * _cellSize,
                    Stroke = Brushes.White,
                    StrokeThickness = 1
                };
                GridCanvas.Children.Add(line);
            }

            // Líneas horizontales
            for (int i = 0; i <= GridRows; i++)
            {
                double y = i * _cellSize;
                var line = new Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = GridCols * _cellSize,
                    Y2 = y,
                    Stroke = Brushes.White,
                    StrokeThickness = 1
                };
                GridCanvas.Children.Add(line);
            }
        }

        private void BtnColocar_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(InputX.Text, out int x) && int.TryParse(InputY.Text, out int y))
            {
                string ciudad = InputCiudad.Text?.Trim() ?? "";
                if (x >= 0 && x < GridCols && y >= 0 && y < GridRows)
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
                            TextAlignment = TextAlignment.Center,
                            Width = _cellSize
                        };
                        double labelX = x * _cellSize;
                        double labelY = y * _cellSize + _cellSize * 0.65;
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
