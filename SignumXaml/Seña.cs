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
    static class Seña
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
    }
}
