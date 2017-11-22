using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using HslCommunication;
using CommonLibrary;

namespace ClientsLibrary.Configuration
{
    public partial class ClientConfiguration : UserControl
    {
        #region Contructor


        public ClientConfiguration()
        {
            InitializeComponent();
        }

        #endregion

        #region Clients Delete


        private void userButton_delete_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem!=null)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }


        #endregion



        private void userButton2_Click(object sender, EventArgs e)
        {
            textBox1.Text = UserClient.JsonSettings.SystemInfo;
        }

        private void userButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) return;

            if(!listBox1.Items.Contains(textBox1.Text))
            {
                listBox1.Items.Add(textBox1.Text);
            }
        }

        private void ClientConfiguration_Load(object sender, EventArgs e)
        {
            // 初始化

            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.请求信任客户端, "");
            if(result.IsSuccess)
            {
                JObject json=JObject.Parse(result.Content);
                checkBox1.Checked = json["TrustEnable"].ToObject<bool>();
                string[] data = json["TrustList"].ToObject<string[]>();
                foreach(var m in data)
                {
                    listBox1.Items.Add(m);
                }
            }
            else
            {
                MessageBox.Show("请求服务器失败，请稍后重试！");
                userButton3.Enabled = false;
            }

            // 本地化
            UILocalization();
        }

        private void userButton3_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach(var m in listBox1.Items)
            {
                if(!string.IsNullOrEmpty(m.ToString()))
                {
                    list.Add(m.ToString());
                }
            }

            JObject json = new JObject
            {
                { "TrustEnable", new JValue(checkBox1.Checked) },
                { "TrustList", new JArray(list.ToArray()) }
            };

            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(
                CommonLibrary.CommonHeadCode.SimplifyHeadCode.上传信任客户端, json.ToString());

            if(result.IsSuccess)
            {
                MessageBox.Show("上传成功！");
            }
            else
            {
                MessageBox.Show("上传失败！");
            }
        }



        #region Localization Support

        /// <summary>
        /// 本地化显示的操作，还未完成
        /// </summary>
        private void UILocalization()
        {
            checkBox1.Text = UserLocalization.Localization.CheckBoxTrustEnable;
            label1.Text = UserLocalization.Localization.GeneralAllowLoginList;
            userButton_delete.UIText = UserLocalization.Localization.ButtonDeleteSelected;
            userButton2.UIText = UserLocalization.Localization.ButtonGetComputerID;
            userButton1.UIText = UserLocalization.Localization.ButtonAddComputerID;
            userButton3.UIText = UserLocalization.Localization.ButtonSave;
        }


        #endregion
    }
}
