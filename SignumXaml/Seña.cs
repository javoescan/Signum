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
        static Dictionary<string, string> diccionario = new Dictionary<string, string>() { };
        public static Dictionary<string,string> CargarSeñas()
        {
            string strFileName = @"D:\Signum proyecto con cambios\señas.json";
            string strFileContent = File.ReadAllText(strFileName);
            JObject o1 = JObject.Parse(strFileContent);
            for (int i = 0; i < o1.GetValue("senasArray").Count(); i++)
            {
                diccionario.Add(o1.GetValue("senasArray")[i]["sena"].ToString(), o1.GetValue("senasArray")[i]["significado"].ToString());
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
