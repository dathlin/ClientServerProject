using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{

    /*******************************************************************************************
     * 
     *    说明：本地化策略机制，优先完成中文版适配
     * 
     *******************************************************************************************/

    /// <summary>
    /// 整个软件系统的本地化策略
    /// </summary>
    public static class UserLocalization
    {

        /// <summary>
        /// 设置系统的语言选项
        /// </summary>
        /// <param name="language">语言名称</param>
        public static void SettingLocalization(string language)
        {
            if(language.ToLower() == "chinese")
            {
                localization = Chinese;
            }
            else
            {
                localization = English;
            }
        }

        /// <summary>
        /// 获取当前指定的语言选项信息
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static ILocalization GetSpecifiedLocalization(string language)
        {
            if (language.ToLower() == "chinese")
            {
                return Chinese;
            }
            else
            {
                return English;
            }
        }


        /// <summary>
        /// 默认的语言选项
        /// </summary>
        public static ILocalization Localization
        {
            get { return localization; }
        }



        private static ILocalization Chinese = new ChineseLocalization();   // 中文语言
        private static ILocalization English = new EnglishLocalization();   // 英文语言
        private static ILocalization localization = Chinese;                // 当前语言
    }

    /// <summary>
    /// 所有支持的语言必须从本接口继承
    /// </summary>
    public interface ILocalization
    {
        string FormateDateTime { get; set; }


        /// <summary>
        /// 唯一标识
        /// </summary>
        string GeneralUniqueID { get; set;}
        /// <summary>
        /// 名称
        /// </summary>
        string GeneralName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        string GeneralDescription { get; set; }
        /// <summary>
        /// 允许登录列表
        /// </summary>
        string GeneralAllowLoginList { get; set; }


        #region 格式化文本

        string FormateBetweenTwoSize { get; set; }

        #endregion

        #region 复选框

        string CheckBoxAllowUserMulti { get; set; }
        string CheckBoxAllowFrameLogin { get; set; }
        string CheckBoxTrustEnable { get; set; }

        #endregion

        #region 文件相关

        string FileName { get; set; }
        string FileSize { get; set; }
        string FileUploadTime { get; set; }
        string FileDownloading { get; set; }
        string FileMy { get; set; }
        string FileMyListTitle { get; set; }

        #endregion

        #region 按钮相关

        /// <summary>
        /// 确认
        /// </summary>
        string ButtonEnsure { get; set; }
        /// <summary>
        /// 新增
        /// </summary>
        string ButtonAdd { get; set; }
        /// <summary>
        /// 编辑
        /// </summary>
        string ButtonEdit { get; set; }
        /// <summary>
        /// 删除
        /// </summary>
        string ButtonDelete { get; set; }
        /// <summary>
        /// 保存
        /// </summary>
        string ButtonSave { get; set; }
        /// <summary>
        /// 取消
        /// </summary>
        string ButtonCancel { get; set; }
        /// <summary>
        /// 上传
        /// </summary>
        string ButtonUpload { get; set; }
        /// <summary>
        /// 下载
        /// </summary>
        string ButtonDownload { get; set; }
        /// <summary>
        /// 删除选中项
        /// </summary>
        string ButtonDeleteSelected { get; set; }
        /// <summary>
        /// 获取本机标识
        /// </summary>
        string ButtonGetComputerID { get; set; }
        /// <summary>
        /// 新增客户端标识
        /// </summary>
        string ButtonAddComputerID { get; set; }

        #endregion

        #region 账户相关


        string AccountSelect { get; set; }
        string AccountName { get; set; }
        string AccountAlias { get; set; }
        string AccountPassword { get; set; }
        string AccountFactory { get; set; }
        string AccountGrade { get; set; }
        string AccountRegisterTime { get; set; }
        string AccountLoginEnable { get; set; }
        string AccountForbidMessage { get; set; }
        string AccountLoginFrequency { get; set; }
        string AccountLastLoginTime { get; set; }
        string AccountLastLoginIpAddress { get; set; }
        string AccountLoginFailedCount { get; set; }
        string AccountLastLoginWay { get; set; }
        string AccountPortrait { get; set; }
        string AccountDetails { get; set; }
        string AccountRegisterTitle { get; set; }
        string AccountRoleAdd { get; set; }
        string AccountRoleEdit { get; set; }
        string AccountRoleNameList { get; set; } // 角色名称列表
        string AccountRoleAccountList { get; set; } // 关联账户列表

        #endregion

        #region 配置相关

        string SettingsText { get; set; }
        string SettingsSystem { get; set; }
        string SettingsGeneral { get; set; }
        string SettingsAccountFactory { get; set; }
        string SettingsTrustClient { get; set; }
        string SettingsRoleAssign { get; set; }
        string SettingsRoleAddTitle { get; set; }
        string SettingsRoleEditTitle { get; set; }

        #endregion

    }



}
