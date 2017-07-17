using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;
using HslCommunication;
using CommonLibrary;
using HslCommunication.BasicFramework;
using Newtonsoft.Json.Linq;
using ClientsLibrary;
using MaterialDesignThemes.Wpf;

namespace 软件系统客户端Wpf
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            UserClient.JsonSettings.FileSavePath = AppDomain.CurrentDomain.BaseDirectory + @"\JsonSettings.txt";
            UserClient.JsonSettings.LoadByFile();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //启动线程登录
            //验证输入
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                SetInformationString("请输入用户名");
                NameTextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                SetInformationString("请输入密码");
                PasswordBox.Focus();
                return;
            }

            SetInformationString("正在验证维护状态...");
            UISettings(false);

            UserName = NameTextBox.Text;
            UserPassword = PasswordBox.Password;
            IsChecked = (bool)Remember.IsChecked;

            ThreadAccountLogin = new Thread(ThreadCheckAccount);
            ThreadAccountLogin.IsBackground = true;
            ThreadAccountLogin.Start();
        }

        #region 账户验证的逻辑块



        private string UserName = string.Empty;
        private string UserPassword = string.Empty;
        private bool IsChecked = false;

        /// <summary>
        /// 用于验证的后台线程
        /// </summary>
        private Thread ThreadAccountLogin = null;
        /// <summary>
        /// 用户账户验证的后台端
        /// </summary>
        private void ThreadCheckAccount()
        {
            //定义委托
            Action<string> message_show = delegate (string message)
            {
                SetInformationString(message);
            };
            Action start_update = delegate
            {
                //需要该exe支持，否则将无法是实现自动版本控制
                string update_file_name = AppDomain.CurrentDomain.BaseDirectory + @"\软件自动更新.exe";
                try
                {
                    System.Diagnostics.Process.Start(update_file_name);
                    Environment.Exit(0);//退出系统
                }
                catch
                {
                    MessageBox.Show("更新程序启动失败，请检查文件是否丢失，联系管理员获取。");
                }
            };
            Action thread_finish = delegate
            {
                UISettings(true);
            };

            //延时
            Thread.Sleep(400);

            //请求指令头数据，该数据需要更具实际情况更改
            OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.维护检查);
            if (result.IsSuccess)
            {
                byte[] temp = Encoding.Unicode.GetBytes(result.Content);
                //例如返回结果为1说明允许登录，0则说明服务器处于维护中，并将信息显示
                if (result.Content != "1")
                {
                    Dispatcher.Invoke(message_show, result.Content.Substring(1));
                    Dispatcher.Invoke(thread_finish);
                    return;
                }
            }
            else
            {
                //访问失败
                Dispatcher.Invoke(message_show, result.Message);
                Dispatcher.Invoke(thread_finish);
                return;
            }



            //检查账户
            Dispatcher.Invoke(message_show, "正在检查账户...");

            //延时
            Thread.Sleep(400);

            //===================================================================================
            //   根据实际情况校验，选择数据库校验或是将用户名密码发至服务器校验
            //   以下展示了服务器校验的方法，如您需要数据库校验，请删除下面并改成SQL访问验证的方式

            //包装数据
            JObject json = new JObject
            {
                { UserAccount.UserNameText, new JValue(UserName) },
                { UserAccount.PasswordText, new JValue(UserPassword) }
            };
            result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.账户检查, json.ToString());
            if (result.IsSuccess)
            {
                //服务器应该返回账户的信息
                UserAccount account = JObject.Parse(result.Content).ToObject<UserAccount>();
                if (!account.LoginEnable)
                {
                    //不允许登录
                    Dispatcher.Invoke(message_show, account.ForbidMessage);
                    Dispatcher.Invoke(thread_finish);
                    return;
                }
                UserClient.UserAccount = account;
            }
            else
            {
                //访问失败
                Dispatcher.Invoke(message_show, result.Message);
                Dispatcher.Invoke(thread_finish);
                return;
            }

            //登录成功，进行保存用户名称和密码
            UserClient.JsonSettings.LoginName = UserName;
            UserClient.JsonSettings.Password = IsChecked ? UserPassword : "";
            UserClient.JsonSettings.LoginTime = DateTime.Now;
            UserClient.JsonSettings.SaveToFile();


            //版本验证
            Dispatcher.Invoke(message_show, "正在验证版本...");

            //延时
            Thread.Sleep(400);

            result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.更新检查);
            if (result.IsSuccess)
            {
                //服务器应该返回服务器的版本号
                SystemVersion sv = new SystemVersion(result.Content);
                //系统账户跳过低版本检测
                if (UserClient.UserAccount.UserName != "admin")
                {
                    if (UserClient.CurrentVersion != sv)
                    {
                        //保存新版本信息
                        UserClient.JsonSettings.IsNewVersionRunning = true;
                        UserClient.JsonSettings.SaveToFile();
                        //和当前系统版本号不一致，启动更新
                        Dispatcher.Invoke(start_update);
                        return;
                    }
                }
                else
                {
                    if (UserClient.CurrentVersion < sv)
                    {
                        //保存新版本信息
                        UserClient.JsonSettings.IsNewVersionRunning = true;
                        UserClient.JsonSettings.SaveToFile();
                        //和当前系统版本号不一致，启动更新
                        Dispatcher.Invoke(start_update);
                        return;
                    }
                }
            }
            else
            {
                //访问失败
                Dispatcher.Invoke(message_show, result.Message);
                Dispatcher.Invoke(thread_finish);
                return;
            }


            //================================================================================
            //验证结束后，根据需要是否下载服务器的数据，或是等到进入主窗口下载也可以
            //如果有参数决定主窗口的显示方式，那么必要在下面向服务器请求数据
            //以下展示了初始化参数的功能
             Dispatcher.Invoke(message_show, "正在下载参数...");

            //延时
            Thread.Sleep(400);


            result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.参数下载);
            if (result.IsSuccess)
            {
                //服务器返回初始化的数据，此处进行数据的提取，有可能包含了多个数据
                json = Newtonsoft.Json.Linq.JObject.Parse(result.Content);
                //例如公告数据
                UserClient.Announcement = SoftBasic.GetValueFromJsonObject(json, nameof(UserClient.Announcement), "");

            }
            else
            {
                //访问失败
                Dispatcher.Invoke(message_show, result.Message);
                Dispatcher.Invoke(thread_finish);
                return;
            }

            //启动主窗口
            Dispatcher.Invoke(new Action(() =>
            {
                DialogResult = true;
                return;
            }));
        }


        #endregion

        private void UISettings(bool enable)
        {
            NameTextBox.IsEnabled = enable;
            PasswordBox.IsEnabled = enable;
            Remember.IsEnabled = enable;
            LoginButton.IsEnabled = enable;
        }

        private void SetInformationString(string str)
        {
            if (WindowToolTip.Opacity == 1)
            {
                DoubleAnimation hidden = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(100));
                hidden.Completed += delegate
                {
                    DoubleAnimation show = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));
                    WindowToolTip.Text = str;
                    WindowToolTip.BeginAnimation(OpacityProperty, show);
                };
                WindowToolTip.BeginAnimation(OpacityProperty, hidden);
            }
            else
            {
                DoubleAnimation show = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));
                WindowToolTip.Text = str;
                WindowToolTip.BeginAnimation(OpacityProperty, show);
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            LoginButton.Focus();
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            LoginButton.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowToolTip.Opacity = 0;
            TextBlockSoftName.Text = CommonLibrary.Resource.StringResouce.SoftName;
            TextBlockSoftVersion.Text = UserClient.CurrentVersion.ToString();
            TextBlockSoftCopyright.Text = $"本软件著作权归{CommonLibrary.Resource.StringResouce.SoftCopyRight}所有";

            

            if ((DateTime.Now - UserClient.JsonSettings.LoginTime).TotalDays < 7)
            {
                //加载数据
                NameTextBox.Text = UserClient.JsonSettings.LoginName ?? "";
                PasswordBox.Password = UserClient.JsonSettings.Password ?? "";
                Remember.IsChecked = UserClient.JsonSettings.Password != "";
            }
            //初始化输入焦点
            if (UserClient.JsonSettings.Password != "") LoginButton.Focus();
            else if (UserClient.JsonSettings.LoginName != "") PasswordBox.Focus();
            else NameTextBox.Focus();
        }

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)  PasswordBox.Focus();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(null, new RoutedEventArgs());
        }
    }
}
