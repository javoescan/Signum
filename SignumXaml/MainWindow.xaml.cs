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
using Microsoft.VisualBasic;

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
        String tempderecha = "";
        String tempizquierda = "";
        string[] arr1 = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i" };
        List<string> keywordsderecha = new List<string>();
        List<string> keywordsizquierda = new List<string>();
        int numo;
        bool grabando = false;
        bool comenzargrabado = false;
        List<int> SectoresRecorridosD = new List<int>();
        List<int> SectoresRecorridosI = new List<int>();

        //Declaracion Joints
        Joint handRight;
        Joint thumbRight;
        Joint wristRight;
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

        Dictionary<ManoSeña, string> diccionario = new Dictionary<ManoSeña, string>() { };

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

            List<string> lstSenasD = new List<string>();
            List<string> lstSenasI = new List<string>();

            foreach (ManoSeña sena in diccionario.Keys)
            {
                lstSenasD.Add(sena.manoderecha);
                lstSenasI.Add(sena.manoizquierda);
            }

            Analisis.GenerarListasEnlazadas(lstSenasD, Analisis.lpDerecha);
            Analisis.GenerarListasEnlazadas(lstSenasI, Analisis.lpIzquierda);

        }

        void BuscarJoints(Body body)
        {
            handRight = body.Joints[JointType.HandRight];
            thumbRight = body.Joints[JointType.ThumbRight];
            wristRight = body.Joints[JointType.WristRight];
            handRight = body.Joints[JointType.HandRight];
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

        void ActualizarSeñasTemporales(List<int> SectoresRecorridos, bool esderecha = true)
        {
            if (esderecha==true)
            {
                numo = -1;
                foreach (int num in SectoresRecorridos)
                {
                    if (numo != num)
                    {
                        tempderecha += arr1[num];
                        numo = num;
                    }
                }
            }
            else
            {
                numo = -1;
                foreach (int num in SectoresRecorridos)
                {
                    if (numo != num)
                    {
                        tempizquierda += arr1[num];
                        numo = num;
                    }
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
       
        char ultimaLetraEnviada = '.';
        char LetraEnviar = '.';

        char ultimaLetraEnviadaI = '.';
        char LetraEnviarI = '.';

        int contadorDerecha = 100;
        int contadorIzquierda = 100;

        string tempSeñaMD ="";
        string tempSeñaMI = "";
        int contSeñaTemp = 0;


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

                    zmano.Text = handRight.Position.Z.ToString();
                    zcabeza.Text = head.Position.Z.ToString();

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

                                if (!grabando && body.HandRightState == HandState.Closed && body.HandLeftState == HandState.Closed)
                                {
                                    grabando = true;
                                    SectoresRecorridosD.Clear();
                                    SectoresRecorridosI.Clear();
                                }

                                if (grabando && comenzargrabado)
                                {
                                    if (body.HandRightState == HandState.Closed && body.HandLeftState == HandState.Closed)
                                    {
                                        grabando = false;
                                        comenzargrabado = false;
                                        tblgrabando.Visibility = Visibility.Hidden;
                                        if (SectoresRecorridosD.Count > 0)
                                        {
                                            var dialog = new Significado();
                                            if (dialog.ShowDialog() == true)
                                            {
                                                if (dialog.ResponseText == "")
                                                {
                                                    MessageBox.Show("Debe escribir significado");
                                                }
                                                else
                                                {
                                                    Seña nueva = new Seña();
                                                    ActualizarSeñasTemporales(SectoresRecorridosD);
                                                    nueva.senad = tempderecha;
                                                    if (SectoresRecorridosI.Count > 0)
                                                    {
                                                        ActualizarSeñasTemporales(SectoresRecorridosI, false);
                                                        nueva.senai = tempizquierda;
                                                    }
                                                    else
                                                    {
                                                        nueva.senai = "";
                                                    }
                                                    nueva.significado = dialog.ResponseText;
                                                    Seña.AgregarSeña(nueva);
                                                    tempizquierda = "";
                                                    tempderecha = "";
                                                    SectoresRecorridosD.Clear();
                                                    SectoresRecorridosI.Clear();
                                                    diccionario.Clear();
                                                    TraerSeñas();
                                                }
                                            }

                                        }
                                    }

                                }
                                else
                                {
                                    if (grabando && !comenzargrabado)
                                    {
                                        if (body.HandRightState != HandState.Closed && body.HandLeftState != HandState.Closed)
                                        {
                                            comenzargrabado = true;
                                            tblgrabando.Visibility = Visibility.Visible;
                                        }
                                    }
                                }

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
                                    SectoresRecorridosD.Add(SectorMD - 1);

                                    LetraEnviar = Convert.ToChar(arr1[SectorMD - 1]);


                                }
                                else
                                {
                                    tblsenaD.Text = "Nada";

                                }

                                if (SectorMI != -1 && SectorMI != 10)
                                {
                                    tblsenaI.Text = "Mano Izquierda en sector " + SectorMI.ToString();
                                    SectoresRecorridosI.Add(SectorMI - 1);

                                    LetraEnviarI = Convert.ToChar(arr1[SectorMI - 1]);
                                }
                                else
                                {
                                    tblsenaI.Text = "Nada";
                                }

                                if (!grabando)
                                {

                                    //MANO DERECHA
                                    contadorDerecha--;
                                    if (ultimaLetraEnviada != LetraEnviar)
                                    {
                                        contadorDerecha = 30;
                                        ultimaLetraEnviada = LetraEnviar;

                                        if (Analisis.NuevoSector(LetraEnviar, Analisis.lpDerecha) != "")
                                        {
                                            tempSeñaMD = Analisis.lpDerecha.señaMostrar;
                                            contSeñaTemp = 5;
                                        }
                                        if (contadorDerecha == 0)
                                        {
                                            contadorDerecha = 30;
                                        }
                                    }
                                    if (contadorDerecha == 0)
                                    {
                                        ultimaLetraEnviada = '.';
                                        LetraEnviar = '.';
                                        if (Analisis.NuevoSector(' ', Analisis.lpDerecha) != "")
                                        {
                                            tempSeñaMD = Analisis.lpDerecha.señaMostrar;
                                            contSeñaTemp = 5;
                                        }
                                        contadorDerecha = 30;
                                    }
                                    //
                                    //MANO IZQUIERDA
                                    contadorIzquierda--;
                                    if (ultimaLetraEnviadaI != LetraEnviarI)
                                    {
                                        contadorIzquierda = 30;
                                        ultimaLetraEnviadaI = LetraEnviarI;

                                        if (Analisis.NuevoSector(LetraEnviarI, Analisis.lpIzquierda) != "")
                                        {
                                            tempSeñaMI = Analisis.lpIzquierda.señaMostrar;
                                            contSeñaTemp = 5;
                                        }
                                        if (contadorIzquierda == 0)
                                        {
                                            contadorIzquierda = 30;
                                        }
                                    }
                                    if (contadorIzquierda == 0)
                                    {
                                        ultimaLetraEnviadaI = '.';
                                        LetraEnviarI = '.';
                                        if (Analisis.NuevoSector(' ', Analisis.lpIzquierda) != "")
                                        {
                                            tempSeñaMI = Analisis.lpIzquierda.señaMostrar;
                                            contSeñaTemp = 5;
                                        }
                                        contadorIzquierda = 30;
                                    }

                                    //

                                    if (contSeñaTemp > 0)
                                    {
                                        contSeñaTemp--;
                                            try
                                            {
                                                ManoSeña ms = new ManoSeña();
                                                ms.manoderecha = tempSeñaMD;
                                                ms.manoizquierda = tempSeñaMI;
                                                tblsena.Text = diccionario[ms];
                                                temp2 = 80;
                                            }
                                            catch (Exception)
                                            {

                                            
                                            }
                                    }
                                    else if (contSeñaTemp == 0)
                                    {
                                        tempSeñaMI = "";
                                        tempSeñaMD = "";
                                    }
                                    

                                    tblsenaI.Text = Analisis.lpDerecha.seña.ToString() + " " + Analisis.lpIzquierda.seña.ToString();

                                    if (temp2 > 0)
                                    {
                                        temp2--;
                                    }
                                    else
                                    {
                                        tblsena.Text = "";
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
    }

    partial class Significado : Window
    {
        public string ResponseText
        {
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
