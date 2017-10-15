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
    /// 密码检查的窗口
    /// </summary>
    public partial class FormPasswordCheck : Form
    {

        /// <summary>
        /// 实例化一个检查输入密码的窗口
        /// </summary>
        /// <param name="pass">需要配对的密码</param>
        public FormPasswordCheck(string pass)
        {
            InitializeComponent();
            password = pass;
            Icon = UserSystem.GetFormWindowIcon();
        }

        private void userButton1_Click(object sender, EventArgs e)
        {
            if (password == textBox2.Text)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("密码错误！");
            }
        }


        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                userButton1.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void FormPasswordCheck_Load(object sender, EventArgs e)
        {

        }


        private string password = "";
    }
}
