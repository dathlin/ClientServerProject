using HslCommunication.BasicFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonLibrary
{
    /***********************************************************************************
     * 
     *    说明：用来客户端和服务器都能够直达访问的一些静态资源
     *          专门放在这下面的数据是需要支持winform和wpf共同访问的
     * 
     ***********************************************************************************/



    public class UserSystem
    {
        static UserSystem()
        {
            /**************************************************************************
             * 
             *    时间：2017年6月29日 07:58:02
             *    说明：更改版本机制，每次提交新增修订版的版本号
             *    发行：发行版将采用主版本加次版本来发行
             * 
             **************************************************************************/

            /**************************************************************************
             * 
             *    说明：以下是大版本号的发布日期
             *    
             *    时间：2017年7月7日 14:11:35       版本号：1.1.0
             *    时间：2017年7月18日 15:10:18      版本号：1.2.0
             *    时间：2017年9月3日 13:27:52       版本号：1.3.0
             *    时间：2017年9月19日 22:06:27      版本号：1.4.0
             *    时间：2017年10月1日 16:00:13      版本号：1.5.0
             *    时间：2017年10月6日 19:23:09      版本号：1.6.0
             *    时间：2017年10月21日 11:55:41     版本号：1.7.0
             *    时间：2018年5月8日 11:09:16       版本号：1.8.0
             * 
             **************************************************************************/


            SoftBasic.FrameworkVersion = new SystemVersion("1.8.0");

        }



        /************************************************************************************************
         * 
         *    注意：您在准备二次开发时，应该重新生成一个自己的GUID码
         *    Note: When you are preparing for secondary development, you should regenerate your own GUID code
         * 
         ************************************************************************************************/


        /// <summary>
        /// 用于整个网络服务交互的身份令牌，可有效的防止来自网络的攻击，其他系统的恶意的连接
        /// 重新生成令牌后就无法更改，否则不支持自动升级
        /// </summary>
        public static Guid KeyToken { get; set; } = new Guid("1275BB9A-14B2-4A96-9673-B0AF0463D474");



        /************************************************************************************************
         * 
         *    注意：此处的所有的网络端口应该重新指定，防止其他人的项目连接到你的程序上
         *          假设你们的多个项目服务器假设在一台电脑的情况，就绝对要替换下面的端口号
         * 
         ************************************************************************************************/


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
        /// 共享文件的端口号
        /// </summary>
        public static int Port_Ultimate_File_Server { get; } = 34261;

        /// <summary>
        /// 用于UDP传输的端口号
        /// </summary>
        public static int Port_Udp_Server { get; } = 32566;

        /// <summary>
        /// 用于服务器版本更新的端口
        /// </summary>
        public static int Port_Advanced_File_Server { get; } = 24672;

        /// <summary>
        /// 用于实时数据推送的消息网络
        /// </summary>
        public static int Port_Push_Server { get; } = 14574;


        /// <summary>
        /// 整个系统的加密解密密码
        /// </summary>
        public static string Security { get; } = "qwertyui";



        /// <summary>
        /// 统一的窗体图标显示
        /// </summary>
        /// <returns></returns>
        public static Icon GetFormWindowIcon()
        {
            return Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }



    }
}
