using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INetCore.Drawing.Objects.DisplayProperties
{
    public class DisplayFlexProperty : DisplayProperty
    {
        public int FlexOrder { get; set; }
        [DefaultValue(FlexDirection.Row)]
        public FlexDirection FlexDirection { get; set; }
        [DefaultValue(0)]
        public int FlexGrow { get; set; }
        [DefaultValue(1)]
        public int FlexShrink { get; set; }
        [DefaultValue(FlexWrap.Nowrap)]
        public FlexWrap FlexWrap { get; set; }
        public SizeValue FlexBasis { get; set; }
        public string FlexFlow { get; set; }
        [DefaultValue(JustifyContent.FlexStart)]
        public JustifyContent JustifyContent { get; set; }
        [DefaultValue(AlignFlex.Auto)]
        public AlignFlex AlignSelf { get; set; }
        [DefaultValue(AlignFlex.FlexStart)]
        public AlignFlex AlignItems { get; set; }
        [DefaultValue(AlignFlex.FlexStart)]
        public AlignFlex AlignContent { get; set; }

        public DisplayFlexProperty()
        {
            FlexBasis = new SizeValue(SizeValueSetting.Auto);
        }


    }

    public enum FlexDirection
    {
        Row,
        RowReverse,
        Column,
        ColumnReverse
    }

    public enum FlexWrap
    {
        Nowrap,
        Wrap,
        WrapReverse
    }

    public enum JustifyContent
    {
        FlexStart,
        FlexEnd,
        Center,
        SpaceBetween,
        SpaceAround
    }

    public enum AlignFlex
    {
        Auto,
        FlexStart,
        FlexEnd,
        Center,
        Baseline,
        Stretch
    }
}
