using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicFramework;

//=========================================================================================
//
//    模版说明：使用BasicFramework框架和网络框架实现了服务器模版，包含了基础的创建操作
//
//=========================================================================================



namespace 软件系统服务端模版
{
    /// <summary>
    /// 服务器类，存储系统运行的静态参数
    /// </summary>
    public class UserServer
    {
        /// <summary>
        /// 版本号，公告，是否允许登录，不能登录存储
        /// </summary>
        public static ServerSettings ServerSettings { get; set; } = new ServerSettings();

        /// <summary>
        /// 所有账户信息的存储对象，具体的账户类可以根据UserAccount进行扩充
        /// </summary>
        public static ServerAccounts<UserAccount> ServerAccounts { get; set; } = new ServerAccounts<UserAccount>(
            new List<UserAccount>() {
                //示例：新增一个默认的超级管理员
                new UserAccount()
                {
                    UserName="admin",
                    Password="123456",
                    Factory="总公司",
                    RegisterTime=DateTime.Now,
                    LastLoginTime=DateTime.Now,
                    LoginEnable=true,
                    Grade=AccountGrade.SuperAdministrator,
                    ForbidMessage="该帐号已被停用",
                    LoginFrequency=0,
                    LastLoginIpAddress="",
                }
            });

    }

    /// <summary>
    /// 一个扩展的用户账户示例，代替服务器和客户端的账户类即可
    /// </summary>
    public class UserAccountEx : UserAccount
    {
        /// <summary>
        /// 示例，扩展一个手机号的属性
        /// </summary>
        public string Phone { get; set; } = "";
        public override void DeepCopy<T>(T account)
        {
            base.DeepCopy<T>(account);
            UserAccountEx accountex = account as UserAccountEx;
            if (accountex != null)
            {
                //所有新增的属性在此进行复制
                accountex.Phone = Phone;
            }
        }
    }
}
