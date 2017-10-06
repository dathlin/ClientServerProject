using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            new List<UserAccount>()
            {
                // 示例：新增一个默认的超级管理员
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
            }
        );

        /// <summary>
        /// 角色信息管理器
        /// </summary>
        public static RoleAssign ServerRoles { get; set; } = new RoleAssign();

    }

    /// <summary>
    /// 一个扩展的用户账户示例，代替服务器和客户端的账户类即可
    /// 也可以直接在源代码中扩展
    /// </summary>
    public class UserAccountEx : UserAccount
    {
        /// <summary>
        /// 用户的年龄
        /// </summary>
        public int Age { get; set; } = 0;
        /// <summary>
        /// 用户的家庭住址
        /// </summary>
        public string HomeAddress { get; set; } = "";
    }
}
