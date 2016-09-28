using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SignumXaml
{
    class Analisis
    {
       public static ListParameters lpDerecha = new ListParameters();
       public static ListParameters lpIzquierda = new ListParameters();

        public static void GenerarListasEnlazadas(List<string> senas, ListParameters lp) {
            lp.listaSiguiente = lp.raiz;
            bool primera = true;
            foreach (string seña in senas)
            {
                for (int i = 0; i < seña.Length; i++)
                {
                    if (i == 0 && !primera)
                    {

                        lp.listaSiguiente = Armar(seña[i], true,ref lp.listaSiguiente,lp);
                    }
                    else
                    {
                        lp.listaSiguiente = Armar(seña[i], false, ref lp.listaSiguiente, lp);
                    }
                }

                if (primera)
                {
                    primera = false;
                }

            }
            lp.listaSiguiente = Armar(' ', true, ref lp.listaSiguiente, lp);
        }

        public static List<Nodo> Armar(char letra, bool nuevaPalabra, ref List<Nodo> listaActual,ListParameters lp)
        {

            if (nuevaPalabra)
            {
                //agrego asterisco
                Nodo ult = new Nodo();
                ult.letra = '*';
                listaActual.Add(ult);


                listaActual = lp.raiz;
                //busco en lista actual
                bool esta = false;
                foreach (Nodo actual in listaActual)
                {
                    if (actual.letra.Equals(letra))
                    {
                        esta = true;
                        return actual.lista;
                    }
                }

                if (!esta && letra != ' ')
                {
                    Nodo nuevo = new Nodo();
                    nuevo.letra = letra;
                    listaActual.Add(nuevo);
                    return nuevo.lista;
                }

                return null;
            }
            else
            {

                bool esta = false;
                foreach (Nodo actual in listaActual)
                {
                    if (actual.letra.Equals(letra))
                    {
                        esta = true;
                        return actual.lista;
                    }
                }

                if (!esta)
                {
                    Nodo nuevo = new Nodo();
                    nuevo.letra = letra;
                    listaActual.Add(nuevo);
                    return nuevo.lista;
                }
                return null;
            }
        }

        public static List<Nodo> Recorrer(char letra, bool nuevaPalabra, ref List<Nodo> listaActual, ListParameters lp)
        {
            if (listaActual == null)
            {
                MessageBox.Show("ak");
            }
            lp.mostrar = false;
            lp.seguira = false;
            if (nuevaPalabra)
            {

                lp.seña = "";
                listaActual = lp.raiz;
                //busco en lista actual
                bool esta = false;
                lp.termina = false;
                foreach (Nodo actual in listaActual)
                {
                    if (actual.letra.Equals(letra))
                    {
                        lp.seña += letra;
                        esta = true;
                        return actual.lista;
                    }
                    if (actual.letra.Equals('*'))
                    {
                        lp.termina = true;
                    }
                }

                if (!esta)
                {
                    if (lp.termina)
                    {
                        listaActual = lp.raiz;
                        return listaActual;
                    }

                }

                return lp.raiz;
            }
            else
            {

                bool esta = false;
                lp.termina = false;
                foreach (Nodo actual in listaActual)
                {
                    if (actual.letra.Equals(letra))
                    {
                        esta = true;
                        lp.seña += letra;
                        return actual.lista;
                    }
                    if (actual.letra.Equals('*'))
                    {
                        lp.termina = true;
                    }
                }

                if (!esta)
                {
                    if (lp.termina)
                    {
                        listaActual = lp.raiz;
                        lp.proximaLetra = letra;
                        lp.seguira = true;
                        lp.mostrar = true;
                        return listaActual;
                    }
                    else
                    {
                        listaActual = lp.raiz;
                        lp.proximaLetra = letra;
                        lp.seguira = true;
                        return listaActual;
                    }

                }
                return listaActual;
            }
        }

        public static String NuevoSector(char sector, ListParameters lp)
        {
            lp.señaMostrar = "";
            if (lp.primero)
            {
                lp.primero = false;
                lp.listaSiguiente = Recorrer(sector, true, ref lp.raiz, lp);
                if (lp.mostrar)
                {
                    lp.señaMostrar = lp.seña;
                }
                if (lp.seguira)
                {
                    lp.listaSiguiente = Recorrer(lp.proximaLetra, true, ref lp.listaSiguiente,lp);
                }
                return lp.señaMostrar;
            }
            else
            {
                lp.listaSiguiente = Recorrer(sector, false, ref lp.listaSiguiente,lp);
                if (lp.mostrar)
                {
                    lp.señaMostrar = lp.seña;

                }
                if (lp.seguira)
                {
                    lp.listaSiguiente = Recorrer(lp.proximaLetra, true, ref lp.listaSiguiente,lp);
                }
                return lp.señaMostrar;
            }

        }
    }
}
