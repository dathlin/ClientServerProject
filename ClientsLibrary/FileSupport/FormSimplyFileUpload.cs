using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using HslCommunication.Enthernet;
using HslCommunication;
using CommonLibrary;

namespace ClientsLibrary.FileSupport
{
    /// <summary>
    /// 用于文件上传的类
    /// </summary>
    public partial class FormSimplyFileUpload : Form
    {
        #region Constructor

        /// <summary>
        /// 实例化一个文件上传的窗口
        /// </summary>
        /// <param name="factory">第一大类</param>
        /// <param name="group">第二大类</param>
        /// <param name="id">第三大类</param>
        public FormSimplyFileUpload(string factory, string group, string id)
        {
            InitializeComponent();

            Icon = UserSystem.GetFormWindowIcon();
            fileItem = new GroupFileItem();
            fileClient = UserClient.Net_File_Client;
            Factory = factory;
            Group = group;
            Id = id;
        }

        #endregion

        #region Close Check

        private void FormUploadFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!userButton1.Enabled)
            {
                MessageBox.Show("请等待上传完成才能关闭窗口。");
                e.Cancel = true;
            }
        }

        #endregion

        #region Window Load Show

        private void FormSimplyFileUpload_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Upload Support
        
        /// <summary>
        /// 用于报告进度的方法
        /// </summary>
        /// <param name="current"></param>
        /// <param name="count"></param>
        private void ReportProgress(long current, long count)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    ReportProgress(current, count);
                }));
                return;
            }

            long percent = 0;
            if (count > 0)
            {
                percent = current * 100 / count;
            }

            progressBar1.Value = (int)percent;
            toolStripStatusLabel2.Text = "正在上传文件(" + percent.ToString() + "%)";
        }

        /// <summary>
        /// 上传文件的后台线程
        /// </summary>
        private void ThreadUploadFile()
        {
            toolStripStatusLabel2.Text = "正在上传文件...";

            Thread.Sleep(500);
            OperateResult result = fileClient.UploadFile(
                file_full_name,                          // 本地文件的完整路径
                fileItem.FileName,                       // 在服务器端需要保存的文件名，此处和本地文件名一致
                Factory,                                 // 文件的第一大类
                Group,                                   // 文件的第二大类
                Id,                                      // 文件的第三大类
                fileItem.Description,                    // 文件的额外描述
                UserClient.UserAccount.UserName,         // 文件的上传人
                ReportProgress                           // 文件的进度报告
                );

            if (result.IsSuccess)
            {
                Invoke(new Action(() =>
                {
                    userButton1.Enabled = true;
                    toolStripStatusLabel2.Text = "文件发送完成。";
                }));
            }
            else
            {
                Invoke(new Action(() =>
                {
                    MessageBox.Show("文件发送失败：" + result.Message);
                    userButton1.Enabled = true;
                    toolStripStatusLabel2.Text = "文件发送失败";
                }));
            }

        }
        
        #endregion
        
        #region File Selected
        
        /// <summary>
        /// 选择需要上传的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userButton2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    file_full_name = ofd.FileName;
                    textBox1.Text = file_full_name;

                    FileInfo info = new FileInfo(file_full_name);
                    fileItem.FileName = info.Name;
                    fileItem.FileSize = info.Length;
                    label4.Text = fileItem.GetTextFromFileSize();
                }
            }
        }

        #endregion

        #region Start Upload

        private void userButton1_Click(object sender, EventArgs e)
        {
            //上传
            if (textBox1.Text != "")
            {
                fileItem.Description = textBox2.Text;
                userButton1.Enabled = false;
                progressBar1.Value = 0;

                Thread thread_upload_file = new Thread(new ThreadStart(ThreadUploadFile));
                thread_upload_file.IsBackground = true;
                thread_upload_file.Start();
            }
        }
        
        #endregion
        
        #region Private Members

        private IntegrationFileClient fileClient;               // 数据传送对象
        private string file_full_name = "";                 // 文件的完整名称
        private GroupFileItem fileItem;                         // 文件对象
        private string Factory = "";                            // 文件的第一大类
        private string Group = "";                              // 文件的第二大类
        private string Id = "";                                 // 文件的第三大类

        #endregion
    }
    
}
