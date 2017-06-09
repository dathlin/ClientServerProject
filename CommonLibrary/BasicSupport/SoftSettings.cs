using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
using HslCommunication.BasicFramework;

//2017-02-10

namespace CommonLibrary
{
    /// <summary>
    /// 服务器的常用参数保存，包含了版本号，公告，是否允许登录，不能登录说明
    /// </summary>
    public class ServerSettings : SoftFileSaveBase
    {
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
        /// 是否允许账户登录
        /// 超级管理员账户除外
        /// </summary>
        public bool Can_Account_Login { get; set; } = true;
        /// <summary>
        /// 不允许登录系统的原因
        /// </summary>
        public string Account_Forbidden_Reason { get; set; } = "系统处于维护中，请稍后登录。";

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
                { nameof(Account_Forbidden_Reason), new JValue(Account_Forbidden_Reason) }
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
            SystemVersion = new SystemVersion(json.Property(nameof(SystemVersion)).Value.Value<string>());
            Announcement = json.Property(nameof(Announcement)).Value.Value<string>();
            Can_Account_Login = SoftBasic.GetValueFromJsonObject(json, nameof(Can_Account_Login), Can_Account_Login);
            Account_Forbidden_Reason = SoftBasic.GetValueFromJsonObject(json, nameof(Account_Forbidden_Reason), Account_Forbidden_Reason);
        }

    }


    /// <summary>
    /// 用户客户端存储本地JSON数据的类，包含了所有需要存储的信息
    /// </summary>
    public class JsonSettings : SoftFileSaveBase
    {
        /// <summary>
        /// 实例化一个设置的对象
        /// </summary>
        public JsonSettings()
        {
            SystemInfo = SoftAuthorize.GetInfo();
        }
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
        /// 当前计算机的机器码，用来判定参数是否是正确的
        /// </summary>
        public string SystemInfo { get; private set; }






        public override string ToSaveString()
        {
            JObject json = new JObject();
            json.Add(nameof(LoginName), new JValue(LoginName));
            json.Add(nameof(Password), new JValue(Password));
            json.Add(nameof(IsNewVersionRunning), new JValue(IsNewVersionRunning));
            json.Add(nameof(SystemInfo), new JValue(SystemInfo));
            json.Add(nameof(LoginTime), new JValue(LoginTime));
            return json.ToString();
        }
        public override void LoadByString(string content)
        {
            JObject json = JObject.Parse(content);
            string systemInfo = SoftBasic.GetValueFromJsonObject(json, nameof(SystemInfo), "");
            if (systemInfo == SystemInfo)
            {
                //文件匹配正确
                LoginName = SoftBasic.GetValueFromJsonObject(json, nameof(LoginName), LoginName);
                IsNewVersionRunning = SoftBasic.GetValueFromJsonObject(json, nameof(IsNewVersionRunning), IsNewVersionRunning);
                Password = SoftBasic.GetValueFromJsonObject(json, nameof(Password), Password);
                LoginTime = SoftBasic.GetValueFromJsonObject(json, nameof(LoginTime), LoginTime);
            }
        }

        /// <summary>
        /// 使用指定的解密实现数据解密
        /// </summary>
        public override void LoadByFile()
        {
            LoadByFile(m => SoftSecurity.MD5Decrypt(m, CommonLibrary.Security));
        }
        public override void SaveToFile()
        {
            SaveToFile(m => SoftSecurity.MD5Encrypt(m, CommonLibrary.Security));
        }

    }
}
