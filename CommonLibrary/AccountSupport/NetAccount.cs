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

        #region Constructor

        /// <summary>
        /// 实例化一个默认的构造函数
        /// </summary>
        public NetAccount( )
        {
            LoginTime = DateTime.Now;
        }

        #endregion

        #region Public Properties


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

        #endregion

        #region Public Method

        /// <summary>
        /// 获取当前客户端的在线时间的文本描述方式
        /// </summary>
        /// <returns></returns>
        public string GetOnlineTime( )
        {
            TimeSpan timeSpan = DateTime.Now - LoginTime;
            if (timeSpan.TotalSeconds < 60)
            {
                return timeSpan.Seconds.ToString( );
            }
            else if (timeSpan.TotalMinutes < 60)
            {
                return timeSpan.Minutes + " : " + timeSpan.Seconds;
            }
            else if (timeSpan.TotalHours < 24)
            {
                return timeSpan.Hours + " : " + timeSpan.Minutes + " : " + timeSpan.Seconds;
            }
            else
            {
                return timeSpan.Days + " D " + timeSpan.Hours + " : " + timeSpan.Minutes + " : ";
            }
        }

        #endregion

        #region Object Override

        /// <summary>
        /// 返回表示当前对象的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString( ) => $"{IpAddress} [ {(string.IsNullOrEmpty( Alias ) ? UserName : Alias)} ] [ {GetOnlineTime( )} ]";

        #endregion

    }
}
