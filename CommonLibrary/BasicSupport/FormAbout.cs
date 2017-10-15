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
    /// 系统的关于类
    /// </summary>
    public partial class FormAbout : Form
    {
        #region Constructor


        /// <summary>
        /// 实例化一个关于系统的窗口对象
        /// </summary>
        /// <param name="softName">应用程序的名称</param>
        /// <param name="sv">系统的版本</param>
        /// <param name="yearStart">应用起始年份</param>
        /// <param name="belongName">本系统的版权归属人</param>
        public FormAbout(string softName, SystemVersion sv, int yearStart, string belongName)
        {
            InitializeComponent();
            SoftName = softName;
            SV = sv;
            YearStart = yearStart;
            BelongName = belongName;
            Icon = UserSystem.GetFormWindowIcon();
        }

        /// <summary>
        /// 实例化一个关于系统的窗口对象
        /// </summary>
        /// <param name="sv">系统的版本</param>
        /// <param name="yearStart">版权起始年份</param>
        /// <param name="belongName">本系统的版权归属人</param>
        public FormAbout(SystemVersion sv, int yearStart, string belongName)
        {
            InitializeComponent();
            SoftName = Application.ProductName;
            SV = sv;
            YearStart = yearStart;
            BelongName = belongName;
        }

        #endregion

        #region Quick Close


        private void FormAbout_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        #endregion

        #region Window Load
        
        private void FormAbout_Load(object sender, EventArgs e)
        {
            Text = "关于系统";
            label5.Text = "框架版本：" + SoftBasic.FrameworkVersion.ToString();

            label1.Text = SoftName;
            label2.Text = "V" + SV.ToString();
            if (DateTime.Now.Year > YearStart)
            {
                label3.Text = $"(C) {YearStart}-{DateTime.Now.Year} {BelongName} 保留所有权利";
            }
            else
            {
                label3.Text = $"(C) {YearStart} {BelongName} 保留所有权利";
            }
        }

        #endregion
        

        #region Private Members

        private string SoftName = string.Empty;
        private SystemVersion SV = null;
        private int YearStart = 2017;
        private string BelongName = "Richard.Hu";

        #endregion

    }
}
