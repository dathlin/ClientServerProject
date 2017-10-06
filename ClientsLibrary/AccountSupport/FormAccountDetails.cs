using CommonLibrary;
using HslCommunication.Enthernet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HslCommunication;
using HslCommunication.BasicFramework;
using System.IO;
using ClientsLibrary.FileSupport;

namespace ClientsLibrary
{
    public partial class FormAccountDetails : Form
    {

        #region Constructor

        public FormAccountDetails(UserPortrait userPortrait)
        {
            InitializeComponent();
            Icon = UserClient.GetFormWindowIcon();
            UserPortrait = userPortrait;
        }

        #endregion

        #region Load Show Close

        private void AccountDetails_Shown(object sender, EventArgs e)
        {
            // 加载各种数据
            textBox_Factory.Text = UserClient.UserAccount.Factory;
            textBox_GradeDescription.Text = AccountGrade.GetDescription(UserClient.UserAccount.Grade);
            textBox_LastLoginIpAddress.Text = UserClient.UserAccount.LastLoginIpAddress;
            textBox_LastLoginTime.Text = UserClient.UserAccount.LastLoginTime.ToString();
            textBox_LastLoginWay.Text = UserClient.UserAccount.LastLoginWay;
            textBox_LoginEnable.Text = UserClient.UserAccount.LoginEnable ? "允许" : "禁止";
            textBox_LoginFailedCount.Text = UserClient.UserAccount.LoginFailedCount.ToString();
            textBox_LoginFrequency.Text = UserClient.UserAccount.LoginFrequency.ToString();
            textBox_NameAlias.Text = UserClient.UserAccount.NameAlias;
            textBox_RegisterTime.Text = UserClient.UserAccount.RegisterTime.ToString();
            textBox_UserName.Text = UserClient.UserAccount.UserName;

            // 加载头像
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolLoadLargePortrait), null);

            // 加载文件列表
            DownloadUserFileNames();

