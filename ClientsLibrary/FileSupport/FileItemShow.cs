using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using HslCommunication;
using HslCommunication.Enthernet;

namespace ClientsLibrary.FileSupport
{
    /// <summary>
    /// 文件显示的子控件类
    /// </summary>
    public partial class FileItemShow : UserControl
    {
        #region Constructor
        
        /// <summary>
        /// 生成一个文件对象的显示控件
        /// </summary>
        /// <param name="client">客户端类</param>
        /// <param name="factory">第一大类</param>
        /// <param name="group">第二大类</param>
        /// <param name="id">第三大类</param>
        /// <param name="deleteCheck">用户自定义的删除确认委托，可用于验证权限及密码</param>
        public FileItemShow(IntegrationFileClient client, string factory, string group, string id, Func<GroupFileItem,bool> deleteCheck)
        {
            InitializeComponent();
            DeleteCheck = deleteCheck;
            m_Factory = factory;
            m_Group = group;
            m_Id = id;
            fileClient = client;
        }

        #endregion

        #region Control Load

        private void FileItemShow_Load(object sender, EventArgs e)
        {

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
            // 设置文件的图标
            pictureBox_file.Image = FileSupport.GetFileIcon(file.FileName);

            label_file_name.Text = "文件名称：" + file.FileName;
            label_file_size.Text = "大小：" + file.GetTextFromFileSize();
            label_file_date.Text = "日期：" + file.UploadTime.ToString("yyyy-MM-dd");
            label_file_mark.Text = "文件备注：" + file.Description;
            label_upload_name.Text = "上传人：" + file.Owner;
            label_download_times.Text = "下载数：" + file.DownloadTimes;


            linkLabel_delete.Enabled = deleteEnable.Invoke();
            linkLabel_download.Enabled = true;                      // 一般都是允许下载，如果不允许下载，在此处设置
        }


        #endregion

        #region Delete Support

        private void linkLabel_delete_Click(object sender, EventArgs e)
        {
            // 删除文件
            if (DeleteCheck != null)
            {
                // 删除的权限检查
                if(!DeleteCheck.Invoke(fileItem))
                {
                    // 没有通过
                    return;
                }
            }

            linkLabel_delete.Enabled = false;
            Thread thread_delete_file = new Thread(new ThreadStart(ThreadDeleteFile));
            thread_delete_file.IsBackground = true;
            thread_delete_file.Start();
        }
        private void ThreadDeleteFile()
        {
            OperateResult result = fileClient.DeleteFile(fileItem.FileName, m_Factory, m_Group, m_Id);
            if(IsHandleCreated) Invoke(new Action(() =>
            {
                if(result.IsSuccess)
                {
                    MessageBox.Show("删除成功！，请刷新界面。");
                }
                else
                {
                    MessageBox.Show("删除失败！错误：" + result.Message);
                }
                linkLabel_delete.Enabled = true;
            }));
        }

        #endregion

        #region Download Support


        private void linkLabel_download_Click(object sender, EventArgs e)
        {
            //下载文件
            linkLabel_download.Enabled = false;

            Thread thread_down_file = new Thread(new ThreadStart(ThreadDownloadFile));
            thread_down_file.IsBackground = true;
            thread_down_file.Start();
        }

        private void ThreadDownloadFile()
        {
            string save_file_name = Application.StartupPath + "\\download\\files";
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
                DownloadProgressReport, 
                save_file_name
                );

            if(IsHandleCreated) Invoke(new Action(() =>
            {
                if(result.IsSuccess)
                {
                    if (MessageBox.Show("下载完成，路径为：" + save_file_name + Environment.NewLine +
                        "是否打开文件路径？", "打开确认", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", @"/select," + save_file_name);
                    }
                }
                else
                {
                    MessageBox.Show("下载失败，错误原因：" + result.Message);
                }
                if (IsHandleCreated) linkLabel_download.Enabled = true;
            }));
        }

        private void DownloadProgressReport(long download, long totle)
        {
            if(IsHandleCreated)
            {
                if(InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        DownloadProgressReport(download, totle);
                    }));
                    return;
                }

                progressBar1.Value = (int)(download * 100 / totle);
            }
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
