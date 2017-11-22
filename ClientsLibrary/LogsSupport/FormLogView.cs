using CommonLibrary;
using HslCommunication;
using HslCommunication.Enthernet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClientsLibrary
{
    public partial class FormLogView : Form
    {
        #region Constructor
        
        public FormLogView()
        {
            InitializeComponent();

            Icon = UserSystem.GetFormWindowIcon();

            net_simplify_client = UserClient.Net_simplify_client;
        }

        #endregion

        #region Window Load
        
        private void FormLogView_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Private Member
        
        private NetSimplifyClient net_simplify_client = null;

        #endregion

        #region Button Click


        private void ReadFromServer(int head_code)
        {
            OperateResult<string> result = net_simplify_client.ReadFromServer(head_code);
            if (result.IsSuccess)
            {
                logNetAnalysisControl1.SetLogNetSource(result.Content);
            }
            else
            {
                MessageBox.Show(result.ToMessageShowString());
            }
        }

        private void ClearFromServer(int head_code)
        {
            OperateResult<string> result = net_simplify_client.ReadFromServer(head_code);
            if (result.IsSuccess) MessageBox.Show("清除成功");
            else MessageBox.Show(result.ToMessageShowString());
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

        private void userButton7_Click(object sender, EventArgs e)
        {
            ReadFromServer(CommonHeadCode.SimplifyHeadCode.运行日志查看);
        }

        private void userButton6_Click(object sender, EventArgs e)
        {
            ClearFromServer(CommonHeadCode.SimplifyHeadCode.运行日志清空);
        }
        private void userButton9_Click(object sender, EventArgs e)
        {
            ReadFromServer(CommonHeadCode.SimplifyHeadCode.文件日志查看);
        }

        private void userButton8_Click(object sender, EventArgs e)
        {
            ClearFromServer(CommonHeadCode.SimplifyHeadCode.文件日志清空);
        }

        private void userButton11_Click(object sender, EventArgs e)
        {
            ReadFromServer(CommonHeadCode.SimplifyHeadCode.反馈日志查看);
        }

        private void userButton10_Click(object sender, EventArgs e)
        {
            ClearFromServer(CommonHeadCode.SimplifyHeadCode.反馈日志清空);
        }
        private void userButton13_Click(object sender, EventArgs e)
        {
            ReadFromServer(CommonHeadCode.SimplifyHeadCode.UDP日志查看);
        }

        private void userButton12_Click(object sender, EventArgs e)
        {
            ClearFromServer(CommonHeadCode.SimplifyHeadCode.UDP日志清空);
        }


        private void userButton14_Click(object sender, EventArgs e)
        {
            ReadFromServer(CommonHeadCode.SimplifyHeadCode.客户端日志查看);
        }

        private void userButton15_Click(object sender, EventArgs e)
        {
            ClearFromServer(CommonHeadCode.SimplifyHeadCode.客户端日志清空);
        }

        #endregion

    }
}
