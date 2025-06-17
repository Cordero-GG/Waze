using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Waze.Estructuras;

namespace Waze
{
    public static class Dibujador
    {
        public static void DibujaCiudad(Canvas canvas, Ciudad ciudad, double cellSize)
        {
            var image = new Image
            {
                Width = cellSize * 0.8,
                Height = cellSize * 0.8,
                Source = new BitmapImage(new Uri("pack://application:,,,/Images/ciudad.png"))
            };
            Canvas.SetLeft(image, ciudad.X * cellSize + (cellSize - image.Width) / 2);
            Canvas.SetTop(image, ciudad.Y * cellSize + (cellSize - image.Height) / 2);
            canvas.Children.Add(image);

            var label = new TextBlock
            {
                Text = ciudad.Nombre,
                Foreground = Brushes.Black,
                FontWeight = FontWeights.Bold,
                FontSize = cellSize * 0.22,
                TextAlignment = TextAlignment.Center,
                Width = cellSize
            };
            double labelX = ciudad.X * cellSize;
            double labelY = ciudad.Y * cellSize + cellSize * 0.85;
            Canvas.SetLeft(label, labelX);
            Canvas.SetTop(label, labelY);
            canvas.Children.Add(label);
        }

        public static void DibujaCarretera(Canvas canvas, Ciudad ciudadInicio, Ciudad ciudadFin, double cellSize)
        {
            double x1 = ciudadInicio.X * cellSize + cellSize / 2;
            double y1 = ciudadInicio.Y * cellSize + cellSize / 2;
            double x2 = ciudadFin.X * cellSize + cellSize / 2;
            double y2 = ciudadFin.Y * cellSize + cellSize / 2;

            double dx = x2 - x1;
            double dy = y2 - y1;
            double length = Math.Sqrt(dx * dx + dy * dy);
            double offset = 8;

            double perpX = -dy / length * offset;
            double perpY = dx / length * offset;

            var lineaIda = new Line
            {
                X1 = x1 + perpX,
                Y1 = y1 + perpY,
                X2 = x2 + perpX,
                Y2 = y2 + perpY,
                Stroke = Brushes.Blue,
                StrokeThickness = 3,
                StrokeDashArray = new DoubleCollection { 2, 2 }
            };
            canvas.Children.Add(lineaIda);

            var lineaVuelta = new Line
            {
                X1 = x2 - perpX,
                Y1 = y2 - perpY,
                X2 = x1 - perpX,
                Y2 = y1 - perpY,
                Stroke = Brushes.Red,
                StrokeThickness = 3,
                StrokeDashArray = new DoubleCollection { 2, 2 }
            };
            canvas.Children.Add(lineaVuelta);
        }
    }
}
