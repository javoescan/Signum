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
        public static Dictionary<ManoSeña, string> CargarSeñas()
        {
            string strFileName = @"D:\Signum proyecto\señas.json";
            string strFileContent = File.ReadAllText(strFileName);
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

            using (StreamReader r = new StreamReader(@"D:\Signum proyecto\señas.json"))
            {
                string jsonn = r.ReadToEnd();
                SeñasJson señas = new SeñasJson();
                //no anda trae señas null
                señas = JsonConvert.DeserializeObject<SeñasJson>(jsonn);
                Seña nueva2 = new Seña();
                nueva2 = nueva;
                señas.data.Add(nueva2);
                string newJson = JsonConvert.SerializeObject(señas.data);
                File.WriteAllText(@"D:\Signum proyecto\señas.json", newJson);
                r.Close();
            }
        }
    }
}
