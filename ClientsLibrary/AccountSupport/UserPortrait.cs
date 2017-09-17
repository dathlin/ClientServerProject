using CommonLibrary;
using HslCommunication;
using HslCommunication.BasicFramework;
using HslCommunication.Enthernet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ClientsLibrary
{

    /********************************************************************************
     * 
     *    时间：2017年9月17日 16:09:05
     *    用户头像类，负责更换头像，下载头像，以及初始化登录操作
     * 
     *********************************************************************************/



    /// <summary>
    /// 头像管理类
    /// </summary>
    public class UserPortrait
    {
        #region Constructor
        
        /// <summary>
        /// 实例化一个新的头像管理类对象
        /// </summary>
        /// <param name="filePath">头像存储的文件夹路径</param>
        /// <param name="loadPic">加载图片的方法</param>
        /// <param name="disPic">加载前的操作</param>
        public UserPortrait(string filePath, Action<string> loadPic, Action disPic)
        {
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            FileSavePath = filePath;
            LoadPic = loadPic;
            DisPic = disPic;
        }

        #endregion
        
        #region Change Portrait

        /// <summary>
        /// 点击更改头像后的操作，打开头像选择对话框，获取到2种分辨率的头像，然后进行上传
        /// </summary>
        public void ChangePortrait()
        {
            using (FormPortraitSelect fps = new FormPortraitSelect())
            {
                if (fps.ShowDialog() == DialogResult.OK)
                {
                    string path300 = FileSavePath + @"\" + PortraitSupport.LargePortrait;
                    string path32 = FileSavePath + @"\" + PortraitSupport.SmallPortrait;

                    DisPic?.Invoke();

                    Bitmap bitmap300 = fps.GetSpecifiedSizeImage(300);
                    bitmap300.Save(path300);
                    Bitmap bitmap32 = fps.GetSpecifiedSizeImage(32);
                    bitmap32.Save(path32);
                    //传送服务器
                    bitmap300.Dispose();
                    bitmap32.Dispose();

                    using (FormFileOperate ffo = new FormFileOperate(
                        UserClient.Net_File_Client, 
                        new string[]
                        {
                            path300,
                            path32
                        }, "Files", "Portrait", UserClient.UserAccount.UserName))
                    {
                        ffo.ShowDialog();
                    }
                    DownloadUserPortraint();
                }
            }
        }


        #endregion

        #region Download Portraint

        /// <summary>
        /// 加载头像信息，检查本地是否有相应的文件，没有则向服务器请求下载新的头像，然后加载，应放到线程池中执行
        /// </summary>
        public void DownloadUserPortraint()
        {
            string path = FileSavePath;
            //获取服务器文件名称
            OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.请求小头, UserClient.UserAccount.UserName);
            if (result.IsSuccess)
            {
                if (result.Content[0] == 'Y')
                {
                    //服务器存在头像
                    string fileName = path + @"\" + PortraitSupport.SmallPortrait;
                    string FileMd5 = result.Content.Substring(1);
                    if (System.IO.File.Exists(fileName))
                    {
                        //文件文件
                        string currentMd5 = SoftBasic.CalculateFileMD5(fileName);
                        if (currentMd5 == FileMd5)
                        {
                            //加载本地头像
                            LoadPic?.Invoke(fileName);
                        }
                        else
                        {
                            //头像已经换了
                            DownloadUserPortraint(path);
                        }
                    }
                    else
                    {
                        //客户端不存在头像
                        DownloadUserPortraint(path);
                    }
                }
                else
                {
                    //服务器不存在头像，本次加载结束
                }
            }
        }

        /// <summary>
        /// 下载小尺寸的头像并使用委托加载它
        /// </summary>
        /// <param name="path"></param>
        public void DownloadUserPortraint(string path)
        {

            OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.下载小头, UserClient.UserAccount.UserName);
            if (result.IsSuccess)
            {
                if (result.Content[0] == 'Y')
                {
                    byte[] data = Convert.FromBase64String(result.Content.Substring(1));
                    string path32 = path + @"\" + PortraitSupport.SmallPortrait;
                    System.IO.File.WriteAllBytes(path32, data);
                    LoadPic?.Invoke(path32);
                }
            }
        }

        /// <summary>
        /// 下载大尺寸的头像并打开它，该方法适合线程池
        /// </summary>
        public void ThreadPoolDownloadSizeLarge()
        {
            string path = FileSavePath;
            string path300 = path + @"\" + PortraitSupport.LargePortrait;

            // 利用客户端文件引擎去下载文件
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

            Thread.Sleep(1000);
        }


        #endregion

        #region Public Method
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetLargePortraitFileName()
        {
            return FileSavePath + @"\" + PortraitSupport.LargePortrait;
        }

        public string GetSmallPortraitFileName()
        {
            return FileSavePath + @"\" + PortraitSupport.SmallPortrait;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// 文件的路径
        /// </summary>
        private string FileSavePath { get; set; }
        /// <summary>
        /// 加载图片的操作
        /// </summary>
        private Action<string> LoadPic = null;
        /// <summary>
        /// 加载图片前的操作
        /// </summary>
        private Action DisPic = null;


        #endregion
    }
}
