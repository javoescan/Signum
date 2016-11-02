using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignumXaml
{
    class Seña
    {
        public  string senad;
        public  string senai;
        public  string significado;
        static Dictionary<ManoSeña, string> diccionario = new Dictionary<ManoSeña, string>() { };
        static string fileName = "señas.json";
        static string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
        public static Dictionary<ManoSeña, string> CargarSeñas()
        {
            
            string strFileContent = File.ReadAllText(path);
            JObject o1 = JObject.Parse(strFileContent);
            for (int i = 0; i < o1.GetValue("senasArray").Count(); i++)
            {
                ManoSeña ms = new ManoSeña();
                ms.manoderecha = o1.GetValue("senasArray")[i]["senad"].ToString();
                ms.manoizquierda = o1.GetValue("senasArray")[i]["senai"].ToString();
                diccionario.Add(ms , o1.GetValue("senasArray")[i]["significado"].ToString());
            }
            return diccionario;
        }

        public static void AgregarSeña(Seña nueva)
        {
                List<Seña> data = new List<Seña>();
                string strFileContent = File.ReadAllText(path);
                JObject o1 = JObject.Parse(strFileContent);
                for (int i = 0; i < o1.GetValue("senasArray").Count(); i++)
                {
                    Seña ms = new Seña();
                    ms.senad = o1.GetValue("senasArray")[i]["senad"].ToString();
                    ms.senai = o1.GetValue("senasArray")[i]["senai"].ToString();
                    ms.significado = o1.GetValue("senasArray")[i]["significado"].ToString();
                    data.Add(ms);
                }
                data.Add(nueva);
            SeñasJson nuevalista = new SeñasJson();
            nuevalista.senasArray = new List<Seña>();
            nuevalista.senasArray=data;
                string newJson = JsonConvert.SerializeObject(nuevalista);
                File.WriteAllText(path, newJson);
            }
        }
    }

