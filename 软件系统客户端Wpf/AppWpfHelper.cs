using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace 软件系统客户端Wpf
{


    /***********************************************************************************
     * 
     *    说明：用于开发一些wpf专有的方法，一些转换方法
     * 
     ***********************************************************************************/



    public class AppWpfHelper
    {
        public static BitmapImage TranslateImageToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            if(bitmap.RawFormat != null) bitmap.Save(ms, bitmap.RawFormat);
            else bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }

    }
}
