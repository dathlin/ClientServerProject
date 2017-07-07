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
        public FormSuper(Func<int[]> getDataFunction)
        {
            InitializeComponent();
            GetDataFunction = getDataFunction;
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

        private Func<int[]> GetDataFunction = null;

        private int[] DataTemp = null;

        private bool IsWindowShow { get; set; }

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

        private Timer timer = null;

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
    }
}
