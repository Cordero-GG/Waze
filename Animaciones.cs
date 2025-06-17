using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Waze.Estructuras;

namespace Waze
{
    public static class DibujadorAnimaciones
    {
        public static void DibujaPunto(Canvas canvas, Punto punto, double cellSize)
        {
            var image = new Image
            {
                Width = cellSize * 0.8,
                Height = cellSize * 0.8,
                Source = new BitmapImage(new Uri("pack://application:,,,/Images/ciudad.png"))
            };
            Canvas.SetLeft(image, punto.Coordenadas.X * cellSize + (cellSize - image.Width) / 2);
            Canvas.SetTop(image, punto.Coordenadas.Y * cellSize + (cellSize - image.Height) / 2);
            canvas.Children.Add(image);

            var label = new TextBlock
            {
                Text = punto.Nombre,
                Foreground = Brushes.Black,
                FontWeight = FontWeights.Bold,
                FontSize = cellSize * 0.22,
                TextAlignment = TextAlignment.Center,
                Width = cellSize
            };
            double labelX = punto.Coordenadas.X * cellSize;
            double labelY = punto.Coordenadas.Y * cellSize + cellSize * 0.85;
            Canvas.SetLeft(label, labelX);
            Canvas.SetTop(label, labelY);
            canvas.Children.Add(label);
        }

        public static void DibujaConexion(Canvas canvas, Conexion conexion, double cellSize)
        {
            var a = conexion.PuntoA.Coordenadas;
            var b = conexion.PuntoB.Coordenadas;

            double x1 = a.X * cellSize + cellSize / 2;
            double y1 = a.Y * cellSize + cellSize / 2;
            double x2 = b.X * cellSize + cellSize / 2;
            double y2 = b.Y * cellSize + cellSize / 2;

            double dx = x2 - x1;
            double dy = y2 - y1;
            double length = Math.Sqrt(dx * dx + dy * dy);
            double offset = 8;

            double perpX = -dy / length * offset;
            double perpY = dx / length * offset;

            var color = conexion.Bloqueada ? Brushes.Gray : Brushes.Blue;

            var linea = new Line
            {
                X1 = x1 + perpX,
                Y1 = y1 + perpY,
                X2 = x2 + perpX,
                Y2 = y2 + perpY,
                Stroke = color,
                StrokeThickness = 3,
                StrokeDashArray = new DoubleCollection { 2, 2 }
            };
            canvas.Children.Add(linea);

            var linea2 = new Line
            {
                X1 = x2 - perpX,
                Y1 = y2 - perpY,
                X2 = x1 - perpX,
                Y2 = y1 - perpY,
                Stroke = color,
                StrokeThickness = 3,
                StrokeDashArray = new DoubleCollection { 2, 2 }
            };
            canvas.Children.Add(linea2);
        }
    }
}

