using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using IndustryEthernet;
using Newtonsoft.Json.Linq;

namespace 软件系统客户端模版
{

    //=================================================================================================
    //      登录窗口，此处实现了一些常用的代码，具体的版本号验证及账户验证需要根据实际需求实现
    //=================================================================================================


    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        #region 窗口属性及方法
        /// <summary>
        /// 指示窗体是否已经显示
        /// </summary>
        private bool IsWindowShow { get; set; } = false;

        private void FormLogin_Load(object sender, EventArgs e)
        {
            label_status.Visible = false;

            UserClient.JsonSettings.FileSavePath = Application.StartupPath + @"\JsonSettings.txt";
            UserClient.JsonSettings.LoadSettings();//根据实际实际情况选中解密算法，默认采用DES加密解密算法

            label_version.Text = "版本：" + UserClient.CurrentVersion.ToString();
            label2.Text = CommonLibrary.Resource.StringResouce.SoftName;
            label_copyright.Text = $"本软件著作权归{CommonLibrary.Resource.StringResouce.SoftCopyRight}所有";
        }

        private void FormLogin_Shown(object sender, EventArgs e)
        {
            IsWindowShow = true;

            //加载数据
            textBox_userName.Text = UserClient.JsonSettings.LoginName ?? "";
            textBox_password.Text = UserClient.JsonSettings.Password ?? "";
            checkBox_remeber.Checked = UserClient.JsonSettings.Password != "";

            //初始化输入焦点
            if (UserClient.JsonSettings.Password != "") userButton_login.Focus();
            else if (UserClient.JsonSettings.LoginName != "") textBox_password.Focus();
            else textBox_userName.Focus();
            
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            //退出
            IsWindowShow = false;
            //延时
            Thread.Sleep(100);

        }




        #endregion


        #region 界面逻辑块

