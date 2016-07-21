using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INetCore2.Drawing.Styles
{
    public class StyleGroup
    {
        public string Name { get; set; }
        public BaseStyleObject[] Styles => styles.ToArray();

        private readonly List<BaseStyleObject> styles = new List<BaseStyleObject>();

    }
}
