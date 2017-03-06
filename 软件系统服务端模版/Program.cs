using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace 软件系统服务端模版
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
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
            //设置应用程序的线程池数量，防止服务器端卡死状态，根据内存及CPU进行更改
            System.Threading.ThreadPool.SetMaxThreads(1000, 256);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
