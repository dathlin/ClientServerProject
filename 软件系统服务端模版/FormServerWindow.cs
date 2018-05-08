using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication.Enthernet;
using System.Threading;
using CommonLibrary;
using Newtonsoft.Json.Linq;
using HslCommunication.BasicFramework;
using System.Diagnostics;
using HslCommunication.LogNet;
using HslCommunication;
using HslCommunication.Core;
using 软件系统服务端模版.BasicSupport;
using HslCommunication.Core.Net;


/********************************************************************************************
 * 
 *    模版日期    2018年5月4日 22:27:58
 *    创建人      Richard.Hu
 *    版权所有    Richard.Hu
 *    授权说明    模版仅授权个人研究学习使用，如需商用，请联系hsl200909@163.com洽谈
 *    说明        JSON组件引用自james newton-king，遵循MIT授权协议
 *    网络组件    网络组件的版权由Richard.Hu所有
 *    免责声明    项目许可证为MIT，由于二次开发带来的经济损失，概不负责。
 *    
 ********************************************************************************************/


/********************************************************************************************
 * 
 *    注意：本代码的相关操作未作密码验证，如有需要，请自行完成
 *    示例：具体示例参照本页面Form1_FormClosing(object sender, FormClosingEventArgs e)方法
 *    账号：默认一个超级管理员账号：admin 密码123456 
 *
 ********************************************************************************************/


/********************************************************************************************
 * 
 *    本项目模版不包含  《软件自动更新.exe》
 *    该exe程序来自另一个开源项目HslCommunication
 *    地址为：https://github.com/dathlin/HslCommunication
 *
 ********************************************************************************************/


/********************************************************************************************
 * 
 *    关于邮件系统：如果你服务器端的程序部署在可上网的计算机上时，可以使用
 *    先进行邮件系统的初始化，指定接收邮件的地址
 *    如果需要使用，请参照下面的邮件功能块说明
 *
 ********************************************************************************************/




namespace 软件系统服务端模版
{
    public partial class FormServerWindow : Form
    {
        #region Constructor
        
        public FormServerWindow()
        {
            InitializeComponent();


            //捕获所有未处理的异常并进行预处理
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //检测日志路径是否存储
            LogSavePath = Application.StartupPath + @"\Logs";
            if (!System.IO.Directory.Exists(LogSavePath))
            {
                System.IO.Directory.CreateDirectory(LogSavePath);
            }
        }

        #endregion
        
        #region Load Show Close

        /// <summary>
        /// 指示窗口是否处于显示中
        /// </summary>
        private bool IsWindowShow { get; set; } = false;
        /// <summary>
        /// 指示系统是否启动
        /// </summary>
        private bool IsSystemStart { get; set; } = false;

        private string LogSavePath { get; set; }

        private void Form1_Load( object sender, EventArgs e )
        {
            // 初始化默认的委托对象
            ActionInitialization( );
            // 邮件系统初始化
            SoftMailInitialization( );
            // 初始化日志工具
            RuntimeLogHelper = new LogNetSingle( LogSavePath + @"\log.txt" );
            // 初始化反馈信息工具
            AdviceLogHelper = new LogNetSingle( LogSavePath + @"\advice_log.txt" );
            // 初始化客户端异常日志工具
            ClientsLogHelper = new LogNetSingle( LogSavePath + @"\clients_log.txt" );
            // 初始化并加载账户信息
            UserServer.ServerSettings.FileSavePath = Application.StartupPath + @"\settings.txt";
            UserServer.ServerSettings.LoadByFile( );
            CommonLibrary.DataBaseSupport.SqlServerSupport.ConnectionString = UserServer.ServerSettings.SqlServerStr;

            // 初始化并加载角色规则
            UserServer.ServerRoles.FileSavePath = Application.StartupPath + @"\roles.txt";
            UserServer.ServerRoles.LoadByFile( );

            // 初始化版权信息
            toolStripStatusLabel_version.Text = UserServer.ServerSettings.SystemVersion.ToString( );
            toolStripStatusLabel1.Text = $"本软件著作权归{SoftResources.StringResouce.SoftCopyRight}所有";
            label5.Text = SoftResources.StringResouce.SoftName;
            // 加载账户信息
            UserServer.ServerAccounts.FileSavePath = Application.StartupPath + @"\accounts.txt";
            UserServer.ServerAccounts.LoadByFile( );
            UserServer.ServerAccounts.ILogNet = RuntimeLogHelper;
            // 初始化聊天信息
            ChatInitialization( );
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 密码验证的示例，此处关闭窗口验证
            using (FormPasswordCheck fpc = new FormPasswordCheck("123456"))
            {
                if (fpc.ShowDialog() == DialogResult.OK)
                {
                    IsWindowShow = false;
                    Thread.Sleep(20);
                    // 关闭网络引擎
                    net_socket_server.ServerClose();
                    net_simplify_server.ServerClose();
                    net_udp_server.ServerClose();
                }
                else
                {
                    // 取消关闭
                    e.Cancel = true;
                }
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

        #region UI Message Add

        /// <summary>
        /// 初始化委托
        /// </summary>
        private void ActionInitialization()
        {
            ActionUserInterfaceMessageRender = new Action<string>(m =>
            {
                UserInterfaceMessageRender(m);
            });
        }

        private Action<string> ActionUserInterfaceMessageRender = null;
        /// <summary>
        /// 往界面添加消息的方法，此方法是线程安全的，无论处于哪个线程都可以调用该方法
        /// </summary>
        /// <param name="msg">新增的数据内容</param>
        private void UserInterfaceMessageRender(string msg)
        {
            if (IsWindowShow)
            {
                if (textBox1.InvokeRequired)
                {
                    textBox1.BeginInvoke(ActionUserInterfaceMessageRender, msg);
                }
                else
                {
                    textBox1.AppendText(msg + Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// 一个处理服务器未处理异常的方法，对该方法进行记录，方便以后的分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                RuntimeLogHelper.WriteException("UnhandledException:", ex);
                // 发送到自己的EMAIL
                SendUserMail(ex);
            }
        }

        #endregion

        #region Menu Click Event

        private void 启动服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Start Server
            if (!IsSystemStart)
            {
                Net_Simplify_Server_Initialization();                              // 同步网络初始化
                Net_Socket_Server_Initialization();                                // 异步网络初始化
                Net_SoftUpdate_Server_Initialization();                            // 软件更新引擎初始化
                Ultimate_File_Initiaization();                                     // 共享文件引擎初始化
                Net_File_Portrait_Initialization();                                // 头像文件管理服务
                Net_Udp_Server_Initialization();                                   // UDP引擎服务初始化
                Net_Push_Server_Initialization( );                                 // 推送网络引擎初始化
                启动服务器ToolStripMenuItem.Text = "已启动"; 
                启动服务器ToolStripMenuItem.BackColor = Color.LimeGreen;
                IsSystemStart = true;
            }
        }

        private void 版本控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormVersionControl fvc = new FormVersionControl(UserServer.ServerSettings))
            {
                fvc.ShowDialog();
                toolStripStatusLabel_version.Text = UserServer.ServerSettings.SystemVersion.ToString();
            }
        }

        private void 维护切换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormMaintenance fm = new FormMaintenance(UserServer.ServerSettings))
            {
                fm.ShowDialog();
                MaintenanceInitialization();
            }
        }


