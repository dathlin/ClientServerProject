using CommonLibrary;
using HslCommunication;
using HslCommunication.BasicFramework;
using HslCommunication.Enthernet;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
using ClientsLibrary;
using System.Threading;
using 软件系统客户端Wpf.Views;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;
using HslCommunication.Core.Net;
using HslCommunication.LogNet;

namespace 软件系统客户端Wpf
{




    /***************************************************************************************
     * 
     *    模版日期    2017-09-24
     *    创建人      Richard Hu
     *    版权所有    Richard Hu
     *    授权说明    模版仅授权个人使用，如需商用，请联系hsl200909@163.com洽谈
     *    说明一      JSON组件引用自james newton-king，遵循MIT授权协议
     *    说明二      主题及各种主件来自:https://github.com/ButchersBoy/MaterialDesignInXamlToolkit
     * 
     ****************************************************************************************/

    /***************************************************************************************
     * 
     *    版本说明    最新版以github为准，由于提交更改比较频繁，需要经常查看官网地址:https://github.com/dathlin/ClientServerProject 
     *    注意        本代码的相关操作未作密码验证，如有需要，请自行完成
     *    如果        遇到启动调试就退出了，请注释掉App.xaml.cs文件中的指允许启动一个实例的代码
     *    
     **************************************************************************************************/

    /*****************************************************************************************
     * 
     *    权限说明    在进行特定权限操作的业务逻辑时，应该提炼成一个角色，这样可以动态绑定带有这些功能的账户
     *    示例        if (UserClient.CheckUserAccountRole("[审计员的GUID码]")) { dosomething(); }// 获取了审计员的角色，名字此处示例
     * 
     ******************************************************************************************/



    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        public MainWindow( )
        {
            InitializeComponent( );

            UserClient.PortraitManager = new UserPortrait( AppDomain.CurrentDomain.BaseDirectory + @"\Portrait\" + UserClient.UserAccount.UserName );
        }

        #endregion

        #region 窗口相关方法

        /// <summary>
        /// 指示窗口是否显示的标志
        /// </summary>
        private bool IsWindowShow { get; set; } = false;

        private void Window_Activated( object sender, EventArgs e )
        {
            // 窗口激活就触发，不应把初始代码放这里
        }

        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
        {
            IsWindowShow = false;

            // 通知服务器退出网络服务
            net_socket_client.ClientClose( );

            // 保存当前的颜色选择
            var p = new PaletteHelper( ).QueryPalette( );
            using (StreamWriter sw = new StreamWriter( AppDomain.CurrentDomain.BaseDirectory + @"Palette.txt", false, Encoding.UTF8 ))
            {
                sw.Write( JObject.FromObject( p ).ToString( ) );
            }

            // 等待一秒退出
            using (FormWaitInfomation fwm = new FormWaitInfomation( "正在退出程序...", 1000 ))
            {
                fwm.ShowDialog( );
            }
        }

        private void Window_ContentRendered( object sender, EventArgs e )
        {
            // 窗口呈现完成触发，已经显示了
            // 窗口显示
            IsWindowShow = true;

            // udp测试
            // SendServerUdpData(0, "显示了窗体");

            // 是否显示更新日志，显示前进行判断该版本是否已经显示过了
            if (!UserClient.JsonSettings.IsNewVersionRunning)
            {
                UserClient.JsonSettings.IsNewVersionRunning = false;
                UserClient.JsonSettings.SaveToFile( );
                MenuItem更新日志_Click( null, new RoutedEventArgs( ) );
            }




            // 根据权限使能菜单
            if (UserClient.UserAccount.Grade < AccountGrade.Admin)
            {
                MenuItem公告管理.IsEnabled = false;
                MenuItem账户管理.IsEnabled = false;
                MenuItem注册账户.IsEnabled = false;
                MenuItem消息发送.IsEnabled = false;
            }


            if (UserClient.UserAccount.Grade < AccountGrade.SuperAdministrator)
            {
                MenuItem日志查看.IsEnabled = false;
                MenuItem远程更新.IsEnabled = false;
                MenuItem系统配置.IsEnabled = false;
            }


            // 启动网络服务
            Net_Socket_Client_Initialization( );
            // 启动定时器
            TimeTickInitilization( );

            // 显示名称和加载头像
            AccountChip.Content = UserClient.UserAccount.UserName;
            AccountPortrait.Source = AppWpfHelper.TranslateImageToBitmapImage(
                UserClient.PortraitManager.GetSmallPortraitByUserName( UserClient.UserAccount.UserName ) );

            SetShowRenderControl( UIControl_Home );
        }

        private void Window_Initialized( object sender, EventArgs e )
        {
            // 在窗口实例化之后触发
        }

        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            // 在窗体加载时触发，窗体还不显示任何东西
            // 窗口载入
            Account_grade.Text = AccountGrade.GetDescription( UserClient.UserAccount.Grade );
            Account_factory.Text = UserClient.UserAccount.Factory;
            Account_register.Text = UserClient.UserAccount.RegisterTime.ToString( );
            Account_last.Text = UserClient.UserAccount.LastLoginTime.ToString( );
            Account_times.Text = UserClient.UserAccount.LoginFrequency.ToString( );
            Account_address.Text = UserClient.UserAccount.LastLoginIpAddress;

            // 状态栏设置
            TextBlock_CopyRight.Text = $"本软件著作权归{SoftResources.StringResouce.SoftCopyRight}所有";

            // 绑定事件，仅执行一次，不能放到show方法里
            net_socket_client.MessageAlerts += Net_socket_client_MessageAlerts;
            net_socket_client.LoginFailed += Net_socket_client_LoginFailed;
            net_socket_client.LoginSuccess += Net_socket_client_LoginSuccess;
            net_socket_client.AcceptByte += Net_socket_client_AcceptByte;
            net_socket_client.AcceptString += Net_socket_client_AcceptString;
            net_socket_client.BeforReConnected += Net_socket_client_BeforReConnected;

            TextBlock_Announcement.Text = UserClient.Announcement;

            TextBlock_Version.Text = UserClient.CurrentVersion.ToString( );

            // 初始化窗口
            MainRenderInitialization( );

            // 加载主题
            new PaletteHelper( ).SetLightDark( UserClient.JsonSettings.IsThemeDark );
        }



