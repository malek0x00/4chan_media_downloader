using System;
using System.Windows.Forms;

namespace lurk_moar.gui
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public static bool chkd=false;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        //add "Form1_MouseDown" to MouseDown event of the form
        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            chkd = checkBox4.Checked;
        }
    }
}
