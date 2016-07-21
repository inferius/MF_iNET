using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INetCore.Drawing.Objects
{
    public class RealObject
    {
        public BaseObject Source { get; private set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public float Top { get; set; }
        public float Left { get; set; }

        public RealObject(BaseObject source)
        {
            Source = source;
        }

    }
}
