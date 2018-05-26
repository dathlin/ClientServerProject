using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using CommonLibrary;
using System.Threading;
using HslCommunication;
using HslCommunication.Enthernet;
using HslCommunication.BasicFramework;
using HslCommunication.Core.Net;
using ClientsLibrary;
using HslCommunication.LogNet;


/***************************************************************************************
 * 
 *    模版日期    2017-10-06
 *    创建人      Richard.Hu
 *    版权所有    Richard.Hu
 *    授权说明    模版仅授权个人使用，如需商用，请联系hsl200909@163.com洽谈
 *    说明一      JSON组件引用自james newton-king，遵循MIT授权协议
 *    说明二      文件的图标来源于http://fileicons.chromefans.org/,感谢作者的无私分享
 * 
 ****************************************************************************************/

/***************************************************************************************
 * 
 *    版本说明    最新版以github为准，由于提交更改比较频繁，需要经常查看官网地址:https://github.com/dathlin/ClientServerProject 
 *    注意        本代码的相关操作未作密码验证，如有需要，请自行完成
 *    示例        密码验证具体示例参照Form1_FormClosing(object sender, FormClosingEventArgs e)方法
 *    如果        遇到启动调试就退出了，请注释掉Program.cs文件中的指允许启动一个实例的代码
 * 
 ****************************************************************************************/

/*****************************************************************************************
 * 
 *    权限说明    在进行特定权限操作的业务逻辑时，应该提炼成一个角色，这样可以动态绑定带有这些功能的账户
 *    示例        if (UserClient.CheckUserAccountRole([审计员的GUID码])) { dosomething(); }// 获取了审计员的角色，名字此处示例
 * 
 ******************************************************************************************/





namespace 软件系统客户端模版
{
    public partial class FormMainWindow : Form
    {
        #region Constructor
        
        public FormMainWindow()
        {
            InitializeComponent();

            Icon = UserSystem.GetFormWindowIcon();

            // 初始化头像管理器
            UserClient.PortraitManager = new UserPortrait(Application.StartupPath + @"\Portrait\" + UserClient.UserAccount.UserName);
        }

        #endregion

        #region Load Show Close
        /// <summary>
        /// 指示窗口是否显示的标志
        /// </summary>
        private bool IsWindowShow { get; set; } = false;

        private void FormMainWindow_Load(object sender, EventArgs e)
        {
            // udp测试
            // SendServerUdpData(0, "载入了窗体");
            
            //窗口载入
            label_userName.Text = UserClient.UserAccount.UserName;
            label_grade.Text = AccountGrade.GetDescription(UserClient.UserAccount.Grade);
            label_factory.Text = UserClient.UserAccount.Factory;
            label_register.Text = UserClient.UserAccount.RegisterTime.ToString();
            label_last.Text = UserClient.UserAccount.LastLoginTime.ToString();
            label_times.Text = UserClient.UserAccount.LoginFrequency.ToString();
            label_address.Text = UserClient.UserAccount.LastLoginIpAddress;

            // 状态栏设置
            toolStripStatusLabel_time.Alignment = ToolStripItemAlignment.Right;
            statusStrip1.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow;
            toolStripStatusLabel1.Text = $"本软件著作权归{SoftResources.StringResouce.SoftCopyRight}所有";

            // 绑定事件，仅执行一次，不能放到show方法里
            net_socket_client.MessageAlerts += Net_socket_client_MessageAlerts;
            net_socket_client.LoginFailed += Net_socket_client_LoginFailed;
            net_socket_client.LoginSuccess += Net_socket_client_LoginSuccess;
            net_socket_client.AcceptByte += Net_socket_client_AcceptByte;
            net_socket_client.AcceptString += Net_socket_client_AcceptString;
            net_socket_client.BeforReConnected += Net_socket_client_BeforReConnected;
            
            // 显示公告
            label_Announcement.Text = UserClient.Announcement;
            // 显示版本
            toolStripStatusLabel_Version.Text = UserClient.CurrentVersion.ToString();

            // 初始化窗口
            MainRenderInitialization();

            // tooltip初始化
            ToolTipInitialization();
        }


        private void FormMainWindow_Shown(object sender, EventArgs e)
        {
            // 窗口显示
            IsWindowShow = true;
            // udp测试
            // SendServerUdpData(0, "显示了窗体");

            // 是否显示更新日志，显示前进行判断该版本是否已经显示过了
            if (UserClient.JsonSettings.IsNewVersionRunning)
            {
                UserClient.JsonSettings.IsNewVersionRunning = false;
                UserClient.JsonSettings.SaveToFile();
                更新日志ToolStripMenuItem_Click(null, new EventArgs());
            }

            // 根据权限使能菜单
            if (UserClient.UserAccount.Grade < AccountGrade.Admin)
            {
                更改公告ToolStripMenuItem.Enabled = false;
                账户管理ToolStripMenuItem.Enabled = false;
                注册账号ToolStripMenuItem.Enabled = false;
                消息发送ToolStripMenuItem.Enabled = false;
            }

            if (UserClient.UserAccount.Grade < AccountGrade.SuperAdministrator)
            {
                日志查看ToolStripMenuItem.Enabled = false;
                远程更新ToolStripMenuItem.Enabled = false;
                系统配置ToolStripMenuItem.Enabled = false;
            }
            
            // 启动网络服务
            Net_Socket_Client_Initialization();
            // 启动定时器
            TimeTickInitilization();

            
            // 显示头像
            pictureBox1.Image = UserClient.PortraitManager.GetSmallPortraitByUserName(UserClient.UserAccount.UserName);
        }
        private void FormMainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 窗口关闭
            IsWindowShow = false;
            // 通知服务器退出网络服务
            net_socket_client.ClientClose();

            // 等待一秒退出
            using (FormWaitInfomation fwm = new FormWaitInfomation("正在退出程序...", 1000))
            {
                fwm.ShowDialog();
            }

            toolTipSystem?.Dispose();
        }


