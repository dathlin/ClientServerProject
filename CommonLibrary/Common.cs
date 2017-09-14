using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HslCommunication.BasicFramework;

namespace CommonLibrary
{
    public class CommonLibrary
    {
        static CommonLibrary()
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
             *    时间：2017年7月7日 14:11:35      版本号：1.1.0
             * 
             **************************************************************************/

            /**************************************************************************
             * 
             *    时间：2017年7月18日 15:10:18      版本号：1.2.0
             * 
             **************************************************************************/

            /**************************************************************************
             * 
             *    时间：2017年9月3日 13:27:52      版本号：1.3.0
             * 
             **************************************************************************/

            /**************************************************************************
             * 
             *    时间：2017年9月14日 22:06:27      版本号：1.4.0
             * 
             **************************************************************************/

            SoftBasic.FrameworkVersion = new SystemVersion("1.4.0");

        }


        #region 公用端口设计块

        //======================================================================================
        //    此处的所有的网络端口应该重新指定，防止其他人的项目连接到你的程序上
        //    假设你们的多个项目服务器假设在一台电脑的情况，就绝对要替换下面的端口号

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
        /// <summary>
        /// 共享文件的端口号
        /// </summary>
        public static int Port_Share_File { get; } = 34261;
        /// <summary>
        /// 用于UDP传输的端口号
        /// </summary>
        public static int Port_Udp_Server { get; } = 32566;
        /// <summary>
        /// 头像管理的服务器端口
        /// </summary>
        public static int Port_Portrait_Server { get; } = 24672;
        #endregion

        /// <summary>
        /// 整个系统的加密解密密码
        /// </summary>
        public const string Security = "qwertyui";
    }


    /// <summary>
    /// 选项类，包含了所有的标识和文本的对应关系
    /// </summary>
    public class BasicOptions
    {
        /// <summary>
        /// 测试，用于生成数据状态的信息存储
        /// </summary>
        public static readonly List<BasicOptions> test = new List<BasicOptions>()
        {
            new BasicOptions(0,"测试一"),
            new BasicOptions(1,"测试二"),
            new BasicOptions(2,"测试三"),
        };






        /// <summary>
        /// 实例化一个对象
        /// </summary>
        public BasicOptions()
        {

        }
        /// <summary>
        /// 根据信息实例化一个选项对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="des"></param>
        public BasicOptions(int code, string des)
        {
            IntegerCode = code;
            Description = des;
        }
        /// <summary>
        /// 整数的代号
        /// </summary>
        public int IntegerCode { get; set; } = 0;
        /// <summary>
        /// 代号描述的文本
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// 过去描述
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Description;
        }
    }
    
}
