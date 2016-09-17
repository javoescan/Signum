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
        static bool primero = true;

        static bool termina = false;
        static bool mostrar = false;

        public static String seña = "";

        public static List<Nodo> raiz = new List<Nodo>();

        public static List<Nodo> listaSiguiente = new List<Nodo>();

        public static bool seguira = false;
        public static char proximaLetra = ' ';

        public static void GenerarListasEnlazadas(List<string> senas) {
            listaSiguiente = raiz;
            bool primera = true;
            foreach (string seña in senas)
            {
                for (int i = 0; i < seña.Length; i++)
                {
                    if (i == 0 && !primera)
                    {

                        listaSiguiente = Armar(seña[i], true, ref listaSiguiente);
                    }
                    else
                    {
                        listaSiguiente = Armar(seña[i], false, ref listaSiguiente);
                    }
                }

                if (primera)
                {
                    primera = false;
                }

            }
            listaSiguiente = Armar(' ', true, ref listaSiguiente);
        }

        public static List<Nodo> Armar(char letra, bool nuevaPalabra, ref List<Nodo> listaActual)
        {

            if (nuevaPalabra)
            {
                //agrego asterisco
                Nodo ult = new Nodo();
                ult.letra = '*';
                listaActual.Add(ult);


                listaActual = raiz;
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

        public static List<Nodo> Recorrer(char letra, bool nuevaPalabra, ref List<Nodo> listaActual)
        {
            mostrar = false;
            seguira = false;
            if (nuevaPalabra)
            {

                seña = "";
                listaActual = raiz;
                //busco en lista actual
                bool esta = false;
                termina = false;
                foreach (Nodo actual in listaActual)
                {
                    if (actual.letra.Equals(letra))
                    {
                        seña += letra;
                        esta = true;
                        return actual.lista;
                    }
                    if (actual.letra.Equals('*'))
                    {
                        termina = true;
                    }
                }

                if (!esta)
                {
                    if (termina)
                    {
                        listaActual = raiz;
                        return null;
                    }

                }

                return raiz;
            }
            else
            {

                bool esta = false;
                termina = false;
                foreach (Nodo actual in listaActual)
                {
                    if (actual.letra.Equals(letra))
                    {
                        esta = true;
                        seña += letra;
                        return actual.lista;
                    }
                    if (actual.letra.Equals('*'))
                    {
                        termina = true;
                    }
                }

                if (!esta)
                {
                    if (termina)
                    {
                        listaActual = raiz;
                        proximaLetra = letra;
                        seguira = true;
                        mostrar = true;
                        return listaActual;
                    }
                    else
                    {
                        listaActual = raiz;
                        proximaLetra = letra;
                        seguira = true;
                        return listaActual;
                    }

                }
                return listaActual;
            }
        }

        public static String NuevoSector(char sector)
        {
            if (primero)
            {
                primero = false;
                listaSiguiente = Recorrer(sector, true, ref raiz);
                if (mostrar)
                {
                    return seña;

                }
                return "";
            }
            else
            {
                listaSiguiente = Recorrer(sector, false, ref listaSiguiente);
                if (mostrar)
                {
                    return seña;
                }
                return "";
            }

        }
    }
}
