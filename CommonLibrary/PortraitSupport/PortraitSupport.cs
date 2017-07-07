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
        public const string SmallPortrait = "Size_32_.png";
        /// <summary>
        /// 大尺寸头像
        /// </summary>
        public const string LargePortrait = "Size_300_.png";


        public static string GetSmallPortraitFileName(string dirPath)
        {
            string[] files = Directory.GetFiles(dirPath);
            foreach(var m in files)
            {
                if(m.Contains(SmallPortrait))
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
                if (m.Contains(LargePortrait))
                {
                    return m;
                }
            }
            return string.Empty;
        }
    }
}
