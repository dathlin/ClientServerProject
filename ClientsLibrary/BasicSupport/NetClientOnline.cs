using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonLibrary;

namespace ClientsLibrary.BasicSupport
{
    public partial class NetClientOnline : UserControl
    {
        public NetClientOnline()
        {
            InitializeComponent();
        }

        private void NetClientOnline_Load(object sender, EventArgs e)
        {

        }
        private void ClearControls()
        {
            while (MyControls.Count > 0)
            {
                MyControls.Pop().Dispose();
            }
        }

        public void SetOnlineRender(NetAccount[] accounts)
        {
            SuspendLayout();
            //清楚缓存
            ClearControls();

            if (accounts != null)
            {
                label2.Text = accounts.Length.ToString();

                if (accounts.Length > 0 && Width > 20)
                {
                    int location_y = 25 - VerticalScroll.Value;
                    //添加子控件
                    foreach (var m in accounts)
                    {
                        NetClientItem item = new NetClientItem();
                        Controls.Add(item);
                        // 添加显示
                        item.SetNetAccount(m);
                        item.Location = new Point(2, location_y);                                 // 控件的位置
                        int width = VerticalScroll.Visible ? Width - 4 - SystemInformation.VerticalScrollBarWidth : Width - 4; // 控件的宽度
                        item.Size = new Size(width, item.Size.Height);                            // 控件的大小
                        item.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;  // 控件随窗口变化的样式

                        location_y += item.Height + 4;                                            // 位置偏移
                        MyControls.Push(item);                                                 // 控件压入堆栈
                    }
                }
            }

            ResumeLayout();
        }

        private Stack<IDisposable> MyControls = new Stack<IDisposable>();

        private void NetClientOnline_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
