using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonLibrary
{
    public class PortraitSupport
    {
        /// <summary>
        /// 小尺寸头像
        /// </summary>
        public const string SmallPortraitHead = "Size_32_";
        /// <summary>
        /// 大尺寸头像
        /// </summary>
        public const string LargePortraitHead = "Size_300_";


        public static string GetSmallPortraitFileName(string dirPath)
        {
            string[] files = Directory.GetFiles(dirPath);
            foreach(var m in files)
            {
                if(m.Contains(SmallPortraitHead))
                {
                    return m;
                }
            }
            return string.Empty;
        }
        public static string GetLargePortraitFileName(string dirPath)
        {
            string[] files = Directory.GetFiles(dirPath);
            foreach (var m in files)
            {
                if (m.Contains(LargePortraitHead))
                {
                    return m;
                }
            }
            return string.Empty;
        }
    }
}
