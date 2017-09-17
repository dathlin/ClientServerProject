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

        /************************************************************************************************
         * 
         *    注意：您在准备二次开发时，应该重新生成一个自己的GUID码
         * 
         **************************************************************************************************/


        /// <summary>
        /// 用于整个网络服务交互的身份令牌，可有效的防止来自网络的攻击，重新生成令牌后就无法更改，否则不支持自动升级
        /// </summary>
        public static Guid KeyToken { get; set; } = new Guid("1275BB9A-14B2-4A96-9673-B0AF0463D474");

        
        
        /// <summary>
        /// 同步通信的指令头说明，以10000开头，后面接20000，30000，
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        public class SimplifyHeadCode
        {
            /// <summary>
            /// 判断指令否是系统指令
            /// </summary>
            /// <param name="customer">指令</param>
            /// <returns>是否符合</returns>
            public static bool IsCustomerGroupSystem(int customer)
            {
                return (customer >= 10000 && customer < 11000);
            }
            public static int 维护检查 { get; } =        10000;//10000开始的表明是系统相关的
            public static int 更新检查 { get; } =        10001;
            public static int 账户检查 { get; } =        10002;
            public static int 参数下载 { get; } =        10003;
            public static int 密码修改 { get; } =        10004;
            public static int 更细账户 { get; } =        10005;
            public static int 获取账户 { get; } =        10006;
            public static int 更新公告 { get; } =        10007;
            public static int 注册账号 { get; } =        10008;
            public static int 更新版本 { get; } =        10009;
            public static int 请求文件 { get; } =        10010;
            public static int 意见反馈 { get; } =        10011;
            public static int 群发消息 { get; } =        10012;
            public static int 异常消息 { get; } =        10013;
            public static int 性能计数 { get; } =        10014;
            public static int 请求小头 { get; } =        10015;
            public static int 下载小头 { get; } =        10016;
            public static int 请求大头 { get; } =        10017;
            public static int 下载大头 { get; } =        10018;
            public static int 请求分厂 { get; } =        10019;
            public static int 上传分厂 { get; } =        10020;




            /// <summary>
            /// 判断指令否是日志相关指令
            /// </summary>
            /// <param name="customer">指令</param>
            /// <returns>是否符合</returns>
            public static bool IsCustomerGroupLogging(int customer)
            {
                return (customer >= 11000 && customer < 12000);
            }
            public static int 网络日志查看 { get; } =      11000;//11000开头的是日志请求和清空
            public static int 网络日志清空 { get; } =      11001;
            public static int 同步日志查看 { get; } =      11002;
            public static int 同步日志清空 { get; } =      11003;
            public static int 更新日志查看 { get; } =      11004;
            public static int 更新日志清空 { get; } =      11005;
            public static int 运行日志查看 { get; } =      11006;
            public static int 运行日志清空 { get; } =      11007;
            public static int 文件日志查看 { get; } =      11008;
            public static int 文件日志清空 { get; } =      11009;
            public static int 反馈日志查看 { get; } =      11010;
            public static int 反馈日志清空 { get; } =      11011;
            public static int UDP日志查看 { get; } =       11012;
            public static int UDP日志清空 { get; } =       11013;
            public static int 客户端日志查看 { get; } =    11014;
            public static int 客户端日志清空 { get; } =    11015;
            public static int 头像日志查看 { get; } =      11016;
            public static int 头像日志清空 { get; } =      11017;

            /**************************************************************************************
             * 
             *    为了保证您的指令头不和系统的冲突，您的指令头应该以20000之后开头
             * 
             **************************************************************************************/

        }
        /// <summary>
        /// 异步通信的头说明，以字母H开头，后面跟I,G,K,L,M
        /// </summary>
        public class MultiNetHeadCode
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="customer">指令</param>
            /// <returns>是否符合</returns>
            public static bool IsCustomerGroupSystem(int customer)
            {
                return (customer >= 50000 && customer < 51000);
            }
            public static int 总在线信息 { get; } =        50000;
            public static int 关闭客户端 { get; } =        50001;
            public static int 弹窗新消息 { get; } =        50002;
            public static int 时间的推送 { get; } =        50003;
            public static int 文件总数量 { get; } =        50004;
            public static int 初始化数据 { get; } =        50005;
            public static int 留言版消息 { get; } =        50006;


            /**************************************************************************************
             * 
             *    为了保证您的指令头不和系统的冲突，您的指令头应该以I,J,K,L开头
             * 
             **************************************************************************************/
        }


        //可以在下面进行扩展，需要保证长度都是统一的，新建您自己的类型
    }
}
