using MaterialDesignThemes.Wpf;
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
    /// UserPaletteSelector.xaml 的交互逻辑
    /// </summary>
    public partial class UserPaletteSelector : UserControl
    {
        public UserPaletteSelector()
        {
            InitializeComponent();
        }

        private void CheckBox_Theme_Checked(object sender, RoutedEventArgs e)
        {
            ClientsLibrary.UserClient.JsonSettings.IsThemeDark = true;
            ClientsLibrary.UserClient.JsonSettings.SaveToFile();
        }

        private void CheckBox_Theme_Unchecked(object sender, RoutedEventArgs e)
        {
            ClientsLibrary.UserClient.JsonSettings.IsThemeDark = false;
            ClientsLibrary.UserClient.JsonSettings.SaveToFile();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ClientsLibrary.UserClient.JsonSettings.IsThemeDark) CheckBox_Theme.IsChecked = true;
        }
    }
}