            // 本地化显示
            UILocalization();
        }

        private void AccountDetails_Load(object sender, EventArgs e)
        {
            Text = "账户详细信息";
            
            treeView1.ImageList = new ImageList();
            treeView1.ImageList.Images.Add(Properties.Resources.ExtensionManager_vsix_OSReg_16x);
            treeView1.ImageList.Images.Add(Properties.Resources.Flagthread_7317);

            treeView1.Nodes[0].ImageIndex = 0;
            treeView1.Nodes[0].SelectedImageIndex = 0;
        }

        #endregion

        #region Localization Support

        /// <summary>
        /// 本地化显示的操作，还未完成
        /// </summary>
        private void UILocalization()
        {
            Text = UserLocalization.Localization.AccountDetails;
            groupBox1.Text = UserLocalization.Localization.AccountDetails;
            label12.Text = UserLocalization.Localization.AccountPortrait + "：";
            label1.Text = UserLocalization.Localization.AccountName + "：";
            label2.Text = UserLocalization.Localization.AccountAlias + "：";
            label3.Text = UserLocalization.Localization.AccountFactory + "：";
            label4.Text = UserLocalization.Localization.AccountGrade + "：";
            label5.Text = UserLocalization.Localization.AccountRegisterTime + "：";
            label6.Text = UserLocalization.Localization.AccountLoginEnable + "：";
            label7.Text = UserLocalization.Localization.AccountLoginFrequency + "：";
            label8.Text = UserLocalization.Localization.AccountLastLoginTime + "：";
            label9.Text = UserLocalization.Localization.AccountLastLoginIpAddress + "：";
            label10.Text = UserLocalization.Localization.AccountLastLoginWay + "：";
            label11.Text = UserLocalization.Localization.AccountLoginFailedCount + "：";
        }


        #endregion

        #region Load Portrait

        public UserPortrait UserPortrait { get; }
        
 
        private void pictureBox_UserPortrait_Click(object sender, EventArgs e)
        {
            UserPortrait.ChangePortrait(LoadLargeProtrait,UnloadLargeProtrait);
        }

        private void ThreadPoolLoadLargePortrait(object obj)
        {
            Thread.Sleep(200);
            UserPortrait.LoadUserLargePortraint(LoadLargeProtrait, UnloadLargeProtrait);
        }

        private void UnloadLargeProtrait()
        {
            if (IsHandleCreated && InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    UnloadLargeProtrait();
                }));
                return;
            }

            pictureBox_UserPortrait.Image = null;
        }

        private void LoadLargeProtrait(string fileName)
        {
            if (IsHandleCreated && InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    LoadLargeProtrait(fileName);
                }));
                return;
            }

            pictureBox_UserPortrait.ImageLocation = fileName;
        }


        #endregion

        #region Download FileNames

        public void DownloadUserFileNames()
        {
            treeView1.Nodes[0].Text = "我的云端文件（下载中）";
            treeView1.Nodes[0].Nodes.Clear();
            treeView1.Refresh();

            // 向服务器请求自身的文件列表
            OperateResult result = UserClient.Net_File_Client.DownloadPathFileNames(
                out GroupFileItem[] files, "Files", "Personal", UserClient.UserAccount.UserName
                );

            if (result.IsSuccess)
            {
                if (files != null)
                {
                    foreach(var file in files)
                    {
                        TreeNode node = new TreeNode(file.FileName, 1, 1);
                        node.Tag = file;
                        treeView1.Nodes[0].Nodes.Add(node);
                    }

                    treeView1.Nodes[0].Text = "我的云端文件（" + files.Length + "）";
                }
                else
                {
                    treeView1.Nodes[0].Text = "我的云端文件（NULL）";
                }
            }
            else
            {
                treeView1.Nodes[0].Text = "我的云端文件（下载失败）";
            }

            treeView1.ExpandAll();
        }


        #endregion

        #region Upload Support

        private void userButton_upload_Click(object sender, EventArgs e)
        {
            // 上传个人文件
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                if(openFileDialog.ShowDialog()==DialogResult.OK)
                {
                    UploadFilesToServer(openFileDialog.FileNames);
                }
            }
        }

        private void UploadFilesToServer(string[] files)
        {
            FormFileOperate upload = new FormFileOperate(UserClient.Net_File_Client,
                        files,
                        "Files",
                        "Personal",
                        UserClient.UserAccount.UserName);
            upload.ShowDialog();
            // 更新文件列表
            DownloadUserFileNames();
        }

        #endregion
        
        #region Download File Support


        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 双击下载
            if (e.Button == MouseButtons.Left)
            {
                userButton1_Click(null, new EventArgs());
            }
        }

        private void userButton1_Click(object sender, EventArgs e)
        {
            // 下载选择文件
            TreeNode treeNode = treeView1.SelectedNode;
            if (treeNode.Name != "files_root")
            {
                // 选择文件夹
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    folderBrowserDialog.Description = "请选择保存路径，如果有重名文件，将会覆盖！";
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        FormFileOperate download = new FormFileOperate(UserClient.Net_File_Client,
                                new string[] { treeNode.Text },
                                "Files",
                                "Personal",
                                UserClient.UserAccount.UserName,
                                folderBrowserDialog.SelectedPath
                                );
                        download.ShowDialog();
                    }
                }
            }
        }

        #endregion

        #region Delete File Support
        
        private void userButton2_Click(object sender, EventArgs e)
        {
            // 删除选中文件
            TreeNode treeNode = treeView1.SelectedNode;
            if (treeNode.Name != "files_root")
            {
                // 删除文件前要先进行密码验证
                using (FormPasswordCheck passwordCheck = new FormPasswordCheck(UserClient.UserAccount.Password))
                {
                    if (passwordCheck.ShowDialog() == DialogResult.OK)
                    {
                        // 密码验证已经通过
                        OperateResult result = UserClient.Net_File_Client.DeleteFile(
                            treeNode.Text,
                            "Files",
                            "Personal",
                            UserClient.UserAccount.UserName);
                        if(result.IsSuccess)
                        {
                            // 更新文件列表
                            DownloadUserFileNames();
                        }
                        else
                        {
                            MessageBox.Show("删除失败，原因：" + result.Message);
                        }
                    }
                }
            }
        }

        #endregion

        #region Drag File Upload

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (paths != null)
                {
                    List<string> files = new List<string>();

                    try
                    {
                        foreach (var m in paths)
                        {
                            FileInfo finfo = new FileInfo(m);
                            if (finfo.Attributes == FileAttributes.Directory)
                            {
                                foreach (var n in Directory.GetFiles(m))
                                {
                                    files.Add(n);
                                }
                            }
                            else
                            {
                                files.Add(m);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        UserClient.LogNet?.WriteException("拖拽文件上传异常：", ex);
                        SoftBasic.ShowExceptionMessage(ex);
                        return;
                    }

                    if (files.Count > 0)
                    {
                        Invoke(new Action(() =>
                        {
                            UploadFilesToServer(files.ToArray());
                        }));
                    }
                }
                
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }


        #endregion

        #region Tree File Select
        
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(e.Node.Tag is GroupFileItem file)
            {
                textBox_file_name.Text = file.FileName;
                textBox_file_size.Text = file.GetTextFromFileSize();
                textBox_file_uploadTime.Text = file.UploadTime.ToLocalTime().ToShortDateString();
            }
        }

        #endregion
    }
}
