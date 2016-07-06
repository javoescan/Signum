using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignumXaml
{
    
    class Prueba
    {
        string input1 = "There were ABCD Perls";
        static bool Contains1(string value)
        {
            // Searches for 4-letter constant string using Boyer-Moore style algorithm.
            // ... Uses switch as lookup table.
            int i = 3; // First index to check.
            int length = value.Length;
            while (i < length)
            {
                switch (value[i])
                {
                    case 'D':
                        // Last character in pattern found.
                        // ... Check for definite match.
                        if (value[i - 1] == 'C' &&
                        value[i - 2] == 'B' &&
                        value[i - 3] == 'A')
                        {
                            return true;
                        }
                        // Must be at least 4 characters away.
                        i += 4;
                        continue;
                    case 'C':
                        // Must be at least 1 character away.
                        i += 1;
                        continue;
                    case 'B':
                        // Must be at least 2 characters away.
                        i += 2;
                        continue;
                    case 'A':
                        // Must be at least 3 characters away.
                        i += 3;
                        continue;
                    default:
                        // Must be at least 4 characters away.
                        i += 4;
                        continue;
                }
            }
            // Nothing found.
            return false;
        }

        static bool Contains2(string value)
        {
            // Searches for 4-letter constant string with IndexOf.
            return value.IndexOf("ABCD", StringComparison.Ordinal) != -1;
        }
    }
}
