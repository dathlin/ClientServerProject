using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BasicFramework;
using IndustryEthernet;
using Newtonsoft.Json.Linq;
using CommonLibrary;
using System.Threading;


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
//    如果：遇到启动调试就退出了，请注释掉Program.cs文件中的指允许启动一个实例的代码
//
//============================================================================




namespace 软件系统客户端模版
{
    public partial class FormMainWindow : Form
    {
        public FormMainWindow()
        {
            InitializeComponent();
        }

        #region 窗口的属性和方法
        /// <summary>
        /// 指示窗口是否显示的标志
        /// </summary>
        private bool IsWindowShow { get; set; } = false;

        private void FormMainWindow_Load(object sender, EventArgs e)
        {
            //窗口载入
            label_userName.Text = UserClient.UserAccount.UserName;
            label_grade.Text = AccountGrade.GetDescription(UserClient.UserAccount.Grade);
            label_factory.Text = UserClient.UserAccount.Factory;
            label_register.Text = UserClient.UserAccount.RegisterTime.ToString();
            label_last.Text = UserClient.UserAccount.LastLoginTime.ToString();
            label_times.Text = UserClient.UserAccount.LoginFrequency.ToString();
            label_address.Text = UserClient.UserAccount.LastLoginIpAddress;

            //绑定事件，仅执行一次，不能放到show方法里
            net_socket_client.MessageAlerts += Net_socket_client_MessageAlerts;
            net_socket_client.LoginFailed += Net_socket_client_LoginFailed;
            net_socket_client.LoginSuccess += Net_socket_client_LoginSuccess;
            net_socket_client.AcceptByte += Net_socket_client_AcceptByte;
            net_socket_client.AcceptString += Net_socket_client_AcceptString;
            //启动网络服务
            Net_Socket_Client_Initialization();

            label_Announcement.Text = UserClient.Announcement;

            toolStripStatusLabel_Version.Text = UserClient.CurrentVersion.ToString();
        }
        private void FormMainWindow_Shown(object sender, EventArgs e)
        {
            //窗口显示
            IsWindowShow = true;


            //是否显示更新日志
            if(UserClient.JsonSettings.IsNewVersionRunning)
            {
                UserClient.JsonSettings.IsNewVersionRunning = false;
                UserClient.JsonSettings.SaveSettings();
                更新日志ToolStripMenuItem_Click(null, new EventArgs());
            }

            //根据权限使能菜单
            if(UserClient.UserAccount.Grade<AccountGrade.SuperAdministrator)
            {
                日志查看ToolStripMenuItem.Enabled = false;
                账户管理ToolStripMenuItem.Enabled = false;
                远程更新ToolStripMenuItem.Enabled = false;
            }
        }
        private void FormMainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //窗口关闭
            IsWindowShow = false;

            net_socket_client.ClientClose();

            //等待一秒退出
            FormWaitInfomation fwm = new FormWaitInfomation("正在退出程序...", 1000);
            fwm.ShowDialog();
            fwm.Dispose();
        }


        #endregion

