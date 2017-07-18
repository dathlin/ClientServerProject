using ClientsLibrary;
using HslCommunication.Enthernet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HslCommunication;
using System.Threading;
using System.IO;

namespace 软件系统客户端Wpf.Views
{
    /// <summary>
    /// UserFileRenderItem.xaml 的交互逻辑
    /// </summary>
    public partial class UserFileRenderItem : UserControl
    {
        public UserFileRenderItem()
        {
            InitializeComponent();
        }

        private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, bitmap.RawFormat);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }

            return bitmapImage;
        }

        private HslSoftFile Hufile { get; set; } = null;
        /// <summary>
        /// 设置文件数据
        /// </summary>
        /// <param name="file">文件的信息对象</param>
        /// <param name="deleteEnable">删除控件的使能委托</param>
        public void SetFile(HslSoftFile file)
        {
            Hufile = file;
            //获取后缀名
            int dotIndex = Hufile.FileName.LastIndexOf('.');
            if (dotIndex >= 0)
            {
                FileIcon.Source = BitmapToBitmapImage(Hufile.GetFileIcon());
            }

            FileName.Text = "文件名称：" + file.FileName;
            FileSize.Text = "大小：" + file.GetTextFromFileSize();
            FileDate.Text = "日期：" + file.UploadDate.ToString("yyyy-MM-dd");
            FileDescription.Text = "文件备注：" + file.FileNote;
            FilePeople.Text = "上传人：" + file.UploadName;
            FileDownloadTimes.Text = "下载数：" + file.FileDownloadTimes;
            

            FileDeleteButton.IsEnabled = file.UploadName == UserClient.UserAccount.UserName;
            FileDownloadButton.IsEnabled = true;
        }

        private void FileDeleteButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //删除文件
            if (Hufile.UploadName != UserClient.UserAccount.UserName)
            {
                MessageBox.Show("无法删除不是自己上传的文件。");
                return;
            }
            if (MessageBox.Show("请确认是否真的删除？", "删除确认", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }

            //确认删除
            OperateResultString result = SimpleFileClient.DeleteFile(UserClient.ServerIp, CommonLibrary.CommonLibrary.Port_Share_File, Hufile.FileName);
            if(result.IsSuccess)
            {
                MessageBox.Show("删除成功！");
            }
            else
            {
                MessageBox.Show("删除失败！原因：" + result.Message);
            }
        }

        private void FileDownloadButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //下载文件
            FileDownloadButton.IsEnabled = false;

            Thread thread_down_file = new Thread(new ThreadStart(ThreadDownloadFile));
            thread_down_file.IsBackground = true;
            thread_down_file.Start();
        }
        private void ThreadDownloadFile()
        {
            string save_file_name = AppDomain.CurrentDomain.BaseDirectory + "download\\files";
            if (!Directory.Exists(save_file_name))
            {
                Directory.CreateDirectory(save_file_name);
            }

            save_file_name += "\\" + Hufile.FileName;


            OperateResultString result = SimpleFileClient.DownloadFile(UserClient.ServerIp, CommonLibrary.CommonLibrary.Port_Share_File, Hufile.FileName,
                (m, n) =>
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        FileDownloadProgress.Value = m * 100d / n;
                    }));
                }, save_file_name);

            Dispatcher.Invoke(new Action(() =>
            {
                if (result.IsSuccess)
                {
                    if (MessageBox.Show("下载完成，路径为：" + save_file_name + Environment.NewLine +
                        "是否打开文件路径？", "打开确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", @"/select," + save_file_name);
                    }
                }
                else
                {
                    MessageBox.Show("下载失败，错误原因：" + result.Message);
                }
                FileDownloadButton.IsEnabled = true;
            }));
        }
    }
}
