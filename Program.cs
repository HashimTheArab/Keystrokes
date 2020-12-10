using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Keystrokes
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process.Start("minecraft://");
            Process[] target = Process.GetProcessesByName("Minecraft.Windows");
            if (target.Length == 0)
            {
                MessageBox.Show("Open Minecraft first.");
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
