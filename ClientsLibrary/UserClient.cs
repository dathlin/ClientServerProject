using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HslCommunication.Enthernet;
using HslCommunication.BasicFramework;
using CommonLibrary;

namespace ClientsLibrary
{


    /***********************************************************************************
     * 
     *    说明：用来存储客户端全局的变量数据，好在任何界面都可以直达数据
     * 
     * 
     ***********************************************************************************/


    /// <summary>
    /// 一个通用的用户客户端类, 包含了一些静态的资源
    /// </summary>
    public class UserClient
    {
        /// <summary>
        /// 客户端需要进行本地存储的信息日志
        /// </summary>
        public static JsonSettings JsonSettings = new JsonSettings();
        

        /// <summary>
        /// 本软件的当前版本，用来验证更新的关键依据
        /// </summary>
        public static SystemVersion CurrentVersion { get; } = new SystemVersion("1.0.0.170914");
        

        /// <summary>
        /// 服务器的IP地址，默认为127.0.0.1，可用于单机调试，云服务器端：117.48.203.204
        /// </summary>
        public static string ServerIp { get; } = "127.0.0.1";//用于测试的云服务器地址
        

        /// <summary>
        /// 系统的分厂信息
        /// </summary>
        public static List<string> SystemFactories { get; set; } = new List<string>();
        


        /// <summary>
        /// 所有版本更新信息的对象
        /// </summary>
        public static List<VersionInfo> HistoryVersions { get; } = new List<VersionInfo>
        {
                // 写入所有的历史版本信息，这样就能在更新日志的界面查看到信息
                new VersionInfo()
                {
                    VersionNum=new SystemVersion("1.0.0"),
                    ReleaseDate=new DateTime(2017,1,1),//该版本发布的日期
                    UpdateDetails=new StringBuilder(
                        "1.本系统第一版本正式发布使用。"+Environment.NewLine+
                        "2.提供了多客户端用时在线的功能。"+Environment.NewLine+
                        "3.需要用户自行添加"),
                },
        };
        

        /// <summary>
        /// 设置或获取系统的公告
        /// </summary>
        public static string Announcement { get; set; } = "";
        /// <summary>
        /// 当前系统的登录账户
        /// </summary>
        public static UserAccount UserAccount { get; set; } = new UserAccount();

        /// <summary>
        /// 服务器的时间，该时间与服务器同步，每隔10秒钟，防止客户端串改单机时间，可以作为各种时间条件判定
        /// </summary>
        public static DateTime DateTimeServer { get; set; } = DateTime.Now;


        /// <summary>
        /// 用于访问服务器数据的网络对象类，必须修改这个端口参数，否则运行失败
        /// </summary>
        public static NetSimplifyClient Net_simplify_client = new NetSimplifyClient(
            new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ServerIp),
                CommonLibrary.CommonLibrary.Port_Second_Net))
        {
            KeyToken = CommonHeadCode.KeyToken,
            ConnectTimeout = 5000,
        };

        /// <summary>
        /// 用于使用udp向服务器进行发送即时可丢失数据的对象
        /// </summary>
        public static NetUdpClient Net_Udp_Client = new NetUdpClient(
            new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ServerIp),
                CommonLibrary.CommonLibrary.Port_Udp_Server))
        {
            KeyToken = CommonHeadCode.KeyToken,
        };

        /// <summary>
        /// 客户端的日志纪录对象
        /// </summary>
        public static HslCommunication.LogNet.ILogNet LogNet { get; set; }


        /// <summary>
        /// 用来处理客户端发生的未捕获的异常，将通过网络组件发送至服务器存储，用于更好的跟踪错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                // 使用TCP方法传送会服务器
                string info = HslCommunication.LogNet.LogNetManagment.GetSaveStringFromException(null, ex);
                Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.异常消息, info);
            }
        }
    }
}
