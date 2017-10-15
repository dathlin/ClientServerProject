using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HslCommunication;

namespace CommonLibrary
{

    /*********************************************************************************************
     * 
     *    说明：用于同步网络和异步网络的各种消息头的区别。
     *    
     *    关于 NetHandle：
     *         一个数据结构值，无论实际数据还是用法上等同于int，可以和int数据无缝的转化
     *         本质上将4个字节的int数据拆分成了三个属性，一个ushort和两个byte，可以分别访问
     *         
     *         2个低字节         第二高字节    最高的字节
     *         [byte1][byte2]    [byte3]       [byte4]
     *         CodeIdentifier    CodeMinor     CodeMajor     
     * 
     *********************************************************************************************/





    /// <summary>
    /// 用于网络通信的二级协议头说明
    /// </summary>
    public class CommonHeadCode
    {
        /// <summary>
        /// 同步通信的指令头说明，从1.1.x到1.255.x
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        public class SimplifyHeadCode
        {

            /*******************************************************************************************
             * 
             *     1.1.* 的指令为系统相关
             *
             *******************************************************************************************/

            #region 1.1.X 指令块


            public static NetHandle 维护检查 { get; } =          new NetHandle(1, 1, 00001);
            public static NetHandle 更新检查 { get; } =          new NetHandle(1, 1, 00002);
            public static NetHandle 参数下载 { get; } =          new NetHandle(1, 1, 00003);
            public static NetHandle 账户检查 { get; } =          new NetHandle(1, 1, 00004); 
            public static NetHandle 密码修改 { get; } =          new NetHandle(1, 1, 00005);
            public static NetHandle 更细账户 { get; } =          new NetHandle(1, 1, 00006);
            public static NetHandle 获取账户 { get; } =          new NetHandle(1, 1, 00007);
            public static NetHandle 更新公告 { get; } =          new NetHandle(1, 1, 00008);
            public static NetHandle 注册账号 { get; } =          new NetHandle(1, 1, 00009);
            public static NetHandle 更新版本 { get; } =          new NetHandle(1, 1, 00010);
            public static NetHandle 请求文件 { get; } =          new NetHandle(1, 1, 00011);
            public static NetHandle 意见反馈 { get; } =          new NetHandle(1, 1, 00012);
            public static NetHandle 群发消息 { get; } =          new NetHandle(1, 1, 00013);
            public static NetHandle 异常消息 { get; } =          new NetHandle(1, 1, 00014);
            public static NetHandle 性能计数 { get; } =          new NetHandle(1, 1, 00015);
            public static NetHandle 上传头像MD5 { get; } =       new NetHandle(1, 1, 00016);
            public static NetHandle 请求分厂 { get; } =          new NetHandle(1, 1, 00017);
            public static NetHandle 上传分厂 { get; } =          new NetHandle(1, 1, 00018);
            public static NetHandle 请求信任客户端 { get; } =    new NetHandle(1, 1, 00019);
            public static NetHandle 上传信任客户端 { get; } =    new NetHandle(1, 1, 00020);
            public static NetHandle 请求一般配置 { get; } =      new NetHandle(1, 1, 00021);
            public static NetHandle 上传一般配置 { get; } =      new NetHandle(1, 1, 00022);
            public static NetHandle 请求角色配置 { get; } =      new NetHandle(1, 1, 00023);
            public static NetHandle 上传角色配置 { get; } =      new NetHandle(1, 1, 00024);
            public static NetHandle 检查角色权限 { get; } =      new NetHandle(1, 1, 00025);


            #endregion


            /*******************************************************************************************
             * 
             *     1.2.* 的指令为日志的请求与查看相关
             *
             *******************************************************************************************/

            #region 1.2.X 指令块



            public static NetHandle 网络日志查看 { get; } =          new NetHandle(1, 2, 00001);//1.2.开头的是日志请求和清空
            public static NetHandle 网络日志清空 { get; } =          new NetHandle(1, 2, 00002);
            public static NetHandle 同步日志查看 { get; } =          new NetHandle(1, 2, 00003);
            public static NetHandle 同步日志清空 { get; } =          new NetHandle(1, 2, 00004);
            public static NetHandle 更新日志查看 { get; } =          new NetHandle(1, 2, 00005);
            public static NetHandle 更新日志清空 { get; } =          new NetHandle(1, 2, 00006);
            public static NetHandle 运行日志查看 { get; } =          new NetHandle(1, 2, 00007);
            public static NetHandle 运行日志清空 { get; } =          new NetHandle(1, 2, 00008);
            public static NetHandle 文件日志查看 { get; } =          new NetHandle(1, 2, 00009);
            public static NetHandle 文件日志清空 { get; } =          new NetHandle(1, 2, 00010);
            public static NetHandle 反馈日志查看 { get; } =          new NetHandle(1, 2, 00011);
            public static NetHandle 反馈日志清空 { get; } =          new NetHandle(1, 2, 00012);
            public static NetHandle UDP日志查看 { get; } =           new NetHandle(1, 2, 00013); 
            public static NetHandle UDP日志清空 { get; } =           new NetHandle(1, 2, 00014);
            public static NetHandle 客户端日志查看 { get; } =        new NetHandle(1, 2, 00015);
            public static NetHandle 客户端日志清空 { get; } =        new NetHandle(1, 2, 00016);
            public static NetHandle 头像日志查看 { get; } =          new NetHandle(1, 2, 00017);
            public static NetHandle 头像日志清空 { get; } =          new NetHandle(1, 2, 00018);


            #endregion


            /**************************************************************************************
             * 
             *    为了保证您的指令头不和系统的冲突，您的指令头应该以1.3.x及之后开头
             * 
             **************************************************************************************/

        }



        /// <summary>
        /// 异步通信的头说明，从2.1.x到2.255.x
        /// </summary>
        public class MultiNetHeadCode
        {

            /*******************************************************************************************
             * 
             *     2.1.* 的指令为异步网络的系统相关指令
             *
             *******************************************************************************************/

            #region 2.1.X 指令块

            public static NetHandle 总在线信息 { get; } =          new NetHandle(2, 1, 00001); 
            public static NetHandle 关闭客户端 { get; } =          new NetHandle(2, 1, 00002);
            public static NetHandle 弹窗新消息 { get; } =          new NetHandle(2, 1, 00003);
            public static NetHandle 时间的推送 { get; } =          new NetHandle(2, 1, 00004);
            public static NetHandle 文件总数量 { get; } =          new NetHandle(2, 1, 00005);
            public static NetHandle 初始化数据 { get; } =          new NetHandle(2, 1, 00006);
            public static NetHandle 留言版消息 { get; } =          new NetHandle(2, 1, 00007);
            public static NetHandle 新用户上线 { get; } =          new NetHandle(2, 1, 00008);
            public static NetHandle 用户下线 { get; } =            new NetHandle(2, 1, 00009);
            public static NetHandle 新头像更新 { get; } =          new NetHandle(2, 1, 00010);


            #endregion


            /**************************************************************************************
             * 
             *    为了保证您的指令头不和系统的冲突，您的指令头应该以2.2.x  2.3.x开头
             * 
             **************************************************************************************/
        }


        //可以在下面进行扩展，需要保证长度都是统一的，新建您自己的类型 可以用3.1.x 4.1.x开头的指令
    }
}
