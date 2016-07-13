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
using System.IO;
using Newtonsoft.Json;

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
        string inicio = "";
        String temp = "";
        string[] arr1 = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i" };
        int numo;
        bool buscoseña = true;
        bool termino = false;
        List<int> SectoresRecorridosD = new List<int>();
        List<int> SectoresRecorridosI = new List<int>();

        //Declaracion Joints
        Joint handRight;
        Joint thumbRight;
        Joint handLeft;
        Joint thumbLeft;
        Joint head;
        Joint neck;
        Joint panza;
        Joint hombroD;
        Joint hombroI;
        Joint espinahombro;
        Joint cadera;

        //Declaracion Sectores y Manos
        Rectangle RectManoDerecha;
        Rectangle RectManoIzquierda;
        Rectangle Sector1;
        Rectangle Sector2;
        Rectangle Sector3;
        Rectangle Sector4;
        Rectangle Sector5;
        Rectangle Sector6;
        Rectangle Sector7;
        Rectangle Sector8;
        Rectangle Sector9;
        Rectangle Sector0;
        Rectangle[] SectoresRecs;

        int SectorMD;
        int SectorMI;

        string rightHandState = "";
        string leftHandState = "";

        int temp2 = 0;

        Dictionary<string, string> diccionario = new Dictionary<string, string>() { };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TraerSeñas();
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

        void TraerSeñas()
        {
            diccionario = Seña.CargarSeñas();
        }
        
        bool BuscarSeñas(string text) {

            string[] keywords = new string[diccionario.Count];

            for (int i = 0; i < diccionario.Count; i++)
            {
                keywords[i] = diccionario.Keys.ElementAt(i);
            }
            List<Coincidencia> states = Analisis.FindAllStates(text, keywords);
            if (states != null && states.Count > 0)
            {
                tblsena.Text = diccionario[states[0].key];
                temp2 = 80;
                return true;
            }
            return false;
        }

        void BuscarJoints(Body body)
        {
            handRight = body.Joints[JointType.HandRight];
            thumbRight = body.Joints[JointType.ThumbRight];
            handLeft = body.Joints[JointType.HandLeft];
            thumbLeft = body.Joints[JointType.ThumbLeft];
            head = body.Joints[JointType.Head];
            neck = body.Joints[JointType.Neck];
            panza = body.Joints[JointType.SpineMid];
            hombroD = body.Joints[JointType.ShoulderRight];
            hombroI = body.Joints[JointType.ShoulderLeft];
            espinahombro = body.Joints[JointType.SpineShoulder];
            cadera = body.Joints[JointType.SpineBase];
        }
        void DibujarEsqueleto(Body body = null)
        {
            if (body!=null)
            {
                canvas.DrawSkeleton(body, _sensor.CoordinateMapper);
            }else { 
            canvas.DrawHand(handRight, _sensor.CoordinateMapper);
            canvas.DrawHand(handLeft, _sensor.CoordinateMapper);
            canvas.DrawThumb(thumbRight, _sensor.CoordinateMapper);
            canvas.DrawThumb(thumbLeft, _sensor.CoordinateMapper);
            canvas.DrawPoint(head, _sensor.CoordinateMapper);
            canvas.DrawPoint(panza, _sensor.CoordinateMapper);
            }
        }
        void DibujarSectores()
        {
            RectManoDerecha = canvas.DibujarSector(handRight, handRight, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 100) / head.Position.Z));
            RectManoIzquierda = canvas.DibujarSector(handLeft, handLeft, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 100) / head.Position.Z));
            Sector0 = canvas.DibujarSector(head, hombroD, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));
            Sector1 = canvas.DibujarSector(head, hombroI, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));
            Sector2 = canvas.DibujarSector(neck, neck, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 200) / head.Position.Z), 80 / head.Position.Z);
            Sector3 = canvas.DibujarSector(hombroD, hombroD, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));
            Sector4 = canvas.DibujarSector(espinahombro, espinahombro, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));
            Sector5 = canvas.DibujarSector(hombroI, hombroI, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));
            Sector6 = canvas.DibujarSector(panza, hombroD, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));
            Sector7 = canvas.DibujarSector(panza, panza, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));
            Sector8 = canvas.DibujarSector(panza, hombroI, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));
            Sector9 = canvas.DibujarSector(cadera, cadera, _sensor.CoordinateMapper, Math.Abs(head.Position.Y - neck.Position.Y) * ((2 * 500) / head.Position.Z));
            SectoresRecs = new Rectangle[10] { Sector0, Sector1, Sector2, Sector3, Sector4, Sector5, Sector6, Sector7, Sector8, Sector9};
        }

        void ActualizarSeñasTemporales(List<int> SectoresRecorridos)
        {
            numo = -1;
            foreach (int num in SectoresRecorridosD)
            {
                if (numo != num)
                {
                    temp += arr1[num];
                    numo = num;
                }
            }
        }

        void EncontrarEstadoMano(Body body)
        {
            rightHandState = "-";
            leftHandState = "-";
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

                                BuscarJoints(body);

                                /*OPCIONAL
                                DibujarEsqueleto(body);*/

                                DibujarSectores();    

                                tblResta.Text = ((head.Position.Y * 100 - 20) - (handRight.Position.Y * 100)).ToString();
                                tblz.Text = panza.Position.Z.ToString();

                                //Mostrar posiciones
                                xPositionR.Text = "RX: " + (handRight.Position.X * 100).ToString();
                                yPositionR.Text = "RY: " + (handRight.Position.Y * 100).ToString();
                                xPositionL.Text = "LX: " + (handLeft.Position.X * 100).ToString();
                                yPositionL.Text = "LY: " + (handLeft.Position.Y * 100).ToString();

                                //Encontrar estado de la mano
                                EncontrarEstadoMano(body);

                                //Mostrar intersecciones con sectores
                                SectorMD = Sectores.Intersecta(RectManoDerecha, SectoresRecs);
                                SectorMI = Sectores.Intersecta(RectManoIzquierda, SectoresRecs);
                                if (SectorMD != -1 && SectorMD != 10)
                                {
                                    tblsenaD.Text = "Mano Derecha en sector " + SectorMD.ToString();
                                    SectoresRecorridosD.Add(SectorMD-1);
                                }
                                else
                                {
                                    tblsenaD.Text = "Nada";
                                }

                                if (SectorMI != -1 && SectorMI != 10)
                                {
                                    tblsenaI.Text = "Mano Izquierda en sector " + SectorMI.ToString();
                                    SectoresRecorridosI.Add(SectorMI-1);
                                }
                                else
                                {
                                    tblsenaI.Text = "Nada";
                                }

                                if (buscoseña)
                                {
                                    ActualizarSeñasTemporales(SectoresRecorridosD);
                                    //Si encuentra la seña la muestra y reinicia los sectores recorridos hasta el momento
                                    //Si no, borra el temporal y va probando frame a frame hasta encontrar alguna seña
                                    if (BuscarSeñas(temp))
                                    {
                                        temp = "";
                                        SectoresRecorridosD.Clear();
                                    }
                                    else
                                    {
                                        temp = "";
                                    }

                                    if (temp2 > 0)
                                    {
                                        temp2--;
                                    }
                                    else
                                    {
                                        tblsena.Text = "";
                                    }
                                }
                                else
                                {
                                    if (termino)
                                    {
                                        //NO MUESTRA TXT NI BTN
                                        txtSignificado.Visibility = Visibility.Visible;
                                        btnSignificado.Visibility = Visibility.Visible;
                                        if(txtSignificado.Text != "")
                                        {
                                            buscoseña = true;
                                            termino = false;
                                            string[] seña = new string[] { SectoresRecorridosD.ToString(), txtSignificado.Text };
                                            btnTerminar.Visibility = Visibility.Hidden;
                                            txtSignificado.Visibility = Visibility.Hidden;
                                            btnSignificado.Visibility = Visibility.Hidden;
                                            Seña.AgregarSeña(seña);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Por favor ingrese el significado de la seña");
                                        }
                                    }
                                }

                            }
                            
                        }
                        else
                        {
                            inicio = "";
                        }
                    }
                }
            }
        }

        //Mostrar u ocultar información
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            tblz.Visibility= Visibility.Visible;
            tblResta.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            tblz.Visibility = Visibility.Hidden;
            tblResta.Visibility = Visibility.Hidden;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

            xPositionL.Visibility = Visibility.Visible;
            xPositionR.Visibility = Visibility.Visible;
            yPositionL.Visibility = Visibility.Visible;
            yPositionR.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {

            xPositionL.Visibility = Visibility.Hidden;
            xPositionR.Visibility = Visibility.Hidden;
            yPositionL.Visibility = Visibility.Hidden;
            yPositionR.Visibility = Visibility.Hidden;
        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            tblLeftHandState.Visibility = Visibility.Visible;
            tblRightHandState.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked_2(object sender, RoutedEventArgs e)
        {
            tblLeftHandState.Visibility = Visibility.Hidden;
            tblRightHandState.Visibility = Visibility.Hidden;
        }

        private void CheckBox_Checked_3(object sender, RoutedEventArgs e)
        {
            tblsena.Visibility = Visibility.Visible;
            tblsenaD.Visibility = Visibility.Visible;
            tblsenaI.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked_3(object sender, RoutedEventArgs e)
        {
            tblsena.Visibility = Visibility.Hidden;
            tblsenaD.Visibility = Visibility.Hidden;
            tblsenaI.Visibility = Visibility.Hidden;
        }

        private void CheckBox_Checked_4(object sender, RoutedEventArgs e)
        {
            tblPosicionMano.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked_4(object sender, RoutedEventArgs e)
        {
            tblPosicionMano.Visibility = Visibility.Hidden;
        }

        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            buscoseña = false;
            btnGrabar.Visibility = Visibility.Hidden;
            btnTerminar.Visibility = Visibility.Visible;
        }

        private void btnTerminar_Click(object sender, RoutedEventArgs e)
        {
            termino = true;
        }
    }
}
