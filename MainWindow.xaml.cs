using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Waze
{
    public partial class MainWindow : Window
    {
        private const int GridRows = 6; // Cambiado a 6 filas
        private const int GridCols = 12;
        // Eliminado el multiplicador para evitar que se salga de la ventana

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
            // Calcula el tamaño máximo disponible para la cuadrícula (80% de la ventana)
            double availableWidth = ActualWidth * 0.8;
            double availableHeight = ActualHeight * 0.8;

            // Calcula el tamaño de celda considerando labels (una fila y columna extra)
            double cellWidth = availableWidth / (GridCols + 1);
            double cellHeight = availableHeight / (GridRows + 1);
            double cellSize = Math.Min(cellWidth, cellHeight);

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
            string ciudad = InputCiudad.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(ciudad))
            {
                MessageBox.Show("Por favor, ingresa el nombre de la ciudad antes de colocar el nodo.");
                return;
            }

            if (int.TryParse(InputX.Text, out int x) && int.TryParse(InputY.Text, out int y))
            {
                if (x >= 0 && x < GridCols && y >= 0 && y < GridRows)
                {
                    // Carga la imagen desde los recursos
                    var image = new Image
                    {
                        Width = _cellSize * 0.8,
                        Height = _cellSize * 0.8,
                        Source = new BitmapImage(new Uri("pack://application:,,,/Images/ciudad.png"))
                    };
                    Canvas.SetLeft(image, x * _cellSize + (_cellSize - image.Width) / 2);
                    Canvas.SetTop(image, y * _cellSize + (_cellSize - image.Height) / 2);
                    GridCanvas.Children.Add(image);

                    // Dibuja el nombre de la ciudad debajo de la imagen
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
                    double labelY = y * _cellSize + _cellSize * 0.85;
                    Canvas.SetLeft(label, labelX);
                    Canvas.SetTop(label, labelY);
                    GridCanvas.Children.Add(label);
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