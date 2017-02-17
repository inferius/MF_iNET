using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INetCore.Core.Language.JavaScript.Events.Mouse
{
    public class Hover : System.EventArgs
    {
        System.Windows.Forms.Cursor c;
        public Hover(System.Windows.Forms.Cursor c)
        {
            this.c = c;
        }

        public System.Windows.Forms.Cursor CursorData
        {
            get { return this.c; }
        }
    }
}
