using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    /// <summary>
    /// 用于网络通信的二级协议头说明
    /// </summary>
    public class CommonHeadCode
    {
        /// <summary>
        /// 用于同步和异步的网络的身份令牌，提升安全性
        /// </summary>
        public static Guid KeyToken { get; set; } = new Guid("1275BB9A-14B2-4A96-9673-B0AF0463D474");


        /// <summary>
        /// 同步通信的头说明，以字母A开头
        /// </summary>
        public class SimplifyHeadCode
        {
            public static string 维护检查 { get; } = "A001";//A开始的表明是系统相关的
            public static string 更新检查 { get; } = "A002";
            public static string 账户检查 { get; } = "A003";
            public static string 参数下载 { get; } = "A004";
            public static string 密码修改 { get; } = "A005";
            public static string 更细账户信息 { get; } = "A006";
            public static string 获取账户信息 { get; } = "A007";
            public static string 更新公告 { get; } = "A008";
            public static string 注册账号 { get; } = "A009";



            public static string 网络日志查看 { get; } = "B001";//B开头的是日志请求和清空
            public static string 网络日志清空 { get; } = "B002";
            public static string 同步日志查看 { get; } = "B003";
            public static string 同步日志清空 { get; } = "B004";
            public static string 更新日志查看 { get; } = "B005";
            public static string 更新日志清空 { get; } = "B006";

        }
        /// <summary>
        /// 异步通信的头说明，以字母H开头，后面跟I,G,K,L,M
        /// </summary>
        public class MultiNetHeadCode
        {
            public static string 所有客户端在线信息 { get; } = "H001";
            public static string 关闭所有客户端 { get; } = "H002";
            public static string 弹窗消息 { get; } = "H003";
            public static string 时间推送 { get; } = "H004";
        }


        //可以在下面进行扩展，需要保证长度都是统一的
    }
}
