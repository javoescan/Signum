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
    class Sectores
    {
        Joint handLeft {get;set;}
        Joint handRight { get; set; }

        public static Ellipse CreateEllipse(double width, double height, double desiredCenterX, double desiredCenterY)
        {
            Ellipse ellipse = new Ellipse { Width = width, Height = height };
            double left = desiredCenterX - (width / 2);
            double top = desiredCenterY - (height / 2);
            ellipse.Fill = new SolidColorBrush(Colors.Black);
            ellipse.Margin = new Thickness(left, top, 0, 0);
            return ellipse;
        }
    }
}
