using ClientsLibrary.FileSupport;
using CommonLibrary;
using HslCommunication;
using HslCommunication.BasicFramework;
using HslCommunication.Enthernet;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HslCommunication.Core;

namespace ClientsLibrary
{

    /********************************************************************************
     * 
     *    时间：2017年9月17日 16:09:05
     *    用户头像类，负责更换头像，下载头像，以及初始化登录操作
     *    说明：类头像类还没有代码重构好，加载部分块还有部分重复代码，也因为.Net3.5这部分的代码异步比较难处理
     * 
     *********************************************************************************/



    /// <summary>
    /// 头像管理器，负责所有小头像数据的缓存
    /// </summary>
    public class UserPortrait
    {
        #region Constructor

        /// <summary>
        /// 实例化一个新的头像管理类对象
        /// </summary>
        /// <param name="filePath">头像存储的文件夹路径</param>
        /// <param name="loadPicSmall">加载头像的委托</param>
        public UserPortrait(string filePath)
        {
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            FileSavePath = filePath;
            
        }

        #endregion
        
        #region Download Portraint


        /// <summary>
        /// 下载大尺寸的头像并打开它，该方法适合线程池
        /// </summary>
        public void ThreadPoolDownloadSizeLarge()
        {
            string path = FileSavePath;
            string path300 = path + @"\" + PortraitSupport.LargePortrait;


            if (string.IsNullOrEmpty(UserClient.UserAccount.LargePortraitMD5))
            {
                // 服务器不存在头像
                return;
            }

            if (System.IO.File.Exists(path300))
            {
                // 验证文件MD5码是否一致
                string currentMd5 = SoftBasic.CalculateFileMD5(path300);

                if (UserClient.UserAccount.LargePortraitMD5 == currentMd5)
                {
                    System.Diagnostics.Process.Start(path300);
                    // 避免屏幕闪烁
                    Thread.Sleep(1000);
                    return;
                }
            }

            // 验证不一致时需要利用客户端文件引擎去下载文件
            OperateResult operateResult = UserClient.Net_File_Client.DownloadFile(
                PortraitSupport.LargePortrait,
                "Files",
                "Portrait",
                UserClient.UserAccount.UserName,
                null,
                path300
                );


            if(operateResult.IsSuccess)
            {
                System.Diagnostics.Process.Start(path300);
            }

            // 避免屏幕闪烁
            Thread.Sleep(1000);
        }


        #endregion

        #region Pricvate Method

        private void SetSmallPortrait(string name,Bitmap bitmap)
        {
            hybirdLock.Enter();

            if(dictSmallPortrait.ContainsKey(name))
            {
                dictSmallPortrait[name] = bitmap;
            }
            else
            {
                dictSmallPortrait.Add(name, bitmap);
            }

            hybirdLock.Leave();
        }

        private Bitmap DownloadSmallPortraitByName(string name)
        {
            OperateResult result = UserClient.Net_File_Client.DownloadFile(
                    PortraitSupport.SmallPortrait,
                    "Files",
                    "Portrait",
                    name,
                    null,
                    out Bitmap bitmap);
            if (result.IsSuccess)
            {
                SetSmallPortrait(name, bitmap);
                return bitmap;
            }
            else
            {
                return Properties.Resources.person_1;
            }
        }

        public void UpdateSmallPortraitByName(string name)
        {
            DownloadSmallPortraitByName(name);
        }

        public Bitmap GetSmallPortraitByUserName(string name)
        {
            Bitmap bitmap = null;
            hybirdLock.Enter();
            if (dictSmallPortrait.ContainsKey(name))
            {
                bitmap = dictSmallPortrait[name];
            }
            hybirdLock.Leave();

            if (bitmap != null) return bitmap;

            return DownloadSmallPortraitByName(name);
        }

        #endregion

        #region Private Members

        /// <summary>
        /// 文件的路径
        /// </summary>
        private string FileSavePath { get; set; }

        private Dictionary<string, Bitmap> dictSmallPortrait = new Dictionary<string, Bitmap>();
        private SimpleHybirdLock hybirdLock = new SimpleHybirdLock();

        #endregion
    }
}
