using HslCommunication.BasicFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonLibrary
{
    /// <summary>
    /// 系统更新日志类窗体
    /// </summary>
    public partial class FormUpdateLog : Form
    {
        /// <summary>
        /// 实例化一个更新窗口的对象，用来呈现更新日志
        /// </summary>
        /// <param name="versions">包含了更新细节的一个对象，更新日志，更新内容</param>
        public FormUpdateLog(IEnumerable<VersionInfo> versions)
        {
            InitializeComponent();
            Versions = versions;
        }
        private IEnumerable<VersionInfo> Versions = null;

        private void FormUpdateLog_Load(object sender, EventArgs e)
        {
            richTextBox1.SelectionIndent = 3;
            richTextBox1.SelectionHangingIndent = 16;
            richTextBox1.SelectionRightIndent = 3;
            
            listBox1.DataSource = Versions;
            listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
            ListBox1_SelectedIndexChanged(null, new EventArgs());
        }
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                VersionInfo info = listBox1.SelectedItem as VersionInfo;
                if (info != null)
                {
                    richTextBox1.Clear();
                    richTextBox1.AppendText("版本：" + info.VersionNum + Environment.NewLine);
                    richTextBox1.AppendText("日期：" + info.ReleaseDate.ToShortDateString() + Environment.NewLine);


                    richTextBox1.AppendText(Environment.NewLine);

                    richTextBox1.AppendText("内容：" + Environment.NewLine +
                        info.UpdateDetails.ToString() + Environment.NewLine);

                    richTextBox1.SelectionStart = 0;
                }
            }
        }
    }
}
