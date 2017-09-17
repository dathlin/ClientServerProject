using CommonLibrary;
using HslCommunication.Enthernet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClientsLibrary.AccountSupport
{
    public partial class AccountDetails : Form
    {


        public AccountDetails(AdvancedFileClient fileClient, UserPortrait userPortrait)
        {
            InitializeComponent();
            FileClient = fileClient;
            UserPortrait = userPortrait;
        }

        private void AccountDetails_Shown(object sender, EventArgs e)
        {
            // 加载各种数据
            textBox_Factory.Text = UserClient.UserAccount.Factory;
            textBox_GradeDescription.Text = AccountGrade.GetDescription(UserClient.UserAccount.Grade);
            textBox_LastLoginIpAddress.Text = UserClient.UserAccount.LastLoginIpAddress;
            textBox_LastLoginTime.Text = UserClient.UserAccount.LastLoginTime.ToString();
            textBox_LastLoginWay.Text = UserClient.UserAccount.LastLoginWay;
            textBox_LoginEnable.Text = UserClient.UserAccount.LoginEnable ? "允许" : "禁止";
            textBox_LoginFailedCount.Text = UserClient.UserAccount.LoginFailedCount.ToString();
            textBox_LoginFrequency.Text = UserClient.UserAccount.LoginFrequency.ToString();
            textBox_NameAlias.Text = UserClient.UserAccount.NameAlias;
            textBox_RegisterTime.Text = UserClient.UserAccount.RegisterTime.ToString();
            textBox_UserName.Text = UserClient.UserAccount.UserName;

            // 加载头像

        }

        private void AccountDetails_Load(object sender, EventArgs e)
        {
            Text = "账户详细信息";
        }


        public AdvancedFileClient FileClient { get; }
        public UserPortrait UserPortrait { get; }



    }
}
