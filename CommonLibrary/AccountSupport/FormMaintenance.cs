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
    /// 维护变更的窗口，用来维护服务器的状态，有可能处于维护中
    /// </summary>
    public partial class FormMaintenance : Form
    {
        /// <summary>
        /// 实例化一个窗口对象，用来更改系统的维护状态，然后进行保存
        /// </summary>
        /// <param name="settings">服务器的参数对象</param>
        public FormMaintenance(ServerSettings settings)
        {
            InitializeComponent();
            Settings = settings;
        }

        private void userButton1_Click(object sender, EventArgs e)
        {
            if (!userButton1.Selected)
            {
                userButton1.Selected = true;
                userButton2.Selected = false;
            }
        }

        private void userButton2_Click(object sender, EventArgs e)
        {
            if (!userButton2.Selected)
            {
                userButton1.Selected = false;
                userButton2.Selected = true;
            }
        }

        private ServerSettings Settings = null;

        private void FormMaintenance_Shown(object sender, EventArgs e)
        {
            if (Settings.Can_Account_Login)
            {
                userButton1.Selected = true;
            }
            else
            {
                userButton2.Selected = true;
            }
            textBox1.Text = Settings.Account_Forbidden_Reason;
        }

        private void userButton3_Click(object sender, EventArgs e)
        {
            if (userButton1.Selected)
            {
                Settings.Can_Account_Login = true;
            }
            else
            {
                Settings.Can_Account_Login = false;
            }

            Settings.Account_Forbidden_Reason = textBox1.Text;
            Settings.SaveToFile();
            MessageBox.Show("保存成功");
            Close();
        }
    }
}
