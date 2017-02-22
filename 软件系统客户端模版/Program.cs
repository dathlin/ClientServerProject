using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace 软件系统客户端模版
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            //=====================================================================
            //为了强制只启动一个应用程序的实例

            Process process = Process.GetCurrentProcess();
            //遍历应用程序的同名进程组
            foreach (Process p in Process.GetProcessesByName(process.ProcessName))
            {
                //不是同一进程则关闭刚刚创建的进程
                if (p.Id != process.Id)
                {
                    //此处显示原先的窗口需要一定的时间，不然无法显示
                    ShowWindowAsync(p.MainWindowHandle, 9);
                    SetForegroundWindow(p.MainWindowHandle);
                    System.Threading.Thread.Sleep(10);
                    Application.Exit();//关闭当前的应用程序
                    return;
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //===================================================================
            //运行主窗口之前先进行账户的验证
            FormLogin login = new FormLogin();
            if (login.ShowDialog() == DialogResult.OK)
            {
                login.Dispose();
                Application.Run(new FormMainWindow());
            }
            else
            {
                login.Dispose();
            }
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
