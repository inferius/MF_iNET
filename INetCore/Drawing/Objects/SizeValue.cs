using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace INetCore.Drawing.Objects
{
    public class SizeValue
    {
        [DefaultValue(0)]
        public float Size { get; set; }
        [DefaultValue(Unit.Pixels)]
        public Unit Unit { get; set; }
        [DefaultValue(SizeValueSetting.Auto)]
        public SizeValueSetting ValueSetting { get; set; }

        public SizeValue(SizeValueSetting valueSetting)
        {
            ValueSetting = valueSetting;
        }

        public SizeValue(float size, Unit unit = Unit.Pixels)
        {
            Size = size;
            Unit = unit;
            ValueSetting = SizeValueSetting.UseSize;
        }
    }

    public enum SizeValueSetting
    {
        Auto,
        Inherit,
        Initial,
        UseSize
    }
}
