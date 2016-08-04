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
        public static string seña;
        public static string significado;
        static Dictionary<ManoSeña, string> diccionario = new Dictionary<ManoSeña, string>() { };
        public static Dictionary<ManoSeña, string> CargarSeñas()
        {
            string strFileName = @"D:\javinio\Signum proyecto con cambios\señas.json";
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

        public static void AgregarSeña(string[] seña)
        {
            //REEMPLAZO TODO, CAMBIAR
            /*string json = JsonConvert.SerializeObject(seña);
            System.IO.File.WriteAllText(@"D:\Signum proyecto con cambios\señas.json", json);*/

            using (StreamReader r = new StreamReader(@"D:\Signum proyecto con cambios\señas.json"))
            {
                string jsonn = r.ReadToEnd();
                List<Seña> persons = JsonConvert.DeserializeObject<List<Seña>>(jsonn);
                //persons.Add(seña); esto no anda
                string newJson = JsonConvert.SerializeObject(persons);
                File.WriteAllText(@"D:\Signum proyecto con cambios\señas.json", newJson);
                r.Close();
            }
        }
    }
}
