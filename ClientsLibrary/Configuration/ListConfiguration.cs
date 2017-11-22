using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication;
using CommonLibrary;

namespace ClientsLibrary.Configuration
{
    public partial class ArrayConfiguration : UserControl
    {
        #region Constructor


        public ArrayConfiguration(int download, int upload, string headText)
        {
            InitializeComponent();

            SetInfomation(download, upload, headText);

        }

        public ArrayConfiguration()
        {
            InitializeComponent();

        }

        #endregion

        #region Private Member

        private int Download = 0;
        private int Upload = 0;
        private string HeadText = string.Empty;

        #endregion

        #region Public Method


        public void SetInfomation(int download, int upload, string headText)
        {
            Download = download;
            Upload = upload;
            HeadText = headText;
        }

        #endregion

        #region Control Load


        private void FactoryConfiguration_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns[0].HeaderText = HeadText;

            // 向服务器请求数据初始化
            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(Download, "");

            if (result.IsSuccess)
            {
                List<string> ListData = Newtonsoft.Json.Linq.JArray.Parse(result.Content).ToObject<List<string>>();

                for (int i = 0; i < ListData.Count; i++)
                {
                    int rowIndex = dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex].Cells[0].Value = ListData[i];
                }
            }
            else
            {
                MessageBox.Show(result.Message);
                userButton_save.Enabled = false;
            }

            // 本地化支持
            UILocalization();
        }

        #endregion

        #region List Add


        private void userButton_Add_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
        }

        #endregion

        #region List Delete


        private void userButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows != null)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                }
            }
        }

        #endregion

        #region Submit


        private void userButton2_Click(object sender, EventArgs e)
        {
            List<string> data = new List<string>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                {
                    data.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                }
            }

            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(Upload,
                Newtonsoft.Json.Linq.JArray.FromObject(data).ToString());

            if (result.IsSuccess)
            {
                MessageBox.Show("修改成功！");
            }
            else
            {
                MessageBox.Show("修改失败！原因：" + result.Message);
            }
        }


        #endregion

        #region Localization Support

        /// <summary>
        /// 本地化显示的操作，还未完成
        /// </summary>
        private void UILocalization()
        {
            userButton_add.UIText = UserLocalization.Localization.ButtonAdd;
            userButton_delete.UIText = UserLocalization.Localization.ButtonDelete;
            userButton_save.UIText = UserLocalization.Localization.ButtonSave;
        }


        #endregion
    }
}
