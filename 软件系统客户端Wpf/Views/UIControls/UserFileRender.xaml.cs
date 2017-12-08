using ClientsLibrary;
using ClientsLibrary.FileSupport;
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
using 软件系统客户端Wpf.Views.Controls;

namespace 软件系统客户端Wpf.Views
{
    /// <summary>
    /// UserFileRender.xaml 的交互逻辑
    /// </summary>
    public partial class UserFileRender : UserControl
    {
        #region Constructor


        public UserFileRender(string factory, string group, string id)
        {
            InitializeComponent();

            m_Factory = factory;
            m_Group = group;
            m_Id = id;
        }

        #endregion
        
        #region Render File List

        private void Button_FileRefresh_Click(object sender, RoutedEventArgs e)
        {
            OperateResult result = UserClient.Net_File_Client.DownloadPathFileNames(out GroupFileItem[] files, m_Factory, m_Group, m_Id);
            if (result.IsSuccess)
            {
                Cache_Files = new List<GroupFileItem>(files);
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

        private void SetFilesShow(List<GroupFileItem> files)
        {
            //清楚缓存
            ClearControls();
            if (files?.Count > 0 && FileListControl.ActualWidth > 20)
            {
                //添加子控件
                foreach (var m in files)
                {
                    UserFileRenderItem item = new UserFileRenderItem(
                        UserClient.Net_File_Client,
                        m_Factory,
                        m_Group,
                        m_Id,
                        DeleteCheck);
                    FileListControl.Children.Add(item);
                    item.SetFile(m, () => m.Owner == UserClient.UserAccount.UserName);
                }
            }
        }

        #endregion

        #region Delete Check

        private bool DeleteCheck(GroupFileItem item)
        {
            if (item.Owner != UserClient.UserAccount.UserName)
            {
                MessageBox.Show("无法删除不是自己上传的文件。");
                return false;
            }
            else
            {
                return MessageBox.Show("请确认是否真的删除？", "删除确认",MessageBoxButton.YesNo) == MessageBoxResult.Yes;
            }
        }

        #endregion

        #region Filter Support


        private void FileSearchFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void FileSearchFilter_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                //搜索时触发的数据
                if (!string.IsNullOrEmpty(FileSearchFilter.Text))
                {
                    string pattern = FileSearchFilter.Text;
                    SetFilesShow(Cache_Files.Where(f =>
                    f.FileName.Contains(pattern) ||
                    f.Description.Contains(pattern) ||
                    f.Owner.Contains(pattern)).ToList());
                }
                else
                {
                    SetFilesShow(Cache_Files);
                }

                e.Handled = true;
            }
        }


        #endregion

        #region File Upload


        private void Button_FileUpload_Click(object sender, RoutedEventArgs e)
        {
            //上传数据，先对权限进行验证
            if (UserClient.UserAccount.Grade < AccountGrade.Technology)
            {
                MessageBox.Show("权限不够！");
                return;
            }

            using (FormSimplyFileUpload upload = new FormSimplyFileUpload(
                m_Factory,
                m_Group,
                m_Id
                ))
            {
                upload.ShowDialog();
            }
        }
        
        #endregion

        #region Private Members

        /// <summary>
        /// 所有文件信息的缓存，以支持直接的搜索
        /// </summary>
        private List<GroupFileItem> Cache_Files { get; set; } = new List<GroupFileItem>();
        /// <summary>
        /// 文件控件的缓存列表，方便清除垃圾
        /// </summary>
        private Stack<IDisposable> FilesControls = new Stack<IDisposable>();

        private string m_Factory;                                  // 文件的第一大类
        private string m_Group;                                    // 文件的第二大类
        private string m_Id;                                       // 文件的第三大类

        #endregion

        #region Control Load

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateFiles();
        }

        #endregion
    }
}
