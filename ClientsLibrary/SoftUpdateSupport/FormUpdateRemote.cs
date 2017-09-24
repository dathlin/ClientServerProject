using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication;
using HslCommunication.Enthernet;
using CommonLibrary;

namespace ClientsLibrary
{

    //=====================================================================================
    //
    //    用途：    本窗口用于实现对本系统的客户端进行全面的远程更新的操作。
    //    第一步：  先将新版本的所有客户端传送到服务器端进行覆盖。
    //    第二步：  再将版本号传送到服务器进行更新，这样所有的客户端登录后就会自动更新新的版本了
    //
    //=====================================================================================


    public partial class FormUpdateRemote : Form
    {
        public FormUpdateRemote()
        {
            InitializeComponent();

            Icon = UserClient.GetFormWindowIcon();
        }

        private void userButton_file_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (FormFileOperate fUpload = new FormFileOperate(
                        UserClient.Net_File_Client,
                        ofd.FileNames,
                        "ClientFiles", 
                        "", 
                        ""
                        ))
                    {
                        fUpload.ShowDialog();
                    }
                }
            }
        }

        private void userButton_version_Click(object sender, EventArgs e)
        {
            OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.更新版本, textBox1.Text);
            if (result.IsSuccess && result.Content == "1")
            {
                MessageBox.Show("更新成功！");
            }
            else
            {
                MessageBox.Show("更新失败！原因：" + result.ToMessageShowString());
            }
        }

        private void FormUpdateRemote_Load(object sender, EventArgs e)
        {
            textBox1.Text = UserClient.CurrentVersion.ToString();
        }
    }
}
