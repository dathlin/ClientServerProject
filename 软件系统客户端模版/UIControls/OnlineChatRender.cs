using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
        }
        /// <summary>
        /// 新增聊天的历史记录
        /// </summary>
        /// <param name="str"></param>
        public void AddChatsHistory(string str)
        {
            richTextBox1.Text = str;
        }

        public void InputFocus()
        {
            textBox1.Focus();
        }
    }
}
