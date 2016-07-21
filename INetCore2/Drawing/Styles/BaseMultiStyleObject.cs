using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INetCore2.Drawing.Styles
{
    public class BaseMultiStyleObject : BaseStyleObject
    {
        public new BaseStyleObject[] Value { get; set; }

        public override string ToString()
        {
            var v = new StringBuilder();
            foreach (var baseStyleObject in Value)
            {
                v.Append(baseStyleObject.Value + " ");
            }

            return $"{DisplayName}: {Value}".Trim() + ";";
        }
    }
}
