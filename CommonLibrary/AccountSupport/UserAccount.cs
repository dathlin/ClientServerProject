using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;


namespace CommonLibrary
{
    /// <summary>
    ///  本系統的用账户类，包含了一些常用的数据信息，如果你想添加额外属性，请继承此类
    /// </summary>
    public class UserAccount
    {
        #region Public Members
        
        /// <summary>
        /// 用户名称，该名称唯一
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
        /// 用户名称的别名，也不可以不使用
        /// </summary>
        public string NameAlias { get; set; } = "";
        /// <summary>
        /// 用户登录的密码
        /// </summary>
        public string Password { get; set; } = "";
        /// <summary>
        /// 账户所属的工厂名称或类别名称
        /// </summary>
        public string Factory { get; set; } = "";
        /// <summary>
        /// 用户的权限等级，目前配置了4个等级
        /// </summary>
        public int Grade { get; set; } = 0;
        /// <summary>
        /// 用户的手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 用户的电子邮件
        /// </summary>
        public string EMail { get; set; }
        /// <summary>
        /// 该用户的注册日期，一旦注册，应该固定
        /// </summary>
        public DateTime RegisterTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 该用户是否允许登录
        /// </summary>
        public bool LoginEnable { get; set; } = false;
        /// <summary>
        /// 该用户不允许被登录的原因
        /// </summary>
        public string ForbidMessage { get; set; } = "该账户被管理员禁止登录！";
        /// <summary>
        /// 该用户自注册以来登录的次数
        /// </summary>
        public int LoginFrequency { get; set; } = 0;
        /// <summary>
        /// 该用户上次登录的时间
        /// </summary>
        public DateTime LastLoginTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 该用户上次登录的IP地址
        /// </summary>
        public string LastLoginIpAddress { get; set; } = "";
        /// <summary>
        /// 该用户连续登录失败的计数，可以用来连续五次失败禁止账户登录
        /// </summary>
        public int LoginFailedCount { get; set; } = 0;
        /// <summary>
        /// 上次登录系统的方式，有winform版，wpf版，web版，Android版
        /// </summary>
        public string LastLoginWay { get; set; } = string.Empty;
        /// <summary>
        /// 小尺寸头像的MD5码
        /// </summary>
        public string SmallPortraitMD5 { get; set; } = string.Empty;
        /// <summary>
        /// 大尺寸头像的MD5码
        /// </summary>
        public string LargePortraitMD5 { get; set; } = string.Empty;

        #endregion

        #region Static Members

        /// <summary>
        /// 用于存储和传送的名称
        /// </summary>
        public static string UserNameText { get { return "UserName"; } }
        /// <summary>
        /// 用于存储和传送的名称
        /// </summary>
        public static string PasswordText { get { return "Password"; } }
        /// <summary>
        /// 用于存储和传送的名称
        /// </summary>
        public static string LoginWayText { get { return "LoginWay"; } }
        /// <summary>
        /// 登录系统的唯一设备ID
        /// </summary>
        public static string DeviceUniqueID { get { return "DeviceUniqueID"; } }
        /// <summary>
        /// 小尺寸头像的MD5传送名称
        /// </summary>
        public static string SmallPortraitText { get { return "SmallPortrait"; } }
        /// <summary>
        /// 大尺寸头像的MD5传送名称
        /// </summary>
        public static string LargePortraitText { get { return "LargePortrait"; } }
        /// <summary>
        /// 系统的框架版本，框架版本不一致，由服务器决定是否允许客户端登录
        /// </summary>
        public static string FrameworkVersion { get { return "FrameworkVersion"; } }

        #endregion

        #region Public Method

        /// <summary>
        /// 获取本账号的JSON字符串，用于在网络中数据传输
        /// </summary>
        /// <returns></returns>
        public string ToJsonString()
        {
            return JObject.FromObject(this).ToString();
        }

        /// <summary>
        /// 深度拷贝当前的账户信息
        /// </summary>
        /// <typeparam name="T">返回的类型，应该为继承后的类型</typeparam>
        /// <returns>新的对象</returns>
        public T DeepCopy<T>() where T : UserAccount
        {
            return JObject.FromObject(this).ToObject<T>();
        }

        #endregion

        #region Override Method

        /// <summary>
        /// 获取账号的用户名
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return UserName;
        }

        #endregion
        
    }
    
    /// <summary>
    /// 账户的等级
    /// </summary>
    public class AccountGrade
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        public static int SuperAdministrator { get; private set; } = 10000;
        /// <summary>
        /// 管理员
        /// </summary>
        public static int Admin { get; private set; } = 1000;
        /// <summary>
        /// 技术员
        /// </summary>
        public static int Technology { get; private set; } = 100;
        /// <summary>
        /// 一般
        /// </summary>
        public static int General { get; private set; } = 10;

        /// <summary>
        /// 获取对应等级的文本描述
        /// </summary>
        /// <param name="grade">等级数据</param>
        /// <returns>等级描述</returns>
        public static string GetDescription(int grade)
        {
            if (grade >= SuperAdministrator)
            {
                return "超级管理员";
            }
            else if (grade >= Admin)
            {
                return "管理员";
            }
            else if (grade >= Technology)
            {
                return "技术员";
            }
            else
            {
                return "普通";
            }
        }
        /// <summary>
        /// 获取权限的数组
        /// </summary>
        /// <returns></returns>
        public static string[] GetDescription()
        {
            return new string[]
            {
                GetDescription(SuperAdministrator),
                GetDescription(Admin),
                GetDescription(Technology),
                GetDescription(General),
            };
        }
        
    }

     
}
