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
using Microsoft.Kinect;

namespace SignumXaml
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;
        int estado=0;
        string inicio = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    camera.Source = frame.ToBitmap();
                }
            }



            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    canvas.Children.Clear();

                    _bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body != null)
                        {
                            if (body.IsTracked)
                            {
                                // Find the joints
                                Joint handRight = body.Joints[JointType.HandRight];
                                Joint thumbRight = body.Joints[JointType.ThumbRight];

                                Joint handLeft = body.Joints[JointType.HandLeft];
                                Joint thumbLeft = body.Joints[JointType.ThumbLeft];

                                Joint head = body.Joints[JointType.Head];

                                Joint panza = body.Joints[JointType.SpineMid]; 

                                // Draw hands and thumbs
                                canvas.DrawHand(handRight, _sensor.CoordinateMapper);
                                canvas.DrawHand(handLeft, _sensor.CoordinateMapper);
                                canvas.DrawThumb(thumbRight, _sensor.CoordinateMapper);
                                canvas.DrawThumb(thumbLeft, _sensor.CoordinateMapper);
                                canvas.DrawPoint(head, _sensor.CoordinateMapper);
                                canvas.DrawPoint(panza, _sensor.CoordinateMapper);


                                //PRUEBA IF
                                tblEstado.Text = estado.ToString();
                                if (estado==0)
                                {
                                if (Math.Abs((head.Position.Y * 100 - 20) - (handRight.Position.Y * 100)) < 10 && Math.Abs((head.Position.X * 100) - (handRight.Position.X * 100)) < 10)
                                {
                                    tblPosicionMano.Text = "Pera MD";
                                        estado = 1;
                                        inicio = "peramd";
                                }else if (Math.Abs((head.Position.Y * 100 - 20) - (handLeft.Position.Y * 100)) < 10 && Math.Abs((head.Position.X * 100) - (handLeft.Position.X * 100)) < 10)
                                {
                                    tblPosicionMano.Text = "Pera MI";
                                        estado = 1;
                                        inicio = "perami";
                                    }
                                else
                                {
                                    tblPosicionMano.Text = "Nada";
                                }
                                }
                                if (estado==1)
                                {
                                    if (inicio == "peramd" && (Math.Abs((panza.Position.Y * 100) - (handRight.Position.Y * 100)) < 10) && (Math.Abs((panza.Position.X * 100) - (handRight.Position.X * 100)) < 10))
                                    {
                                        tblPosicionMano.Text = "Hola derecho";
                                        MessageBox.Show("Hola derecho");
                                        estado = 0;
                                        inicio = "";
                                    }
                                    else if (inicio == "perami" && (Math.Abs((panza.Position.Y * 100) - (handLeft.Position.Y * 100)) < 10) && (Math.Abs((panza.Position.X * 100) - (handLeft.Position.X * 100)) < 10))
                                    {
                                        MessageBox.Show("Hola izquierdo");
                                        estado = 0;
                                        inicio = "";
                                    }
                                }
                                //

                                xPositionR.Text = "RX: " + (handRight.Position.X*100).ToString();
                                yPositionR.Text = "RY: " + (handRight.Position.Y*100).ToString();

                                xPositionL.Text = "LX: " + (handLeft.Position.X*100).ToString();
                                yPositionL.Text = "LY: " + (handLeft.Position.Y*100).ToString();
                                // Find the hand states
                                string rightHandState = "-";
                                string leftHandState = "-";

                                switch (body.HandRightState)
                                {
                                    case HandState.Open:
                                        rightHandState = "Open";
                                        break;
                                    case HandState.Closed:
                                        rightHandState = "Closed";
                                        break;
                                    case HandState.Lasso:
                                        rightHandState = "Lasso";
                                        break;
                                    case HandState.Unknown:
                                        rightHandState = "Unknown...";
                                        break;
                                    case HandState.NotTracked:
                                        rightHandState = "Not tracked";
                                        break;
                                    default:
                                        break;
                                }

                                switch (body.HandLeftState)
                                {
                                    case HandState.Open:
                                        leftHandState = "Open";
                                        break;
                                    case HandState.Closed:
                                        leftHandState = "Closed";
                                        break;
                                    case HandState.Lasso:
                                        leftHandState = "Lasso";
                                        break;
                                    case HandState.Unknown:
                                        leftHandState = "Unknown...";
                                        break;
                                    case HandState.NotTracked:
                                        leftHandState = "Not tracked";
                                        break;
                                    default:
                                        break;
                                }

                                tblRightHandState.Text = rightHandState;
                                tblLeftHandState.Text = leftHandState;
                            }
                            
                        }
                        else
                        {
                            estado = 0;
                            inicio = "";
                        }
                    }
                }
            }
        }
    }
}
