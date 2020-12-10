using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using Utilities;

namespace Keystrokes
{
    public partial class Form1 : Form
    {
        globalKeyboardHook gkh = new globalKeyboardHook();
        MouseHook mkh = new MouseHook();

        public const string WINDOW = "Minecraft";
        int clicks = 0;
        int time;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);

        public static IntPtr handle = FindWindow(null, WINDOW);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string IpClassName, string IpWindowName);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        public static extern bool GetWindowRect(IntPtr hwnd, out RECT IpRect);

        public static RECT rect;

        public struct RECT
        {
            public int left, top, right, bottom;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            this.BackColor = Color.Wheat;
            this.TransparencyKey = Color.Wheat;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            CPSLabel.Text = GetCPS().ToString() + " CPS";

            gkh.HookedKeys.Add(Keys.W);
            gkh.HookedKeys.Add(Keys.A);
            gkh.HookedKeys.Add(Keys.S);
            gkh.HookedKeys.Add(Keys.D);

            gkh.KeyDown += new KeyEventHandler(gkh_keypress);
            gkh.KeyUp += new KeyEventHandler(gkh_keyrelease);

            mkh.LeftButtonDown += new MouseHook.MouseHookCallback(left_pressed);
            mkh.LeftButtonUp += new MouseHook.MouseHookCallback(left_released);
            mkh.RightButtonDown += new MouseHook.MouseHookCallback(right_pressed);
            mkh.RightButtonUp += new MouseHook.MouseHookCallback(right_released);
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            mkh.Install();

            int style = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, style | 0x8000 | 0x20);

            GetWindowRect(handle, out rect);
            this.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);

            this.Left = rect.right - 100;
            this.Top = rect.top + 20;
            backgroundWorker1.RunWorkerAsync();

        }

        void OnApplicationExit(object sender, EventArgs e)
        {
            mkh.Uninstall();
        }

        void left_pressed(MouseHook.MSLLHOOKSTRUCT e)
        {
            LMBPanel.BackColor = Color.White;
            LMBLabel.ForeColor = Color.Black;
        }

        void right_pressed(MouseHook.MSLLHOOKSTRUCT e)
        {
            RMBPannel.BackColor = Color.White;
            RMBLabel.ForeColor = Color.Black;
        }

        void left_released(MouseHook.MSLLHOOKSTRUCT e)
        {
            LMBPanel.BackColor = Color.Black;
            LMBLabel.ForeColor = Color.White;
            AddClicks();
        }

        void right_released(MouseHook.MSLLHOOKSTRUCT e)
        {
            RMBPannel.BackColor = Color.Black;
            RMBLabel.ForeColor = Color.White;
        }

        public void AddClicks()
        {
            if (clicks == 0) time = Time();
            if(time != Time())
            {
                time = Time();
                clicks = 0;
            }
            clicks++;
            time = Time();
        }

        public int GetCPS()
        {
            if (clicks == 0) return 0;
            if (time != Time())
            {
                clicks = 0;
                time = 0;
                return 0;
            }
            return clicks;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                GetWindowRect(handle, out rect);
                this.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);

                this.Left = rect.right - 200;
                this.Top = rect.top + 30;
                CPSLabel.Text = GetCPS().ToString() + " CPS";
                Thread.Sleep(1);
            }
        }

        void gkh_keypress(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode.ToString();
            switch (key)
            {
                case "W":
                    WPanel.BackColor = Color.White;
                    WLabel.ForeColor = Color.Black;
                    break;
                case "A":
                    APanel.BackColor = Color.White;
                    ALabel.ForeColor = Color.Black;
                    break;
                case "S":
                    SPanel.BackColor = Color.White;
                    SLabel.ForeColor = Color.Black;
                    break;
                case "D":
                    DPanel.BackColor = Color.White;
                    DLabel.ForeColor = Color.Black;
                    break;
            }
        }

        void gkh_keyrelease(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode.ToString();
            switch (key)
            {
                case "W":
                    WPanel.BackColor = Color.Black;
                    WLabel.ForeColor = Color.White;
                    break;
                case "A":
                    APanel.BackColor = Color.Black;
                    ALabel.ForeColor = Color.White;
                    break;
                case "S":
                    SPanel.BackColor = Color.Black;
                    SLabel.ForeColor = Color.White;
                    break;
                case "D":
                    DPanel.BackColor = Color.Black;
                    DLabel.ForeColor = Color.White;
                    break;
            }
        }

        private void panelMAIN_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void WPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void WLabel_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        public int Time()
        {
            return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
