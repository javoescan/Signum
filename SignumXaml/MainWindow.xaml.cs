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
        Ellipse e1;
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;
        int estado=0;
        string inicio = "";

        List<int> SectoresRecorridosD = new List<int>();
        List<int> SectoresRecorridosI = new List<int>();

        double ElipseMedida = 0;
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
                                Joint neck = body.Joints[JointType.Neck];
                                Joint panza = body.Joints[JointType.SpineMid];
                                Joint hombroD = body.Joints[JointType.ShoulderRight];
                                Joint hombroI = body.Joints[JointType.ShoulderLeft];
                                Joint espinahombro = body.Joints[JointType.SpineShoulder];
                                Joint cadera = body.Joints[JointType.SpineBase];


                                // Draw hands and thumbs
                                //canvas.DrawHand(handRight, _sensor.CoordinateMapper);
                                // canvas.DrawHand(handLeft, _sensor.CoordinateMapper);
                                // canvas.DrawThumb(thumbRight, _sensor.CoordinateMapper);
                                // canvas.DrawThumb(thumbLeft, _sensor.CoordinateMapper);
                                // canvas.DrawPoint(head, _sensor.CoordinateMapper);
                                // canvas.DrawPoint(panza, _sensor.CoordinateMapper);

                                Rectangle RectManoDerecha = canvas.DibujarSector(handRight, handRight, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 100) / head.Position.Z));
                                Rectangle RectManoIzquierda = canvas.DibujarSector(handLeft, handLeft, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 100) / head.Position.Z));

                               // canvas.DrawSkeleton(body, _sensor.CoordinateMapper);

                                //Sector 1

                               Rectangle Sector1 = canvas.DibujarSector(head, hombroD, _sensor.CoordinateMapper, Math.Abs(head.Position.Y-neck.Position.Y)* ((2  * 500 )/ head.Position.Z));

                                //Sector 2
                                Rectangle Sector2 = canvas.DibujarSector(head, hombroI, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));

                                //Sector 3
                                Rectangle Sector3 = canvas.DibujarSector(neck, neck, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 200) / head.Position.Z),80/ head.Position.Z);

                                //Sector 4
                                Rectangle Sector4 = canvas.DibujarSector(hombroD, hombroD, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));

                                //Sector 5
                                Rectangle Sector5 = canvas.DibujarSector(espinahombro, espinahombro, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));

                                //Sector 6
                                Rectangle Sector6 = canvas.DibujarSector(hombroI, hombroI, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));

                                //Sector 7
                                Rectangle Sector7 = canvas.DibujarSector(panza, hombroD, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));

                                //Sector 8
                                Rectangle Sector8 = canvas.DibujarSector(panza, panza, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));

                                //Sector 9
                                Rectangle Sector9 = canvas.DibujarSector(panza, hombroI, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));

                                //Sector 10
                                Rectangle Sector10 = canvas.DibujarSector(cadera, cadera, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));

                                Rectangle[] SectoresRecs = new Rectangle[10] {Sector1,Sector2, Sector3, Sector4, Sector5, Sector6, Sector7, Sector8, Sector9, Sector10 };

                                tblResta.Text = ((head.Position.Y * 100 - 20) - (handRight.Position.Y * 100)).ToString();
                                tblz.Text = panza.Position.Z.ToString();

                                int SectorMD = Sectores.Intersecta(RectManoDerecha, SectoresRecs);
                                int SectorMI = Sectores.Intersecta(RectManoIzquierda, SectoresRecs);

                                if (SectorMD != -1 && SectorMD != 10)
                                {
                                    tblsenaD.Text = "Mano Derecha en sector " + SectorMD.ToString();
                                    SectoresRecorridosD.Add(SectorMD);
                                }
                                else if(SectorMD == 10)
                                {
                                    String ahora = "";
                                    int numo = -1;
                                    foreach (int num in SectoresRecorridosD)
                                    {
                                        if (numo != num)
                                        {
                                            ahora += num.ToString() + ",";
                                            numo = num;
                                        }
                                    }

                                    MessageBox.Show(ahora);
                                }
                                else
                                {
                                    tblsenaD.Text = "Nada";
                                }

                                if (SectorMI != -1 && SectorMI != 10)
                                {
                                    tblsenaI.Text = "Mano Izquierda en sector " + SectorMI.ToString();
                                    SectoresRecorridosI.Add(SectorMI);
                                }
                                else if(SectorMI == 10)
                                {
                                    String ahora = "";
                                    int numo = -1;
                                    foreach (int num in SectoresRecorridosI)
                                    {
                                        if (numo != num) { 
                                        ahora += num.ToString() + ",";
                                            numo = num;
                                        }
                                    }

                                    MessageBox.Show(ahora);
                                }
                                else
                                {
                                    tblsenaI.Text = "Nada";
                                }

                                

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
