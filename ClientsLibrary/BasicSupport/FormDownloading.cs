using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication;
using CommonLibrary;

namespace ClientsLibrary
{
    //=============================================================================
    //
    //    时间：2017-03-08 12:41:37
    //    用于下载数据的提示窗口
    //
    //=============================================================================




    public partial class FormDownloading : Form
    {
        public FormDownloading(int customer,Action<OperateResult<string>> action)
        {
            InitializeComponent();

            Icon = UserSystem.GetFormWindowIcon();

            net_cmd = customer;
            DealWithResult = action;
            DoubleBuffered = true;
        }
        

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

        Pen pen_dash = new Pen(Color.Green);
        float Pen_Offect = 0;

        private void FormDownloading_Load(object sender, EventArgs e)
        {
            pen_dash.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            pen_dash.DashPattern = new float[] { 5, 5 };
            pen_dash.DashOffset = 0;

            time.Interval = 38;//2017-03-08 13:20:33
            time.Tick += Time_Tick;

            label1.Text = "正在请求数据...";
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            Pen_Offect--;
            if (Pen_Offect < -5) Pen_Offect = 4;
            pen_dash.DashOffset = Pen_Offect;
            Invalidate();//引发重画
        }


        //定时块
        private Timer time = new Timer();

        private void FormDownloading_Shown(object sender, EventArgs e)
        {
            time.Start();
            System.Threading.Thread thread = new System.Threading.Thread(ThreadRequestServer);
            thread.IsBackground = true;
            thread.Start();
        }

        
        private int net_cmd = 0;
        private Action<OperateResult<string>> DealWithResult = null;

        private void ThreadRequestServer()
        {
            //后台请求数据
            System.Threading.Thread.Sleep(100);
            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(net_cmd);
            Invoke(new Action(() =>
            {
                DealWithResult(result);
                time.Stop();
                System.Threading.Thread.Sleep(20);
                Dispose();
            }));
        }
           
    }
}
