using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace CommonLibrary
{
    public partial class FormSuper : Form
    {
        #region Constructor
        
        public FormSuper(Func<int[]> getDataFunction)
        {
            InitializeComponent();
            GetDataFunction = getDataFunction;
        }

        #endregion

        #region Form Load Show Close
        
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


        #endregion

        #region Time Tick


        private void Timer_Tick(object sender, EventArgs e)
        {
            if (GetDataFunction != null)
            {
                int[] data = GetDataFunction();
                if (data != null)
                {
                    if (IsWindowShow)
                    {
                        if (pictureBox1.Width > 10)
                        {
                            pictureBox1.Image?.Dispose();
                            pictureBox1.Image = HslCommunication.BasicFramework.SoftPainting.GetGraphicFromArray(data, pictureBox1.Width - 2, pictureBox1.Height - 2, 7, Color.Blue);
                        }
                    }
                }
                DataTemp = data;
            }
        }


        #endregion

        #region Button Click


        private void userButton4_Click(object sender, EventArgs e)
        {
            if (DataTemp != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                for (int i = 0; i < DataTemp.Length; i++)
                {
                    sb.Append(DataTemp[i].ToString() + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                MessageBox.Show(sb.ToString());
            }
        }

        #endregion

        #region Private Member

        private Func<int[]> GetDataFunction = null;          // 获取数据的委托
        private int[] DataTemp = null;                       // 内存数组，真实数据
        private Timer timer = null;                          // 定时器
        private bool IsWindowShow = false;                   // 窗体是否显示

        #endregion
    }
}