        #endregion

        #region Menu Click Event

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //实例化一个密码修改的窗口，并指定了实现修改的具体方法，指定了密码长度
            using (FormPasswordModify fpm = new FormPasswordModify(UserClient.UserAccount.Password,
                p =>
                {
                    JObject json = new JObject
                    {
                        { UserAccount.UserNameText, UserClient.UserAccount.UserName },
                        { UserAccount.PasswordText, p }
                    };
                    return UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.密码修改, json.ToString()).IsSuccess;
                }, 6, 8))
            {
                fpm.ShowDialog();
            }
        }

        private void 关于本软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormAbout fa = new FormAbout(SoftResources.StringResouce.SoftName,
                UserClient.CurrentVersion, 2017, SoftResources.StringResouce.SoftCopyRight))
            {
                fa.ShowDialog();
            }
        }

        private void 更新日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 更新情况复位
            if (UserClient.JsonSettings.IsNewVersionRunning)
            {
                UserClient.JsonSettings.IsNewVersionRunning = false;
                UserClient.JsonSettings.SaveToFile();
            }
            using (FormUpdateLog ful = new FormUpdateLog(UserClient.HistoryVersions))
            {
                ful.ShowDialog();
            }
        }

        private void 版本号说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormAboutVersion fav = new FormAboutVersion(UserClient.CurrentVersion))
            {
                fav.ShowDialog();
            }
        }

        private void 更改公告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormInputAndAction fiaa = new FormInputAndAction(str => UserClient.Net_simplify_client.ReadFromServer(
                 CommonHeadCode.SimplifyHeadCode.更新公告, str).IsSuccess, UserClient.Announcement, "请输入公告内容"))
            {
                fiaa.ShowDialog();
            }
        }

        private void 日志查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormLogView flg = new FormLogView())
            {
                flg.ShowDialog();
            }
        }

        private void 注册账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormRegisterAccount fra = new FormRegisterAccount(UserClient.SystemFactories.ToArray()))
            {
                fra.ShowDialog();
            }
        }

        private void 账户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAccountManage fam = new FormAccountManage(() =>
            {
                OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.获取账户);
                if (result.IsSuccess) return result.Content;
                else return result.ToMessageShowString();
            }, m => UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.更细账户, m).IsSuccess);
            fam.ShowDialog();
            fam.Dispose();
        }

        private void 远程更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserClient.UserAccount.UserName == "admin")
            {
                using (FormUpdateRemote fur = new FormUpdateRemote())
                {
                    fur.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("权限不足！");
            }
        }

        private void linkLabel_logout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.QuitCode = 1;
            Close();
        }

        private void 留言板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetShowRenderControl(UIControls_Chat);
            UIControls_Chat?.InputFocus();
            UIControls_Chat?.ScrollToDown();
        }

        private void 意见反馈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormInputAndAction fiaa = new FormInputAndAction(str => UserClient.Net_simplify_client.ReadFromServer(
                 CommonHeadCode.SimplifyHeadCode.意见反馈, UserClient.UserAccount.UserName + ":" + str).IsSuccess, "", "请输入意见反馈："))
            {
                fiaa.ShowDialog();
            }
        }

        private void 消息发送ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormInputAndAction fiaa = new FormInputAndAction(str => UserClient.Net_simplify_client.ReadFromServer(
                 CommonHeadCode.SimplifyHeadCode.群发消息, UserClient.UserAccount.UserName + ":" + str).IsSuccess, "", "请输入群发的消息："))
            {
                fiaa.ShowDialog();
            }
        }
        
        

        private void 我的信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormAccountDetails form = new FormAccountDetails())
            {
                form.ShowDialog();
            }
        }

        private void 系统配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (FormInputAndAction fiaa = new FormInputAndAction(
            //    str => UserClient.Net_simplify_client.ReadFromServer(
            //        CommonHeadCode.SimplifyHeadCode.上传分厂, str).IsSuccess, 
            //    JArray.FromObject(UserClient.SystemFactories).ToString(), 
            //    "请按照JSON格式更新分厂信息，然后提交："))
            //{
            //    fiaa.ShowDialog();
            //}
            using (FormConfiguration fc = new FormConfiguration())
            {
                fc.ShowDialog();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //点击了头像，请求下载高清版本头像
            using (FormMatterRemind fmr = new FormMatterRemind("正在下载图片", UserClient.PortraitManager.ThreadPoolDownloadSizeLarge))
            {
                fmr.ShowDialog();
            }
        }

        #endregion

        #region 异步网络块

        private NetComplexClient net_socket_client = new NetComplexClient();

        private void Net_Socket_Client_Initialization()
        {
            try
            {
                net_socket_client.Token = UserSystem.KeyToken; // 新增的身份令牌
                net_socket_client.LogNet = UserClient.LogNet;
                net_socket_client.EndPointServer = new System.Net.IPEndPoint(
                    System.Net.IPAddress.Parse(UserClient.ServerIp),
                    UserSystem.Port_Main_Net);
                net_socket_client.ClientAlias = UserClient.UserAccount.UserName; // 传入账户名
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
        /// <param name="session">网络连接对象</param>
        /// <param name="customer">用户自定义的指令头，用来区分数据用途</param>
        /// <param name="data">数据</param>
        private void Net_socket_client_AcceptString(AppSession session, NetHandle customer, string data)
        {
            if (customer == CommonHeadCode.MultiNetHeadCode.弹窗新消息)
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    FormPopup fpp = new FormPopup(data, Color.DodgerBlue, 10000);
                    fpp.Show();
                }));
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.总在线信息)
            {
                //if (IsHandleCreated) Invoke(new Action(() =>
                //{
                //    // listBox1.DataSource = data.Split('#');

                //    NetAccount[] accounts = JArray.Parse(data).ToObject<NetAccount[]>();

                //    netClientOnline1.SetOnlineRender(accounts);
                //}));
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.关闭客户端)
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    Close();
                }));
            }
            else if (customer == CommonHeadCode.SimplifyHeadCode.更新公告)
            {
                //此处应用到了同步类的指令头
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    UserClient.Announcement = data;
                    label_Announcement.Text = data;
                    FormPopup fpp = new FormPopup(data, Color.DodgerBlue, 10000);
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


                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    toolStripStatusLabel_time.Text = UserClient.DateTimeServer.ToString("yyyy-MM-dd HH:mm:ss");
                    label_file_count.Text = json["FileCount"].ToObject<int>().ToString();
                    UIControls_Chat.AddChatsHistory(sb.ToString());

                    UserClient.LogNet?.WriteDebug("Online Clients : " + json["ClientsOnline"].ToString());
                    NetAccount[] accounts = JArray.Parse(json["ClientsOnline"].ToString()).ToObject<NetAccount[]>();
                    netClientOnline1.SetOnlineRender(accounts);
                }));
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.文件总数量)
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    label_file_count.Text = data;
                }));
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.留言版消息)
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    UIControls_Chat?.DealwithReceive(data);
                }));
            }
            else if(customer == CommonHeadCode.MultiNetHeadCode.新用户上线)
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    UserClient.LogNet?.WriteDebug("Online Event : " + data);
                    netClientOnline1.ClientOnline(JObject.Parse(data).ToObject<NetAccount>());
                }));
            }
            else if(customer == CommonHeadCode.MultiNetHeadCode.用户下线)
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    netClientOnline1.ClinetOffline(data);
                }));
            }
            else if(customer == CommonHeadCode.MultiNetHeadCode.新头像更新)
            {
                UserClient.PortraitManager.UpdateSmallPortraitByName(data);
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    if(data == UserClient.UserAccount.UserName)
                    {
                        pictureBox1.Image = UserClient.PortraitManager.GetSmallPortraitByUserName(data);
                    }
                    netClientOnline1.ClientUpdatePortrait(data);
                }));
            }
        }

        private void Net_socket_client_AcceptByte(AppSession session, NetHandle customer, byte[] object2)
        {
            // 接收到服务器发来的字节数据
            if (IsHandleCreated) Invoke(new Action(() =>
            {
                MessageBox.Show(customer.ToString());
            }));
        }

        private void Net_socket_client_LoginSuccess()
        {
            // 登录成功，或重新登录成功的事件，有些数据的初始化可以放在此处
            if (IsHandleCreated) Invoke(new Action(() =>
            {
                toolStripStatusLabel_status.Text = "客户端启动成功";
            }));
        }


        private void Net_socket_client_BeforReConnected()
        {
            // 和服务器断开后，重新连接之前操作，清理在线信息
            if (IsHandleCreated && IsWindowShow) Invoke(new Action(() =>
            {
                netClientOnline1.ClearOnlineClients();
            }));
        }


        private void Net_socket_client_LoginFailed(int object1)
        {
            // 登录失败的情况，如果连续三次连接失败，请考虑退出系统
            if (object1 > 3)
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    Close();
                }));
            }
        }

        private void Net_socket_client_MessageAlerts(string object1)
        {
            // 信息提示
            if (IsHandleCreated) Invoke(new Action(() =>
            {
                toolStripStatusLabel_status.Text = object1;
            }));
        }


        #endregion

        #region 主界面管理块

        /// <summary>
        /// 文件显示的控件
        /// </summary>
        private UIControls.GroupFilesRender UIControls_Files { get; set; }

        /// <summary>
        /// 用于聊天的控件
        /// </summary>
        private UIControls.OnlineChatRender UIControls_Chat { get; set; }

        /// <summary>
        /// 主界面
        /// </summary>
        private UIControls.RenderMain UIControls_Main { get; set; }



        /// <summary>
        /// 所有在主界面显示的控件集
        /// </summary>
        private List<UserControl> all_main_render = new List<UserControl>();
        /// <summary>
        /// 正在显示的子界面
        /// </summary>
        private UserControl CurrentRender { get; set; } = null;
        /// <summary>
        /// 主界面的初始化
        /// </summary>
        private void MainRenderInitialization()
        {
            //将所有的子集控件添加进去

            /*******************************************************************************
             * 
             *    例如此处展示了文件控件是如何添加进去的 
             *    1.先进行实例化，赋值初始参数
             *    2.添加进项目
             *    3.显示
             *
             *******************************************************************************/

            UIControls_Files = new UIControls.GroupFilesRender("ShareFiles", "", "")
            {
                Visible = false,
                Parent = panel_main,//决定了放在哪个界面显示，此处示例
                Dock = DockStyle.Fill,
            };
            all_main_render.Add(UIControls_Files);

            UIControls_Chat = new UIControls.OnlineChatRender((m) =>
            {
                net_socket_client.Send(CommonHeadCode.MultiNetHeadCode.留言版消息, m);
            })
            {
                Visible = false,
                Parent = panel_main,//决定了放在哪个界面显示，此处示例
                Dock = DockStyle.Fill,
            };
            all_main_render.Add(UIControls_Chat);

            UIControls_Main = new UIControls.RenderMain()
            {
                Visible = true,
                Parent = panel_main,//决定了放在哪个界面显示，此处示例
                Dock = DockStyle.Fill,
            };
            all_main_render.Add(UIControls_Main);

        }

        private void SetShowRenderControl(UserControl control)
        {
            if (!ReferenceEquals(CurrentRender, control))
            {
                CurrentRender = control;
                all_main_render.ForEach(c => c.Visible = false);
                control.Visible = true;
            }
        }
        private void SetShowRenderControl(Type typeControl)
        {
            UserControl control = null;
            foreach (var c in all_main_render)
            {
                if (c.GetType() == typeControl)
                {
                    control = c;
                    break;
                }
            }
            if (control != null) SetShowRenderControl(control);
        }







        #endregion

        #region Udp发送示例

        /// <summary>
        /// 调用该方法并指定参数即可，最长字符串不得超过服务器配置的长度，否则无效
        /// </summary>
        /// <param name="customer">指令头</param>
        /// <param name="data">实际数据</param>
        private void SendServerUdpData(NetHandle customer, string data)
        {
            // 测试发送udp消息
            UserClient.Net_Udp_Client.SendMessage(customer, data);
        }

        #endregion

        #region 后台计数线程

        /*********************************************************************************************
         * 
         *    说明       一个后台线程，用来执行一些周期执行的东西
         *    注意       它不仅执行每秒触发的代码，也支持每分钟，每天，每月，每年等等
         * 
         ********************************************************************************************/


        /// <summary>
        /// 初始化后台的计数线程
        /// </summary>
        public void TimeTickInitilization()
        {

            Thread thread = new Thread(new ThreadStart(ThreadTimeTick));
            thread.IsBackground = true;
            thread.Start();
        }

        public void ThreadTimeTick()
        {
            Thread.Sleep(300);//加一个微小的延时
            int second = DateTime.Now.Second - 1;
            int minute = -1;
            int hour = -1;
            int day = -1;
            Action DTimeShow = delegate
            {
                //显示服务器的时间和当前网络的延时时间，通常为毫秒
                toolStripStatusLabel_time.Text = net_socket_client.ServerTime.ToString("yyyy-MM-dd HH:mm:ss") + $"({net_socket_client.DelayTime}ms)";
            };

            while (IsWindowShow)
            {
                while (DateTime.Now.Second == second)
                {
                    Thread.Sleep(20);
                }
                second = DateTime.Now.Second;
                if (IsWindowShow && IsHandleCreated) Invoke(DTimeShow);
                // 每秒钟执行的代码
                UserClient.DateTimeServer = net_socket_client.ServerTime;

                if (second == 0)
                {
                    // 每个0秒执行的代码
                }

                if (minute != DateTime.Now.Minute)
                {
                    minute = DateTime.Now.Minute;
                    // 每分钟执行的代码
                }

                if (hour != DateTime.Now.Hour)
                {
                    hour = DateTime.Now.Hour;
                    // 每小时执行的代码
                }

                if (day != DateTime.Now.Day)
                {
                    day = DateTime.Now.Day;
                    // 每天执行的代码
                }
            }
        }





        #endregion

        #region ToolTip配置


        private ToolTip toolTipSystem = null;

        private void ToolTipInitialization()
        {
            toolTipSystem = new ToolTip();
            toolTipSystem.UseFading = true;
            toolTipSystem.SetToolTip(pictureBox_right_shrink, "隐藏右边的信息栏，显示精简界面。");
            toolTipSystem.SetToolTip(pictureBox_right_restore, "显示右边的信息栏，隐藏精简界面。");
            toolTipSystem.SetToolTip(pictureBox_right_profile, "我的账户信息");
            toolTipSystem.SetToolTip(pictureBox_right_file, "共享文件信息");
        }


        #endregion

        #region User Interface Logic
        
        private void pictureBox_right_shrink_Click(object sender, EventArgs e)
        {
            // 收缩
            panel_right.Width = 25;
            panel_right_mini.Visible = true;
        }

        private void pictureBox_right_restore_Click(object sender, EventArgs e)
        {
            // 复原
            panel_right.Width = 224;
            panel_right_mini.Visible = false;
        }

        private void pictureBox_right_profile_Click(object sender, EventArgs e)
        {
            // 点击我的信息
            我的信息ToolStripMenuItem_Click(sender, e);
        }
        private void pictureBox_right_file_Click(object sender, EventArgs e)
        {
            // 点击共享文件图标
            label_file_count_Click(sender, e);
        }

        private void label_file_count_Click(object sender, EventArgs e)
        {
            // 点击查看了共享文件
            SetShowRenderControl(UIControls_Files);
            UIControls_Files.UpdateFiles();
        }

        #endregion


    }
}
