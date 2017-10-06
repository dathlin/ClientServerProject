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
        /// 默认的语言选项
        /// </summary>
        public static ILocalization Localization = new ChineseLocalization();

        /// <summary>
        /// 设置系统的语言选项
        /// </summary>
        /// <param name="language">语言名称</param>
        public static void SettingLocalization(string language)
        {
            if(language.ToLower() == "chinese")
            {
                Localization = Chinese;
            }
            else
            {
                Localization = English;
            }
        }

        private static ILocalization Chinese = new ChineseLocalization();
        private static ILocalization English = new EnglishLocalization();
    }

    public interface ILocalization
    {
        string FormateDateTime { get; set; }

        string ButtonEnsure { get; set; }


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


        #endregion

        #region 配置相关

        string SettingsText { get; set; }
        string SettingsSystem { get; set; }
        string SettingsGeneral { get; set; }
        string SettingsAccountFactory { get; set; }
        string SettingsTrustClient { get; set; }
        string SettingsRoleAssign { get; set; }

        #endregion

    }



}