        private void AddStringRenderShow( string str )
        {
            var messageQueue = SoftSnackbar.MessageQueue;
            // the message queue can be called from any thread
            Task.Factory.StartNew( ( ) => SoftSnackbar.MessageQueue.Enqueue( str ) );
        }

        #endregion

        #region 菜单逻辑块


        private void MenuItem公告管理_Click( object sender, RoutedEventArgs e )
        {
            using (FormInputAndAction fiaa = new FormInputAndAction( str => UserClient.Net_simplify_client.ReadFromServer(
                  CommonHeadCode.SimplifyHeadCode.更新公告, str ).IsSuccess, UserClient.Announcement, "请输入公告内容" ))
            {
                fiaa.ShowDialog( );
            }
        }

        private void MenuItem账户管理_Click( object sender, RoutedEventArgs e )
        {
            FormAccountManage fam = new FormAccountManage( ( ) =>
             {
                 OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer( CommonHeadCode.SimplifyHeadCode.获取账户 );
                 if (result.IsSuccess) return result.Content;
                 else return result.ToMessageShowString( );
             }, m => UserClient.Net_simplify_client.ReadFromServer( CommonHeadCode.SimplifyHeadCode.更细账户, m ).IsSuccess );
            fam.ShowDialog( );
            fam.Dispose( );
        }

        private void MenuItem注册账户_Click( object sender, RoutedEventArgs e )
        {
            using (FormRegisterAccount fra = new FormRegisterAccount( UserClient.SystemFactories.ToArray( ) ))
            {
                fra.ShowDialog( );
            }
        }

        private void MenuItem日志查看_Click( object sender, RoutedEventArgs e )
        {
            using (FormLogView flg = new FormLogView( ))
            {
                flg.ShowDialog( );
            }
        }

        private void MenuItem远程更新_Click( object sender, RoutedEventArgs e )
        {
            if (UserClient.UserAccount.UserName == "admin")
            {
                using (FormUpdateRemote fur = new FormUpdateRemote( ))
                {
                    fur.ShowDialog( );
                }
            }
            else
            {
                MessageBox.Show( "权限不足！" );
            }
        }

        private void MenuItem消息发送_Click( object sender, RoutedEventArgs e )
        {
            using (FormInputAndAction fiaa = new FormInputAndAction( str => UserClient.Net_simplify_client.ReadFromServer(
                  CommonHeadCode.SimplifyHeadCode.群发消息, UserClient.UserAccount.UserName + ":" + str ).IsSuccess, "", "请输入群发的消息：" ))
            {
                fiaa.ShowDialog( );
            }
        }