        private void textBox_userName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) textBox_password.Focus();
        }

        private void textBox_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) userButton_login.PerformClick();
        }

        private void userButton_login_Click(object sender, EventArgs e)
        {
            label_status.Visible = true;
            //验证输入
            if (string.IsNullOrEmpty(textBox_userName.Text))
            {
                label_status.Text = "请输入用户名";
                textBox_userName.Focus();
                return;
            }

            if(string.IsNullOrEmpty(textBox_password.Text))
            {
                label_status.Text = "请输入密码";
                textBox_password.Focus();
                return;
            }

            label_status.Text = "正在验证维护状态...";
            UISettings(false);

            ThreadAccountLogin = new Thread(ThreadCheckAccount);
            ThreadAccountLogin.IsBackground = true;
            ThreadAccountLogin.Start();
        }
        /// <summary>
        /// 界面的UI使能操作
        /// </summary>
        /// <param name="enable">值</param>
        private void UISettings(bool enable)
        {
            textBox_userName.Enabled = enable;
            textBox_password.Enabled = enable;
            userButton_login.Enabled = enable;
            checkBox_remeber.Enabled = enable;
        }

        #endregion

        #region 账户验证的逻辑块

        

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
                  label_status.Text = message;
              };
            Action start_update = delegate
            {
                //需要该exe支持，否则将无法是实现自动版本控制
                string update_file_name = Application.StartupPath + @"\软件自动更新.exe";
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
            Thread.Sleep(200);

            //请求指令头数据，该数据需要更具实际情况更改
            OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.维护检查);
            if(result.IsSuccess)
            {
                //例如返回结果为1说明允许登录，0则说明服务器处于维护中，并将信息显示
                if (result.Content != "1")
                {
                    if (IsHandleCreated) Invoke(message_show, result.Content.Substring(1));
                    if (IsHandleCreated) Invoke(thread_finish);
                    return;
                }
            }
            else
            {
                //访问失败
                if (IsHandleCreated) Invoke(message_show, result.Message);
                if (IsHandleCreated) Invoke(thread_finish);
                return;
            }

            

            //检查账户
            if (IsHandleCreated) Invoke(message_show, "正在检查账户...");
            else return;

            //延时
            Thread.Sleep(200);

            //===================================================================================
            //   根据实际情况校验，选择数据库校验或是将用户名密码发至服务器校验
            //   以下展示了服务器校验的方法，如您需要数据库校验，请删除下面并改成SQL访问验证的方式

            //包装数据
            Newtonsoft.Json.Linq.JObject json = new Newtonsoft.Json.Linq.JObject();
            json.Add(BasicFramework.UserAccount.UserNameText, new Newtonsoft.Json.Linq.JValue(textBox_userName.Text));
            json.Add(BasicFramework.UserAccount.PasswordText, new Newtonsoft.Json.Linq.JValue(textBox_password.Text));

            result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.账户检查+json.ToString());
            if (result.IsSuccess)
            {
                //服务器应该返回账户的信息
                BasicFramework.UserAccount account = JObject.Parse(result.Content).ToObject<BasicFramework.UserAccount>();
                if(!account.LoginEnable)
                {
                    //不允许登录
                    if (IsHandleCreated) Invoke(message_show, account.ForbidMessage);
                    if (IsHandleCreated) Invoke(thread_finish);
                    return;
                }
                UserClient.UserAccount = account;
            }
            else
            {
                //访问失败
                if (IsHandleCreated) Invoke(message_show, result.Message);
                if (IsHandleCreated) Invoke(thread_finish);
                return;
            }

            //登录成功，进行保存用户名称和密码
            UserClient.JsonSettings.LoginName = textBox_userName.Text;
            UserClient.JsonSettings.Password = checkBox_remeber.Checked ? textBox_password.Text : "";
            UserClient.JsonSettings.SaveSettings();


            //版本验证
            if (IsHandleCreated) Invoke(message_show, "正在验证版本...");
            else return;

            //延时
            Thread.Sleep(200);

            result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.更新检查);
            if (result.IsSuccess)
            {
                //服务器应该返回服务器的版本号
                BasicFramework.SystemVersion sv = new BasicFramework.SystemVersion(result.Content);
                //系统账户跳过低版本检测
                if (UserClient.UserAccount.UserName != "admin")
                {
                    if (UserClient.CurrentVersion != sv)
                    {
                        //保存新版本信息
                        UserClient.JsonSettings.IsNewVersionRunning = true;
                        UserClient.JsonSettings.SaveSettings();
                        //和当前系统版本号不一致，启动更新
                        if (IsHandleCreated) Invoke(start_update);
                        return;
                    }
                }
                else
                {
                    if (UserClient.CurrentVersion < sv)
                    {
                        //保存新版本信息
                        UserClient.JsonSettings.IsNewVersionRunning = true;
                        UserClient.JsonSettings.SaveSettings();
                        //和当前系统版本号不一致，启动更新
                        if (IsHandleCreated) Invoke(start_update);
                        return;
                    }
                }
            }
            else
            {
                //访问失败
                if (IsHandleCreated) Invoke(message_show, result.Message);
                if (IsHandleCreated) Invoke(thread_finish);
                return;
            }


            //================================================================================
            //验证结束后，根据需要是否下载服务器的数据，或是等到进入主窗口下载也可以
            //如果有参数决定主窗口的显示方式，那么必要在下面向服务器请求数据
            //以下展示了初始化参数的功能
            if (IsHandleCreated) Invoke(message_show, "正在下载参数...");
            else return;

            //延时
            Thread.Sleep(200);


            result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.参数下载);
            if(result.IsSuccess)
            {
                //服务器返回初始化的数据，此处进行数据的提取，有可能包含了多个数据
                json = Newtonsoft.Json.Linq.JObject.Parse(result.Content);
                //例如公告数据
                UserClient.Announcement = BasicFramework.SoftBasic.GetValueFromJsonObject(json, nameof(UserClient.Announcement), "");

            }
            else
            {
                //访问失败
                if (IsHandleCreated) Invoke(message_show, result.Message);
                if (IsHandleCreated) Invoke(thread_finish);
                return;
            }

            //启动主窗口
            if (IsHandleCreated) Invoke(new Action(() =>
            {
                DialogResult = DialogResult.OK;
                return;
            }));
        }


        #endregion


    }
}
