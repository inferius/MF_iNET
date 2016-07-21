using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INetCore.Core.Language.JavaScript.Events.Mouse
{
    public class Click : System.EventArgs
    {
        System.Windows.Forms.Cursor c;
        Drawing.Objects.BaseObject _parent;
        Drawing.Objects.BaseObject _sender;
        MouseButton _button;

        public Click(System.Windows.Forms.Cursor c, Drawing.Objects.BaseObject sender, Drawing.Objects.BaseObject parent, MouseButton buttonClick)
        {
            this.c = c;
            _sender = sender;
            _parent = parent;
            _button = buttonClick;
        }

        public System.Windows.Forms.Cursor CursorData
        {
            get { return this.c; }
        }

        public MouseButton MouseButton
        {
            get { return _button; }
        }

        public Drawing.Objects.BaseObject Parent
        {
            get { return _parent; }
        }

        public Drawing.Objects.BaseObject Sender
        {
            get { return _sender; }
        }
    }
}
