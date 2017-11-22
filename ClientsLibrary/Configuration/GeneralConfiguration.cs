using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication;
using Newtonsoft.Json.Linq;
using CommonLibrary;

namespace ClientsLibrary.Configuration
{
    public partial class GeneralConfiguration : UserControl
    {
        #region Constructor


        public GeneralConfiguration()
        {
            InitializeComponent();
        }


        #endregion

        #region Control Load


        private void GeneralConfiguration_Load(object sender, EventArgs e)
        {
            // 初始化

            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.请求信任客户端, "");
            if (result.IsSuccess)
            {
                JObject json = JObject.Parse(result.Content);
                checkBox1.Checked = HslCommunication.BasicFramework.SoftBasic.GetValueFromJsonObject(json, "AllowUserMulti", false);
            }
            else
            {
                MessageBox.Show("请求服务器失败，请稍后重试！");
                userButton2.Enabled = false;
            }

            // 文本本地化
            UILocalization();
        }

        #endregion

        #region Submit

        
        private void userButton2_Click(object sender, EventArgs e)
        {
            JObject json = new JObject
            {
                { "AllowUserMulti", new JValue(checkBox1.Checked) },
                { "AllowFrameLogin", new JValue(checkBox2.Checked) },
            };

            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(
                CommonLibrary.CommonHeadCode.SimplifyHeadCode.上传信任客户端, json.ToString());

            if (result.IsSuccess)
            {
                MessageBox.Show("上传成功！");
            }
            else
            {
                MessageBox.Show("上传失败！");
            }
        }

        #endregion

        #region Localization Support

        /// <summary>
        /// 本地化显示的操作，还未完成
        /// </summary>
        private void UILocalization()
        {
            checkBox1.Text = UserLocalization.Localization.CheckBoxAllowUserMulti;
            checkBox2.Text = UserLocalization.Localization.CheckBoxAllowFrameLogin;
            userButton2.UIText = UserLocalization.Localization.ButtonSave;

        }


        #endregion
    }
}
