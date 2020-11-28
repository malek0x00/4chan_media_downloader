using ChanDownloader;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lurk_moar.gui
{
    public partial class Form1 : Form
    {

        // \r\n to insert new line
        //per = Val * 100 / total

        public int current_files_number;
        public int filesc = 0;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        

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


        public static Downloader downloader = new Downloader();
        public int count;

        public Form1()
        {
            InitializeComponent();
            
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            int thc = 0;
            string[] url = geturl();
            guna2CircleProgressBar2.Maximum = url.Length;
            
            foreach (string u in url)
            {
                thc++;
                await download(u);
                logwrite(thc+"/"+url.Length+" downloaded");
                logwrite("\r\n ******************************");
                guna2CircleProgressBar2.Value += thc;
                
            }

            sleepmode();
            
        }


        public async Task download(string url)
        {
            try { 
            var thread = await downloader.LoadThread(url);
            var id = thread.Id;
            var subject = thread.Subject;
            var safeName = thread.SemanticSubject;
            var files = thread.Files;
            count = files.Count;
            current_files_number = count;
            label3.Text = "0 / "+ files.Count.ToString();
            guna2CircleProgressBar1.Maximum = count;
            
            /*
             * the webclient is exposed so you can hook up your event callbacks
             * you can get the current downloaded file index from downloader.CurrentFileNumber
            */

            var path = $"{Directory.GetCurrentDirectory()}\\{safeName}";
            Directory.CreateDirectory(path);
            logwrite("thread: "+id+ " has "+files.Count+" media files");
            logwrite("Downloading, please wait...");


            downloader.WebClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            await downloader.DownloadFiles(thread, path);
            Console.ForegroundColor = ConsoleColor.Green;
            logwrite("  > thread '"+subject+"' downloaded successfully");
            
            }
            catch (Exception ex)
            {
                logwrite("exception error: enter a valid url ");
            }


        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var x = downloader.CurrentFileNumber;
            guna2CircleProgressBar1.Value = x;
            label3.Text = x.ToString() + " / " + current_files_number.ToString();
            
            //per = Val * 100 / total
            //var per = (x * 100) / count;
            //guna2CircleProgressBar1.Value += per;
            




        }




        public void logwrite(string txt)
        {
            richTextBox1.Text += "\r\n" + txt;
        }

        public string[] geturl()
        {

            string txt = guna2TextBox1.Text;

            string[] lines = txt.Split(new[] { '\r', '\n' });
            
            lines = lines.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            
            return lines;

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Form2 secf = new Form2();

            secf.Show();
        }

        public void sleepmode()
        {
            if (Form2.chkd)
            {
                logwrite("finished, sleep well fren");
                SetSuspendState(false, true, true);
            }
            
        }
    }
}