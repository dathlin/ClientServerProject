using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
using HslCommunication.BasicFramework;




/*************************************************************************************
 * 
 *    时间：2017年9月3日 09:18:21
 *    说明：本文档主要包含一个主要内容，关于客户端和服务器的一些本地参数存储解析
 *    
 *    服务器：此处存储了系统版本号，公告，是否允许用户登录，分厂信息等等共享
 *    客户端：此处存储了用户名，密码，上次登录时间，机器码等等，用于客户端自我校验
 *    
 *    格式：存储采用json字符串存储，客户端还进行了双向加密，防止用户直接打开更改
 * 
 *************************************************************************************/









namespace CommonLibrary
{
    

    /// <summary>
    /// 服务器的常用参数保存，包含了版本号，公告，是否允许登录，不能登录说明
    /// </summary>
    public sealed class ServerSettings : SoftFileSaveBase
    {
        #region Public Property
        
        /// <summary>
        /// 系统的版本号，可以用来验证版本更新的依据
        /// 初始化1.0.0
        /// </summary>
        public SystemVersion SystemVersion { get; set; } = new SystemVersion("1.0.0");
        /// <summary>
        /// 系统的公告信息，默认为测试公告
        /// </summary>
        public string Announcement { get; set; } = "测试公告";
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string SqlServerStr { get; set; } = string.Empty;
        /// <summary>
        /// 是否允许账户登录，超级管理员账户除外
        /// </summary>
        public bool Can_Account_Login { get; set; } = true;
        /// <summary>
        /// 是否允许一个用户名重复登录系统
        /// </summary>
        public bool AllowUserMultiOnline { get; set; } = false;
        /// <summary>
        /// 当框架版本不对的时候，是否允许登录
        /// </summary>
        public bool AllowLoginWhenFramewordVersionNotCheck { get; set; } = false;
        /// <summary>
        /// 不允许登录系统的原因
        /// </summary>
        public string Account_Forbidden_Reason { get; set; } = "系统处于维护中，请稍后登录。";
        /// <summary>
        /// 系统的所有分厂信息
        /// </summary>
        public List<string> SystemFactories { get; set; } = new List<string>()
        {
            "分厂示例1","分厂示例2"
        };

        #endregion

        #region TrustedClientAuthentication
        
        /// <summary>
        /// 是否开启仅信任客户端验证
        /// </summary>
        public bool WhetherToEnableTrustedClientAuthentication { get; set; } = false;

        /// <summary>
        /// 信任的客户端列表
        /// </summary>
        public List<string> TrustedClientList { get; set; } = new List<string>();

        /// <summary>
        /// 列表锁
        /// </summary>
        private HslCommunication.Core.SimpleHybirdLock hybirdLock = new HslCommunication.Core.SimpleHybirdLock();

        /// <summary>
        /// 判断一个客户端的ID能否登录到系统
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public bool CanClientLogin(string machineId)
        {
            bool result = false;
            hybirdLock.Enter();
            result = TrustedClientList.Contains(machineId);
            hybirdLock.Leave();
            return result;
        }

        /// <summary>
        /// 新增一个客户端ID到信任列表中，新增成功True，原来已经存在False
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public bool AddTrustedClient(string machineId)
        {
            bool result = false;
            hybirdLock.Enter();
            if(!TrustedClientList.Contains(machineId))
            {
                TrustedClientList.Add(machineId);
                result = true;
            }
            hybirdLock.Leave();
            return result;
        }
        /// <summary>
        /// 从信任的列表中删除一个存在的客户端ID
        /// </summary>
        /// <param name="machineId"></param>
        public bool DeleteTrustedClient(string machineId)
        {
            bool result = false;
            hybirdLock.Enter();
            if (TrustedClientList.Contains(machineId))
            {
                TrustedClientList.Remove(machineId);
                result = true;
            }
            hybirdLock.Leave();
            return result;
        }
        

        #endregion
        
        #region Override Method


