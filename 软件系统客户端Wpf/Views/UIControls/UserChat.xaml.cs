using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        #region Constructor
        
        public UserChat(Action<string> send)
        {
            InitializeComponent();
            SendString = send;
        }

        #endregion

        #region Add Chat Show


        public void DealwithReceive(string str)
        {
            TextBox_ChatHistory.AppendText(str + Environment.NewLine);
            int length = str.IndexOf(Environment.NewLine) + 1;
            if (length > 0)
            {
                TextBox_ChatHistory.Select(TextBox_ChatHistory.Text.Length - str.Length + 1, length);
                TextBox_ChatHistory.SelectionBrush = Brushes.Blue;
            }
            ScrollToDown();
        }

        /// <summary>
        /// 新增聊天的历史记录
        /// </summary>
        /// <param name="str"></param>
        public void AddChatsHistory(string str)
        {
            TextBox_ChatHistory.Text = str;
            MatchCollection mc = Regex.Matches(str, @"\u0002.+\r\n");
            int indexrow = 0;
            if (str != "")
            {
                foreach (Match m in mc)
                {
                    TextBox_ChatHistory.Select(m.Index - indexrow * 2, m.Length - 2);
                    TextBox_ChatHistory.SelectionBrush = Brushes.Blue;
                    indexrow++;
                }
            }
            ScrollToDown();
        }


        #endregion

        #region Chat Focus Scroll


        public void InputFocus()
        {
            //textBox1.Focus();
        }

        /// <summary>
        /// 光标滚动到最底端
        /// </summary>
        public void ScrollToDown()
        {
            TextBox_ChatHistory.ScrollToEnd();
        }


        #endregion

        #region Text Send


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBox_Send.Text))
            {
                SendString?.Invoke(TextBox_Send.Text);
                TextBox_Send.Text = "";
            }
        }


        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, new RoutedEventArgs());
            }
        }

        #endregion

        #region Private Member


        private Action<string> SendString = null;

        #endregion
    }
}
