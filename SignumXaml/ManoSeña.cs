using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignumXaml
{
    class ManoSeña : IEquatable<ManoSeña>
    {
        public string manoderecha = "";
        public string manoizquierda = "";

        public override int GetHashCode()
        {
            return 0;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as ManoSeña);
        }
        public bool Equals(ManoSeña obj)
        {
            return obj != null && obj.manoderecha == this.manoderecha && obj.manoizquierda == this.manoizquierda;
        }
    }
}
