using ClientsLibrary;
using HslCommunication.Enthernet;
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
    /// UserFileRenderItem.xaml 的交互逻辑
    /// </summary>
    public partial class UserFileRenderItem : UserControl
    {
        public UserFileRenderItem()
        {
            InitializeComponent();
        }

        private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, bitmap.RawFormat);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }

            return bitmapImage;
        }

        private HslSoftFile Hufile { get; set; } = null;
        /// <summary>
        /// 设置文件数据
        /// </summary>
        /// <param name="file">文件的信息对象</param>
        /// <param name="deleteEnable">删除控件的使能委托</param>
        public void SetFile(HslSoftFile file)
        {
            Hufile = file;
            //获取后缀名
            int dotIndex = Hufile.FileName.LastIndexOf('.');
            if (dotIndex >= 0)
            {
                FileIcon.Source = BitmapToBitmapImage(Hufile.GetFileIcon());
            }

            FileName.Text = "文件名称：" + file.FileName;
            FileSize.Text = "大小：" + file.GetTextFromFileSize();
            FileDate.Text = "日期：" + file.UploadDate.ToString("yyyy-MM-dd");
            FileDescription.Text = "文件备注：" + file.FileNote;
            FilePeople.Text = "上传人：" + file.UploadName;
            FileDownloadTimes.Text = "下载数：" + file.FileDownloadTimes;

            FileDeleteButton.IsEnabled = file.UploadName == UserClient.UserAccount.UserName;
            FileDownloadButton.IsEnabled = true;
        }
    }
}
