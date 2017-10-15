using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using HslCommunication.Enthernet;
using CommonLibrary;

namespace ClientsLibrary.FileSupport
{
    /// <summary>
    /// 用于文件上传或下载的窗口
    /// </summary>
    public partial class FormFileOperate : Form
    {
        #region Constructor
        
        /// <summary>
        /// 实例化一个文件上传的窗口
        /// </summary>
        /// <param name="advancedFile">客户端的文件引擎</param>
        /// <param name="files">等待上传的文件数组</param>
        /// <param name="factory">第一大类</param>
        /// <param name="group">第二大类</param>
        /// <param name="id">第三大类</param>
        public FormFileOperate(IntegrationFileClient IntegratedFile, string[] files,string factory,string group,string id)
        {
            InitializeComponent();
            is_down_file = false;
            FormInitialization(IntegratedFile, files, factory, group, id);
        }

        /// <summary>
        /// 实例化一个文件下载的窗口
        /// </summary>
        /// <param name="advancedFile">客户端的文件传送引擎</param>
        /// <param name="files">等待下载的文件</param>
        /// <param name="factory">第一大类</param>
        /// <param name="group">第二大类</param>
        /// <param name="id">第三大类</param>
        /// <param name="savepath">保存路径</param>
        public FormFileOperate(IntegrationFileClient advancedFile, string[] files, string factory, string group, string id, string savepath)
        {
            InitializeComponent();
            SavePath = savepath;
            is_down_file = true;
            FormInitialization(advancedFile, files, factory, group, id);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void FormInitialization(IntegrationFileClient IntegratedFile, string[] files, string factory, string group, string id)
        {
            Icon = UserSystem.GetFormWindowIcon();     // 设置窗口ICON
            Files = files;                             // 设置文件名队列
            Factory = factory;                         // 第一大类
            Group = group;                             // 第二大类
            Id = id;                                   // 第三大类
            IntegrationFile = IntegratedFile;               
        }

        #endregion

        #region Window Load Show
        
        private void FormFileUpload_Load(object sender, EventArgs e)
        {
            // pictureBox1.Image = Properties.Resources.asset_progressBar_24x24_on;
        }

        private void FormFileOperate_Shown(object sender, EventArgs e)
        {
            int location_y = 1;
            int every_height = 85;
            if (is_down_file)
            {
                Text = "文件下载";

                if (Files != null)
                {
                    for (int i = 0; i < Files.Length; i++)
                    {
                        FileOperateControl item = new FileOperateControl(IntegrationFile, Files[i], Factory, Group, Id, SavePath);
                        panel1.Controls.Add(item);
                        item.Location = new Point(3, location_y);
                        all_file_controls.Add(item);
                        location_y += every_height;
                    }
                }
            }
            else
            {
                Text = "文件上传";

                if (Files != null)
                {
                    for (int i = 0; i < Files.Length; i++)
                    {
                        FileOperateControl item = new FileOperateControl(IntegrationFile, Files[i], Factory, Group, Id);
                        panel1.Controls.Add(item);
                        item.Location = new Point(3, location_y);
                        all_file_controls.Add(item);
                        location_y += every_height;
                    }
                }
            }

            
            Thread thread = new Thread(new ThreadStart(ThreadCheckFinish));
            thread.IsBackground = true;
            thread.Start();
        }


        private void FormFileOperate_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < all_file_controls.Count; i++)
            {
                if(!all_file_controls[i].IsOperateFinished)
                {
                    e.Cancel = true;
                    MessageBox.Show("当前任务没有完成不能退出！");
                    break;
                }
            }
        }


        #endregion
        
        #region Finish Check


        private void ThreadCheckFinish()
        {
            Thread.Sleep(400);

            Invoke(new Action(() =>
            {
                label1.Text += all_file_controls.Count;
                label_finish.Text = "0/" + all_file_controls.Count;

                for (int i = 0; i < all_file_controls.Count; i++)
                {
                    if (is_down_file)
                    {
                        all_file_controls[i].StartDownloadFile();
                    }
                    else
                    {
                        all_file_controls[i].StartUploadFile();
                    }
                }
            }));
            Thread.Sleep(400);
            while (true)
            {
                int complete = 0;
                for (int i = 0; i < all_file_controls.Count; i++)
                {
                    if (all_file_controls[i].IsOperateFinished)
                    {
                        complete++;
                    }
                }

                try
                { 
                    // 更新显示进度
                    Invoke(new Action(() =>
                    {
                        label_finish.Text = complete + "/" + all_file_controls.Count;
                    }));
                }
                catch
                {

                }
                
                if (complete >= all_file_controls.Count)
                {
                    break;
                }
                Thread.Sleep(490);
            }
        }

    
        #endregion
        
        #region Private Members
        
        private string[] Files = null;                          // 需要上传或是需要下载的文件列表
        private string Factory = "";                            // 文件的第一大类
        private string Group = "";                              // 文件的第二大类
        private string Id = "";                                 // 文件的第三大类
        private string SavePath = "";                           // 用于文件下载时候的保存位置
        private IntegrationFileClient IntegrationFile = null;   // 文件客户端
        private bool is_down_file = true;                       // 指示上传文件还是下载文件
        private List<FileOperateControl> all_file_controls = new List<FileOperateControl>();

        #endregion
    }
}
