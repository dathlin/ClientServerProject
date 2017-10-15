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
    /// 控制版本号的窗口类
    /// </summary>
    public partial class FormVersionControl : Form
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        public FormVersionControl(ServerSettings settings)
        {
            InitializeComponent();
            Settings = settings;
        }
        private ServerSettings Settings = null;

        private void FormVersionControl_Shown(object sender, EventArgs e)
        {
            textBox1.Text = Settings.SystemVersion.ToString();
            textBox2.Focus();
        }

        private void userButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Settings.SystemVersion = new SystemVersion(textBox2.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show("版本号格式错误：应为1.0.0" + Environment.NewLine +
                    "错误描述：" + ex.Message);
            }

            try
            {
                Settings.SaveToFile();
                MessageBox.Show("保存成功");
            }
            catch(Exception ex)
            {
                MessageBox.Show("数据保存失败" + Environment.NewLine +
                    "错误描述：" + ex.Message);
            }
        }

        private void FormVersionControl_Load(object sender, EventArgs e)
        {

        }
    }
}
