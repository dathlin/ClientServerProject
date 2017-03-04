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


    
}
