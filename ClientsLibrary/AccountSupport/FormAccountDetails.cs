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

namespace ClientsLibrary
{
    public partial class FormAccountDetails : Form
    {

        #region Constructor

        public FormAccountDetails(UserPortrait userPortrait)
        {
            InitializeComponent();
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

        #region 头像加载显示块

        public UserPortrait UserPortrait { get; }
        

        private void LoadPortraitByFileName(string fileName)
        {
            if(IsHandleCreated)
            {
                if(InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        LoadPortraitByFileName(fileName);
                    }));
                    return;
                }

                try
                {
                    pictureBox_UserPortrait.LoadAsync(fileName);
                }
                catch(Exception ex)
                {
                    UserClient.LogNet?.WriteException("加载图片异常！", ex);
                }
            }
        }
        

        private void pictureBox_UserPortrait_Click(object sender, EventArgs e)
        {
            UserPortrait.ChangePortrait();
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolLoadLargePortrait), null);
        }

        private void ThreadPoolLoadLargePortrait(object obj)
        {
            UserPortrait.LoadUserLargePortraint(LoadPortraitByFileName);
        }

        #endregion

        #region 个人文件管理块

        public void DownloadUserFileNames()
        {
            treeView1.Nodes[0].Text = "我的云端文件（下载中）";
            treeView1.Nodes[0].Nodes.Clear();
            treeView1.Refresh();

            // 向服务器请求自身的文件列表
            OperateResult result = UserClient.Net_File_Client.DownloadPathFileNames(
                out string[] files, "Files", "Personal", UserClient.UserAccount.UserName
                );

            if (result.IsSuccess)
            {
                if (files != null)
                {
                    foreach(var m in files)
                    {
                        TreeNode node = new TreeNode(m, 1, 1);
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

        private void userButton_upload_Click(object sender, EventArgs e)
        {
            // 上传个人文件
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                if(openFileDialog.ShowDialog()==DialogResult.OK)
                {
                    FormFileOperate upload = new FormFileOperate(UserClient.Net_File_Client,
                        openFileDialog.FileNames,
                        "Files",
                        "Personal",
                        UserClient.UserAccount.UserName);
                    upload.ShowDialog();

                    // 更新文件列表
                    DownloadUserFileNames();
                }
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

        private void userButton2_Click(object sender, EventArgs e)
        {
            // 删除选中文件
            TreeNode treeNode = treeView1.SelectedNode;
            if (treeNode.Name != "files_root")
            {
                using (FormPasswordCheck passwordCheck = new FormPasswordCheck(UserClient.UserAccount.Password))
                {
                    if (passwordCheck.ShowDialog() == DialogResult.OK)
                    {
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


    }
}
