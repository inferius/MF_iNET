using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

            bw.Dock = DockStyle.Fill;
            Controls.Add(bw);


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
            Console.WriteLine(@"Test color CSS [Default]: " + clr);

            //Console.WriteLine("HTML: " + main.ToHTML());
        }
    }
}
