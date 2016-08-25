using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using INetCore.BrowserTools;
using INetCore.Core.Language.CSS;
using INetCore.Core.Language.CSS.Values;
using CoreClass = INetCore.Core.Language.HTML.CoreClass;

namespace MiCHALosoft_INet
{
    public partial class Form1 : Form
    {
#if DEBUG
        #region DebugConsole
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        #endregion
#endif

        public Form1()
        {
            InitializeComponent();
#if DEBUG
            #region DebugConsole
            IntPtr ptr = GetForegroundWindow();

            int u;

            GetWindowThreadProcessId(ptr, out u);
            Process process = Process.GetProcessById(u);
            if (process.ProcessName == "cmd")    //Is the uppermost window a cmd process?
            {
                AttachConsole(process.Id);
                Console.WriteLine("hello. It looks like you started me from an existing console.");
            }
            else
            {
                AllocConsole();
            }
            #endregion
#endif

            //List<HtmlTag> tag = s.ParseString("<div style='background-color: red;' onclick=''><a href='http://www.test.cz/test' title='Zkušební odkaz'>text</a><br><a href='http://www.test.cz/test' title='Zkušební odkaz'>text</a></div>");
            //string t = System.IO.File.ReadAllText("C:\\Temp\\test.html");
            //List<HtmlTag> tag = s.ParseString(t);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Console.Clear();
            INetCore.Drawing.BrowserWindow bw = new INetCore.Drawing.BrowserWindow();
            INetCore.Drawing.Objects.BaseObject main = new INetCore.Drawing.Objects.BaseObject(bw);

            INetCore.Drawing.Objects.BaseObject b = new INetCore.Drawing.Objects.BaseObject(main);
            INetCore.Drawing.Objects.BaseObject x = new INetCore.Drawing.Objects.BaseObject(b);
            INetCore.Drawing.Objects.BaseObject y = new INetCore.Drawing.Objects.BaseObject(b);
            INetCore.Drawing.Objects.BaseObject z = new INetCore.Drawing.Objects.BaseObject(b);

            INetCore.Drawing.Objects.BaseObject m = new INetCore.Drawing.Objects.BaseObject(main);
            INetCore.Drawing.Objects.BaseObject q = new INetCore.Drawing.Objects.BaseObject(m);
            INetCore.Drawing.Objects.BaseObject r = new INetCore.Drawing.Objects.BaseObject(m);
            INetCore.Drawing.Objects.BaseObject s = new INetCore.Drawing.Objects.BaseObject(m);

            bw.Dock = DockStyle.Fill;
            Controls.Add(bw);

            //Odpovida HTML kodu:
            //<div style="position: relative; top: 100px; left: 100px; width: 600px; height: 100px; background-color: red;">
            //    <div style="position: static; float:right; left: 100px; top: 10px; width: 100px; height: 100px;  background-color: blue;"></div>
            //    <div style="position: static; float:right; left: 100px; top: 10px; width: 100px; height: 100px;  background-color: violet;"></div>
            //    <div style="position: relative; float:right; left: 100px; top: 10px; width: 100px; height: 100px;  background-color: black;"></div>
            //</div>

            //<div style="position: relative; top: 100px; left: 100px; width: 600px; height: 100px; background-color: red;">
            //    <div style="position: static; float:right; left: 100px; top: 10px; width: 100px; height: 100px;  background-color: blue;"></div>
            //    <div style="position: static; float:right; left: 100px; top: 10px; width: 100px; height: 100px;  background-color: violet;"></div>
            //    <div style="position: relative; float:right; left: 100px; top: 10px; width: 100px; height: 100px;  background-color: black;"></div>
            //</div>

            var main_test_style = "top: 0px; left: 0px; width: 0px; height: 0px; background-color: transparent;";
            INetCore.Core.Language.CSS.CoreClass.ApplyStyles(main, main_test_style);

            q.Background.Color = Color.Blue;
            q.Width = 100;
            q.Height = 100;
            q.ObjectPosition = new INetCore.Drawing.Objects.Position(100, 10);
            q.PositionType = INetCore.Drawing.Objects.PositionType.Static;
            q.ObjectName = "Child";
            q.Float = INetCore.Drawing.Objects.Float.Right;

            r.Background.Color = Color.Violet;
            r.Width = 100;
            r.Height = 100;
            r.ObjectPosition = new INetCore.Drawing.Objects.Position(100, 10);
            r.PositionType = INetCore.Drawing.Objects.PositionType.Static;
            r.Float = INetCore.Drawing.Objects.Float.Right;

            s.Background.Color = Color.Black;
            s.Width = 100;
            s.Height = 100;
            s.ObjectPosition = new INetCore.Drawing.Objects.Position(100, 10);
            s.PositionType = INetCore.Drawing.Objects.PositionType.Relative;
            s.Float = INetCore.Drawing.Objects.Float.Right;

            m.Background.Color = Color.Red;
            m.InnerText = "Zkušební text";
            m.Width = 600;
            m.Height = 100;
            //m.WidthUnit = INetCore.Drawing.Objects.Unit.Percentage;

            m.ObjectPosition = new INetCore.Drawing.Objects.Position(100, 100);
            m.PositionType = INetCore.Drawing.Objects.PositionType.Relative;
            //b.Childrens.Add(x);



            //x.BorderBottom.Color = Color.Black;
            //x.BorderBottom.Width = 3;
            x.Background.Color = Color.Blue;
            x.Width = 100;
            x.Height = 100;
            x.ObjectPosition = new INetCore.Drawing.Objects.Position(100, 10);
            x.PositionType = INetCore.Drawing.Objects.PositionType.Static;
            x.ObjectName = "Child";
            x.Float = INetCore.Drawing.Objects.Float.Right;

            y.Background.Color = Color.Violet;
            y.Width = 100;
            y.Height = 100;
            y.ObjectPosition = new INetCore.Drawing.Objects.Position(100, 10);
            y.PositionType = INetCore.Drawing.Objects.PositionType.Static;
            y.Float = INetCore.Drawing.Objects.Float.Right;

            var z_test_style = "background-color: black; width:100px; height: 100px; position: relative; left: 100px; top: 10px; float: right;";
            INetCore.Core.Language.CSS.CoreClass.ApplyStyles(z, z_test_style);
            //z.Background.Color = Color.Black;
            //z.Width = 100;
            //z.Height = 100;
            //z.ObjectPosition = new INetCore.Drawing.Objects.Position(100, 10);
            //z.PositionType = INetCore.Drawing.Objects.PositionType.Relative;
            //z.Float = INetCore.Drawing.Objects.Float.Right;

            var b_test_style = "background-color: green; width:600px; height: 100px; position: relative; top: 100px; left: 100px;";
            INetCore.Core.Language.CSS.CoreClass.ApplyStyles(b, b_test_style);
            ////b.BorderBottom.Color = Color.Black;
            ////b.BorderBottom.Width = 3;
            //b.Background.Color = Color.Red;
            //b.Width = 600;
            //b.Height = 100;
            ////b.WidthUnit = INetCore.Drawing.Objects.Unit.Percentage;

            //b.ObjectPosition = new INetCore.Drawing.Objects.Position(100, 100);
            //b.PositionType = INetCore.Drawing.Objects.PositionType.Relative;
            ////b.Childrens.Add(x);

            //INetCore.BrowserTools.DeveloperToolsAlpha dta = new DeveloperToolsAlpha(main);
            //dta.Show();

            var ss = new CoreClass();
            var t = System.IO.File.ReadAllText("C:\\Temp\\test2.html");
            //var tag = ss.ParseString(t);
            var completeList = new List<INetCore.Drawing.Objects.BaseObject>(100);
            main = ss.ToBaseObjects(bw, t, completeList);

            var styles = new CSSLinq(completeList.ToArray());
            styles.LoadStyleFromFile("C:\\Temp\\test.css");



            main.Draw();

            var bwpf = new BrowserWindow.TechnicalPreview(main);
            bwpf.Show();


            //var clr = new INetCore.Core.Language.CSS.Values.CssColorValue("#ce9c9c"); //206, 156, 156 | 0, 34%, 71%
            var clr = new INetCore.Core.Language.CSS.Values.CssColorValue("hsl(36, 100%, 90.2%)"); //255, 235, 205 | 36, 100%, 90.2%
            //var clr = new INetCore.Core.Language.CSS.Values.CssColorValue("rgb(255, 235, 205)"); //255, 235, 205 | 36, 100%, 90.2%
            //var clr = new INetCore.Core.Language.CSS.Values.CssColorValue("blanchedalmond"); //206, 156, 156 | 0, 34%, 71%

            Console.WriteLine(@"Šířka okna: " + bw.Width);
            Console.WriteLine(@"Test color CSS [HEX]: " + clr.ToString(CSSColorTypeUsed.Hex));
            Console.WriteLine(@"Test color CSS [ColorName]: " + clr.ToString(CSSColorTypeUsed.ColorName));
            Console.WriteLine(@"Test color CSS [RGB]: " + clr.ToString(CSSColorTypeUsed.Rgb));
            Console.WriteLine(@"Test color CSS [HSL]: " + clr.ToString(CSSColorTypeUsed.Hsl));
            Console.WriteLine(@"Test color CSS [Original]: " + clr.ToString(CSSColorTypeUsed.Original));

            //Console.WriteLine("HTML: " + main.ToHTML());
        }
    }
}
