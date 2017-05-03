using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonLibrary
{
    /// <summary>
    /// 一个简单的账户管理窗口类
    /// </summary>
    public partial class FormAccountManage : Form
    {
        /// <summary>
        /// 实例化一个简单的账户管理
        /// </summary>
        /// <param name="setAccounts">设置账户信息的方法</param>
        /// <param name="getAccounts">获取账户信息的方法</param>
        public FormAccountManage(Func<string> getAccounts,Func<string,bool> setAccounts)
        {
            InitializeComponent();
            GetAccounts = getAccounts;
            SetAccounts = setAccounts;
        }

        private Func<string> GetAccounts = null;
        private Func<string, bool> SetAccounts = null;

        private void FormAccountManage_Load(object sender, EventArgs e)
        {

        }

        private void userButton1_Click(object sender, EventArgs e)
        {
            textBox1.Text = GetAccounts();
        }

        private void userButton2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                SetAccounts(textBox1.Text);
                MessageBox.Show("账户更新成功");
            }
        }

        private void FormAccountManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            GetAccounts = null;
            SetAccounts = null;
        }
    }
}
