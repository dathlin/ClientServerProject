using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndustryEthernet;

namespace 软件系统客户端模版
{
    public partial class FormRegisterAccount : Form
    {
        public FormRegisterAccount(Net_Simplify_Client client)
        {
            InitializeComponent();

            net_client = client;
        }
        
        private void FormRegisterAccount_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = BasicFramework.AccountGrade.GetDescription();
            comboBox2.DataSource = new string[] { "允许", "不允许" };

            comboBox1.SelectedItem = BasicFramework.AccountGrade.GetDescription(BasicFramework.AccountGrade.Technology);
            comboBox2.SelectedItem = "允许";

            textBox4.Text = (new BasicFramework.UserAccount()).ForbidMessage;
        }

        private Net_Simplify_Client net_client = null;

        private void userButton_login_Click(object sender, EventArgs e)
        {
            //点击了注册，先获取数据
            BasicFramework.UserAccount account = new BasicFramework.UserAccount();
            account.UserName = textBox1.Text;
            account.Password = textBox2.Text;
            switch (comboBox1.SelectedIndex)
            {
                case 0: account.Grade = BasicFramework.AccountGrade.SuperAdministrator; break;
                case 1: account.Grade = BasicFramework.AccountGrade.Admin; break;
                case 2: account.Grade = BasicFramework.AccountGrade.Technology; break;
                default: account.Grade = BasicFramework.AccountGrade.General; break;
            }
            account.LoginEnable = comboBox2.SelectedItem.ToString() == "允许";
            account.ForbidMessage = textBox4.Text;

            OperateResultString result = net_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.注册账号 + account.ToJsonString());
            if (result.IsSuccess && result.Content == "1")
            {
                MessageBox.Show("注册成功！");
            }
            else
            {
                MessageBox.Show("注册失败！");
            }
        }
    }
}
