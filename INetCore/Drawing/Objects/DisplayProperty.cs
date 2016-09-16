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
    }

    public enum DisplayPropertyType
    {
        Inline,
        Block,
        Grid,
        Flex,
        [Description("inline-grid")]
        InlineGrid,
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

   

}