        /// <summary>
        /// 获取需要存储的数据
        /// </summary>
        /// <returns></returns>
        public override string ToSaveString()
        {
            JObject json = new JObject
            {
                { nameof(SystemVersion), new JValue(SystemVersion.ToString()) },
                { nameof(Announcement), new JValue(Announcement) },
                { nameof(Can_Account_Login), new JValue(Can_Account_Login) },
                { nameof(AllowUserMultiOnline),new JValue(AllowUserMultiOnline) },
                { nameof(Account_Forbidden_Reason), new JValue(Account_Forbidden_Reason) },
                { nameof(SystemFactories), new JArray(SystemFactories) },
                { nameof(WhetherToEnableTrustedClientAuthentication),new JValue(WhetherToEnableTrustedClientAuthentication) },
                { nameof(TrustedClientList),new JArray(TrustedClientList) },
                { nameof(AllowLoginWhenFramewordVersionNotCheck), new JValue(AllowLoginWhenFramewordVersionNotCheck) },
                { nameof(SqlServerStr), new JValue(SqlServerStr) },
            };
            return json.ToString();
        }
        /// <summary>
        /// 从字符串数据加载配置
        /// </summary>
        /// <param name="content"></param>
        public override void LoadByString(string content)
        {
            JObject json = JObject.Parse(content);
            SystemVersion = new SystemVersion(SoftBasic.GetValueFromJsonObject(json, nameof(SystemVersion), "1.0.0"));
            Announcement = SoftBasic.GetValueFromJsonObject(json, nameof(Announcement), Announcement);
            Can_Account_Login = SoftBasic.GetValueFromJsonObject(json, nameof(Can_Account_Login), Can_Account_Login);
            AllowUserMultiOnline = SoftBasic.GetValueFromJsonObject(json, nameof(AllowUserMultiOnline), AllowUserMultiOnline);
            Account_Forbidden_Reason = SoftBasic.GetValueFromJsonObject(json, nameof(Account_Forbidden_Reason), Account_Forbidden_Reason);
            AllowLoginWhenFramewordVersionNotCheck = SoftBasic.GetValueFromJsonObject(json, nameof(AllowLoginWhenFramewordVersionNotCheck), AllowLoginWhenFramewordVersionNotCheck);
            SqlServerStr = SoftBasic.GetValueFromJsonObject(json, nameof(SqlServerStr), SqlServerStr);


            if (json[nameof(SystemFactories)] != null)
            {
                SystemFactories = json[nameof(SystemFactories)].ToObject<List<string>>();
            }

            WhetherToEnableTrustedClientAuthentication = SoftBasic.GetValueFromJsonObject(json, nameof(WhetherToEnableTrustedClientAuthentication), false);

            if (json[nameof(TrustedClientList)] != null)
            {
                TrustedClientList = json[nameof(TrustedClientList)].ToObject<List<string>>();
            }

            ;
        }

        #endregion

    }


    /// <summary>
    /// 用户客户端存储本地JSON数据的类，包含了所有需要存储的信息
    /// </summary>
    public sealed class JsonSettings : SoftFileSaveBase
    {
        #region Constructor
        
        /// <summary>
        /// 实例化一个设置的对象
        /// </summary>
        public JsonSettings()
        {
            SystemInfo = SoftAuthorize.GetInfo();
        }

        #endregion

        #region 客户端本地保存的数据


        /// <summary>
        /// 指示系统是否是更新后第一次运行
        /// </summary>
        public bool IsNewVersionRunning { get; set; } = true;
        /// <summary>
        /// 上次系统登录的用户名
        /// </summary>
        public string LoginName { get; set; } = "";
        /// <summary>
        /// 上次系统登录的密码
        /// </summary>
        public string Password { get; set; } = "";
        /// <summary>
        /// 上次系统登录的时间
        /// </summary>
        public DateTime LoginTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 指示系统的主题色是否是深色，目前只适合于wpf
        /// </summary>
        public bool IsThemeDark { get; set; } = false;
        /// <summary>
        /// 本地保存的密码过期天数
        /// </summary>
        public int PasswordOverdueDays { get; set; } = 7;

        /// <summary>
        /// 当前计算机的机器码，用来判定参数是否是正确的
        /// </summary>
        public string SystemInfo { get; private set; }


        #endregion

        #region Override Method



        public override string ToSaveString()
        {
            JObject json = new JObject();
            json.Add(nameof(LoginName), new JValue(LoginName));
            json.Add(nameof(Password), new JValue(Password));
            json.Add(nameof(IsNewVersionRunning), new JValue(IsNewVersionRunning));
            json.Add(nameof(SystemInfo), new JValue(SystemInfo));
            json.Add(nameof(LoginTime), new JValue(LoginTime));
            json.Add(nameof(IsThemeDark), new JValue(IsThemeDark));
            json.Add(nameof(PasswordOverdueDays), new JValue(PasswordOverdueDays));
            return json.ToString();
        }

        public override void LoadByString(string content)
        {
            JObject json = JObject.Parse(content);
            string systemInfo = SoftBasic.GetValueFromJsonObject(json, nameof(SystemInfo), "");
            // 用户名不会因此更改
            LoginName = SoftBasic.GetValueFromJsonObject(json, nameof(LoginName), LoginName);

            if (systemInfo == SystemInfo)
            {
                //确认账户名及密码是本机的记录，而不是从其他电脑端拷贝过来的
                IsNewVersionRunning = SoftBasic.GetValueFromJsonObject(json, nameof(IsNewVersionRunning), IsNewVersionRunning);
                Password = SoftBasic.GetValueFromJsonObject(json, nameof(Password), Password);
                LoginTime = SoftBasic.GetValueFromJsonObject(json, nameof(LoginTime), LoginTime);
                IsThemeDark = SoftBasic.GetValueFromJsonObject(json, nameof(IsThemeDark), IsThemeDark);
                PasswordOverdueDays = SoftBasic.GetValueFromJsonObject(json, nameof(PasswordOverdueDays), PasswordOverdueDays);
            }
        }

        /// <summary>
        /// 使用指定的解密实现数据解密
        /// </summary>
        public override void LoadByFile()
        {
            LoadByFile(m => SoftSecurity.MD5Decrypt(m, UserSystem.Security));
        }
        /// <summary>
        /// 使用指定的加密实现数据加密
        /// </summary>
        public override void SaveToFile()
        {
            SaveToFile(m => SoftSecurity.MD5Encrypt(m, UserSystem.Security));
        }



        #endregion
    }
}
