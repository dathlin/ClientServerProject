using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 软件系统客户端Wpf.Views
{
    /// <summary>
    /// UserChat.xaml 的交互逻辑
    /// </summary>
    public partial class UserChat : UserControl
    {
        public UserChat(Action<string> send)
        {
            InitializeComponent();
            SendString = send;
        }


        private Action<string> SendString = null;

        public void DealwithReceive(string str)
        {
            //richTextBox1.AppendText(str + Environment.NewLine);
            //int length = str.IndexOf(Environment.NewLine) + 1;
            //if (length > 0)
            //{
            //    richTextBox1.Select(richTextBox1.Text.Length - str.Length + 1, length);
            //    richTextBox1.SelectionColor = Color.Blue;
            //}
            //ScrollToDown();
        }
        /// <summary>
        /// 新增聊天的历史记录
        /// </summary>
        /// <param name="str"></param>
        public void AddChatsHistory(string str)
        {
            //richTextBox1.Text = str;
            //MatchCollection mc = Regex.Matches(str, @"\u0002.+\r\n");
            //int indexrow = 0;
            //if (str != "")
            //{
            //    foreach (Match m in mc)
            //    {
            //        richTextBox1.Select(m.Index - indexrow * 2, m.Length - 2);
            //        richTextBox1.SelectionColor = Color.Blue;
            //        indexrow++;
            //    }
            //}
            //ScrollToDown();
        }

        public void InputFocus()
        {
            //textBox1.Focus();
        }



        ///// <summary>
        ///// 光标滚动到最底端
        ///// </summary>
        //public void ScrollToDown()
        //{
        //    richTextBox1.SelectionStart = richTextBox1.Text.Length;
        //    richTextBox1.ScrollToCaret();
        //}
    }
}
