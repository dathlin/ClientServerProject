using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using HslCommunication.Enthernet;
using HslCommunication.BasicFramework;
using HslCommunication;

namespace ClientsLibrary.FileSupport
{
    /// <summary>
    /// 上传或下载的控件
    /// </summary>
    public partial class FileOperateControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// 上传的实例化方法
        /// </summary>
        /// <param name="advancedFile">客户端文件传送引擎</param>
        /// <param name="filepath">完整的包含路径的本地文件路径</param>
        /// <param name="factory">文件所属的工厂</param>
        /// <param name="group">文件所属的分类</param>
        /// <param name="id">文件所属的设备ID</param>
        public FileOperateControl(IntegrationFileClient advancedFile, string filepath, string factory, string group, string id)
        {
            InitializeComponent();
            Is_down_file = false;
            FilePath = filepath;
            Factory = factory;
            Group = group;
            Id = id;
            AdvancedFileClient = advancedFile;
        }


        /// <summary>
        /// 下载的实例化方法
        /// </summary>
        /// <param name="advancedFile">客户端文件传送引擎</param>
        /// <param name="fileName">服务器的文件名称，包含后缀</param>
        /// <param name="factory">文件所属的第一大类</param>
        /// <param name="group">文件所属的第二大类</param>
        /// <param name="id">文件所属的第三大类</param>
        /// <param name="savepath">本地保存的路径</param>
        public FileOperateControl(IntegrationFileClient advancedFile, string fileName, string factory, string group, string id, string savepath)
        {
            InitializeComponent();
            //下载文件
            Is_down_file = true;
            FilePath = fileName;
            Factory = factory;
            Group = group;
            Id = id;
            SavaPathDirectory = savepath;
            AdvancedFileClient = advancedFile;
        }


        #endregion

        #region Load Close
        
        private void FileOperateControl_Load(object sender, EventArgs e)
        {
            label_filename.Text = FilePath;
            label_now_info.Text = "正在确认...";

            progressBar1.Maximum = 100;

            button1.Location = new Point(389, 55);
            if(Is_down_file)
            {
                button1.Text = "重新下载文件";
            }
            else
            {
                button1.Text = "重新上传文件";
            }
        }

        #endregion

        #region Public Property


        /// <summary>
        /// 指示本次上传或是下载是否结束
        /// </summary>
        public bool IsOperateFinished { get; set; } = false;


        #endregion

        #region Upload Support
        
        /// <summary>
        /// 开始上传文件
        /// </summary>
        public void StartUploadFile()
        {
            try
            {
                label_now_info.Text = "正在确认...";
                if (File.Exists(FilePath))
                {
                    Thread thread = new Thread(new ThreadStart(ThreadUploadFile));
                    thread.IsBackground = true;
                    thread.Start();
                }
                else
                {
                    label_now_info.Text = "文件不存在";
                    IsOperateFinished = true;
                }
            }
            catch
            {

            }
        }
        private void ThreadUploadFile()
        {
            FileInfo finfo = new FileInfo(FilePath);

            Invoke(new Action(() =>
            {
                label_now_info.Text = "正在上传文件...";
                label_filesize.Text = SoftBasic.GetSizeDescription(finfo.Length);
                progressBar1.Value = 0;
            }));

            OperateResult result = AdvancedFileClient.UploadFile(
                FilePath,
                finfo.Name,
                Factory,
                Group,
                Id,
                "",
                UserClient.UserAccount.UserName,
                ReportProgress);

            if (result.IsSuccess)
            {
                Invoke(new Action(() =>
                {
                    label_now_info.Text = "文件上传成功";
                }));
            }
            else
            {
                WrongTextShow("异常：" + result.Message);
            }

            IsOperateFinished = true;
        }

        #endregion

        #region Foundation Methon



        private void WrongTextShow(string text)
        {
            if (IsHandleCreated && InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    WrongTextShow(text);
                }));
                return;
            }

            label_now_info.Text = text;
            button1.Visible = true;
            IsOperateFinished = true;
            progressBar1.Value = 0;
        }


        

        

        private void button1_Click(object sender, EventArgs e)
        {
            IsOperateFinished = false;
            button1.Visible = false;
            if (Is_down_file)
            {
                //下载文件
                StartDownloadFile();
            }
            else
            {
                //上传文件
                StartUploadFile();
            }
        }


        private void ReportProgress(long current,long count)
        {
            Invoke(new Action(() =>
            {
                long percent = 0;
                if (count > 0)
                {
                    percent = current * 100 / count;
                }

                progressBar1.Value = (int)percent;
                label_now_info.Text = "已完成：" + percent.ToString() + "%";

                label_filesize.Text = SoftBasic.GetSizeDescription(count);
            }));
        }

        #endregion

        #region Download Support


        /// <summary>
        /// 启动下载文件
        /// </summary>
        public void StartDownloadFile()
        {
            Thread thread = new Thread(new ThreadStart(ThreadDownloadFile))
            {
                IsBackground = true
            };
            thread.Start();
        }

        private void ThreadDownloadFile()
        {
            Invoke(new Action(() =>
            {
                label_now_info.Text = "正在下载文件...";
                progressBar1.Value = 0;
            }));


            if (!SavaPathDirectory.EndsWith(@"\"))
            {
                SavaPathDirectory = SavaPathDirectory + @"\";
            }

            

            OperateResult result = AdvancedFileClient.DownloadFile(
                FilePath,
                Factory,
                Group,
                Id,
                ReportProgress,
                SavaPathDirectory + FilePath
                );

            if(result.IsSuccess)
            {
                Invoke(new Action(() =>
                {
                    label_now_info.Text = "文件下载成功";
                }));
            }
            else
            {
                WrongTextShow("异常：" + result.Message);
            }

            IsOperateFinished = true;
        }


        #endregion

        #region Paint Support


        private void FileOperateControl_Paint(object sender, PaintEventArgs e)
        {
            // 绘制外观
            e.Graphics.DrawRectangle(Pens.DodgerBlue, new Rectangle(0, 0, Width - 1, Height - 1));
        }

        #endregion
        
        #region Private Member
        private IntegrationFileClient AdvancedFileClient;   // 支持文件操作的
        private bool Is_down_file = true;                   // 文件是否在下载
        private string FilePath = "";                       // 文件路径
        private string Factory = "";                        // 文件所属的工厂分类
        private string Group = "";                          // 文件所属的类别
        private string Id = "";                             // 文件所属的特殊ID
        private string SavaPathDirectory = "";              // 文件保存的路径，文件下载的时候所需的
        #endregion

    }
}
