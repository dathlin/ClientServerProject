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
using System.Net;
using ClientsLibrary.FileSupport;

namespace 软件系统客户端Wpf.Views.Controls
{
    /// <summary>
    /// UserFileRenderItem.xaml 的交互逻辑
    /// </summary>
    public partial class UserFileRenderItem : UserControl
    {
        #region Constructor

        
        public UserFileRenderItem(IntegrationFileClient client, string factory, string group, string id, Func<GroupFileItem, bool> deleteCheck)
        {
            InitializeComponent();

            DeleteCheck = deleteCheck;
            m_Factory = factory;
            m_Group = group;
            m_Id = id;
            fileClient = client;
        }


        #endregion

        #region Private Method


        private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();

            using (MemoryStream ms = new MemoryStream())
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

        #endregion

        #region Render File Information

        /// <summary>
        /// 设置文件数据
        /// </summary>
        /// <param name="file">文件的信息对象</param>
        /// <param name="deleteEnable">删除控件的使能委托</param>
        /// <exception cref="ArgumentNullException">file参数不能为空</exception>
        public void SetFile(GroupFileItem file, Func<bool> deleteEnable)
        {
            fileItem = file;


            // 设置文件图标
            FileIcon.Source = BitmapToBitmapImage(FileSupport.GetFileIcon(file.FileName));

            FileName.Text = "文件名称：" + file.FileName;
            FileSize.Text = "大小：" + file.GetTextFromFileSize();
            FileDate.Text = "日期：" + file.UploadTime.ToString("yyyy-MM-dd");
            FileDescription.Text = "文件备注：" + file.Description;
            FilePeople.Text = "上传人：" + file.Owner;
            FileDownloadTimes.Text = "下载数：" + file.DownloadTimes;


            FileDeleteButton.IsEnabled = deleteEnable.Invoke();
            FileDownloadButton.IsEnabled = true;                 // 一般都是允许下载，如果不允许下载，在此处设置
        }


        #endregion

        #region Delete Support

        private void FileDeleteButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 删除文件
            if (DeleteCheck != null)
            {
                // 删除的权限检查
                if (!DeleteCheck.Invoke(fileItem))
                {
                    // 没有通过
                    return;
                }
            }

            if (MessageBox.Show("请确认是否真的删除？", "删除确认", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }

            //确认删除
            OperateResult result = fileClient.DeleteFile(
                fileItem.FileName,                               // 文件的名称
                m_Factory,                                       // 第一大类
                m_Group,                                         // 第二大类
                m_Id                                             // 第三大类
                );
            if (result.IsSuccess)
            {
                MessageBox.Show("删除成功！");
            }
            else
            {
                MessageBox.Show("删除失败！原因：" + result.Message);
            }
        }

        #endregion

        #region Download Support


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

            save_file_name += "\\" + fileItem.FileName;


            OperateResult result = fileClient.DownloadFile(
                fileItem.FileName,
                m_Factory,
                m_Group,
                m_Id,
                (m, n) =>
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        FileDownloadProgress.Value = m * 100 / n;
                    }));
                }, 
                save_file_name
                );

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


        #endregion
        
        #region Private Members

        private IntegrationFileClient fileClient;                  // 进行文件操作的客户端
        private Func<GroupFileItem, bool> DeleteCheck;             // 删除操作时的检查方法
        private GroupFileItem fileItem;                            // 本控件关联显示的文件
        private string m_Factory;                                  // 文件的第一大类
        private string m_Group;                                    // 文件的第二大类
        private string m_Id;                                       // 文件的第三大类

        #endregion
    }
}
