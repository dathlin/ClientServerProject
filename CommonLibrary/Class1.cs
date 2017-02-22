using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public class CommonLibrary
    {
        #region 公用端口设计块

        /// <summary>
        /// 主网络端口，此处随机定义了一个数据
        /// </summary>
        public static int Port_Main_Net { get; } = 17652;
        /// <summary>
        /// 同步网络访问的端口，此处随机定义了一个数据
        /// </summary>
        public static int Port_Second_Net { get; } = 14568;
        /// <summary>
        /// 用于软件系统更新的端口，此处随机定义了一个数据
        /// </summary>
        public static int Port_Update_Net { get; } = 17538;
        /// <summary>
        /// 用于软件远程更新的端口，此处随机定义了一个数据
        /// </summary>
        public static int Port_Update_Remote { get; } = 26435;
        
        #endregion

        
    }


    /// <summary>
    /// 用于网络通信的二级协议头说明
    /// </summary>
    public class CommonHeadCode
    {
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
        }
       

        //可以在下面进行扩展，需要保证长度都是统一的
    }
}
