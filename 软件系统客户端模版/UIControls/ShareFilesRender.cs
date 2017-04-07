using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndustryEthernet;
using Newtonsoft.Json.Linq;


namespace 软件系统客户端模版.UIControls
{
    public partial class ShareFilesRender : UserControl
    {
        public ShareFilesRender()
        {
            InitializeComponent();

            DoubleBuffered = true;
        }

        private void userButton_upload_Click(object sender, EventArgs e)
        {
            //上传数据，先对权限进行验证
            if(UserClient.UserAccount.Grade<BasicFramework.AccountGrade.Technology)
            {
                MessageBox.Show("权限不够！");
                return;
            }

            using (FormSimplyFileUpload upload = new FormSimplyFileUpload(
                UserClient.ServerIp,
                CommonLibrary.CommonLibrary.Port_Share_File,
                UserClient.UserAccount.UserName))
            {
                upload.ShowDialog();
            }
        }

        private void userButton_refresh_Click(object sender, EventArgs e)
        {
            //向服务器请求数据
            OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.请求文件列表);
            if(result.IsSuccess)
            {
                Cache_Files = JArray.Parse(result.Content).ToObject<List<File_Save>>();
                SetFilesShow(Cache_Files);
            }
            else
            {
                MessageBox.Show(result.ToMessageShowString());
            }
        }

        private void SetFilesShow(List<File_Save> files)
        {
            panel2.SuspendLayout();
            //清楚缓存
            ClearControls();
            if (files?.Count > 0 && panel2.Width > 20)
            {
                int location_y = 2;
                //添加子控件
                foreach(var m in files)
                {
                    FileItemShow item = new FileItemShow(UserClient.ServerIp,
                        CommonLibrary.CommonLibrary.Port_Share_File,
                        () =>
                        {
                            if (m.UploadName != UserClient.UserAccount.UserName)
                            {
                                MessageBox.Show("无法删除不是自己上传的文件。");
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        });
                    panel2.Controls.Add(item);
                    item.BackColor = Color.White;
                    item.BorderStyle = BorderStyle.FixedSingle;
                    item.SetFile(m, () => m.UploadName == UserClient.UserAccount.UserName);
                    item.Location = new Point(2, location_y);
                    item.Size = new Size(panel2.Width - 4, item.Size.Height);
                    item.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);

                    location_y += item.Height + 4;
                    FilesControls.Push(item);
                }
            }

            panel2.ResumeLayout();
        }

        public void UpdateFiles()
        {
            userButton_refresh.PerformClick();
        }

        private void ClearControls()
        {
            while (FilesControls.Count > 0)
            {
                FilesControls.Pop().Dispose();
            }
        }

        /// <summary>
        /// 所有文件信息的缓存，以支持直接的搜索
        /// </summary>
        private List<File_Save> Cache_Files { get; set; } = new List<File_Save>();
        /// <summary>
        /// 文件控件的缓存列表，方便清除垃圾
        /// </summary>
        private Stack<IDisposable> FilesControls = new Stack<IDisposable>();


        private void ShareFilesRender_Load(object sender, EventArgs e)
        {
            //选择是否禁用上传键
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //搜索时触发的数据
            if(!string.IsNullOrEmpty(textBox1.Text))
            {
                string pattern = textBox1.Text;
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
    }
}
