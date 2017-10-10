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
            for (int i = MyControls.Count - 1; i >= 0; i--)
            {
                MyControls[i].Dispose();
                MyControls.RemoveAt(i);
            }
        }


        public void ClientOnline(NetAccount account)
        {
            if (account != null)
            {
                AddControl(account);

                label2.Text = MyControls.Count.ToString();
            }
        }

        public void ClientUpdatePortrait(string userName)
        {
            for (int i = 0; i < MyControls.Count; i++)
            {
                MyControls[i].UpdatePortrait(userName);
            }
        }

        public void ClinetOffline(string userName)
        {
            int index = -1;
            for (int i = 0; i < MyControls.Count; i++)
            {
                if (MyControls[i].UniqueId == userName)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            {
                MyControls[index].Dispose();
                MyControls.RemoveAt(index);

                // 重新计算偏移

                Location_Y = 0 - VerticalScroll.Value;
                for (int i = 0; i < MyControls.Count; i++)
                {
                    MyControls[i].Location = new Point(2, Location_Y);
                    Location_Y += MyControls[i].Height + 4;      
                }
            }

            label2.Text = MyControls.Count.ToString();
        }

        private void AddControl(NetAccount account)
        {
            NetClientItem item = new NetClientItem();
            panel1.Controls.Add(item);
            // 添加显示
            item.SetNetAccount(account);
            item.Location = new Point(2, Location_Y);                                 // 控件的位置
            int width = VerticalScroll.Visible ? Width - 4 - SystemInformation.VerticalScrollBarWidth : Width - 4; // 控件的宽度
            item.Size = new Size(width, item.Size.Height);                            // 控件的大小
            item.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;  // 控件随窗口变化的样式

            Location_Y += item.Height + 4;                                            // 位置偏移
            MyControls.Add(item);
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
                    int Location_Y = 0 - VerticalScroll.Value;
                    //添加子控件
                    foreach (var m in accounts)
                    {
                        AddControl(m);
                    }
                }
            }

            ResumeLayout();
        }

        private int Location_Y = 0;

        private List<NetClientItem> MyControls = new List<NetClientItem>();

        private void NetClientOnline_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
