using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    /// <summary>
    /// 所有的数据资源中心
    /// </summary>
    public class SoftResources
    {
        /// <summary>
        /// 字符串资源中心
        /// </summary>
        public class StringResouce
        {
            public static string SoftName { get; } = "你的软件系统";
            public static string SoftCopyRight { get; } = "版权归属人";



            public const string AccountLoadFailed = "新增账户失败";
            public const string AccountDeleteSuccess = "账户删除：";
            public const string AccountAddSuccess = "账户新增：";
            public const string AccountModifyPassword = "账户更改密码：";
            public const string AccountUploadPortrait = "账户更改头像：";
        }
    }
}