        private void 消息发送ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 测试发送字节数据
            // net_socket_server.SendAllClients(BitConverter.GetBytes(12345678));
            // 将消息群发给所有的客户端，并使用消息弹窗的方式显示
            using (FormInputAndAction fiaa = new FormInputAndAction(
                m =>
                {
                    net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.弹窗新消息, m); return true;
                }))
            {
                fiaa.ShowDialog();
            }
        }

        private void 一键断开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 关闭信号发送至所有在线客户端
            net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.关闭客户端, "");
        }

        private void 关于软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormAbout fm = new FormAbout(
                SoftResources.StringResouce.SoftName, 
                UserServer.ServerSettings.SystemVersion,
                2017, 
                SoftResources.StringResouce.SoftCopyRight
                ))
            {
                fm.ShowDialog();
            }
        }
        private void 框架作者ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormAuthorAdvertisement faa = new FormAuthorAdvertisement())
            {
                faa.ShowDialog();
            }
        }
        private void 账户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 该部分比较复杂，需要对委托，匿名委托概念比较清晰
            using (FormAccountManage fam = new FormAccountManage(() => UserServer.ServerAccounts.GetAllAccountsJson(),
                m => { UserServer.ServerAccounts.LoadAllAccountsJson(m); return true; }))
            {
                fam.ShowDialog();
            }
        }

        private void 版本号说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormAboutVersion fav = new FormAboutVersion(UserServer.ServerSettings.SystemVersion))
            {
                fav.ShowDialog();
            }
        }
        private void MaintenanceInitialization()
        {
            // 维护状态变更
            if (UserServer.ServerSettings.Can_Account_Login)
            {
                label3.Text = "可登录";                  // Login Enabled True
                label3.BackColor = Color.LimeGreen;
            }
            else
            {
                label3.Text = "维护中";                  // Login Enabled False
                label3.BackColor = Color.Tomato;
            }
        }

        private void 日志查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormLogNetView fln = new FormLogNetView())
            {
                fln.ShowDialog();
            }
        }

        #endregion

        #region Software Auto Update Function

        /// <summary>
        /// 支持软件自动更新的后台服务引擎
        /// </summary>
        private NetSoftUpdateServer net_soft_update_Server = new NetSoftUpdateServer();

        private void Net_SoftUpdate_Server_Initialization()
        {
            try
            {
                net_soft_update_Server.LogNet = new LogNetSingle(LogSavePath + @"\update_log.txt");
                net_soft_update_Server.Token = UserSystem.KeyToken;

                // 在服务器的这个路径下，放置客户端运行的所有文件，不要包含settings文件，不要从此处运行
                // 只放置exe和dll组件，必须放置：软件自动更新.exe

                // In this path of the server, place all files run by the client, do not include the settings file, do not run from here
                // Only exe and dll components are placed and must be placed: 软件自动更新.exe
                net_soft_update_Server.FileUpdatePath = Application.StartupPath + @"\AdvancedFiles\ClientFiles";//客户端文件路径
                net_soft_update_Server.ServerStart(UserSystem.Port_Update_Net);
            }
            catch (Exception ex)
            {
                SoftBasic.ShowExceptionMessage(ex);
            }
        }

        #endregion

        #region 同步数据传送引擎

        /// <summary>
        /// 用户同步数据传送的引擎
        /// </summary>
        private NetSimplifyServer net_simplify_server = new NetSimplifyServer();

        /// <summary>
        /// 同步传送数据的初始化
        /// </summary>
        private void Net_Simplify_Server_Initialization()
        {
            try
            {
                net_simplify_server.Token = UserSystem.KeyToken;//设置身份令牌
                net_simplify_server.LogNet = new LogNetSingle(LogSavePath + @"\simplify_log.txt");//日志路径
                net_simplify_server.LogNet.SetMessageDegree(HslMessageDegree.INFO);//默认debug及以上级别日志均进行存储，根据需要自行选择
                net_simplify_server.ReceiveStringEvent += Net_simplify_server_ReceiveStringEvent;//接收到字符串触发
                net_simplify_server.ReceivedBytesEvent += Net_simplify_server_ReceivedBytesEvent;//接收到字节触发
                net_simplify_server.ServerStart(UserSystem.Port_Second_Net);
            }
            catch (Exception ex)
            {
                SoftBasic.ShowExceptionMessage(ex);
            }
        }

        /// <summary>
        /// 接收来自客户端的字节数据
        /// </summary>
        /// <param name="session">网络状态</param>
        /// <param name="customer">字节数据，根据实际情况选择是否使用</param>
        /// <param name="data">来自客户端的字节数据</param>
        private void Net_simplify_server_ReceivedBytesEvent( AppSession session, NetHandle customer, byte[] data )
        {
            net_simplify_server.SendMessage( session, customer, data );
        }




        /******************************************************************************************************************
         * 
         *    方法说明：    当接收到来自客户端的数据的时候触发的方法
         *    特别注意：    如果你的数据处理中引发了异常，应用程序将会奔溃，SendMessage异常系统将会自动处理
         * 
         ******************************************************************************************************************/


        /// <summary>
        /// 接收到来自客户端的数据，此处需要放置维护验证，更新验证等等操作
        /// </summary>
        /// <param name="session">客户端的地址</param>
        /// <param name="handle">用于自定义的指令头，可不用，转而使用data来区分</param>
        /// <param name="data">接收到的服务器的数据</param>
        private void Net_simplify_server_ReceiveStringEvent(AppSession session, NetHandle handle, string data)
        {

            /*******************************************************************************************
             * 
             *     说明：同步消息处理总站，应该根据不同的消息设置分流到不同的处理方法
             *     
             *     注意：处理完成后必须调用 net_simplify_server.SendMessage(state, customer, "处理结果字符串，可以为空");
             *
             *******************************************************************************************/

            if (handle.CodeMajor == 1 && handle.CodeMinor == 1)
            {
                DataProcessingWithStartA(session, handle, data);
            }
            else if (handle.CodeMajor == 1 && handle.CodeMinor == 2)
            {
                DataProcessingWithStartB(session, handle, data);
            }
            else
            {
                net_simplify_server.SendMessage(session, handle, data);
            }
        }


        /****************************************************************************************************
         * 
         *    数据处理中心，同步信息中的所有的细节处理均要到此处来处理
         * 
         ****************************************************************************************************/
         
        #region 1.1.X 系统指令块
            
        /// <summary>
        /// 1.1.x指令块，处理系统基础运行的消息
        /// </summary>
        /// <param name="state">网络状态对象</param>
        /// <param name="handle">用户自定义的指令头</param>
        /// <param name="data">实际的数据</param>
        private void DataProcessingWithStartA( AppSession state, NetHandle handle, string data)
        {
            if (handle == CommonHeadCode.SimplifyHeadCode.维护检查)
            {
                net_simplify_server.SendMessage(state, handle,
                UserServer.ServerSettings.Can_Account_Login ? "1" : "0" +
                UserServer.ServerSettings.Account_Forbidden_Reason);
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.更新检查)
            {
                net_simplify_server.SendMessage(state, handle, UserServer.ServerSettings.SystemVersion.ToString());
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.参数下载)
            {
                JObject json = new JObject
                {
                    { nameof(UserServer.ServerSettings.Announcement), new JValue(UserServer.ServerSettings.Announcement) },
                    { nameof(UserServer.ServerSettings.SystemFactories), new JArray(UserServer.ServerSettings.SystemFactories) },
                    { nameof(UserServer.ServerSettings.SqlServerStr), new JValue(UserServer.ServerSettings.SqlServerStr) },
                };
                net_simplify_server.SendMessage(state, handle, json.ToString());
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.账户检查)
            {
                //此处使用的是组件自带的验证的方式，如果使用SQL数据库，另行验证
                JObject json = JObject.Parse(data.Trim());

                //提取账户，密码
                string name = SoftBasic.GetValueFromJsonObject(json, UserAccount.UserNameText, "");
                string password = SoftBasic.GetValueFromJsonObject(json, UserAccount.PasswordText, "");
                string way = SoftBasic.GetValueFromJsonObject(json, UserAccount.LoginWayText, "winform");
                string machineId = SoftBasic.GetValueFromJsonObject(json, UserAccount.DeviceUniqueID, "ABCDEFHIGKLMN");
                string frameworkVersion = SoftBasic.GetValueFromJsonObject(json, UserAccount.FrameworkVersion, "1.0.0");


                UserAccount account = UserServer.ServerAccounts.CheckAccount(name, password, state.IpEndPoint.Address.ToString(), way);


                // 先判定框架版本是否正确
                //if (!UserServer.ServerSettings.AllowLoginWhenFramewordVersionNotCheck)
                //{
                //    SystemVersion sv = new SystemVersion(frameworkVersion);
                //    if (sv < SoftBasic.FrameworkVersion)
                //    {
                //        account.LoginEnable = false;
                //        account.ForbidMessage = "框架版本检测失败，请更新";
                //        RuntimeLogHelper?.WriteWarn("框架版本验证失败，version:" + frameworkVersion);
                //    }
                //}
                
                // 检测是否重复登录
                if (account.LoginEnable)
                {
                    if (!UserServer.ServerSettings.AllowUserMultiOnline)
                    {
                        if (IsClinetOnline(account.UserName))
                        {
                            account.LoginEnable = false;
                            account.ForbidMessage = "该账户已经登录";
                        }
                    }
                }

                // 检测客户端id是否被授权
                if(UserServer.ServerSettings.WhetherToEnableTrustedClientAuthentication)
                {
                    if(!UserServer.ServerSettings.CanClientLogin(machineId))
                    {
                        account.LoginEnable = false;
                        account.ForbidMessage = "该客户端不再服务器信任列表";
                    }
                }

                net_simplify_server.SendMessage(state, handle, JObject.FromObject(account).ToString());
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.更新公告)
            {
                UserServer.ServerSettings.Announcement = data;
                //通知所有客户端更新公告
                net_socket_server.SendAllClients(handle, data);
                net_simplify_server.SendMessage(state, handle, "成功");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.获取账户)
            {
                //返回服务器的账户信息
                if (string.IsNullOrEmpty(data))
                {
                    // 获取所有账户
                    net_simplify_server.SendMessage(state, handle, UserServer.ServerAccounts.GetAllAccountsJson());
                }
                else if (data.Length < 16)
                {
                    // 获取所有分厂器的账户
                    net_simplify_server.SendMessage(state, handle, UserServer.ServerAccounts.GetAllAccountsJson(data));
                }
                else
                {
                    // 获取一个角色的所有关联账户
                    string[] names = UserServer.ServerRoles.GetUsernamesByRolename(data);
                    net_simplify_server.SendMessage(state, handle, UserServer.ServerAccounts.GetAllAccountsJson(names));
                }
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.更细账户)
            {
                //更新服务器的账户信息
                UserServer.ServerAccounts.LoadAllAccountsJson(data);
                net_simplify_server.SendMessage(state, handle, "成功");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.密码修改)
            {
                //更新服务器的账户密码，此处使用的是组件自带的验证的方式，如果使用SQL数据库，另行验证
                JObject json = JObject.Parse(data);
                //提取账户，密码
                string name = SoftBasic.GetValueFromJsonObject(json, UserAccount.UserNameText, "");
                string password = SoftBasic.GetValueFromJsonObject(json, UserAccount.PasswordText, "");
                UserServer.ServerAccounts.UpdatePassword(name, password);
                UserServer.ServerAccounts.SaveToFile();
                net_simplify_server.SendMessage(state, handle, "成功");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.更新版本)
            {
                try
                {
                    UserServer.ServerSettings.SystemVersion = new SystemVersion(data);
                    UserServer.ServerSettings.SaveToFile();
                    toolStripStatusLabel_version.Text = UserServer.ServerSettings.SystemVersion.ToString();
                    //记录数据
                    RuntimeLogHelper.WriteInfo($"更改了版本号：{data}");
                    net_simplify_server.SendMessage(state, handle, "1");
                }
                catch
                {
                    net_simplify_server.SendMessage(state, handle, "0");
                }
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.注册账号)
            {
                bool result = UserServer.ServerAccounts.AddNewAccount(data);
                if (result) UserServer.ServerAccounts.SaveToFile();
                net_simplify_server.SendMessage(state, handle, result ? "1" : "0");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.请求文件)
            {
                net_simplify_server.SendMessage(state, handle, ShareFileContainer.JsonArrayContent);
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.意见反馈)
            {
                AdviceLogHelper.WriteInfo(data);
                net_simplify_server.SendMessage(state, handle, "成功");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.群发消息)
            {
                net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.弹窗新消息, data);
                net_simplify_server.SendMessage(state, handle, "成功");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.异常消息)
            {
                ClientsLogHelper.WriteFatal(data);
                net_simplify_server.SendMessage(state, handle, "成功");
                //发送到邮箱
                SendUserMail("异常记录", "时间：" + DateTime.Now.ToString("O") + Environment.NewLine + data);
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.上传头像MD5)
            {
                try
                {
                    // 此处上传两个头像的MD5数据
                    JObject json = JObject.Parse(data);

                    string SmallPortraitMD5 = SoftBasic.GetValueFromJsonObject(json, UserAccount.SmallPortraitText, "");
                    string LargePortraitMD5 = SoftBasic.GetValueFromJsonObject(json, UserAccount.LargePortraitText, "");
                    string UserName = SoftBasic.GetValueFromJsonObject(json, UserAccount.UserNameText, ""); 

                    UserServer.ServerAccounts.UpdatePortraitMD5(UserName, SmallPortraitMD5, LargePortraitMD5);
                    net_simplify_server.SendMessage(state, handle, "成功");


                    // 推送头像更新消息
                    net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.新头像更新, UserName);
                }
                catch(Exception ex)
                {
                    net_simplify_server.SendMessage(state, handle, "失败，原因是：" + ex.Message);
                }


            }
            else if(handle==CommonHeadCode.SimplifyHeadCode.请求分厂)
            {
                net_simplify_server.SendMessage(state, handle, JArray.FromObject(UserServer.ServerSettings.SystemFactories).ToString());
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.上传分厂)
            {
                try
                {
                    UserServer.ServerSettings.SystemFactories = JArray.Parse(data).ToObject<List<string>>();
                    RuntimeLogHelper?.WriteInfo("上传了分厂信息：" + data);
                }
                catch (Exception ex)
                {
                    RuntimeLogHelper?.WriteException(null, ex);
                }
                net_simplify_server.SendMessage(state, handle, "1");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.请求信任客户端)
            {
                JObject json = new JObject
                {
                    { "TrustEnable", new JValue(UserServer.ServerSettings.WhetherToEnableTrustedClientAuthentication) },
                    { "TrustList", new JArray(UserServer.ServerSettings.TrustedClientList) }
                };
                net_simplify_server.SendMessage(state, handle, json.ToString());
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.上传信任客户端)
            {
                JObject json = JObject.Parse(data);
                UserServer.ServerSettings.WhetherToEnableTrustedClientAuthentication = json["TrustEnable"].ToObject<bool>();
                UserServer.ServerSettings.TrustedClientList = json["TrustList"].ToObject<List<string>>();
                UserServer.ServerSettings.SaveToFile();
                net_simplify_server.SendMessage(state, handle, "成功");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.请求一般配置)
            {
                JObject json = new JObject
                {
                    { "AllowUserMulti", new JValue(UserServer.ServerSettings.AllowUserMultiOnline) },
                    { "AllowFrameLogin", new JValue(UserServer.ServerSettings.AllowLoginWhenFramewordVersionNotCheck) },
                };
                net_simplify_server.SendMessage(state, handle, json.ToString());
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.上传一般配置)
            {
                JObject json = JObject.Parse(data);
                UserServer.ServerSettings.AllowUserMultiOnline = SoftBasic.GetValueFromJsonObject(json, "AllowUserMulti", false);
                UserServer.ServerSettings.AllowLoginWhenFramewordVersionNotCheck = SoftBasic.GetValueFromJsonObject(json, "AllowFrameLogin", false);
                net_simplify_server.SendMessage(state, handle, json.ToString());
            }
            else if(handle == CommonHeadCode.SimplifyHeadCode.请求角色配置)
            {
                net_simplify_server.SendMessage(state, handle, UserServer.ServerRoles.ToSaveString());
            }
            else if(handle == CommonHeadCode.SimplifyHeadCode.上传角色配置)
            {
                UserServer.ServerRoles.LoadByString(data);
                UserServer.ServerRoles.SaveToFile();
                net_simplify_server.SendMessage(state, handle, "1");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.检查角色权限)
            {
                JObject json = JObject.Parse(data);
                string name = SoftBasic.GetValueFromJsonObject(json, "Name", "");
                string roleCode = SoftBasic.GetValueFromJsonObject(json, "Role", "");
                net_simplify_server.SendMessage(state, handle, UserServer.ServerRoles.IsAllowAccountOperate(roleCode, name).ToString());
            }
            else
            {
                net_simplify_server.SendMessage(state, handle, data);
            }
        }


        #endregion
        
        #region 1.2.X 系统日志块

        /// <summary>
        /// B指令块，处理日志相关的消息
        /// </summary>
        /// <param name="state">网络状态对象</param>
        /// <param name="handle">用户自定义的命令头</param>
        /// <param name="data">实际的数据</param>
        private void DataProcessingWithStartB( AppSession state, NetHandle handle, string data)
        {
            if (handle == CommonHeadCode.SimplifyHeadCode.网络日志查看)
            {
                LogNetSingle logNet = (LogNetSingle)net_socket_server.LogNet;
                net_simplify_server.SendMessage(state, handle, logNet.GetAllSavedLog());
                RuntimeLogHelper.WriteInfo("网络日志查看");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.网络日志清空)
            {
                if (net_socket_server.LogNet is LogNetSingle logNet)
                {
                    logNet.ClearLog();
                }
                net_simplify_server.SendMessage(state, handle, "成功");
                RuntimeLogHelper.WriteWarn("网络日志清空");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.同步日志查看)
            {
                LogNetSingle logNet = (LogNetSingle)net_simplify_server.LogNet;
                net_simplify_server.SendMessage(state, handle, logNet.GetAllSavedLog());
                RuntimeLogHelper.WriteInfo("同步日志查看");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.同步日志清空)
            {
                if (net_simplify_server.LogNet is LogNetSingle logNet)
                {
                    logNet.ClearLog();
                }
                net_simplify_server.SendMessage(state, handle, "成功");
                RuntimeLogHelper.WriteWarn("同步日志清空");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.更新日志查看)
            {
                LogNetSingle logNet = (LogNetSingle)net_soft_update_Server.LogNet;
                net_simplify_server.SendMessage(state, handle, logNet.GetAllSavedLog());
                RuntimeLogHelper.WriteInfo("更新日志查看");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.更新日志清空)
            {
                if (net_soft_update_Server.LogNet is LogNetSingle logNet)
                {
                    logNet.ClearLog();
                }
                net_simplify_server.SendMessage(state, handle, "成功");
                RuntimeLogHelper.WriteWarn("更新日志清空");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.运行日志查看)
            {
                LogNetSingle logNet = (LogNetSingle)RuntimeLogHelper;
                net_simplify_server.SendMessage(state, handle, logNet.GetAllSavedLog());
                RuntimeLogHelper.WriteInfo("运行日志查看");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.运行日志清空)
            {
                if (RuntimeLogHelper is LogNetSingle logNet)
                {
                    logNet.ClearLog();
                }
                net_simplify_server.SendMessage(state, handle, "成功");
                RuntimeLogHelper.WriteWarn("运行日志清空");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.文件日志查看)
            {
                LogNetSingle logNet = (LogNetSingle)net_ultimate_file_server.LogNet;
                net_simplify_server.SendMessage(state, handle, logNet.GetAllSavedLog());
                RuntimeLogHelper.WriteInfo("共享文件日志查看");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.文件日志清空)
            {
                if (net_ultimate_file_server.LogNet is LogNetSingle logNet)
                {
                    logNet.ClearLog();
                }
                net_simplify_server.SendMessage(state, handle, "成功");
                RuntimeLogHelper.WriteWarn("共享文件日志清空");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.反馈日志查看)
            {
                LogNetSingle logNet = (LogNetSingle)AdviceLogHelper;
                net_simplify_server.SendMessage(state, handle, logNet.GetAllSavedLog());
                RuntimeLogHelper.WriteInfo("建议反馈日志查看");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.反馈日志清空)
            {
                if (AdviceLogHelper is LogNetSingle logNet)
                {
                    logNet.ClearLog();
                }
                net_simplify_server.SendMessage(state, handle, "成功");
                RuntimeLogHelper.WriteWarn("建议反馈日志清空");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.UDP日志查看)
            {
                LogNetSingle logNet = (LogNetSingle)net_udp_server.LogNet;
                net_simplify_server.SendMessage(state, 0, logNet.GetAllSavedLog());
                RuntimeLogHelper.WriteInfo("UDP日志查看");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.UDP日志清空)
            {
                if (net_udp_server.LogNet is LogNetSingle logNet)
                {
                    logNet.ClearLog();
                }
                net_simplify_server.SendMessage(state, handle, "成功");
                RuntimeLogHelper.WriteWarn("UDP日志清空");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.客户端日志查看)
            {
                LogNetSingle logNet = ClientsLogHelper as LogNetSingle;
                net_simplify_server.SendMessage(state, 0, logNet.GetAllSavedLog());
                RuntimeLogHelper.WriteInfo("客户端日志查看");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.客户端日志清空)
            {
                if (ClientsLogHelper is LogNetSingle logNet)
                {
                    logNet.ClearLog();
                }
                net_simplify_server.SendMessage(state, handle, "成功");
                RuntimeLogHelper.WriteWarn("客户端日志清空");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.头像日志查看)
            {
                LogNetSingle logNet = (LogNetSingle)net_file_Advanced.LogNet;
                net_simplify_server.SendMessage(state, 0, logNet.GetAllSavedLog());
                RuntimeLogHelper.WriteInfo("头像日志查看");
            }
            else if (handle == CommonHeadCode.SimplifyHeadCode.头像日志清空)
            {
                if (net_file_Advanced.LogNet is LogNetSingle logNet)
                {
                    logNet.ClearLog();
                }
                net_simplify_server.SendMessage(state, handle, "成功");
                RuntimeLogHelper.WriteWarn("头像日志清空");
            }
            else
            {
                net_simplify_server.SendMessage(state, handle, data);
            }
        }


        #endregion

        #region 1.3.X 自行扩充块

        /****************************************************************************************************
         * 
         *   您在下面可以自己扩展数据处理的方法，设计原则为运行速度尽可能的快，不要长时间阻塞
         * 
         ****************************************************************************************************/

        private void DataProcessingWithStartC( AppSession state, NetHandle handle, string data)
        {

        }

        #endregion

        #endregion

        #region 异步数据传送引擎

        /******************************************************************************************************************
         * 
         *    说明：    异步网络主要显示的功能是对所有在线客户端的管理能力，并允许群发所有在线客户端消息
         *    注意：    如果客户端需要向服务器请求数据并明确的需要返回消息，则必须选择同步网络通信
         * 
         ******************************************************************************************************************/

        /// <summary>
        /// 异步客户端管理引擎，维护所有的客户端在线情况，支持主动发数据到所有的客户端
        /// </summary>
        private NetComplexServer net_socket_server = new NetComplexServer();

        /// <summary>
        /// 异步传送数据的初始化
        /// </summary>
        private void Net_Socket_Server_Initialization()
        {
            try
            {
                net_socket_server.Token = UserSystem.KeyToken;                                                 // 设置身份令牌
                net_socket_server.LogNet =new LogNetSingle(LogSavePath + @"\net_log.txt");                     // 设置日志存储路径
                net_socket_server.LogNet.SetMessageDegree(HslMessageDegree.INFO);                              // 默认debug及以上级别日志均进行存储，根据需要自行选择
                net_socket_server.IsSaveLogClientLineChange = true;                                            // 设置客户端上下线是否记录到日志
                net_socket_server.ClientOnline += Net_socket_server_ClientOnline;                              // 客户端上线触发
                net_socket_server.ClientOffline += Net_socket_server_ClientOffline;                            // 客户端下线触发，包括异常掉线
                net_socket_server.AllClientsStatusChange += Net_socket_server_AllClientsStatusChange;          // 客户端上下线变化时触发
                net_socket_server.AcceptByte += Net_socket_server_AcceptByte;                                  // 服务器接收到字节数据触发
                net_socket_server.AcceptString += Net_socket_server_AcceptString;                              // 服务器接收到字符串数据触发
                net_socket_server.ServerStart(UserSystem.Port_Main_Net);
            }
            catch (Exception ex)
            {
                SoftBasic.ShowExceptionMessage(ex);
            }
        }
        

        /******************************************************************************************************************
         * 
         *    方法说明：    当接收到来自客户端的数据的时候触发的方法
         *    特别注意：    如果你的数据处理中引发了异常，应用程序将会继续运行，该异常将会记录在网络日志中，
         *                   所以有必要的话，对可能发生的异常需要提前处理。
         * 
         ******************************************************************************************************************/


        private void Net_socket_server_AcceptString(AppSession session, NetHandle handle, string data)
        {
            if (handle.CodeMajor == 2 && handle.CodeMinor == 1)
            {
                DataProcessingWithStartH( session, handle, data);
            }

        }


        #region 2.1.X 指令处理块
        
        /// <summary>
        /// H开头的处理块
        /// </summary>
        /// <param name="state">网络状态</param>
        /// <param name="customer">用户自定义的指令头</param>
        /// <param name="data">字符串数据</param>
        private void DataProcessingWithStartH( AppSession session, NetHandle customer, string data)
        {
            if (customer == CommonHeadCode.MultiNetHeadCode.留言版消息)
            {
                ChatAddMessage( session.LoginAlias, data);
            }
        }

        
        #endregion


        private void Net_socket_server_AcceptByte(AppSession session, NetHandle customer, byte[] data)
        {
            // 如果此处充斥大量if语句，影响观感，则考虑进行指令头分类操作，客户端异步发送的字节数组都会到此处处理
        }
        

        private void Net_socket_server_ClientOffline(AppSession session, string object2)
        {
            netAccountManager.RemoveOnlineClient(session.ClientUniqueID);
            net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.用户下线, session.ClientUniqueID);

            UserInterfaceMessageRender(DateTime.Now.ToString("MM-dd HH:mm:ss ") + session.IpAddress + "：" + session.LoginAlias + " " + object2);
        }

        private void Net_socket_server_ClientOnline(AppSession session)
        {
            NetAccount account = new NetAccount
            {
                UserName = session.LoginAlias,
                Roles = UserServer.ServerRoles.GetRolesByUserName(session.LoginAlias),
                IpAddress = session.IpAddress,
                Alias = UserServer.ServerAccounts.GetAccountAlias(session.LoginAlias),
                Factory = UserServer.ServerAccounts.GetAccountFactory(session.LoginAlias),
                LoginTime = DateTime.Now,
                UniqueId = session.ClientUniqueID
            };

            net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.新用户上线, JObject.FromObject(account).ToString());

            
            // 上线后回发一条数据初始化信息
            JObject json = new JObject
            {
                { "Time", new JValue(DateTime.Now) },
                { "FileCount", new JValue(ShareFileContainer.FileCount) },
                { "chats", new JValue(Chats_Managment.ToSaveString())},
                { "ClientsOnline", new JValue(netAccountManager.ClientsOnlineCache) }
            };
            
            // 发送客户端的初始化数据
            net_socket_server.Send(session, CommonHeadCode.MultiNetHeadCode.初始化数据, json.ToString());
            // 新增到在线客户端的队列中
            netAccountManager.AddOnlineClient(account);
            // 触发上下线功能
            UserInterfaceMessageRender(DateTime.Now.ToString("MM-dd HH:mm:ss ") + session.IpAddress + "：" + session.LoginAlias + " 上线");
        }


        private void Net_socket_server_AllClientsStatusChange(int count)
        {
            // 此处决定要不要将在线客户端的数据发送所有客户端
            // net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.总在线信息, netAccountManager.ClientsOnlineCache);
        }
        

        /// <summary>
        /// 用来判断客户端是否已经在线，除了超级管理员，其他的账户不允许重复在线，重复登录的账户予以特殊标记
        /// </summary>
        /// <returns>该客户端是否在线</returns>
        private bool IsClinetOnline(string userName)
        {
            // 超级管理员允许重复登录
            if (userName == "admin") return false;
            // 其他的需要检测信息
            return netAccountManager.IsClientOnline( userName );
        }

        /// <summary>
        /// 发送数据给在线的指定角色客户端
        /// </summary>
        /// <param name="handle">信息句柄</param>
        /// <param name="message">消息</param>
        /// <param name="roleCode">角色代码</param>
        private void SendToClientsByRoleCode(NetHandle handle, string message, string roleCode)
        {
            foreach (var m in UserServer.ServerRoles.GetUsernamesByRolename(roleCode))
            {
                net_socket_server.SendClientByAlias(m, handle, roleCode);
            }
        }

        #endregion

        #region 在线客户端管理器

        /// <summary>
        /// 所有在线客户端的管理器
        /// </summary>
        private NetAccountManager netAccountManager { get; set; } = new NetAccountManager();

        /// <summary>
        /// 现在所有的在线客户端
        /// </summary>
        private void RenderOnlineAccount( )
        {
            string[] buffer = netAccountManager.GetOnlineInformation( );
            label4.Text = buffer.Length.ToString( );
            listBox1.DataSource = buffer;
        }

        #endregion

        #region Push Net [ 推送网络 ]

        private NetPushServer pushServer;


        private void Net_Push_Server_Initialization( )
        {
            try
            {
                pushServer = new NetPushServer( );
                pushServer.Token = UserSystem.KeyToken;
                pushServer.LogNet = RuntimeLogHelper;
                pushServer.ServerStart( UserSystem.Port_Push_Server );
            }
            catch(Exception ex)
            {
                SoftBasic.ShowExceptionMessage( ex );
            }
        }

        #endregion

        #region Time Tick Thread

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
            toolStripStatusLabel_time.Alignment = ToolStripItemAlignment.Right;
            statusStrip1.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow;
            toolStripStatusLabel_time.ForeColor = Color.Purple;                      // 紫色


            Thread thread = new Thread(new ThreadStart(ThreadTimeTick));
            thread.IsBackground = true;
            thread.Start();
        }



        private void MethodOccurEverySecond( )
        {
            toolStripStatusLabel_time.Text = DateTime.Now.ToString( );
            RenderOnlineAccount( );
            if (net_simplify_server != null) label_Count_Simplify.Text = net_simplify_server.ClientCount.ToString( );
            if (pushServer != null) label_Count_Push.Text = pushServer.OnlineCount.ToString( );
        }


        public void ThreadTimeTick()
        {
            Thread.Sleep(300);                // 加一个微小的延时
            int second = DateTime.Now.Second - 1;
            int minute = -1;
            int hour = -1;
            int day = -1;
            Action DTimeShow = MethodOccurEverySecond;

            while (IsWindowShow)
            {
                while (DateTime.Now.Second == second)
                {
                    Thread.Sleep(20);
                }
                second = DateTime.Now.Second;
                if (IsWindowShow && IsHandleCreated) Invoke(DTimeShow);
                // 每秒钟执行的代码

                if (second == 0)
                {
                    // 每个0秒执行的代码
                    // net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.时间的推送, DateTime.Now.ToString("O"));
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

        #region Advanced File Server

        /**************************************************************************************
         * 
         *    本文件管理器引擎目前主要实现1个功能
         *    1. 允许客户端上传服务器的客户端文件，用来提供软件自动更新使用的
         *    
         *    AFS 文件服务器的特点是支持原生的文件上传下载，和你在服务器上看到的本地目录是一致的
         *    优点：原生，上传的文件可以用于其他操作的识别
         *    缺点：不支持额外的信息输入
         * 
         **************************************************************************************/


        /// <summary>
        /// 一个高级的文件管理服务器引擎
        /// </summary>
        private AdvancedFileServer net_file_Advanced = new AdvancedFileServer();

        /// <summary>
        /// 初始化高级的文件管理引擎
        /// </summary>
        private void Net_File_Portrait_Initialization()
        {
            try
            {
                net_file_Advanced.FilesDirectoryPath = Application.StartupPath + @"\AdvancedFiles";
                net_file_Advanced.FilesDirectoryPathTemp = Application.StartupPath + @"\AdvancedFiles\Temp";
                net_file_Advanced.LogNet = new LogNetSingle(LogSavePath + @"\Advanced_file_log.txt");
                net_file_Advanced.Token = UserSystem.KeyToken;
                net_file_Advanced.ServerStart(UserSystem.Port_Advanced_File_Server);
            }
            catch (Exception ex)
            {
                SoftBasic.ShowExceptionMessage(ex);
            }
        }


        #endregion

        #region Ultimate File Server


        /**************************************************************************************
         * 
         *    本文件管理器引擎目前主要实现3个大功能
         *    1. 用于管理客户端的头像文件存储服务
         *    2. 用于管理每个账户的私有文件存储服务
         *    3. 用于主界面的共享文件管理
         *    
         *    UFS文件引擎的特点是支持复杂的额外信息，更彻底的读写分离，缺点是不支持原生访问，需要通过映射表来实现操作。
         * 
         **************************************************************************************/

        /// <summary>
        /// 终极文件管理服务器
        /// </summary>
        private UltimateFileServer net_ultimate_file_server { get; set; } = null;

        /// <summary>
        /// 终极文件管理服务器
        /// </summary>
        private void Ultimate_File_Initiaization()
        {
            try
            {
                net_ultimate_file_server = new UltimateFileServer();                                            // 实例化
                net_ultimate_file_server.Token = UserSystem.KeyToken;                                           // 设置系统的令牌
                net_ultimate_file_server.LogNet = new LogNetSingle(LogSavePath + @"\ultimate_file_log.txt");    // 设置日志路径
                net_ultimate_file_server.LogNet.SetMessageDegree(HslMessageDegree.DEBUG);                       // 默认debug及以上级别日志均进行存储，根据需要自行选择
                net_ultimate_file_server.FilesDirectoryPath = Application.StartupPath + @"\UltimateFiles";      // 设置文件根目录
                net_ultimate_file_server.ServerStart(UserSystem.Port_Ultimate_File_Server);                     // 启动服务

                // 共享文件管理器只是终极文件管理器的一个子容器
                ShareFileContainer = net_ultimate_file_server.GetGroupFromFilePath(Application.StartupPath + @"\UltimateFiles\ShareFiles");
                ShareFileContainer.FileCountChanged += ShareFileContainer_FileCountChanged;
            }
            catch (Exception ex)
            {
                SoftBasic.ShowExceptionMessage(ex);
            }
        }


        // 以下是共享文件的功能

        private void ShareFileContainer_FileCountChanged(int fileCount)
        {
            // 将文件数据发送给客户端
            net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.文件总数量, fileCount.ToString());
        }

        // 主界面的共享文件的容器 [The main interface of the shared file container]
        private GroupFileContainer ShareFileContainer;



        #endregion

        #region Feedback Function

        /**********************************************************************************************************
         * 
         *    说明：    用来记录意见反馈的数据
         *    举例：    AdviceLogHelper.SaveInformation("张三：主界面的颜色稍微调整一下");
         * 
         **********************************************************************************************************/


        /// <summary>
        /// 用来记录一般的事物日志
        /// </summary>
        private ILogNet AdviceLogHelper { get; set; }

        #endregion

        #region Udp网络通信块

        /*********************************************************************************************************
         * 
         *    说明    一个用于网络间通信的UDP服务引擎，客户端调用UserClient.Net_Udp_Client.SendMessage(data);
         *            发送详细请参考客户端FormMainWindow中的udp发送说明
         *    特点    该Udp引擎非常健壮，接收失败了会抛弃本次接收，自动进入下一轮接收。
         *    安全    本引擎含有数据长度校验机制，确保服务器接收到的数据是正确的，没有丢失的，发送的数据是经过加密的
         *    注意    如果服务器配置了ReceiveCacheLength = 1024，那么客户端发送的字符串数据长度不能超过1000，否则服务器会自动丢弃，可在日志中查看
         *    警告    如果想要你自己的软件支持向本引擎访问，必须使用该网络组件实现，参考客户端定义，否则发送失败
         * 
         **********************************************************************************************************/


        /// <summary>
        /// 服务器的UDP核心引擎
        /// </summary>
        /// <remarks>
        /// A UDP net server, receive data from client
        /// </remarks>
        private NetUdpServer net_udp_server { get; set; }

        private void Net_Udp_Server_Initialization()
        {
            try
            {
                net_udp_server = new NetUdpServer();
                net_udp_server.LogNet =new LogNetSingle(LogSavePath + @"\udp_log.txt");    // 日志路径
                net_udp_server.LogNet.SetMessageDegree(HslMessageDegree.DEBUG);            // 默认debug及以上级别日志均进行存储，根据需要自行选择
                net_udp_server.Token = UserSystem.KeyToken;
                net_udp_server.ReceiveCacheLength = 1024;                                  // 单次接收数据的缓冲长度
                net_udp_server.AcceptByte += Net_udp_server_AcceptByte;                    // 接收到字节数据的时候触发事件
                net_udp_server.AcceptString += Net_udp_server_AcceptString;                // 接收到字符串数据的时候触发事件
                net_udp_server.ServerStart(UserSystem.Port_Udp_Server);
            }
            catch (Exception ex)
            {
                SoftBasic.ShowExceptionMessage(ex);
            }
        }

        private void Net_udp_server_AcceptString(AppSession session, NetHandle customer, string data)
        {
            // 此处为测试 [Just for testing, there is no meaning]
            Invoke( new Action(() =>
            {
                textBox1.AppendText($"{DateTime.Now.ToString("MM-dd HH:mm:ss ")}来自IP:{session.IpEndPoint.Address.ToString()} 内容:{data}{Environment.NewLine}");
            }));
        }

        private void Net_udp_server_AcceptByte( AppSession session, NetHandle customer, byte[] data)
        {
            //具体用法参考上面字符串方法
        }

        #endregion
        
        #region 流水号生成示例代码

        private SoftNumericalOrder OrderAutoCreate { get; set; }

        /// <summary>
        /// 流水号初始化方法，如果需要可以放到窗口的load方法中
        /// </summary>
        private void OrderInitialization()
        {
            /*********************************************************************************************************
             * 
             *    说明     此处的时间格式是年月日，7是指跟在时间后面的序号的位数，不够补零
             *    示例1    调用 string str = OrderAutoCreate.GetNumericalOrder();//str为 AB201705190000001
             *    示例2    调用 string str = OrderAutoCreate.GetNumericalOrder("KN");//str为 KN201705190000002
             *    注意     默认计数不清空，后面的1，2会一值累加，可以调用900亿亿次，如果需要定期清空，请自行周期调用OrderAutoCreate.ClearNumericalOrder();
             *    提示     如果需要定期清空，在本页面的ThreadTimeTick()方法中清空即可
             *    性能     一秒钟可以响应请求100万次以上，具体值依据电脑的性能而定，并成功存储当前计数值
             * 
             **********************************************************************************************************/

            OrderAutoCreate = new SoftNumericalOrder("AB", "yyyyMMdd", 7, Application.StartupPath + @"\order.txt");
        }

        #endregion

        #region 服务器临时聊天消息存储块

        private SoftMsgQueue<string> Chats_Managment { get; set; }

        /// <summary>
        /// 聊天消息块的初始化
        /// </summary>
        private void ChatInitialization()
        {
            Chats_Managment = new SoftMsgQueue<string>()
            {
                MaxCache = 200,  // 记录200条临时消息
                FileSavePath = Application.StartupPath + @"\chats.txt"  // 设置保存的路径
            };
            Chats_Managment.LoadByFile();// 加载以前的数据
        }

        /// <summary>
        /// 新增一个消息，需要指明发送人和消息内容
        /// </summary>
        /// <param name="user">消息发送人</param>
        /// <param name="message">内容</param>
        private void ChatAddMessage(string user, string message)
        {
            string content = "\u0002" + user + DateTime.Now.ToString(" yyyy-MM-dd HH:mm:ss") + Environment.NewLine + " " + message;
            // 转发所有的客户端，包括发送者
            net_socket_server.SendAllClients(CommonHeadCode.MultiNetHeadCode.留言版消息, content);
            // 添加缓存
            Chats_Managment.AddNewItem(content);
        }


        /// <summary>
        /// 如果需要发送一些系统自己的消息，请调用这个方法。
        /// 一般来说将系统消息和用户聊天消息进行区分
        /// </summary>
        /// <param name="message">消息</param>
        private void ChatAddMessage(string message)
        {
            ChatAddMessage("[系统]", message);
        }

        #endregion

        #region System Log Function


        /*********************************************************************************************************
         * 
         *    说明     日志的使用方式分为6个等级，1.DEBUG 2.INFO 3.WARN 4.ERROR 5.FATAL 6.None 对应的调用方法不一致
         *    具体     如果想要调用存储[信息]等级日志，调用 RuntimeLogHelper.WriteInfo("等待存储的信息")
         *    大小     调用10000次存储信息后，日志文件大小在200K左右，需要手动进行情况日志
         *    注意     在存储信息时不要带有一个特殊字符，[\u0002]不可见的标识文本开始字符，会影响日志筛选时的准确性
         *    性能     该类使用了乐观并发模型技术，支持高并发的数据存储，可以安全的在线程间调用
         * 
         **********************************************************************************************************/

        /// <summary>
        /// 用来记录一般的事物日志
        /// </summary>
        private ILogNet RuntimeLogHelper { get; set; }

        /// <summary>
        /// 用来记录客户端的异常日志
        /// </summary>
        private ILogNet ClientsLogHelper { get; set; }


        #endregion

        #region Mail Send Function

        /******************************************************************************************
         * 
         *    本邮件系统使用了组件中预设好的中间发送地址，已经内置了两个邮件地址，一个163邮箱，另一个是QQ邮箱
         *    本处仅仅使用了163网易的邮箱发送
         *    下面提供了两个方法，实现了方便的发送，可以在程序的其他地方进行调用
         *
         ********************************************************************************************/

        /// <summary>
        /// 控制系统是否真的发送邮件到指定地址
        /// </summary>
        private bool IsSendMailEnable { get; set; }


        /// <summary>
        /// 邮件发送系统的初始方式，所有的参数将在下面进行
        /// </summary>
        private void SoftMailInitialization()
        {
            // 先进行关闭 如果想发送设置为True    [If you want to send the setting to True]
            IsSendMailEnable = false;
            // 作者测试的邮箱地址，实际需要换成你自己的    [You should replace it with your own email address]
            SoftMail.MailSystem163.MailSendAddress = "hsl200909@163.com";
        }

        /// <summary>
        /// 调用该方法可以直接将异常发送到你的邮箱里，如果服务器连接网络的话
        /// </summary>
        /// <remarks>
        /// If the server is connected to the network, calling this method can send the exception directly to your mailbox
        /// </remarks>
        /// <param name="ex">异常</param>
        private void SendUserMail(Exception ex)
        {
            if(IsSendMailEnable) SoftMail.MailSystem163.SendMail(ex);
        }

        /// <summary>
        /// 发送指定的主题和内容到指定的邮箱
        /// </summary>
        /// <remarks>
        /// Send the specified topic and content to the specified mailbox
        /// </remarks>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        private void SendUserMail(string subject, string body)
        {
            if (IsSendMailEnable) SoftMail.MailSystem163.SendMail(subject, body);
        }

        #endregion

        
    }
}
