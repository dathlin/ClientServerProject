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
    /// <summary>
    /// 头像管理类
    /// </summary>
    public class UserPortrait
    {
        public UserPortrait(string filePath,Action<string> loadPic)
        {
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            FileSavePath = filePath;
            LoadPic = loadPic;
        }

        private string FileSavePath { get; set; }

        private Action<string> LoadPic = null;
        
        public void ChangePortrait()
        {
            using (FormPortraitSelect fps = new FormPortraitSelect())
            {
                if (fps.ShowDialog() == DialogResult.OK)
                {
                    string path300 = FileSavePath + @"\" + PortraitSupport.LargePortrait;
                    string path32 = FileSavePath + @"\" + PortraitSupport.SmallPortrait;

                    Bitmap bitmap300 = fps.GetSpecifiedSizeImage(300);
                    bitmap300.Save(path300);
                    Bitmap bitmap32 = fps.GetSpecifiedSizeImage(32);
                    bitmap32.Save(path32);
                    //传送服务器
                    bitmap300.Dispose();
                    bitmap32.Dispose();

                    using (FormFileOperate ffo = new FormFileOperate(CommonHeadCode.KeyToken, new System.Net.IPEndPoint(
                        System.Net.IPAddress.Parse(UserClient.ServerIp), CommonLibrary.CommonLibrary.Port_Portrait_Server),
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
        
        public void ThreadPoolDownloadSizeLarge()
        {
            string path = FileSavePath;
            OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.下载大头, UserClient.UserAccount.UserName);
            if (result.IsSuccess)
            {
                if (result.Content[0] == 'Y')
                {
                    byte[] data = Convert.FromBase64String(result.Content.Substring(1));
                    string path32 = path + @"\" + PortraitSupport.LargePortrait;
                    System.IO.File.WriteAllBytes(path32, data);
                    System.Diagnostics.Process.Start(path32);
                }
            }
            Thread.Sleep(1000);
        }


    }
}
