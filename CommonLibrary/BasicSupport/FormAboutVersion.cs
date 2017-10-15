using HslCommunication.BasicFramework;
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
    /// 关于版本的信息类
    /// </summary>
    public partial class FormAboutVersion : Form
    {
        /// <summary>
        /// 实例化一个版本信息说明的窗口对象
        /// </summary>
        /// <param name="sv">系统的版本号</param>
        public FormAboutVersion(SystemVersion sv)
        {
            InitializeComponent();
            m_SystemVersion = sv;
            Icon = UserSystem.GetFormWindowIcon();
        }

        private SystemVersion m_SystemVersion = null;
        private void FormAboutVersion_Load(object sender, EventArgs e)
        {
            Text = "版本号说明";

            label1.Text = "版本：V" + m_SystemVersion.ToString();
            label2.Text = m_SystemVersion.MainVersion.ToString();
            label3.Text = m_SystemVersion.SecondaryVersion.ToString();
            label4.Text = m_SystemVersion.EditVersion.ToString();

            label5.Text = "主版本，架构升级或是界面大更新。";
            label6.Text = "次版本，界面小更新或是新功能增加。";
            label7.Text = "修订版，性能优化或是BUG修复。";
        }

        private void FormAboutVersion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
