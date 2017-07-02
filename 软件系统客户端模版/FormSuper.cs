using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 软件系统客户端模版
{
    public partial class FormSuper : Form
    {
        public FormSuper()
        {
            InitializeComponent();
        }

        private void FormSuper_Load(object sender, EventArgs e)
        {

        }

        private void FormSuper_Shown(object sender, EventArgs e)
        {
            IsWindowShow = true;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        

        private void FormSuper_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsWindowShow = false;
        }



        private bool IsWindowShow { get; set; }

        private void Timer_Tick(object sender, EventArgs e)
        {
            HslCommunication.OperateResultBytes result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.性能计数, new byte[0]);
            //解析
            if (result.IsSuccess)
            {
                int[] data = new int[result.Content.Length / 4];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = BitConverter.ToInt32(result.Content, i * 4);
                }
                if (IsWindowShow)
                {
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = HslCommunication.BasicFramework.SoftPainting.GetGraphicFromArray(data, pictureBox1.Width - 2, pictureBox1.Height - 2, 7, Color.Blue);
                }
            }
        }

        private Timer timer = null;
    }
}
