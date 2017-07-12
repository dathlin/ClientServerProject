using CommonLibrary;
using HslCommunication.BasicFramework;
using HslCommunication.Enthernet;
using Newtonsoft.Json.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 软件系统客户端Wpf
{






    /***************************************************************************************
     * 
     *    模版日期    2017-07-11
     *    创建人      胡少林
     *    版权所有    胡少林
     *    授权说明    模版仅授权个人使用，如需商用，请联系hsl200909@163.com洽谈
     *    说明一      JSON组件引用自james newton-king，遵循MIT授权协议
     *    说明二      主题及各种主件来自:https://github.com/ButchersBoy/MaterialDesignInXamlToolkit
     * 
     ****************************************************************************************/

    /***************************************************************************************
     * 
     *    版本说明    最新版以github为准，由于提交更改比较频繁，需要经常查看官网地址:https://github.com/dathlin/C-S-
     *    注意        本代码的相关操作未作密码验证，如有需要，请自行完成
     *    如果        遇到启动调试就退出了，请注释掉App.xaml.cs文件中的指允许启动一个实例的代码
     *    
     **************************************************************************************************/





    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }





        #region 异步网络块

        private Net_Socket_Client net_socket_client = new Net_Socket_Client();

        private void Net_Socket_Client_Initialization()
        {
            try
            {
                net_socket_client.KeyToken = CommonHeadCode.KeyToken;//新增的身份令牌
                net_socket_client.EndPointServer = new System.Net.IPEndPoint(
                    System.Net.IPAddress.Parse(UserClient.ServerIp),
                    CommonLibrary.CommonLibrary.Port_Main_Net);
                net_socket_client.ClientAlias = $"{UserClient.UserAccount.UserName} ({UserClient.UserAccount.Factory})";//标记客户端在线的名称
                net_socket_client.ClientStart();
            }
            catch (Exception ex)
            {
                SoftBasic.ShowExceptionMessage(ex);
            }
        }
        /// <summary>
        /// 接收到服务器的字节数据的回调方法
        /// </summary>
        /// <param name="state">网络连接对象</param>
        /// <param name="customer">用户自定义的指令头，用来区分数据用途</param>
        /// <param name="data">数据</param>
        private void Net_socket_client_AcceptString(AsyncStateOne state, int customer, string data)
        {
            if (customer == CommonHeadCode.MultiNetHeadCode.弹窗新消息)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    FormPopup fpp = new FormPopup(data, System.Drawing.Color.DodgerBlue, 10000);
                    fpp.Show();
                }));
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.总在线信息)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //listBox1.DataSource = data.Split('#');
                }));
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.关闭客户端)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    Close();
                }));
            }
            else if (customer == CommonHeadCode.SimplifyHeadCode.更新公告)
            {
                //此处应用到了同步类的指令头
                Dispatcher.Invoke(new Action(() =>
                {
                    UserClient.Announcement = data;
                    //label_Announcement.Text = data;
                    FormPopup fpp = new FormPopup(data, System.Drawing.Color.DodgerBlue, 10000);
                    fpp.Show();
                }));
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.初始化数据)
            {
                //收到服务器的数据
                JObject json = JObject.Parse(data);
                UserClient.DateTimeServer = json["Time"].ToObject<DateTime>();
                List<string> chats = JArray.Parse(json["chats"].ToString()).ToObject<List<string>>();
                StringBuilder sb = new StringBuilder();
                chats.ForEach(m => { sb.Append(m + Environment.NewLine); });
                Dispatcher.Invoke(new Action(() =>
                {
                    //toolStripStatusLabel_time.Text = UserClient.DateTimeServer.ToString("yyyy-MM-dd HH:mm");
                    //label_file_count.Text = json["FileCount"].ToObject<int>().ToString();
                    //UIControls_Chat.AddChatsHistory(sb.ToString());
                }));
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.文件总数量)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //label_file_count.Text = data;
                }));
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.留言版消息)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //UIControls_Chat?.DealwithReceive(data);
                }));
            }
        }

        private void Net_socket_client_AcceptByte(AsyncStateOne object1, int customer, byte[] object2)
        {
            //接收到服务器发来的字节数据
            Dispatcher.Invoke(new Action(() =>
            {
                MessageBox.Show(customer.ToString());
            }));
        }

        private void Net_socket_client_LoginSuccess()
        {
            //登录成功，或重新登录成功的事件，有些数据的初始化可以放在此处
            Dispatcher.Invoke(new Action(() =>
            {
                //toolStripStatusLabel_status.Text = "客户端启动成功";
            }));
        }

        private void Net_socket_client_LoginFailed(int object1)
        {
            //登录失败的情况，如果连续三次连接失败，请考虑退出系统
            if (object1 > 3)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    Close();
                }));
            }
        }

        private void Net_socket_client_MessageAlerts(string object1)
        {
            //信息提示
            Dispatcher.Invoke(new Action(() =>
            {
                //toolStripStatusLabel_status.Text = object1;
            }));
        }


        #endregion









    }
}
