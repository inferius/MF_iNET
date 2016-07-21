using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INetCore2.Drawing.Styles.StyleValues;

namespace INetCore2.Drawing.Styles.StyleObjects
{
    public class PositionObject : BaseStyleObject
    {
        //public new PositionValue Value { get; set; } = new PositionValue();
        public PositionObject()
        {
            Name = "position";
            DisplayName = "position";
        }

        public PositionObject(PositionType value) : this()
        {
            Value = new PositionValue(value);
        }
    }
}
