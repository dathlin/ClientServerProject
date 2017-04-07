using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndustryEthernet;
using System.Threading;
using CommonLibrary;
using BasicFramework;
using Newtonsoft.Json.Linq;


//============================================================================
//    模版日期  2017-02-21
//    创建人    胡少林
//    版权所有  胡少林
//    授权说明  模版仅授权个人使用，如需商用，请联系hsl200909@163.com洽谈
//    说明      JSON组件引用自james newton-king，遵循MIT授权协议
//============================================================================


//============================================================================
//
//    注意：本代码的相关操作未作密码验证，如有需要，请自行完成
//    示例：具体示例参照Form1_FormClosing(object sender, FormClosingEventArgs e)方法
//
//============================================================================


//============================================================================
//
//    本项目模版不包含  软件自动更新.exe
//    如需支持自动升级  请联系hsl200909@163.com获取
//    软件自动更新.exe  将绑定IP和端口后授权销售，30元人民币一组，永久使用
//
//============================================================================





namespace 软件系统服务端模版
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 窗口属性+窗口方法

        /// <summary>
        /// 指示窗口是否处于显示中
        /// </summary>
        private bool IsWindowShow { get; set; } = false;
        /// <summary>
        /// 指示系统是否启动
        /// </summary>
        private bool IsSystemStart { get; set; } = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            //保存路径初始化
            UserServer.ServerSettings.FileSavePath = Application.StartupPath + @"\settings.txt";
            //加载参数
            UserServer.ServerSettings.LoadByFile();
            toolStripStatusLabel_version.Text = UserServer.ServerSettings.SystemVersion.ToString();
            toolStripStatusLabel1.Text = $"本软件著作权归{Resource.StringResouce.SoftCopyRight}所有";
            //加载账户信息
            UserServer.ServerAccounts.FileSavePath = Application.StartupPath + @"\accounts.txt";
            UserServer.ServerAccounts.LoadByFile();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //密码验证的示例，此处关闭窗口验证
            BasicFramework.FormPasswordCheck fpc = new BasicFramework.FormPasswordCheck("123456");
            if (fpc.ShowDialog() == DialogResult.OK)
            {
                IsWindowShow = false;
                Thread.Sleep(20);
            }
            else
            {
                //取消关闭
                e.Cancel = true;
            }

            //紧急数据的保存已经放置到dispose方法中，即时发生BUG或直接关机，也能存储数据
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            IsWindowShow = true;
            //维护初始化
            MaintenanceInitialization();
            //时间引擎初始化
            TimeTickInitilization();
            Refresh();
            启动服务器ToolStripMenuItem.PerformClick();
        }

        #endregion

        #region 菜单逻辑块
        private void 启动服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsSystemStart)
            {
                Net_Simplify_Server_Initialization();
                Net_Socket_Server_Initialization();
                Net_SoftUpdate_Server_Initialization();
                Net_File_Update_Initialization();
                Simple_File_Initiaization();
                启动服务器ToolStripMenuItem.Text = "已启动";
                启动服务器ToolStripMenuItem.BackColor = Color.LimeGreen;
                IsSystemStart = true;
            }
        }

        private void 版本控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicFramework.FormVersionControl fvc = new BasicFramework.FormVersionControl(UserServer.ServerSettings);
            fvc.ShowDialog();
            fvc.Dispose();
            toolStripStatusLabel_version.Text = UserServer.ServerSettings.SystemVersion.ToString();
        }

        private void 维护切换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicFramework.FormMaintenance fm = new BasicFramework.FormMaintenance(UserServer.ServerSettings);
            fm.ShowDialog();
            fm.Dispose();
            MaintenanceInitialization();
        }


        private void 消息发送ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //测试发送字节数据
            //net_socket_server.SendAllClients(BitConverter.GetBytes(12345678));
            //将消息群发给所有的客户端，并使用消息弹窗的方式显示
            FormInputAndAction fiaa = new FormInputAndAction(m =>
            {
                net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.弹窗消息 + m); return true;
            });
            fiaa.ShowDialog();
            fiaa.Dispose();
        }

        private void 一键断开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //关闭信号发送至所有在线客户端
            net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.关闭所有客户端);
        }

        private void 关于软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicFramework.FormAbout fm = new BasicFramework.FormAbout(
                CommonLibrary.Resource.StringResouce.SoftName, UserServer.ServerSettings.SystemVersion,
                2017, CommonLibrary.Resource.StringResouce.SoftCopyRight);
            fm.ShowDialog();
            fm.Dispose();
        }
        private void 账户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //该部分比较复杂，需要对委托，匿名委托概念比较清晰
            FormAccountManage fam = new FormAccountManage(() => UserServer.ServerAccounts.GetAllAccountsJson(),
                m => { UserServer.ServerAccounts.LoadAllAccountsJson(m); return true; });
            fam.ShowDialog();
            fam.Dispose();
        }

        private void 版本号说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicFramework.FormAboutVersion fav = new BasicFramework.FormAboutVersion(UserServer.ServerSettings.SystemVersion);
            fav.ShowDialog();
            fav.Dispose();
        }
        private void MaintenanceInitialization()
        {
            //维护状态变更
            if (UserServer.ServerSettings.Can_Account_Login)
            {
                label3.Text = "可登录";
                label3.BackColor = Color.LimeGreen;
            }
            else
            {
                label3.Text = "维护中";
                label3.BackColor = Color.Tomato;
            }
        }


        #endregion

        #region 软件更新服务引擎
        /// <summary>
        /// 支持软件自动更新的后台服务引擎
        /// </summary>
        private Net_SoftUpdate_Server net_soft_update_Server = new Net_SoftUpdate_Server();
        private void Net_SoftUpdate_Server_Initialization()
        {
            try
            {
                net_soft_update_Server.log_record.log_save_path = Application.StartupPath + @"\update_log.txt";
                //在服务器的这个路径下，放置客户端运行的所有文件，不要包含settings文件，不要从此处运行
                //只放置exe和dll组件，必须放置：软件自动更新.exe
                net_soft_update_Server.KeyToken = CommonHeadCode.KeyToken;
                net_soft_update_Server.FileUpdatePath = @"C:\ClientFiles";
                net_soft_update_Server.AutoUpdateEngineStart(CommonLibrary.CommonLibrary.Port_Update_Net);
            }
            catch (Exception ex)
            {
                BasicFramework.SoftBasic.ShowExceptionMessage(ex);
            }
        }

        #endregion

        #region 软件异地更新文件传送引擎
        /// <summary>
        /// 用于局域网异地更新服务器的客户端程序的引擎，仅限客户端
        /// </summary>
        private Net_File_Server net_file_update = new Net_File_Server();
        /// <summary>
        /// 软件异地更新的初始化，如不需要可以不启动，该功能需要稳定性测试
        /// </summary>
        private void Net_File_Update_Initialization()
        {
            try
            {
                net_file_update.FilesPath = @"C:\ClientFiles";//服务器客户端需要更新的路径，与上述一致
                net_file_update.log_record.log_save_path = Application.StartupPath + @"\update_file_log.txt";
                net_file_update.KeyToken = CommonHeadCode.KeyToken;
                net_file_update.Server_Start(CommonLibrary.CommonLibrary.Port_Update_Remote);
            }
            catch (Exception ex)
            {
                BasicFramework.SoftBasic.ShowExceptionMessage(ex);
            }
        }
        #endregion

        #region 同步数据传送引擎
        /// <summary>
        /// 用户同步数据传送的引擎
        /// </summary>
        private Net_Simplify_Server net_simplify_server = new Net_Simplify_Server();
        /// <summary>
        /// 同步传送数据的初始化
        /// </summary>
        private void Net_Simplify_Server_Initialization()
        {
            try
            {
                net_simplify_server.KeyToken = CommonHeadCode.KeyToken;//设置身份令牌
                net_simplify_server.log_record.log_save_path = Application.StartupPath + @"\simplify_log.txt";
                net_simplify_server.ReceiveStringEvent += Net_simplify_server_ReceiveStringEvent;
                net_simplify_server.ReceivedBytesEvent += Net_simplify_server_ReceivedBytesEvent;
                net_simplify_server.Server_Start(CommonLibrary.CommonLibrary.Port_Second_Net);
            }
            catch (Exception ex)
            {
                BasicFramework.SoftBasic.ShowExceptionMessage(ex);
            }
        }
        /// <summary>
        /// 接收来自客户端的字节数据
        /// </summary>
        /// <param name="object1">客户端的地址</param>
        /// <param name="object2">字节数据，根据实际情况选择是否使用</param>
        private void Net_simplify_server_ReceivedBytesEvent(HuStateOne object1, byte[] object2)
        {
            net_simplify_server.SendMessage(object1, object2);
        }
        /// <summary>
        /// 接收到来自客户端的数据，此处需要放置维护验证，更新验证等等操作
        /// </summary>
        /// <param name="object1">客户端的地址</param>
        /// <param name="object2">消息数据，应该使用指令头+数据组成</param>
        private void Net_simplify_server_ReceiveStringEvent(HuStateOne object1, string object2)
        {
            //如果此处充斥大量if语句，影响观感，则考虑进行指令头分类操作
            //必须返回结果，调用SendMessage(object1,[实际数据]);
            string head_code = object2.Substring(0, 4);
            if (head_code == CommonHeadCode.SimplifyHeadCode.维护检查)
            {
                net_simplify_server.SendMessage(object1, UserServer.ServerSettings.Can_Account_Login ? "1" : "0" +
                    UserServer.ServerSettings.Account_Forbidden_Reason);
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.更新检查)
            {
                net_simplify_server.SendMessage(object1, UserServer.ServerSettings.SystemVersion.ToString());
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.参数下载)
            {
                Newtonsoft.Json.Linq.JObject json = new Newtonsoft.Json.Linq.JObject();
                json.Add(nameof(UserServer.ServerSettings.Announcement), new Newtonsoft.Json.Linq.JValue(UserServer.ServerSettings.Announcement));
                net_simplify_server.SendMessage(object1, json.ToString());
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.账户检查)
            {
                //此处使用的是组件自带的验证的方式，如果使用SQL数据库，另行验证
                Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(object2.Substring(4));
                //提取账户，密码
                string name = SoftBasic.GetValueFromJsonObject(json, UserAccount.UserNameText, "");
                string password = SoftBasic.GetValueFromJsonObject(json, UserAccount.PasswordText, "");
                net_simplify_server.SendMessage(object1, UserServer.ServerAccounts.CheckAccountJson(
                    name, password, ((System.Net.IPEndPoint)(object1._WorkSocket.RemoteEndPoint)).Address.ToString()));
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.更新公告)
            {
                UserServer.ServerSettings.Announcement = object2.Substring(4);
                //通知所有客户端更新公告
                net_socket_server.SendAllClients(object2);
                net_simplify_server.SendMessage(object1, "成功");
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.获取账户信息)
            {
                //返回服务器的账户信息
                net_simplify_server.SendMessage(object1, UserServer.ServerAccounts.GetAllAccountsJson());
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.更细账户信息)
            {
                //更新服务器的账户信息
                UserServer.ServerAccounts.LoadAllAccountsJson(object2.Substring(4));
                net_simplify_server.SendMessage(object1, "成功");
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.密码修改)
            {
                //更新服务器的账户密码
                //此处使用的是组件自带的验证的方式，如果使用SQL数据库，另行验证
                Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(object2.Substring(4));
                //提取账户，密码
                string name = SoftBasic.GetValueFromJsonObject(json, UserAccount.UserNameText, "");
                string password = SoftBasic.GetValueFromJsonObject(json, UserAccount.PasswordText, "");
                UserServer.ServerAccounts.UpdatePassword(name, password);
                net_simplify_server.SendMessage(object1, "成功");
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.更新版本号)
            {
                try
                {
                    UserServer.ServerSettings.SystemVersion = new SystemVersion(object2.Substring(4));
                    UserServer.ServerSettings.SaveToFile();
                    toolStripStatusLabel_version.Text = UserServer.ServerSettings.SystemVersion.ToString();
                    net_simplify_server.SendMessage(object1, "1");
                }
                catch
                {
                    net_simplify_server.SendMessage(object1, "0");
                }
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.网络日志查看)
            {
                net_simplify_server.SendMessage(object1, net_socket_server.LogReacord.GetLogText());
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.网络日志清空)
            {
                net_socket_server.LogReacord.ClearLogText();
                net_simplify_server.SendMessage(object1, "成功");
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.同步日志查看)
            {
                net_simplify_server.SendMessage(object1, net_simplify_server.log_record.GetLogText());
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.同步日志清空)
            {
                net_simplify_server.log_record.ClearLogText();
                net_simplify_server.SendMessage(object1, "成功");
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.更新日志查看)
            {
                net_simplify_server.SendMessage(object1, net_soft_update_Server.log_record.GetLogText());
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.更新日志清空)
            {
                net_soft_update_Server.log_record.ClearLogText();
                net_simplify_server.SendMessage(object1, "成功");
            }
            else if (head_code == CommonHeadCode.SimplifyHeadCode.注册账号)
            {
                bool result = UserServer.ServerAccounts.AddNewAccount(object2.Substring(4));
                net_simplify_server.SendMessage(object1, result ? "1" : "0");
            }
            else if(head_code == CommonHeadCode.SimplifyHeadCode.请求文件列表)
            {
                net_simplify_server.SendMessage(object1, net_simple_file_server.ToJsonString());
            }
            else
            {
                net_simplify_server.SendMessage(object1, object2);
            }
        }
        #endregion

        #region 异步数据传送引擎
        //异步客户端管理引擎
        private Net_Socket_Server net_socket_server = new Net_Socket_Server();
        /// <summary>
        /// 异步传送数据的初始化
        /// </summary>
        private void Net_Socket_Server_Initialization()
        {
            try
            {
                net_socket_server.KeyToken = CommonHeadCode.KeyToken;//设置身份令牌
                net_socket_server.LogReacord.log_save_path = Application.StartupPath + @"\net_log.txt";
                net_socket_server.FormatClientOnline = "#IP:{0} Name:{1}";//必须为#开头，具体格式可由自身需求确定
                net_socket_server.IsSaveLogClientLineChange = true;
                net_socket_server.ClientOnline += Net_socket_server_ClientOnline;
                net_socket_server.ClientOffline += Net_socket_server_ClientOffline;
                net_socket_server.MessageAlerts += Net_socket_server_MessageAlerts;
                net_socket_server.AcceptByte += Net_socket_server_AcceptByte;
                net_socket_server.AcceptString += Net_socket_server_AcceptString;
                net_socket_server.SocketStart(CommonLibrary.CommonLibrary.Port_Main_Net);
            }
            catch (Exception ex)
            {
                BasicFramework.SoftBasic.ShowExceptionMessage(ex);
            }
        }

        private void Net_socket_server_AcceptString(HuTcpState object1, string object2)
        {
            //如果此处充斥大量if语句，影响观感，则考虑进行指令头分类操作，客户端异步发送的字符串都会到此处处理
            string head_code = object2.Substring(0, 4);
            byte[] result = Convert.FromBase64String(object2.Substring(4));

        }

        private void Net_socket_server_AcceptByte(HuTcpState object1, byte[] object2)
        {
            //如果此处充斥大量if语句，影响观感，则考虑进行指令头分类操作，客户端异步发送的字节数组都会到此处处理
        }

        private void Net_socket_server_MessageAlerts(string object1)
        {
            //同上的方法，此处处理数据时处于后台线程
            if (IsWindowShow && IsHandleCreated)
            {
                BeginInvoke(new Action(() =>
                {
                    textBox1.AppendText(object1 + Environment.NewLine);
                }));
            }
        }

        private void Net_socket_server_ClientOffline(HuTcpState object1, string object2)
        {
            Net_socket_clients_change(DateTime.Now.ToString("MM-dd HH:mm:ss ") + object1._IpEnd_Point.Address.ToString() + "：" +
                        object1._Login_Alias + " " + object2);
        }

        private void Net_socket_server_ClientOnline(HuTcpState object1)
        {
            //上线后回发一条数据初始化信息
            JObject json = new JObject
            {
                { "Time", new JValue(DateTime.Now) },
                { "FileCount", new JValue(net_simple_file_server.File_Count()) }
            };
            net_socket_server.Send(object1, CommonHeadCode.MultiNetHeadCode.初始化数据 + json.ToString());
            //触发上下线功能
            Net_socket_clients_change(DateTime.Now.ToString("MM-dd HH:mm:ss ") + object1._IpEnd_Point.Address.ToString() + "：" +
                        object1._Login_Alias + " 上线");
        }


        private void Net_socket_clients_change(string str)
        {
            if (IsWindowShow && IsHandleCreated)
            {
                BeginInvoke(new Action(() =>
                {
                    textBox1.AppendText(str + Environment.NewLine);
                    listBox1.DataSource = net_socket_server.AllClients.Split('#');
                    label4.Text = net_socket_server.ClientCount.ToString();
                    //此处决定要不要将在线客户端的数据发送所有客户端
                    net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.所有客户端在线信息 + net_socket_server.AllClients);
                }));
            }
        }

        #endregion

        #region 后台计数线程
        //=============================================================================
        //后台计数的线程

        public void TimeTickInitilization()
        {
            toolStripStatusLabel_time.Alignment = ToolStripItemAlignment.Right;
            statusStrip1.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow;
            toolStripStatusLabel_time.ForeColor = Color.Purple;//紫色

            Thread thread = new Thread(new ThreadStart(ThreadTimeTick));
            thread.IsBackground = true;
            thread.Start();
        }

        public void ThreadTimeTick()
        {
            Thread.Sleep(300);//加一个微小的延时
            int second = DateTime.Now.Second - 1;
            Action DTimeShow = delegate
              {
                  toolStripStatusLabel_time.Text = DateTime.Now.ToString();
              };

            while (IsWindowShow)
            {
                while (DateTime.Now.Second == second)
                {
                    Thread.Sleep(20);
                }
                second = DateTime.Now.Second;
                if (IsWindowShow && IsHandleCreated) Invoke(DTimeShow);
                //每隔一分钟将时间发送给所有客户端，格式为标准时间
                if (second == 0)
                {
                    net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.时间推送 +
                        DateTime.Now.ToString("O"));
                }
            }
        }


        #endregion

        #region 共享文件下载块

        private SimpleShareFileServer net_simple_file_server { get; set; } = null;

        private void Simple_File_Initiaization()
        {
            try
            {
                net_simple_file_server = new SimpleShareFileServer(
                    list => JArray.FromObject(list).ToString(),
                    str => JArray.Parse(str).ToObject<List<File_Save>>());
                //文件信息存储路径
                net_simple_file_server.FileSavePath = Application.StartupPath + @"\files.txt";
                net_simple_file_server.ReadFromFile();
                //文件存储路径
                net_simple_file_server.File_save_path = Application.StartupPath + @"\Files";
                net_simple_file_server.FileChange += Net_simple_file_server_FileChange;
                net_simple_file_server.FileEngineStart(CommonLibrary.CommonLibrary.Port_Share_File);
            }
            catch(Exception ex)
            {
                SoftBasic.ShowExceptionMessage(ex);
            }
        }

        private void Net_simple_file_server_FileChange()
        {
            //将文件数据发送给客户端
            net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.文件数量 + net_simple_file_server.File_Count());
        }


        #endregion


        /// <summary>
        /// 还未有其他什么用途
        /// </summary>
        private Log_Record log = new Log_Record();
    }
}
