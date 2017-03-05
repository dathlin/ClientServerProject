using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndustryEthernet;


namespace 软件系统客户端模版
{

    //====================================================================================================
    //   模版说明：
    //====================================================================================================


    public class UserClient
    {
        public static BasicFramework.JsonSettings JsonSettings = new BasicFramework.JsonSettings();
        /// <summary>
        /// 本软件的当前版本，用来验证更新的关键依据
        /// </summary>
        public static BasicFramework.SystemVersion CurrentVersion { get; } = new BasicFramework.SystemVersion("1.0.0");

        /// <summary>
        /// 服务器的IP地址，默认为127.0.0.1，可用于单机调试
        /// </summary>
        public static string ServerIp { get; } = "127.0.0.1";

        /// <summary>
        /// 所有版本更新信息的对象
        /// </summary>
        public static BasicFramework.ClassSystemVersion HistoryVersions { get; } = new BasicFramework.ClassSystemVersion()
        {
            Versions = new List<BasicFramework.VersionInfo>()
            {
                //写入所有的历史版本信息，这样就能在更新日志的界面查看到信息
                new BasicFramework.VersionInfo()
                {
                    VersionNum=new BasicFramework.SystemVersion("1.0.0"),
                    ReleaseDate=new DateTime(2017,1,1),//该版本发布的日期
                    UpdateDetails=new StringBuilder("1.本系统第一版本正式发布使用。"+Environment.NewLine+
                        "2.提供了多客户端用时在线的功能。"+Environment.NewLine+
                        "3.需要用户自行添加"),
                },
            },
        };




        /// <summary>
        /// 设置或获取系统的公告
        /// </summary>
        public static string Announcement { get; set; } = "";
        /// <summary>
        /// 当前系统的登录账户
        /// </summary>
        public static BasicFramework.UserAccount UserAccount { get; set; } = new BasicFramework.UserAccount();


        /// <summary>
        /// 用于访问服务器数据的网络对象类，必须修改这个端口参数，否则运行失败
        /// </summary>
        public static Net_Simplify_Client Net_simplify_client = new Net_Simplify_Client(
            new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ServerIp),
                CommonLibrary.CommonLibrary.Port_Second_Net))
        {
            KeyToken = CommonLibrary.CommonHeadCode.KeyToken,
        };
    }
}
