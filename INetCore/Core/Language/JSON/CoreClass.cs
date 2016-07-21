using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INetCore.Core.Language.JSON
{
    public class CoreClass
    {
        public static INetCore.Core.Language.JavaScript.Object ParseJSON(string json)
        {
            INetCore.Core.Language.JavaScript.Object ret = new JavaScript.Object();

            INetCore.Core.Language.JavaScript.Object last = ret;

            StringBuilder buf = new StringBuilder();
            int bracket = 0;
            bool obj_name = false;
            bool obj_value = false;

            for (int i = 0; i < json.Length; i++)
            {
                if (json[i] == '{')
                {
                    bracket++;
                }
                if (json[i] == '"')
                {
                    if (obj_name)
                    {
                        obj_name = false;
                        obj_value = true;
                    }
                    if (obj_value)
                    {
                        obj_value = false;
                    }
                }
            }


                return ret;
        }
    }
}
