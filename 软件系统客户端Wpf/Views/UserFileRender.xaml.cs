using ClientsLibrary;
using CommonLibrary;
using HslCommunication;
using HslCommunication.Enthernet;
using Newtonsoft.Json.Linq;
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

namespace 软件系统客户端Wpf.Views
{
    /// <summary>
    /// UserFileRender.xaml 的交互逻辑
    /// </summary>
    public partial class UserFileRender : UserControl
    {
        public UserFileRender()
        {
            InitializeComponent();
        }

        private void FileSearchFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            //搜索时触发的数据
            if (!string.IsNullOrEmpty(FileSearchFilter.Text))
            {
                string pattern = FileSearchFilter.Text;
                SetFilesShow(Cache_Files.Where(f =>
                f.FileName.Contains(pattern) ||
                f.FileNote.Contains(pattern) ||
                f.UploadName.Contains(pattern)).ToList());
            }
            else
            {
                SetFilesShow(Cache_Files);
            }
        }

        private void Button_FileUpload_Click(object sender, RoutedEventArgs e)
        {
            //上传数据，先对权限进行验证
            if (UserClient.UserAccount.Grade < AccountGrade.Technology)
            {
                MessageBox.Show("权限不够！");
                return;
            }

            using (FormSimplyFileUpload upload = new FormSimplyFileUpload(
                CommonHeadCode.KeyToken,
                UserClient.LogNet,
                UserClient.ServerIp,
                CommonLibrary.CommonLibrary.Port_Share_File,
                UserClient.UserAccount.UserName))
            {
                upload.ShowDialog();
            }
        }

        private void Button_FileRefresh_Click(object sender, RoutedEventArgs e)
        {
            //向服务器请求数据
            OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.请求文件);
            if (result.IsSuccess)
            {
                Cache_Files = JArray.Parse(result.Content).ToObject<List<HslSoftFile>>();
                SetFilesShow(Cache_Files);
            }
            else
            {
                MessageBox.Show(result.ToMessageShowString());
            }
        }


        public void UpdateFiles()
        {
            Button_FileRefresh_Click(null, new RoutedEventArgs());
        }

        private void ClearControls()
        {
            FileListControl.Children.Clear();
            //while (FilesControls.Count > 0)
            //{
            //    FilesControls.Pop().Dispose();
            //}
        }

        private void SetFilesShow(List<HslSoftFile> files)
        {
            //清楚缓存
            ClearControls();
            if (files?.Count > 0 && FileListControl.ActualWidth > 20)
            {
                //添加子控件
                foreach (var m in files)
                {
                    UserFileRenderItem item = new UserFileRenderItem();
                    FileListControl.Children.Add(item);
                    item.SetFile(m);
                }
            }
        }

        /// <summary>
        /// 所有文件信息的缓存，以支持直接的搜索
        /// </summary>
        private List<HslSoftFile> Cache_Files { get; set; } = new List<HslSoftFile>();
        /// <summary>
        /// 文件控件的缓存列表，方便清除垃圾
        /// </summary>
        private Stack<IDisposable> FilesControls = new Stack<IDisposable>();

    }
}
