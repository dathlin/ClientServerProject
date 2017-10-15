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
    /// 延时指定时间的窗口类
    /// </summary>
    public partial class FormWaitInfomation : Form
    {
        #region Constructor


        /// <summary>
        /// 实例化一个等待指定显示时间的窗口并进行一些相关的操作，操作完成或是延时结束即退出
        /// </summary>
        /// <param name="info">显示的文本</param>
        /// <param name="waitTime">等待时间，单位毫秒，最低为100毫秒</param>
        /// <param name="back">文本的背景色</param>
        /// <param name="fore">文本的前景色</param>
        public FormWaitInfomation(string info, int waitTime, Color back, Color fore, Action action = null)
        {
            InitializeComponent();

            textShow = info;
            WaitClose = waitTime;
            ColorBack = back;
            ColorFont = fore;
            operater = action;
        }
        /// <summary>
        /// 实例化一个等待指定显示时间的窗口，时间一到，自动退出
        /// </summary>
        /// <param name="info">显示的文本</param>
        /// <param name="waitTime">等待时间，单位毫秒</param>
        public FormWaitInfomation(string info, int waitTime, Action action = null)
        {
            InitializeComponent();

            textShow = info;
            WaitClose = waitTime;
            operater = action;
        }

        #endregion

        private void FormWaitInfomation_Load(object sender, EventArgs e)
        {
            label1.BackColor = ColorBack;
            label1.ForeColor = ColorFont;
            label1.Text = textShow;

            if (WaitClose < 100) WaitClose = 100;
        }


        private void FormWaitInfomation_Shown(object sender, EventArgs e)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ThreadPoolWait), null);
        }
        

        private void ThreadPoolWait(object obj)
        {
            DateTime start = DateTime.Now;
            operater?.Invoke();

            double timeSpan = (DateTime.Now - start).TotalMilliseconds;
            while (timeSpan < WaitClose)
            {
                timeSpan = (DateTime.Now - start).TotalMilliseconds;
                System.Threading.Thread.Sleep(100);
            }

            if (IsHandleCreated) Invoke(new Action(() =>
            {
                Close();
            }));
        }

        #region Private Members

        private bool isActionComplete = false;                               // 操作是否完成
        private string textShow = "正在退出系统...";                         // 文本消息提示
        private int WaitClose = 1200;                                        // 等待时间
        private Action operater = null;                                      // 操作
        private Color ColorBack = Color.FromArgb(255, 255, 192);             // 背景颜色
        private Color ColorFont = Color.FromArgb(192, 0, 192);               // 字体颜色

        #endregion
    }
}