        private void MenuItem密码更改_Click( object sender, RoutedEventArgs e )
        {
            using (FormPasswordModify fpm = new FormPasswordModify( UserClient.UserAccount.Password,
                p =>
                {
                    JObject json = new JObject
                    {
                        { UserAccount.UserNameText, UserClient.UserAccount.UserName },
                        { UserAccount.PasswordText, p }
                    };
                    return UserClient.Net_simplify_client.ReadFromServer( CommonHeadCode.SimplifyHeadCode.密码修改, json.ToString( ) ).IsSuccess;
                }, 6, 8 ))
            {
                fpm.ShowDialog( );
            }
        }

        private void MenuItem聊天信息_Click( object sender, RoutedEventArgs e )
        {
            // var messageQueue = SoftSnackbar.MessageQueue;
            // var message = "这是一条测试数据这是一条测试数据这是一条测试数据这是一条测试数据这是一条测试数据" + index++;

            //// the message queue can be called from any thread
            // Task.Factory.StartNew(() => messageQueue.Enqueue(message));

            SetShowRenderControl( UIControls_Chat );
            UIControls_Chat.ScrollToDown( );
        }

        private void MenuItem我的信息_Click( object sender, RoutedEventArgs e )
        {
            using (FormAccountDetails form = new FormAccountDetails( ))
            {
                form.ShowDialog( );
            }
        }


        private void MenuItem主题色调_Click( object sender, RoutedEventArgs e )
        {
            SetShowRenderControl( UIControl_Palette );
        }

        private void MenuItem关于本软件_Click( object sender, RoutedEventArgs e )
        {
            using (FormAbout fa = new FormAbout( SoftResources.StringResouce.SoftName,
                UserClient.CurrentVersion, 2017, SoftResources.StringResouce.SoftCopyRight ))
            {
                fa.ShowDialog( );
            }
        }

        private void MenuItem更新日志_Click( object sender, RoutedEventArgs e )
        {
            // 更新情况复位
            if (UserClient.JsonSettings.IsNewVersionRunning)
            {
                UserClient.JsonSettings.IsNewVersionRunning = false;
                UserClient.JsonSettings.SaveToFile( );
            }
            using (FormUpdateLog ful = new FormUpdateLog( UserClient.HistoryVersions ))
            {
                ful.ShowDialog( );
            }
        }

        private void MenuItem版本号说明_Click( object sender, RoutedEventArgs e )
        {
            using (FormAboutVersion fav = new FormAboutVersion( UserClient.CurrentVersion ))
            {
                fav.ShowDialog( );
            }
        }

        private void MenuItem意见反馈_Click( object sender, RoutedEventArgs e )
        {
            using (FormInputAndAction fiaa = new FormInputAndAction( str => UserClient.Net_simplify_client.ReadFromServer(
                  CommonHeadCode.SimplifyHeadCode.意见反馈, UserClient.UserAccount.UserName + ":" + str ).IsSuccess, "", "请输入意见反馈：" ))
            {
                fiaa.ShowDialog( );
            }
        }

        private void Button_Quit_Click( object sender, RoutedEventArgs e )
        {
            App.QuitCode = 1;
            Close( );
        }


