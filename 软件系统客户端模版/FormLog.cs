using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonLibrary;
using System.Text.RegularExpressions;

using HslCommunication;
using HslCommunication.Enthernet;


namespace 软件系统客户端模版
{
    public partial class FormLog : Form
    {
        public FormLog()
        {
            InitializeComponent();
            net_simplify_client = UserClient.Net_simplify_client;
        }

        private Net_Simplify_Client net_simplify_client = null;

        private void FormLog_Load(object sender, EventArgs e)
        {

        }

        private void ReadFromServer(string head_code)
        {
            OperateResultString result = net_simplify_client.ReadFromServer(head_code);
            if (result.IsSuccess)
            {
                textBox1.Text = result.Content;
                LogTemp = result.Content;
            }
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
            ReadFromServer(CommonHeadCode.SimplifyHeadCode.共享文件日志查看);
        }

        private void userButton8_Click(object sender, EventArgs e)
        {
            ClearFromServer(CommonHeadCode.SimplifyHeadCode.共享文件日志清空);
        }

        private void userButton11_Click(object sender, EventArgs e)
        {
            ReadFromServer(CommonHeadCode.SimplifyHeadCode.建议反馈日志查看);
        }

        private void userButton10_Click(object sender, EventArgs e)
        {
            ClearFromServer(CommonHeadCode.SimplifyHeadCode.建议反馈日志清空);
        }
        private void userButton13_Click(object sender, EventArgs e)
        {
            ReadFromServer(CommonHeadCode.SimplifyHeadCode.UDP日志查看);
        }

        private void userButton12_Click(object sender, EventArgs e)
        {
            ClearFromServer(CommonHeadCode.SimplifyHeadCode.UDP日志清空);
        }


        /// <summary>
        /// 查询日志的缓存
        /// </summary>
        private string LogTemp = string.Empty;

        /// <summary>
        /// 筛选出符合需求的日志
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private string FilterString(string filter)
        {
            StringBuilder sb = new StringBuilder();
            MatchCollection mc = Regex.Matches(LogTemp, @"\[" + filter + @"[^\[]+");
            foreach (Match m in mc)
            {
                sb.Append(m.Value);
            }
            return sb.ToString();
        }

        private void userButton_log1_Click(object sender, EventArgs e)
        {
            textBox1.Text = FilterString(HslCommunication.BasicFramework.SoftLogHelper.Normal);
        }

        private void userButton_log2_Click(object sender, EventArgs e)
        {
            textBox1.Text = FilterString(HslCommunication.BasicFramework.SoftLogHelper.Information);
        }

        private void userButton_log3_Click(object sender, EventArgs e)
        {
            textBox1.Text = FilterString(HslCommunication.BasicFramework.SoftLogHelper.Warnning);
        }

        private void userButton_log4_Click(object sender, EventArgs e)
        {
            textBox1.Text = FilterString(HslCommunication.BasicFramework.SoftLogHelper.Error);
        }

        private void userButton_log_Click(object sender, EventArgs e)
        {
            textBox1.Text = LogTemp;
        }


    }
}
