using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public class EnglishLocalization : ILocalization
    {
        public string FormateDateTime { get; set; } = "yyyy/MM/dd HH:mm:ss";
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string GeneralUniqueID { get; set; } = "Unique ID";
        /// <summary>
        /// 名称
        /// </summary>
        public string GeneralName { get; set; } = "Name";
        /// <summary>
        /// 描述
        /// </summary>
        public string GeneralDescription { get; set; } = "Descrition";
        public string GeneralAllowLoginList { get; set; } = "Allow login list";


        #region 复选框

        public string CheckBoxAllowUserMulti { get; set; } = "Allow an account to log if online (except admin)";
        public string CheckBoxAllowFrameLogin { get; set; } = "Allows the client to log in if the framework version does not match";
        public string CheckBoxTrustEnable { get; set; } = "Enable the specified client login";

        #endregion

        #region 按钮相关

        /// <summary>
        /// 确认
        /// </summary>
        public string ButtonEnsure { get; set; } = "Sure";
        /// <summary>
        /// 新增
        /// </summary>
        public string ButtonAdd { get; set; } = "Add";
        /// <summary>
        /// 编辑
        /// </summary>
        public string ButtonEdit { get; set; } = "Edit";
        /// <summary>
        /// 删除
        /// </summary>
        public string ButtonDelete { get; set; } = "Delete";
        /// <summary>
        /// 保存
        /// </summary>
        public string ButtonSave { get; set; } = "Save";
        /// <summary>
        /// 取消
        /// </summary>
        public string ButtonCancel { get; set; } = "Cancel";
        /// <summary>
        /// 删除选中项
        /// </summary>
        public string ButtonDeleteSelected { get; set; } = "Delete Selected";
        /// <summary>
        /// 获取本机标识
        /// </summary>
        public string ButtonGetComputerID { get; set; } = "Get ComputerID";
        /// <summary>
        /// 新增客户端标识
        /// </summary>
        public string ButtonAddComputerID { get; set; } = "Add ComputerID";

        #endregion

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
        public string AccountRoleAdd { get; set; } = "Role add new";
        public string AccountRoleEdit { get; set; } = "Role edit";
        public string AccountRoleNameList { get; set; } = "Roles List";
        public string AccountRoleAccountList { get; set; } = "Account of selected role";
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
