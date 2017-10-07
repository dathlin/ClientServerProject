using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public class EnglishLocalization : ILocalization
    {
        public string FormateDateTime { get; set; } = "yyyy/MM/dd HH:mm:ss";
        public string ButtonEnsure { get; set; } = "Sure";



        #region 账户相关
        

        public string AccountSelect { get; set; } = "Select";

        public string AccountName { get; set; } = "Name";
        public string AccountAlias { get; set; } = "Alias";
        public string AccountPassword { get; set; } = "Password";
        public string AccountFactory { get; set; } = "Factory";  // 可以在此处修改为部门 
        public string AccountGrade { get; set; } = "Authority";
        public string AccountRegisterTime { get; set; } = "Register Time";
        public string AccountLoginEnable { get; set; } = "Login Enable";
        public string AccountForbidMessage { get; set; } = "Forbid Reason";
        public string AccountLoginFrequency { get; set; } = "Login Totle";
        public string AccountLastLoginTime { get; set; } = "Login Last Time";
        public string AccountLastLoginIpAddress { get; set; } = "Login Last Ip";
        public string AccountLoginFailedCount { get; set; } = "Login Failed Totle";
        public string AccountLastLoginWay { get; set; } = "Login Last Way";
        public string AccountPortrait { get; set; } = "Portrait";
        public string AccountDetails { get; set; } = "Account Details";
        public string AccountRegisterTitle { get; set; } = "Register a new account";


        #endregion


        #region 配置相关

        public string SettingsText { get; set; } = "System parameters settings";
        public string SettingsSystem { get; set; } = "System Settings";
        public string SettingsGeneral { get; set; } = "General";
        public string SettingsAccountFactory { get { return AccountFactory + " List"; } set { } }
        public string SettingsTrustClient { get; set; } = "Trust Client";
        public string SettingsRoleAssign { get; set; } = "Role Assign";
        public string SettingsRoleAddTitle { get; set; } = "Role Add New";
        public string SettingsRoleEditTitle { get; set; } = "Role Edit";


        #endregion

    }
}
