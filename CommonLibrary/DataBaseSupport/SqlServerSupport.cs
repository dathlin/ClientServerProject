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
        /// <summary>
        /// 数据库的连接字符串，该信息应来源于服务器保存的连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }

    }
}
