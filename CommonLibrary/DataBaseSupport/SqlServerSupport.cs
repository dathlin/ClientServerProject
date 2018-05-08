using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.DataBaseSupport
{
    /// <summary>
    /// 使用SQL SERVER服务器的时的数据支持
    /// </summary>
    public class SqlServerSupport
    {
        #region Constructor

        /// <summary>
        /// 实例化一个默认的构造函数
        /// </summary>
        public SqlServerSupport( )
        {

        }

        #endregion


        /// <summary>
        /// 数据库的连接字符串，该信息应来源于服务器保存的连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }

    }
}
