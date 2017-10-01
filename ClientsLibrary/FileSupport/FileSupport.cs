using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientsLibrary.FileSupport
{


    /******************************************************************************
     * 
     *    时间： 2017年9月30日 22:11:21
     *    作者： Richard.Hu
     *    说明： 命名空间FileSupport中放置了一些文件操作的控件，可以用来方便的学习和
     *           操作文件上传下载。
     * 
     ******************************************************************************/


    #region File Support



    /// <summary>
    /// 放了一些支持文件操作的静态方法
    /// </summary>
    public class FileSupport
    {
        #region Get Render Icon


        /// <summary>
        /// 获取文件对象显示图标
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Bitmap GetFileIcon(string FileName)
        {
            string exc = "";
            int index = FileName.LastIndexOf('.');
            if (index > 0)
            {
                if (index < FileName.Length)
                {
                    exc = FileName.Substring(index + 1);
                }
            }
            return GetFileRenderIcon(exc);
        }

        /// <summary>
        /// 根据文件后缀选择需要对应显示的文件图标，包含了二十多种常用文件的图标
        /// </summary>
        /// <param name="exc">文件的扩展名</param>
        /// <returns>图形对象</returns>
        public static System.Drawing.Bitmap GetFileRenderIcon(string exc)
        {
            exc = exc.ToLower();
            if (exc.Contains("docx"))
            {
                return Properties.Resources.docx;
            }
            else if (exc.Contains("doc"))
            {
                return Properties.Resources.doc;
            }
            else if (exc.Contains("xls"))
            {
                return Properties.Resources.xls;
            }
            else if (exc.Contains("ppt"))
            {
                return Properties.Resources.ppt;
            }
            else if (
                exc.Contains("jpg") ||
                exc.Contains("png") ||
                exc.Contains("bmp") ||
                exc.Contains("jpeg"))
            {
                return Properties.Resources.image;
            }
            else if (exc.Contains("rar"))
            {
                return Properties.Resources.rar;
            }
            else if (exc.Contains("zip"))
            {
                return Properties.Resources.zip;
            }
            else if (exc.Contains("exe"))
            {
                return Properties.Resources.exe;
            }
            else if (exc.Contains("pdf"))
            {
                return Properties.Resources.pdf;
            }
            else if (exc.Contains("aiff"))
            {
                return Properties.Resources.aiff;
            }
            else if (exc.Contains("ai"))
            {
                return Properties.Resources.ai;
            }
            else if (exc.Contains("audio"))
            {
                return Properties.Resources.audio;
            }
            else if (exc.Contains("dll"))
            {
                return Properties.Resources.bin;
            }
            else if (exc.Contains("bin"))
            {
                return Properties.Resources.bin;
            }
            else if (exc.Contains("csv"))
            {
                return Properties.Resources.csv;
            }
            else if (exc.Contains("html"))
            {
                return Properties.Resources.html;
            }
            else if (exc.Contains("js"))
            {
                return Properties.Resources.js;
            }
            else if (exc.Contains("php"))
            {
                return Properties.Resources.php;
            }
            else if (exc.Contains("py"))
            {
                return Properties.Resources.py;
            }
            else if (exc.Contains("sql"))
            {
                return Properties.Resources.sql;
            }
            else if (exc.Contains("svg"))
            {
                return Properties.Resources.svg;
            }
            else if (exc.Contains("txt"))
            {
                return Properties.Resources.txt;
            }
            else if (exc.Contains("xml"))
            {
                return Properties.Resources.xml;
            }
            else if (exc.Contains("iso"))
            {
                return Properties.Resources.iso;
            }
            else if (exc.Contains("jar"))
            {
                return Properties.Resources.jar;
            }
            else if (exc.Contains("mp3"))
            {
                return Properties.Resources.mp3;
            }
            else if (exc.Contains("css"))
            {
                return Properties.Resources.css;
            }
            else if (exc.Contains("perl"))
            {
                return Properties.Resources.perl;
            }
            else if (exc.Contains("7z"))
            {
                return Properties.Resources._7z;
            }
            else if (exc.Contains("ttf"))
            {
                return Properties.Resources.ttf;
            }
            else if (exc.Contains("asp"))
            {
                return Properties.Resources.asp;
            }
            else
            {
                return Properties.Resources.file;
            }
        }
        
        #endregion
    }


    #endregion
}
