using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace 软件系统客户端模版.UIControls
{
    public partial class OnlineChatRender : UserControl
    {
        public OnlineChatRender(Action<string> send)
        {
            InitializeComponent();
            SendString = send;
        }
        
        private Action<string> SendString = null;
        
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //按下Enter键后进行发送数据到服务器
            if(!string.IsNullOrEmpty(textBox1.Text))
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendString?.Invoke(textBox1.Text);
                    textBox1.Text = "";
                }
            }
        }

        private void OnlineChatRender_Load(object sender, EventArgs e)
        {

        }

        public void DealwithReceive(string str)
        {
           richTextBox1.AppendText(str + Environment.NewLine);
            int length = str.IndexOf(Environment.NewLine) + 1;
            if (length > 0)
            {
                richTextBox1.Select(richTextBox1.Text.Length - str.Length + 1, length);
                richTextBox1.SelectionColor = Color.Blue;
            }
            ScrollToDown();
        }
        /// <summary>
        /// 新增聊天的历史记录
        /// </summary>
        /// <param name="str"></param>
        public void AddChatsHistory(string str)
        {
            richTextBox1.Text = str;
            MatchCollection mc = Regex.Matches(str, @"\u0002.+\r\n");
            int indexrow = 0;
            if (str != "")
            {
                foreach (Match m in mc)
                {
                    richTextBox1.Select(m.Index - indexrow * 2, m.Length - 2);
                    richTextBox1.SelectionColor = Color.Blue;
                    indexrow++;
                }
            }
            ScrollToDown();
        }

        public void InputFocus()
        {
            textBox1.Focus();
        }



        /// <summary>
        /// 光标滚动到最底端
        /// </summary>
        public void ScrollToDown()
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
    }
}
