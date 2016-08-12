using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace SignumXaml
{
     public static class Sectores
    {
        // public static Joint handLeft {get;set;}
        // public static Joint handRight { get; set; }

        public static Rectangle DibujarSector(this Canvas canvas, Joint jointy, Joint jointx, CoordinateMapper mapper, double diametro, double marginabajo = 0)
        {
            if (jointy.TrackingState == TrackingState.NotTracked || jointx.TrackingState == TrackingState.NotTracked) return null;

            Point point = jointx.Scale(mapper);
            Point point2 = jointy.Scale(mapper);

            Rectangle rectangle = new Rectangle
            {
                Width = diametro,
                Height = diametro,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 4
            };

            Canvas.SetLeft(rectangle, point.X - rectangle.Width / 2);
            if (marginabajo != 0)
            {
                Canvas.SetTop(rectangle, (point2.Y-marginabajo) - rectangle.Height / 2);
            }
            else {
                Canvas.SetTop(rectangle, point2.Y - rectangle.Height / 2);
            }

            canvas.Children.Add(rectangle);

            return rectangle;
        }

        public static int Intersecta(Rectangle recta1, Rectangle[] sectores) {
            Rect rect1 = new Rect(Canvas.GetLeft(recta1), Canvas.GetTop(recta1), recta1.Width, recta1.Height);
            for (int i = 0; i < 10; i++)

            {
                if (sectores[i]!=null) { 
                Rect rect2 = new Rect(Canvas.GetLeft(sectores[i]), Canvas.GetTop(sectores[i]), sectores[i].Width, sectores[i].Height);
                if (rect1.IntersectsWith(rect2))
                {
                    return i+1;
                }
                }
            }
            return -1;
        }
    }
}
