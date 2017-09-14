using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using HslCommunication.Enthernet;
using HslCommunication;
using CommonLibrary;
using ClientsLibrary;

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
            //上传数据
            if(UserClient.UserAccount.Grade<AccountGrade.Technology)
            {
                MessageBox.Show("权限不够！");
                return;
            }

            // 上传文件
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

        private void userButton_refresh_Click(object sender, EventArgs e)
        {
            //向服务器请求数据
            OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.请求文件);
            if(result.IsSuccess)
            {
                Cache_Files = JArray.Parse(result.Content).ToObject<List<HslSoftFile>>();
                SetFilesShow(Cache_Files);
            }
            else
            {
                MessageBox.Show(result.ToMessageShowString());
            }
        }

        private void SetFilesShow(List<HslSoftFile> files)
        {
            panel2.SuspendLayout();
            //清楚缓存
            ClearControls();
            if (files?.Count > 0 && panel2.Width > 20)
            {
                int location_y = 2 - panel2.VerticalScroll.Value;
                //添加子控件
                foreach(var m in files)
                {
                    FileItemShow item = new FileItemShow(
                        CommonHeadCode.KeyToken,
                        UserClient.LogNet, UserClient.ServerIp,
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
                                return MessageBox.Show("请确认是否真的删除？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                            }
                        });
                    panel2.Controls.Add(item);                                            // 添加显示
                    item.BackColor = Color.White;                                         // 白色的背景
                    item.BorderStyle = BorderStyle.FixedSingle;                                 // 边框样式
                    item.SetFile(m, () => m.UploadName == UserClient.UserAccount.UserName);    // 设置文件显示并提供一个删除使能的权限,此处设置是登录名和上传人不一致时,删除键不能点击
                    item.Location = new Point(2, location_y);                                 // 控件的位置
                    int width = panel2.VerticalScroll.Visible ? panel2.Width - 4 - SystemInformation.VerticalScrollBarWidth : panel2.Width - 4; // 控件的宽度
                    item.Size = new Size(width, item.Size.Height);  // 控件的大小
                    item.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;// 控件随窗口变化的样式

                    location_y += item.Height + 4; // 位置偏移
                    FilesControls.Push(item);// 控件压入堆栈
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
        private List<HslSoftFile> Cache_Files { get; set; } = new List<HslSoftFile>();
        /// <summary>
        /// 文件控件的缓存列表，方便清除垃圾
        /// </summary>
        private Stack<IDisposable> FilesControls = new Stack<IDisposable>();


        private void ShareFilesRender_Load(object sender, EventArgs e)
        {
            // 选择是否禁用上传键
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // 搜索时触发的数据
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
