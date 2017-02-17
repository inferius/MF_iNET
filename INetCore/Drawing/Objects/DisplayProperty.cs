using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace INetCore.Drawing.Objects
{
    public class DisplayProperty
    {
        [DefaultValue(DisplayPropertyType.Block)]
        public DisplayPropertyType Type { get; set; }
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


        public DisplayProperty()
        {
            FlexBasis = new SizeValue(SizeValueSetting.Auto);
        }

    }

    public enum DisplayPropertyType
    {
        Inline,
        Block,
        Flex,
        [Description("inline-block")]
        InlineBlock,
        [Description("inline-flex")]
        InlineFlex,
        [Description("inline-table")]
        InlineTable,
        [Description("list-item")]
        ListItem,
        RunIn,
        Table,
        [Description("table-caption")]
        TableCaption,
        [Description("table-column-group")]
        TableColumnGroup,
        [Description("table-header-group")]
        TableHeaderGroup,
        [Description("table-footer-group")]
        TableFooterGroup,
        [Description("table-row-group")]
        TableRowGroup,
        [Description("table-cell")]
        TableCell,
        [Description("table-column")]
        TableColumn,
        [Description("table-row")]
        TableRow,
        None,
        Initial,
        Inherit
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
