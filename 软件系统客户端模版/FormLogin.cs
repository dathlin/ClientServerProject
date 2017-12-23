using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json.Linq;
using HslCommunication;
using CommonLibrary;
using HslCommunication.BasicFramework;
using ClientsLibrary;

namespace 软件系统客户端模版
{

    /*******************************************************************************************************************
     * 
     *     登录窗口，此处实现了一些常用的代码，具体的版本号验证及账户验证需要根据实际需求实现
     *     
     *     本界面支持后台验证版本，账户，密码记录，七天未登录记住的密码状态清除
     * 
     ****************************************************************************************************************/


    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            Icon = UserSystem.GetFormWindowIcon();
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
            UserClient.JsonSettings.LoadByFile();//根据实际实际情况选中解密算法，默认采用DES加密解密算法

            label_version.Text = "版本：" + UserClient.CurrentVersion.ToString();
            label2.Text = CommonLibrary.SoftResources.StringResouce.SoftName;
            label_copyright.Text = $"本软件著作权归{CommonLibrary.SoftResources.StringResouce.SoftCopyRight}所有";
        }

        private void FormLogin_Shown(object sender, EventArgs e)
        {
            IsWindowShow = true;

            //如果七天未登录，账户密码清除
            if ((DateTime.Now - UserClient.JsonSettings.LoginTime).TotalDays < UserClient.JsonSettings.PasswordOverdueDays)
            {
                //加载数据
                textBox_userName.Text = UserClient.JsonSettings.LoginName ?? "";
                textBox_password.Text = UserClient.JsonSettings.Password ?? "";
                checkBox_remeber.Checked = UserClient.JsonSettings.Password != "";
            }
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
            // 定义委托

            // 消息显示委托
            Action<string> message_show = delegate (string message)
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    label_status.Text = message;
                }));
            };
            // 启动更新委托
            Action start_update = delegate
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    // 需要该exe支持，否则将无法是实现自动版本控制
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
                }));
            };
            // 结束的控件使能委托
            Action thread_finish = delegate
            {
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    UISettings(true);
                }));
            };

            // 启动密码验证
            if(AccountLogin.AccountLoginServer(
                message_show,
                start_update,
                thread_finish,
                textBox_userName.Text,
                textBox_password.Text,
                checkBox_remeber.Checked,
                "winform"))
            {
                // 启动主窗口
                if (IsHandleCreated) Invoke(new Action(() =>
                {
                    DialogResult = DialogResult.OK;
                    return;
                }));
            }
        }



        #endregion

        #region 显示本机ID

        private void label_version_Click(object sender, EventArgs e)
        {
            using (FormShowMachineId form = new FormShowMachineId())
            {
                form.ShowDialog();
            }
        }
        #endregion

    }
}