        #region 菜单逻辑块

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //实例化一个密码修改的窗口，并指定了实现修改的具体方法，指定了密码长度
            FormPasswordModify fpm = new FormPasswordModify(UserClient.UserAccount.Password,
                p => {
                    JObject json = new JObject();json.Add(UserAccount.UserNameText, UserClient.UserAccount.UserName);
                    json.Add(UserAccount.PasswordText, p);
                    return UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.密码修改 + json.ToString()).IsSuccess;
                }, 6, 8);
            fpm.ShowDialog();
            fpm.Dispose();
        }

        private void 关于本软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout fa = new FormAbout(Resource.StringResouce.SoftName, 
                UserClient.CurrentVersion, 2017, Resource.StringResouce.SoftCopyRight);
            fa.ShowDialog();
            fa.Dispose();
        }

        private void 更新日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //更新情况复位
            if (UserClient.JsonSettings.IsNewVersionRunning)
            {
                UserClient.JsonSettings.IsNewVersionRunning = false;
                UserClient.JsonSettings.SaveSettings();
            }
            FormUpdateLog ful = new FormUpdateLog(UserClient.HistoryVersions);
            ful.ShowDialog();
            ful.Dispose();
        }

        private void 版本号说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAboutVersion fav = new FormAboutVersion(UserClient.CurrentVersion);
            fav.ShowDialog();
            fav.Dispose();
        }

        private void 更改公告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormInputAndAction fiaa = new FormInputAndAction(str => UserClient.Net_simplify_client.ReadFromServer(
                CommonHeadCode.SimplifyHeadCode.更新公告 + str).IsSuccess,UserClient.Announcement);
            fiaa.ShowDialog();
            fiaa.Dispose();
        }

        private void 日志查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLog flg = new FormLog(UserClient.Net_simplify_client);
            flg.ShowDialog();
            flg.Dispose();
        }

        private void 注册账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormRegisterAccount fra = new FormRegisterAccount(UserClient.Net_simplify_client))
            {
                fra.ShowDialog();
            }
        }

        private void 账户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAccountManage fam = new FormAccountManage(() =>
            {
                OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.获取账户信息);
                if (result.IsSuccess) return result.Content;
                else return result.ToMessageShowString();
            },m => UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.更细账户信息 + m).IsSuccess);
            fam.ShowDialog();
            fam.Dispose();
        }

        private void 远程更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                if(ofd.ShowDialog()==DialogResult.OK)
                {
                    using (FormFileOperate fUpload = new FormFileOperate(new System.Net.IPEndPoint(
                        System.Net.IPAddress.Parse(UserClient.ServerIp),CommonLibrary.CommonLibrary.Port_Update_Remote),
                        ofd.FileNames, "", "", ""))
                    {
                        fUpload.ShowDialog();
                    }
                }
            }
        }

        #endregion

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
                net_socket_client.ClientAlias = UserClient.UserAccount.UserName;
                net_socket_client.ClientStart();
            }
            catch(Exception ex)
            {
                SoftBasic.ShowExceptionMessage(ex);
            }
        }

        private void Net_socket_client_AcceptString(HuTcpState object1, string object2)
        {
            //接收到服务器发来的字符串数据
            string head_code = object2.Substring(0, 4);
            if (head_code == CommonHeadCode.MultiNetHeadCode.弹窗消息)
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    FormPopup fpp = new FormPopup(object2.Substring(4), Color.DodgerBlue, 10000);
                    fpp.Show();
                }));
            }
            else if (head_code == CommonHeadCode.MultiNetHeadCode.所有客户端在线信息)
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    listBox1.DataSource = object2.Substring(4).Split('#');
                }));
            }
            else if (head_code == CommonHeadCode.MultiNetHeadCode.关闭所有客户端)
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    Close();
                }));
            }
            else if(head_code==CommonHeadCode.SimplifyHeadCode.更新公告)
            {
                //此处应用到了同步类的指令头
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    UserClient.Announcement = object2.Substring(4);
                    label_Announcement.Text= object2.Substring(4);
                    FormPopup fpp = new FormPopup(object2.Substring(4), Color.DodgerBlue, 10000);
                    fpp.Show();
                }));
            }
        }

        private void Net_socket_client_AcceptByte(HuTcpState object1, byte[] object2)
        {
            //接收到服务器发来的字节数据
            if (IsHandleCreated) Invoke(new Action(() =>
            {
                MessageBox.Show(BitConverter.ToInt32(object2, 0).ToString());
            }));
        }

        private void Net_socket_client_LoginSuccess()
        {
            //登录成功，或重新登录成功的事件，有些数据的初始化可以放在此处
            if (IsHandleCreated) Invoke(new Action(() =>
            {
                toolStripStatusLabel_status.Text = "客户端启动成功";
            }));
        }

        private void Net_socket_client_LoginFailed(int object1)
        {
            //登录失败的情况，如果连续三次连接失败，请考虑退出系统
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
            //信息提示
            if (IsHandleCreated) Invoke(new Action(() =>
            {
                toolStripStatusLabel_status.Text = object1;
            }));
        }

        #endregion

        
    }
}
