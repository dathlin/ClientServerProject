using ClientsLibrary;
using CommonLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 软件系统客户端Wpf.Views.Controls
{
    /// <summary>
    /// UserClientRenderItem.xaml 的交互逻辑
    /// </summary>
    public partial class UserClientRenderItem : UserControl
    {
        #region Constructor
        
        public UserClientRenderItem()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Property

        /// <summary>
        /// 当前会话的唯一ID
        /// </summary>
        public string UniqueId
        {
            get
            {
                return netAccount == null ? string.Empty : netAccount.UniqueId;
            }
        }

        #endregion

        #region Show Client Information

        public void SetClientRender(NetAccount account)
        {
            if (account != null)
            {
                netAccount = account;
                UserName.Text = string.IsNullOrEmpty(account.Alias) ? account.UserName : account.Alias;
                Factory.Text = $"({account.Factory})";

                Roles.Children.Clear();

                if (account.Roles?.Length > 0)
                {
                    foreach (var m in account.Roles)
                    {
                        TextBlock block = new TextBlock
                        {
                            Background = Brushes.LightSkyBlue,
                            Foreground = Brushes.Blue,
                            Margin = new Thickness(0, 0, 4, 0),
                            Text = m
                        };
                        Roles.Children.Add(block);
                    }
                }
                else
                {
                    Roles.Children.Add(new TextBlock());
                }

                // 启动线程池去显示头像
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ThreadPoolLoadPortrait), account);
            }
        }

        #endregion

        #region Update Portrait

        public void UpdatePortrait(string userName)
        {
            if(netAccount?.UserName == userName)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ThreadPoolLoadPortrait), netAccount);
            }
        }

        private void ThreadPoolLoadPortrait(object obj)
        {
            // 向服务器请求小头像
            if (obj is NetAccount m_NetAccount)
            {
                System.Drawing.Bitmap bitmap = UserClient.PortraitManager.GetSmallPortraitByUserName(m_NetAccount.UserName);
                Dispatcher.Invoke(new Action(() =>
                {
                    Image1.Source = AppWpfHelper.TranslateImageToBitmapImage(bitmap);
                }));
            }
        }

        #endregion

        #region Pricvate Member

        private NetAccount netAccount = null;

        #endregion
    }
}