        private void Border_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            // 点击了文件查看
            SetShowRenderControl( UIControl_Files );
            UIControl_Files.UpdateFiles( );
        }


        private void MenuItem系统配置_Click( object sender, RoutedEventArgs e )
        {
            using (FormConfiguration fc = new FormConfiguration( ))
            {
                fc.ShowDialog( );
            }
        }

        private void AccountChip_Click( object sender, RoutedEventArgs e )
        {
            // 点击了头像，请求查看高清版本头像
            using (FormMatterRemind fmr = new FormMatterRemind( "正在下载图片", UserClient.PortraitManager.ThreadPoolDownloadSizeLarge ))
            {
                fmr.ShowDialog( );
            }
        }


        #endregion

        #region 异步网络块

        private NetComplexClient net_socket_client = new NetComplexClient( );


        private void Net_Socket_Client_Initialization( )
        {
            try
            {
                net_socket_client.Token = UserSystem.KeyToken; // 新增的身份令牌
                net_socket_client.EndPointServer = new System.Net.IPEndPoint(
                    System.Net.IPAddress.Parse( UserClient.ServerIp ),
                    UserSystem.Port_Main_Net );
                net_socket_client.LogNet = UserClient.LogNet;
                net_socket_client.ClientAlias = UserClient.UserAccount.UserName; // 传入账户名
                net_socket_client.ClientStart( );
            }
            catch (Exception ex)
            {
                SoftBasic.ShowExceptionMessage( ex );
            }
        }
        /// <summary>
        /// 接收到服务器的字节数据的回调方法
        /// </summary>
        /// <param name="session">网络连接对象</param>
        /// <param name="customer">用户自定义的指令头，用来区分数据用途</param>
        /// <param name="data">数据</param>
        private void Net_socket_client_AcceptString( AppSession session, NetHandle customer, string data )
        {
            if (customer == CommonHeadCode.MultiNetHeadCode.弹窗新消息)
            {
                if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
                  {
                      FormPopup fpp = new FormPopup( data, System.Drawing.Color.DodgerBlue, 10000 );
                      fpp.Show( );
                  } ) );
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.总在线信息)
            {

            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.关闭客户端)
            {
                if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
                  {
                      Close( );
                  } ) );
            }
            else if (customer == CommonHeadCode.SimplifyHeadCode.更新公告)
            {
                //此处应用到了同步类的指令头
                if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
                  {
                      UserClient.Announcement = data;
                      TextBlock_Announcement.Text = data;
                      FormPopup fpp = new FormPopup( data, System.Drawing.Color.DodgerBlue, 10000 );
                      fpp.Show( );
                  } ) );
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.初始化数据)
            {
                //收到服务器的数据
                JObject json = JObject.Parse( data );
                UserClient.DateTimeServer = json["Time"].ToObject<DateTime>( );
                List<string> chats = JArray.Parse( json["chats"].ToString( ) ).ToObject<List<string>>( );
                StringBuilder sb = new StringBuilder( );
                chats.ForEach( m => { sb.Append( m + Environment.NewLine ); } );


                if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
                  {
                      TextBlock_ServerTime.Text = UserClient.DateTimeServer.ToString( "yyyy-MM-dd HH:mm:ss" );
                      TextBlock_FileCount.Text = json["FileCount"].ToObject<int>( ).ToString( );
                      UIControls_Chat.AddChatsHistory( sb.ToString( ) );

                      NetAccount[] accounts = JArray.Parse( json["ClientsOnline"].ToString( ) ).ToObject<NetAccount[]>( );

                      foreach (var m in accounts)
                      {
                          Views.Controls.UserClientRenderItem userClient = new Views.Controls.UserClientRenderItem( );
                          userClient.SetClientRender( m );
                          ClientsOnline.Children.Add( userClient );
                      }
                  } ) );
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.文件总数量)
            {
                if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
                  {
                      TextBlock_FileCount.Text = data;
                  } ) );
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.留言版消息)
            {
                if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
                  {
                      UIControls_Chat?.DealwithReceive( data );
                  } ) );
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.新用户上线)
            {
                if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
                  {
                      Views.Controls.UserClientRenderItem userClient = new Views.Controls.UserClientRenderItem( );
                      userClient.SetClientRender( JObject.Parse( data ).ToObject<NetAccount>( ) );
                      ClientsOnline.Children.Add( userClient );
                  } ) );
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.用户下线)
            {
                if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
                  {
                      Views.Controls.UserClientRenderItem item = null;
                      foreach (Views.Controls.UserClientRenderItem m in ClientsOnline.Children)
                      {
                          if (m?.UniqueId == data)
                          {
                              item = m;
                          }
                      }
                      if (item != null) ClientsOnline.Children.Remove( item );
                  } ) );
            }
            else if (customer == CommonHeadCode.MultiNetHeadCode.新头像更新)
            {
                UserClient.PortraitManager.UpdateSmallPortraitByName( data );
                if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
                  {
                      if (data == UserClient.UserAccount.UserName)
                      {
                          AccountPortrait.Source = AppWpfHelper.TranslateImageToBitmapImage(
                              UserClient.PortraitManager.GetSmallPortraitByUserName( data ) );
                      }
                      foreach (Views.Controls.UserClientRenderItem m in ClientsOnline.Children)
                      {
                          m.UpdatePortrait( data );
                      }
                  } ) );
            }
        }

        private void Net_socket_client_AcceptByte( AppSession session, NetHandle customer, byte[] object2 )
        {
            // 接收到服务器发来的字节数据
            if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
              {
                  MessageBox.Show( customer.ToString( ) );
              } ) );
        }



        private void Net_socket_client_LoginSuccess( )
        {
            // 登录成功，或重新登录成功的事件，有些数据的初始化可以放在此处
            if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
              {
                  TextBlock_ClientStatus.Text = "客户端启动成功";
              } ) );
        }

        private void Net_socket_client_BeforReConnected( )
        {
            // 和服务器断开，重新连接之前发生的事件，清理已加载资源使用，比如客户端在线信息
            if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
              {
                  ClientsOnline.Children?.Clear( );
              } ) );
        }

        private void Net_socket_client_LoginFailed( int object1 )
        {
            // 登录失败的情况，如果连续三次连接失败，请考虑退出系统
            if (object1 > 3)
            {
                if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
                  {
                      Close( );
                  } ) );
            }
        }

        private void Net_socket_client_MessageAlerts( string object1 )
        {
            // 信息提示
            if (IsWindowShow) Dispatcher.Invoke( new Action( ( ) =>
              {
                  TextBlock_ClientStatus.Text = object1;
              } ) );
        }


        #endregion

        #region 后台计数线程

        /*********************************************************************************************
         * 
         *    说明       一个后台线程，用来执行一些周期执行的东西
         *    注意       它不仅执行每秒触发的代码，也支持每分钟，每天，每月，每年等等
         * 
         ********************************************************************************************/


        /// <summary>
        /// 初始化后台的计数线程
        /// </summary>
        public void TimeTickInitilization( )
        {

            Thread thread = new Thread( new ThreadStart( ThreadTimeTick ) );
            thread.IsBackground = true;
            thread.Start( );
        }

        public void ThreadTimeTick( )
        {
            Thread.Sleep( 300 );//加一个微小的延时
            int second = DateTime.Now.Second - 1;
            int minute = -1;
            int hour = -1;
            int day = -1;
            Action DTimeShow = delegate
            {
                //显示服务器的时间和当前网络的延时时间，通常为毫秒
                TextBlock_ServerTime.Text = net_socket_client.ServerTime.ToString( "yyyy-MM-dd HH:mm:ss" );
                TextBlock_ServerDelay.Text = net_socket_client.DelayTime.ToString( );
            };

            while (IsWindowShow)
            {
                while (DateTime.Now.Second == second)
                {
                    Thread.Sleep( 20 );
                }
                second = DateTime.Now.Second;
                if (IsWindowShow) Dispatcher.Invoke( DTimeShow );
                //每秒钟执行的代码
                UserClient.DateTimeServer = net_socket_client.ServerTime;

                if (second == 0)
                {
                    //每个0秒执行的代码
                }
                if (minute != DateTime.Now.Minute)
                {
                    minute = DateTime.Now.Minute;
                    //每分钟执行的代码
                }
                if (hour != DateTime.Now.Hour)
                {
                    hour = DateTime.Now.Hour;
                    //每小时执行的代码
                }
                if (day != DateTime.Now.Day)
                {
                    day = DateTime.Now.Day;
                    //每天执行的代码
                }
            }
        }




        #endregion

        #region Udp发送示例

        /// <summary>
        /// 调用该方法并指定参数即可，最长字符串不得超过服务器配置的长度，否则无效
        /// </summary>
        /// <param name="customer">指令头</param>
        /// <param name="data">实际数据</param>
        private void SendServerUdpData( NetHandle customer, string data )
        {
            //测试发送udp消息
            UserClient.Net_Udp_Client.SendMessage( customer, data );
        }

        #endregion

        #region 多界面管理块

        private List<UserControl> all_main_render = new List<UserControl>( );


        private UserChat UIControls_Chat { get; set; }

        private UserHome UIControl_Home { get; set; }

        private UserFileRender UIControl_Files { get; set; }

        private UserPaletteSelector UIControl_Palette { get; set; }

        /// <summary>
        /// 正在显示的子界面
        /// </summary>
        private UserControl CurrentRender { get; set; }

        /// <summary>
        /// 主界面的初始化
        /// </summary>
        private void MainRenderInitialization( )
        {
            //将所有的子集控件添加进去

            /*******************************************************************************
             * 
             *    例如此处展示了文件控件是如何添加进去的 
             *    1.先进行实例化，赋值初始参数
             *    2.添加进项目
             *    3.显示
             *
             *******************************************************************************/

            //UIControls_Files = new UIControls.ShareFilesRender()
            //{
            //    Visible = false,
            //    Parent = panel_main,//决定了放在哪个界面显示，此处示例
            //    Dock = DockStyle.Fill,
            //};
            //all_main_render.Add(UIControls_Files);

            UIControls_Chat = new UserChat( ( m ) =>
             {
                 net_socket_client.Send( CommonHeadCode.MultiNetHeadCode.留言版消息, m );
             } );
            all_main_render.Add( UIControls_Chat );

            UIControl_Home = new UserHome( );
            all_main_render.Add( UIControl_Home );

            UIControl_Palette = new UserPaletteSelector( ) { DataContext = new PaletteSelectorViewModel( ) };
            all_main_render.Add( UIControl_Palette );

            UIControl_Files = new UserFileRender( "ShareFiles", "", "" );
            all_main_render.Add( UIControl_Files );

        }

        private void SetShowRenderControl( UserControl control )
        {
            if (!ReferenceEquals( CurrentRender, control ))
            {
                CurrentRender = control;
                //UserContentControl.Content = control;

                DoubleAnimation d_opacity = new DoubleAnimation( 1, 0, TimeSpan.FromMilliseconds( 100 ) );
                DoubleAnimation d_y = new DoubleAnimation( 0, -10, TimeSpan.FromMilliseconds( 100 ) );
                TranslateTransform tt = new TranslateTransform( );
                UserContentControl.RenderTransform = tt;
                d_opacity.Completed += delegate
                {
                    UserContentControl.Content = control;
                    DoubleAnimation d_opacity2 = new DoubleAnimation( 0, 1, TimeSpan.FromMilliseconds( 100 ) );
                    DoubleAnimation d_y2 = new DoubleAnimation( 10, 0, TimeSpan.FromMilliseconds( 100 ) );
                    TranslateTransform tt2 = new TranslateTransform( );
                    UserContentControl.RenderTransform = tt2;
                    tt2.BeginAnimation( TranslateTransform.XProperty, d_y2 );
                    UserContentControl.BeginAnimation( OpacityProperty, d_opacity2 );
                };

                UserContentControl.BeginAnimation( OpacityProperty, d_opacity );
                tt.BeginAnimation( TranslateTransform.XProperty, d_y );
            }
        }




        private void SetShowRenderControl( Type typeControl )
        {
            UserControl control = null;
            foreach (var c in all_main_render)
            {
                if (c.GetType( ) == typeControl)
                {
                    control = c;
                    break;
                }
            }
            if (control != null) SetShowRenderControl( control );
        }

        #endregion

        #region User Interface


        /**********************************************************************************************
         * 
         *    说明： 以下的两个方法是针对主界面左侧的按钮，用于测试
         * 
         **********************************************************************************************/




        private void Button_BackMain_Click( object sender, RoutedEventArgs e )
        {
            //点击了主页
            SetShowRenderControl( UIControl_Home );
        }

        private async void Button_Dialog_Click( object sender, RoutedEventArgs e )
        {
            //点击了显示一个提示的窗体
            DialogHostWait.IsOpen = true;
            await Task.Delay( 2000 );
            DialogHostWait.IsOpen = false;
        }
        private void packIcon_right_profile_MouseDown( object sender, MouseButtonEventArgs e )
        {
            // 点击我的信息
            MenuItem我的信息_Click( sender, null );
        }

        private void packIcon_right_file_MouseDown( object sender, MouseButtonEventArgs e )
        {
            // 点击共享文件
            Border_MouseLeftButtonDown( sender, e );
        }


        private void packIcon_right_shrink_Click( object sender, RoutedEventArgs e )
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(
                25, new Duration( TimeSpan.FromMilliseconds( 300 ) ) );
            doubleAnimation.Completed += DoubleAnimation_Completed;
            MainPageRight.BeginAnimation( WidthProperty, doubleAnimation );
        }

        private void DoubleAnimation_Completed( object sender, EventArgs e )
        {
            MainPageRightMini.Visibility = Visibility.Visible;
            MainPageRight.Visibility = Visibility.Collapsed;
        }

        private void packIcon_right_restore_Click( object sender, RoutedEventArgs e )
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(
                25, 230, new Duration( TimeSpan.FromMilliseconds( 300 ) ) );
            MainPageRight.Visibility = Visibility.Visible;
            MainPageRightMini.Visibility = Visibility.Collapsed;
            MainPageRight.BeginAnimation( WidthProperty, doubleAnimation );
        }


        #endregion
    }
}
