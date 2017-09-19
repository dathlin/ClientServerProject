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

                    ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
                    {
                        // 上传文件MD5码
                        string SmallPortraitMD5 = ""; SoftBasic.CalculateFileMD5(path32);
                        string LargePortraitMD5 = ""; SoftBasic.CalculateFileMD5(path300);

                        try
                        {
                            SmallPortraitMD5 = SoftBasic.CalculateFileMD5(path32);
                            LargePortraitMD5 = SoftBasic.CalculateFileMD5(path300);
                        }
                        catch(Exception ex)
                        {
                            UserClient.LogNet.WriteException("获取文件MD5码失败：", ex);
                            MessageBox.Show("文件信息确认失败，请重新上传！");
                            return;
                        }

                        JObject json = new JObject
                        {
                            { UserAccount.UserNameText, new JValue(UserClient.UserAccount.UserName) },
                            { UserAccount.SmallPortraitText, new JValue(SmallPortraitMD5) },
                            { UserAccount.LargePortraitText, new JValue(LargePortraitMD5) }
                        };


                        OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(
                            CommonHeadCode.SimplifyHeadCode.上传头像MD5,
                            json.ToString());

                        if (result.IsSuccess)
                        {
                            if (result.Content.Substring(0, 2) == "成功")
                            {
                                UserClient.UserAccount.SmallPortraitMD5 = SmallPortraitMD5;
                                UserClient.UserAccount.LargePortraitMD5 = LargePortraitMD5;
                                // 成功上传MD5码
                                LoadUserSmallPortraint();
                            }
                            else
                            {
                                MessageBox.Show("上传头像失败！原因：" + result.Content);
                            }
                        }
                        else
                        {
                            MessageBox.Show("上传头像失败！原因：" + result.Message);
                        }

                    }), null);
                   
                }
                
            }
        }


        #endregion

        #region Load Portraint

        /// <summary>
        /// 加载小尺寸的头像操作，可以放到主线程
        /// </summary>
        public void LoadUserSmallPortraint()
        {
            // 先获取服务器端的MD5码
            string fileMd5 = UserClient.UserAccount.SmallPortraitMD5;
            if(string.IsNullOrEmpty(fileMd5))
            {
                // 服务器端没有文件，加载结束
                return;
            }

            // 获取本地MD5
            string fileName = FileSavePath + @"\" + PortraitSupport.SmallPortrait;
            if (System.IO.File.Exists(fileName))
            {
                // 本地存在文件
                string currentMd5 = SoftBasic.CalculateFileMD5(fileName);

                // 对比验证
                if (fileName == currentMd5)
                {
                    // 加载本地文件
                    LoadPic?.Invoke(fileName);
                    return;
                }
            }

            // 本地不存在文件或校验失败，需要重新下载
            OperateResult result = UserClient.Net_File_Client.DownloadFile(PortraitSupport.SmallPortrait,
                "Files",
                "Portrait",
                UserClient.UserAccount.UserName,
                null,
                fileName);

            if(result.IsSuccess)
            {
                // 下载成功
                LoadPic?.Invoke(fileName);
            }
            else
            {
                MessageBox.Show("头像从服务器下载失败，错误原因：" + result.Message);
            }
        }


        /// <summary>
        /// 加载大尺寸的头像方法，需要放到线程池中操作
        /// </summary>
        /// <param name="largeLoadAction"></param>
        public void LoadUserLargePortraint(Action<string> largeLoadAction)
        {
            // 先获取服务器端的MD5码
            string fileMd5 = UserClient.UserAccount.LargePortraitMD5;
            if (string.IsNullOrEmpty(fileMd5))
            {
                // 服务器端没有文件，加载结束
                return;
            }

            // 获取本地MD5
            string fileName = FileSavePath + @"\" + PortraitSupport.LargePortrait;
            if (System.IO.File.Exists(fileName))
            {
                // 本地存在文件，先进行加载，如果运算不一致，再重新加载
                largeLoadAction?.Invoke(fileName);
                string currentMd5 = null;
                try
                {
                     currentMd5 = SoftBasic.CalculateFileMD5(fileName);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("计算文件MD5码错误：" + ex.Message);
                    return;
                }

                // 对比验证
                if (fileMd5 == currentMd5)
                {
                    return;
                }
            }

            // 本地不存在文件或校验失败，需要重新下载
            OperateResult result = UserClient.Net_File_Client.DownloadFile(PortraitSupport.LargePortrait,
                "Files",
                "Portrait",
                UserClient.UserAccount.UserName,
                null,
                fileName);

            if (result.IsSuccess)
            {
                // 下载成功
                largeLoadAction?.Invoke(fileName);
            }
            else
            {
                MessageBox.Show("头像从服务器下载失败，错误原因：" + result.Message);
            }

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
