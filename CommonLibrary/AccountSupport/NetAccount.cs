using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    /// <summary>
    /// 用于网络账户的在线情况监视
    /// </summary>
    public class NetAccount
    {
        /// <summary>
        /// 唯一的用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 工厂或是部分分类
        /// </summary>
        public string Factory { get; set; }
        /// <summary>
        /// 登陆时间
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 包含的角色名称
        /// </summary>
        public string[] Roles { get; set; }
        /// <summary>
        /// 本地连接唯一的身份标识
        /// </summary>
        public string UniqueId { get; set; }
    }
}
