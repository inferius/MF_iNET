using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INetCore.Core.Language.JavaScript
{
    public class EventManager
    {
        public List<EventArgs> EventsList { get; private set; }

        public EventManager()
        {
            this.EventsList = new List<EventArgs>();
        }
    }
}
