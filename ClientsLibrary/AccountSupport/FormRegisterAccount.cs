using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication.Enthernet;
using HslCommunication;
using CommonLibrary;

namespace ClientsLibrary
{
    public partial class FormRegisterAccount : Form
    {
        #region Constructor
        
        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="factories"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public FormRegisterAccount(string[] factories)
        {
            InitializeComponent();
            Icon = UserSystem.GetFormWindowIcon();

            net_client = UserClient.Net_simplify_client;
            Factories = new List<string>(factories);


            // 根据自身需求是否添加总公司名称
            Factories.Add("总公司");
        }

        #endregion

        #region Form Load

        private void FormRegisterAccount_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = AccountGrade.GetDescription();
            comboBox2.DataSource = new string[] { "允许", "不允许" };

            comboBox1.SelectedItem = AccountGrade.GetDescription(AccountGrade.Technology);
            comboBox2.SelectedItem = "允许";

            comboBox_factory.DataSource = Factories.ToArray();

            textBox4.Text = (new UserAccount()).ForbidMessage;

            UILocalization();
        }

        #endregion

        #region Localization Support

        /// <summary>
        /// 本地化显示的操作，还未完成
        /// </summary>
        private void UILocalization()
        {
            Text = UserLocalization.Localization.AccountRegisterTitle;

            label1.Text = UserLocalization.Localization.AccountName + "：";
            label7.Text = UserLocalization.Localization.AccountAlias + "：";
            label2.Text = UserLocalization.Localization.AccountPassword + "：";
            label3.Text = UserLocalization.Localization.AccountFactory + "：";
            label4.Text = UserLocalization.Localization.AccountGrade + "：";
            label5.Text = UserLocalization.Localization.AccountLoginEnable + "：";
            label6.Text = UserLocalization.Localization.AccountForbidMessage + "：";

        }


        #endregion

        #region Register Account


        private void userButton_login_Click(object sender, EventArgs e)
        {
            // 点击了注册，先获取数据
            UserAccount account = new UserAccount();
            account.UserName = textBox1.Text;
            account.NameAlias = textBox3.Text;
            account.Password = textBox2.Text;
            account.Factory = comboBox_factory.SelectedItem.ToString();

            switch (comboBox1.SelectedIndex)
            {
                case 0: account.Grade = AccountGrade.SuperAdministrator; break;
                case 1: account.Grade = AccountGrade.Admin; break;
                case 2: account.Grade = AccountGrade.Technology; break;
                default: account.Grade = AccountGrade.General; break;
            }

            account.LoginEnable = comboBox2.SelectedItem.ToString() == "允许";
            account.ForbidMessage = textBox4.Text;
            account.Phone = textBox5.Text;
            account.EMail = textBox6.Text;


            OperateResult<string> result = net_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.注册账号, account.ToJsonString());
            if (result.IsSuccess && result.Content == "1")
            {
                MessageBox.Show("注册成功！");
            }
            else
            {
                MessageBox.Show("注册失败！");
            }
        }

        #endregion

        #region Private Members

        private List<string> Factories = null;              // 分厂信息
        private NetSimplifyClient net_client = null;        // 服务器数据交换的通信对象

        #endregion
    }
}
