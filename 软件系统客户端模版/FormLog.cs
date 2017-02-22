using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndustryEthernet;
using CommonLibrary;

namespace 软件系统客户端模版
{
    public partial class FormLog : Form
    {
        public FormLog(Net_Simplify_Client client)
        {
            InitializeComponent();
            net_simplify_client = client;
        }

        private Net_Simplify_Client net_simplify_client = null;

        private void FormLog_Load(object sender, EventArgs e)
        {

        }

        private void ReadFromServer(string head_code)
        {
            OperateResultString result = net_simplify_client.ReadFromServer(head_code);
            if (result.IsSuccess) textBox1.Text = result.Content;
            else textBox1.Text = result.ToMessageShowString();
        }
        private void ClearFromServer(string head_code)
        {
            OperateResultString result = net_simplify_client.ReadFromServer(head_code);
            if (result.IsSuccess) textBox1.Text = "清除成功";
            else textBox1.Text = result.ToMessageShowString();
        }

        private void userButton_login_Click(object sender, EventArgs e)
        {
            ReadFromServer(CommonHeadCode.SimplifyHeadCode.网络日志查看);
        }

        private void userButton1_Click(object sender, EventArgs e)
        {
            ClearFromServer(CommonHeadCode.SimplifyHeadCode.网络日志清空);
        }

        private void userButton2_Click(object sender, EventArgs e)
        {
            ReadFromServer(CommonHeadCode.SimplifyHeadCode.同步日志查看);
        }

        private void userButton3_Click(object sender, EventArgs e)
        {
            ClearFromServer(CommonHeadCode.SimplifyHeadCode.同步日志清空);
        }

        private void userButton4_Click(object sender, EventArgs e)
        {
            ReadFromServer(CommonHeadCode.SimplifyHeadCode.更新日志查看);
        }

        private void userButton5_Click(object sender, EventArgs e)
        {
            ClearFromServer(CommonHeadCode.SimplifyHeadCode.更新日志清空);
        }
    }
}
