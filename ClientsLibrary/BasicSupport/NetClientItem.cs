using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonLibrary;
using System.Threading;

namespace ClientsLibrary.BasicSupport
{
    public partial class NetClientItem : UserControl
    {
        public NetClientItem()
        {
            InitializeComponent();
        }

        private void NetClientItem_Paint(object sender, PaintEventArgs e)
        {
            if (m_NetAccount != null)
            {
                // 绘制角色名称
                if (m_NetAccount.Roles?.Length > 0)
                {
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    int m_x = 45;
                    int m_y = 22;

                    for (int i = 0; i < m_NetAccount.Roles.Length; i++)
                    {
                        SizeF m_size = e.Graphics.MeasureString(m_NetAccount.Roles[i], Font);
                        e.Graphics.FillRectangle(Brushes.LightBlue, m_x, m_y - 2, m_size.Width, m_size.Height);
                        e.Graphics.DrawString(m_NetAccount.Roles[i], Font, Brushes.Blue, new Point(m_x, m_y));
                        m_x += (int)m_size.Width + 3;
                    }
                }
            }
        }


        public void SetNetAccount(NetAccount account)
        {
            m_NetAccount = account;
            // 加载头像，显示信息
            label1.Text = (string.IsNullOrEmpty(account.Alias) ? account.UserName : account.Alias) + $" ({account.Factory})";

            ThreadPool.QueueUserWorkItem(ThreadPoolLoadPortrait, null);
        }

        /// <summary>
        /// 当前网络会话的唯一ID
        /// </summary>
        public string UniqueId
        {
            get
            {
                return m_NetAccount == null ? string.Empty : m_NetAccount.UniqueId;
            }
        }

        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="userName"></param>
        public void UpdatePortrait(string userName)
        {
            if (m_NetAccount?.UserName == userName)
            {
                ThreadPool.QueueUserWorkItem(ThreadPoolLoadPortrait, null);
            }
        }

        private void ThreadPoolLoadPortrait(object obj)
        {
            // 向服务器请求小头像
            if (m_NetAccount != null)
            {
                Bitmap bitmap = UserClient.PortraitManager.GetSmallPortraitByUserName(m_NetAccount.UserName);

                if(IsHandleCreated)
                {
                    Invoke(new Action(() =>
                    {
                        pictureBox1.Image = bitmap;
                    }));
                }
            }
        }


        private NetAccount m_NetAccount;

        private void NetClientItem_MouseEnter(object sender, EventArgs e)
        {
        }

        private void NetClientItem_MouseLeave(object sender, EventArgs e)
        {
        }

        private void NetClientItem_Load( object sender, EventArgs e )
        {

        }
    }
}
