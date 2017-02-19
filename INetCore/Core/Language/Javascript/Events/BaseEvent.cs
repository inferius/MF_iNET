using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace INetCore.Core.Language.JavaScript.Events
{
    public class BaseEvent
    {
        JObject eventData = new JObject();

        public static void FireEvent(string name, JObject eventData)
        {
            return;
        }
    }
}
