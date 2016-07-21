using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BrowserWindow.Utils
{
    public static class Converters
    {
        public static Brush FromColorToBrushes(System.Drawing.Color color)
        {
            return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }
    }
}
