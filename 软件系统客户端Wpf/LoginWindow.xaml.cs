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
using System.Windows.Shapes;

namespace 软件系统客户端Wpf
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlockSoftName.Text = CommonLibrary.Resource.StringResouce.SoftName;
            TextBlockSoftVersion.Text = UserClient.CurrentVersion.ToString();
            TextBlockSoftCopyright.Text = $"本软件著作权归{CommonLibrary.Resource.StringResouce.SoftCopyRight}所有";

            UserClient.JsonSettings.FileSavePath = AppDomain.CurrentDomain.BaseDirectory + @"\JsonSettings.txt";
            UserClient.JsonSettings.LoadByFile();

            if ((DateTime.Now - UserClient.JsonSettings.LoginTime).TotalDays < 7)
            {
                //加载数据
                NameTextBox.Text = UserClient.JsonSettings.LoginName ?? "";
                PasswordBox.Password = UserClient.JsonSettings.Password ?? "";
                Remember.IsChecked = UserClient.JsonSettings.Password != "";
            }
            //初始化输入焦点
            if (UserClient.JsonSettings.Password != "") LoginButton.Focus();
            else if (UserClient.JsonSettings.LoginName != "") PasswordBox.Focus();
            else NameTextBox.Focus();
        }

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            PasswordBox.Focus();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            Button_Click(null, new RoutedEventArgs());
        }
    }
}
