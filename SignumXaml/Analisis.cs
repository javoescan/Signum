using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignumXaml
{
    class Analisis
    {

        private const int MaxStates = 6 * 50 + 10;
        private const int MaxChars = 26;

        private static int[] Out = new int[MaxStates];
        private static int[] FF = new int[MaxStates];
        private static int[,] GF = new int[MaxStates, MaxChars];

        private static int BuildMatchingMachine(string[] words, char lowestChar = 'a', char highestChar = 'z')
        {
            Out = Enumerable.Repeat(0, Out.Length).ToArray();
            FF = Enumerable.Repeat(-1, FF.Length).ToArray();

            for (int i = 0; i < MaxStates; ++i)
            {
                for (int j = 0; j < MaxChars; ++j)
                {
                    GF[i, j] = -1;
                }
            }

            int states = 1;

            for (int i = 0; i < words.Length; ++i)
            {
                string keyword = words[i];
                int currentState = 0;

                for (int j = 0; j < keyword.Length; ++j)
                {
                    int c = keyword[j] - lowestChar;

                    if (GF[currentState, c] == -1)
                    {
                        GF[currentState, c] = states++;
                    }

                    currentState = GF[currentState, c];
                }

                Out[currentState] |= (1 << i);
            }

            for (int c = 0; c < MaxChars; ++c)
            {
                if (GF[0, c] == -1)
                {
                    GF[0, c] = 0;
                }
            }

            List<int> q = new List<int>();
            for (int c = 0; c <= highestChar - lowestChar; ++c)
            {
                if (GF[0, c] != -1 && GF[0, c] != 0)
                {
                    FF[GF[0, c]] = 0;
                    q.Add(GF[0, c]);
                }
            }

            while (Convert.ToBoolean(q.Count))
            {
                int state = q[0];
                q.RemoveAt(0);

                for (int c = 0; c <= highestChar - lowestChar; ++c)
                {
                    if (GF[state, c] != -1)
                    {
                        int failure = FF[state];

                        while (GF[failure, c] == -1)
                        {
                            failure = FF[failure];
                        }

                        failure = GF[failure, c];
                        FF[GF[state, c]] = failure;
                        Out[GF[state, c]] |= Out[failure];
                        q.Add(GF[state, c]);
                    }
                }
            }

            return states;
        }

        private static int FindNextState(int currentState, char nextInput, char lowestChar = 'a')
        {
            int answer = currentState;
            int c = nextInput - lowestChar;

            while (GF[answer, c] == -1)
            {
                answer = FF[answer];
            }

            return GF[answer, c];
        }

        public static List<Coincidencia> FindAllStates(string text, string[] keywords, char lowestChar = 'a', char highestChar = 'z')
        {
            BuildMatchingMachine(keywords, lowestChar, highestChar);

            int currentState = 0;
            List<Coincidencia> retVal = new List<Coincidencia>();

            for (int i = 0; i < text.Length; ++i)
            {
                currentState = FindNextState(currentState, text[i], lowestChar);

                if (Out[currentState] == 0)
                    continue;

                for (int j = 0; j < keywords.Length; ++j)
                {
                    if (Convert.ToBoolean(Out[currentState] & (1 << j)))
                    {
                        Coincidencia encuentro = new Coincidencia();
                        encuentro.pos = i - keywords[j].Length + 1;
                        encuentro.key = keywords[j];
                        retVal.Insert(0, encuentro);
                    }
                }
            }

            return retVal;
        }

    }
}
