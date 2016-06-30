using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignumXaml
{
    class Analisis
    {
        public static int[] señas = new int[] { 123, 234, 456 };
        static string prueba = "3,4,5,6,5,8,1,2,3";
        public static string[] texto = prueba.Split(',');
        public static bool flag1;
        public void Analizar()
        {
            flag1 = true;
            //primer for corresponde a texto[]
            for (int i = 0; i <texto.Length; i++)
            {
                //segundo for corresponde a señas[]
                for (int j = 0; j < señas.Length; j++)
                {
                    //tercer for corresponde a señas[j]
                    for (int k = 0; k < señas[j].ToString().Length; k++)
                    {

                        if (texto[i].ToString() == señas[j].ToString()[k].ToString() && flag1 == true)
                        {

                        }
                        else {
                            flag1 = false;

                        }

                    }
                }
                
            }


        }

    }
}
