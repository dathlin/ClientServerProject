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
using ClientsLibrary.FileSupport;
using System.Runtime.InteropServices;

namespace 软件系统客户端模版.UIControls
{
    public partial class GroupFilesRender : UserControl
    {
        #region Constructor
        
        public GroupFilesRender(string factory, string group, string id)
        {
            InitializeComponent();

            m_Factory = factory;
            m_Group = group;
            m_Id = id;

            DoubleBuffered = true;
        }

        #endregion

        #region Load Show
        
        private void ShareFilesRender_Load(object sender, EventArgs e)
        {
            // 选择是否禁用上传键
        }

        #endregion

        #region Upload File
        
        private void userButton_upload_Click(object sender, EventArgs e)
        {
            // 上传数据
            if(UserClient.UserAccount.Grade<AccountGrade.Technology)
            {
                MessageBox.Show("权限不够！");
                return;
            }

            // 上传文件
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

        #region Render File List


        private void ClearControls()
        {
            while (FilesControls.Count > 0)
            {
                FilesControls.Pop().Dispose();
            }
        }

        public void UpdateFiles()
        {
            userButton_refresh.PerformClick();
        }

        private void userButton_refresh_Click(object sender, EventArgs e)
        {
            OperateResult result = UserClient.Net_File_Client.DownloadPathFileNames(out GroupFileItem[] files, m_Factory, m_Group, m_Id);
            if(result.IsSuccess)
            {
                Cache_Files = new List<GroupFileItem>(files);
                SetFilesShow(Cache_Files);
            }
            else
            {
                MessageBox.Show(result.ToMessageShowString());
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }
        

        private void SetFilesShow(List<GroupFileItem> files)
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
                        UserClient.Net_File_Client,      // 文件传送客户端
                        m_Factory,                       // 文件第一大类
                        m_Group,                         // 文件第二大类
                        m_Id,                            // 文件第三大类
                        DeleteCheck                      // 删除键是否激活方法
                        );
                    panel2.Controls.Add(item);                                                // 添加显示
                    item.BackColor = Color.White;                                             // 白色的背景
                    item.BorderStyle = BorderStyle.FixedSingle;                               // 边框样式
                    item.SetFile(m, () => m.Owner == UserClient.UserAccount.UserName);        // 设置文件显示并提供一个删除使能的权限,此处设置是登录名和上传人不一致时,删除键不能点击
                    item.Location = new Point(2, location_y);                                 // 控件的位置
                    int width = panel2.VerticalScroll.Visible ? panel2.Width - 4 - SystemInformation.VerticalScrollBarWidth : panel2.Width - 4; // 控件的宽度
                    item.Size = new Size(width, item.Size.Height);                            // 控件的大小
                    item.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;  // 控件随窗口变化的样式

                    location_y += item.Height + 4;                                            // 位置偏移
                    FilesControls.Push(item);                                                 // 控件压入堆栈
                }
            }

            panel2.ResumeLayout();
            
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
                return MessageBox.Show("请确认是否真的删除？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
            }
        }

        #endregion
        
        #region Filter Support

        /**************************************************************************************************
         * 
         *    说明： 此处实现的功能是更改了输入框文本后，按了Enter键就开始执行筛选
         * 
         **************************************************************************************************/

        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 搜索时触发的数据
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    string pattern = textBox1.Text;
                    SetFilesShow(Cache_Files.Where(f =>
                    f.FileName.Contains(pattern) ||
                    f.Description.Contains(pattern) ||
                    f.Owner.Contains(pattern)).ToList());
                }
                else
                {
                    SetFilesShow(Cache_Files);
                }
                // 已处理过该值
                e.Handled = true;
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
    }
}
