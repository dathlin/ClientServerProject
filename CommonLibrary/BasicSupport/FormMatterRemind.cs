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
    //=============================================================================
    //
    //    时间：2017-03-08 12:41:37
    //    用于耗时操作的提醒
    //
    //=============================================================================
 

    /// <summary>
    /// 用于稍微耗时事件处理时的消息框提醒
    /// </summary>
    public partial class FormMatterRemind : Form
    {
        #region Constructor


        /// <summary>
        /// 实例化一个耗时处理的对象
        /// </summary>
        /// <param name="description">需要显示的文本描述</param>
        /// <param name="action">需要操作的方法</param>
        public FormMatterRemind(
            string description,
            Action action
            )
        {
            InitializeComponent();
            Description = description;
            DealWithResult = action;
            DoubleBuffered = true;
            Icon = UserSystem.GetFormWindowIcon();
        }


        #endregion

        #region Paint Support


        private void FormDownloading_Paint(object sender, PaintEventArgs e)
        {
            //绘制显示

            e.Graphics.DrawLines(pen_dash, new Point[]
            {
                new Point(44,44),
                new Point(14,44),
                new Point(14,14),
                new Point(44,14),
                new Point(44,44),
            });
            
            //画边框
            e.Graphics.DrawRectangle(Pens.LightGray, new Rectangle(0, 0, Width - 1, Height - 1));
        }

        #endregion

        #region Form Load Show
        
        private void FormDownloading_Load(object sender, EventArgs e)
        {
            pen_dash.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            pen_dash.DashPattern = new float[] { 5, 5 };
            pen_dash.DashOffset = 0;

            time.Interval = 38;//2017-03-08 13:20:33
            time.Tick += Time_Tick;

            label1.Text = Description;
        }

        private void FormDownloading_Shown(object sender, EventArgs e)
        {
            time.Start();
            System.Threading.Thread thread = new System.Threading.Thread(ThreadRequestServer);
            thread.IsBackground = true;
            thread.Start();
        }

        #endregion

        #region Time Tick

        private void ThreadRequestServer()
        {
            //后台请求数据
            System.Threading.Thread.Sleep(100);
            DealWithResult();
            Invoke(new Action(() =>
            {
                time.Stop();
                System.Threading.Thread.Sleep(20);
                Dispose();
            }));
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            Pen_Offect--;
            if (Pen_Offect < -5) Pen_Offect = 4;
            pen_dash.DashOffset = Pen_Offect;
            Invalidate();//引发重画
        }

        #endregion

        #region Private Members


        //定时块
        private Timer time = new Timer();
        private Pen pen_dash = new Pen(Color.Green);
        private float Pen_Offect = 0;
        private Action DealWithResult = null;
        private string Description = string.Empty;


        #endregion
    }
}
