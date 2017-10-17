using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{

    public class ChineseLocalization : ILocalization
    {
        public string FormateDateTime { get; set; } = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string GeneralUniqueID { get; set; } = "唯一标识";
        /// <summary>
        /// 名称
        /// </summary>
        public string GeneralName { get; set; } = "名称";
        /// <summary>
        /// 描述
        /// </summary>
        public string GeneralDescription { get; set; } = "描述";
        public string GeneralAllowLoginList { get; set; } = "允许登录列表";

        #region 格式化文本

        public string FormateBetweenTwoSize { get; set; } = "选择的数量范围为 {0} - {1}";

        #endregion

        #region 复选框

        public string CheckBoxAllowUserMulti { get; set; } = "是否允许一个账户重复登录 (admin 除外)";
        public string CheckBoxAllowFrameLogin { get; set; } = "是否允许框架版本不匹配的客户端登录";
        public string CheckBoxTrustEnable { get; set; } = "是否强制启用指定客户端登录";

        #endregion

        #region 文件相关

        public string FileName { get; set; } = "文件名";
        public string FileSize { get; set; } = "文件大小";
        public string FileUploadTime { get; set; } = "上传时间";
        public string FileDownloading { get; set; } = "我的云端文件（下载中）";
        public string FileMy { get; set; } = "我的云端文件";
        public string FileMyListTitle { get; set; } = "个人文件，支持拖拽上传，双击下载";

        #endregion

        #region 按钮相关

        /// <summary>
        /// 确认
        /// </summary>
        public string ButtonEnsure { get; set; } = "确认";
        /// <summary>
        /// 新增
        /// </summary>
        public string ButtonAdd { get; set; } = "新增";
        /// <summary>
        /// 编辑
        /// </summary>
        public string ButtonEdit { get; set; } = "编辑";
        /// <summary>
        /// 删除
        /// </summary>
        public string ButtonDelete { get; set; } = "删除";
        /// <summary>
        /// 保存
        /// </summary>
        public string ButtonSave { get; set; } = "保存";
        /// <summary>
        /// 取消
        /// </summary>
        public string ButtonCancel { get; set; } = "取消";
        /// <summary>
        /// 上传
        /// </summary>
        public string ButtonUpload { get; set; } = "上传";
        /// <summary>
        /// 下载
        /// </summary>
        public string ButtonDownload { get; set; } = "下载";
        /// <summary>
        /// 删除选中项
        /// </summary>
        public string ButtonDeleteSelected { get; set; } = "删除选中项";
        /// <summary>
        /// 获取本机标识
        /// </summary>
        public string ButtonGetComputerID { get; set; } = "获取本机标识";
        /// <summary>
        /// 新增客户端标识
        /// </summary>
        public string ButtonAddComputerID { get; set; } = "新增客户端标识";
            

        #endregion

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
        public string AccountRoleAdd { get; set; } = "角色新增";
        public string AccountRoleEdit { get; set; } = "角色编辑";
        public string AccountRoleNameList { get; set; } = "角色名称列表";
        public string AccountRoleAccountList { get; set; } = "关联账户列表";
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
