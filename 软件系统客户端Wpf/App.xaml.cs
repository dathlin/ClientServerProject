using ClientsLibrary;
using HslCommunication.LogNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace 软件系统客户端Wpf
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 指示了应用程序退出时的代码
        /// </summary>
        public static int QuitCode { get; set; } = 0;


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // 处理异常
            UserClient.LogNet = new LogNetDateTime(AppDomain.CurrentDomain.BaseDirectory + @"\Logs", GenerateMode.ByEveryDay);
            //捕获未处理的异常
            AppDomain.CurrentDomain.UnhandledException += ClientsLibrary.UserClient.CurrentDomain_UnhandledException;

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
                    Thread.Sleep(10);
                    Current.Shutdown();//关闭当前的应用程序
                    return;
                }
            }


            P1:
            LoginWindow lw = new LoginWindow();
            bool? result = lw.ShowDialog();
            if (result.Value == true)
            {
                MainWindow mw = new MainWindow();
                MainWindow = mw;

                mw.ShowDialog();
                if (QuitCode == 1)
                {
                    //继续显示登录窗口
                    QuitCode = 0;
                    goto P1;
                }
                else
                {
                    Current.Shutdown();//关闭当前的应用程序
                }
            }
            else
            {
                Environment.Exit(0);
            }

        }
        

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
