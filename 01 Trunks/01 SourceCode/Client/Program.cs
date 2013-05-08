using System;
using System.Windows.Forms;
using Core.Architecure;
using Core.Authority;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            WinApplication.GetInstance().Run();
        }

     }
}
