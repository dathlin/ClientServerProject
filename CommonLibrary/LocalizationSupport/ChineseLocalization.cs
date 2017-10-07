using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{

    public class ChineseLocalization : ILocalization
    {
        public string FormateDateTime { get; set; } = "yyyy-MM-dd HH:mm:ss";
        public string ButtonEnsure { get; set; } = "确认";


        #region 账户相关


        public string AccountSelect { get; set; } = "选择";

        public string AccountName { get; set; } = "用户名";
        public string AccountAlias { get; set; } = "别名";
        public string AccountPassword { get; set; } = "密码";
        public string AccountFactory { get; set; } = "分厂";  // 可以在此处修改为部门 
        public string AccountGrade { get; set; } = "权限";
        public string AccountRegisterTime { get; set; } = "注册时间";
        public string AccountLoginEnable { get; set; } = "是否允许登录";
        public string AccountForbidMessage { get; set; } = "禁止登录原因";
        public string AccountLoginFrequency { get; set; } = "总登录次数";
        public string AccountLastLoginTime { get; set; } = "上次登录时间";
        public string AccountLastLoginIpAddress { get; set; } = "上次登录地址";
        public string AccountLoginFailedCount { get; set; } = "登录失败次数";
        public string AccountLastLoginWay { get; set; } = "上次登录方式";
        public string AccountPortrait { get; set; } = "头像";
        public string AccountDetails { get; set; } = "账户详细信息";
        public string AccountRegisterTitle { get; set; } = "注册一个新的账户";
        
        #endregion


        #region 配置相关

        public string SettingsText { get; set; } = "配置系统的参数";
        public string SettingsSystem { get; set; } = "系统相关";
        public string SettingsGeneral { get; set; } = "常规配置";
        public string SettingsAccountFactory { get { return "配置" + AccountFactory + "信息"; } set { } }
        public string SettingsTrustClient { get; set; } = "信任客户端列表";
        public string SettingsRoleAssign { get; set; } = "配置角色信息";
        public string SettingsRoleAddTitle { get; set; } = "新增角色信息";
        public string SettingsRoleEditTitle { get; set; } = "编辑当前角色信息";

        #endregion
    }
}
