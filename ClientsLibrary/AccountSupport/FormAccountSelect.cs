using CommonLibrary;
using HslCommunication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClientsLibrary
{
    public partial class FormAccountSelect : Form
    {
        #region Constructor

        /// <summary>
        /// 实例化一个选择服务器账户的窗口，该窗口可以根据工厂属性或是角色属性来筛选
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="selected"></param>
        public FormAccountSelect(string condition = null, List<string> selected = null)
        {
            InitializeComponent();

            Icon = UserSystem.GetFormWindowIcon();
            m_selected = selected;
            m_condition = condition;
        }


        #endregion

        #region Form Load


        private void FormAccountSelect_Load(object sender, EventArgs e)
        {
            // 初始化

            OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.获取账户, m_condition);
            if (result.IsSuccess)
            {
                List<UserAccount> accounts = JArray.Parse(result.Content).ToObject<List<UserAccount>>();
                // 添加到数据表

                foreach (var m in accounts)
                {
                    DataGridViewRow dgvr = dataGridView1.Rows[dataGridView1.Rows.Add()];
                    if(m_selected != null)
                    {
                        if(m_selected.Contains(m.UserName))
                        {
                            dgvr.Cells[0].Value = true;
                        }
                    }

                    dgvr.Cells[1].Value = m.UserName;
                    dgvr.Cells[2].Value = m.NameAlias;
                    dgvr.Cells[3].Value = m.Factory;
                    dgvr.Cells[4].Value = m.RegisterTime.ToString();
                    dgvr.Tag = m;
                }
            }
            else
            {
                MessageBox.Show("请求服务器失败，请稍后重试！");

            }

            // 本地化
            UILocalization();

            // dataGridView1.RowsDefaultCellStyle.SelectionBackColor = dataGridView1.RowsDefaultCellStyle.BackColor;
            // dataGridView1.RowsDefaultCellStyle.SelectionForeColor = dataGridView1.RowsDefaultCellStyle.ForeColor;
        }


        #endregion

        #region Localization Support

        private void UILocalization()
        {

            Text = UserLocalization.Localization.AccountSelect;
            Column1.HeaderText = UserLocalization.Localization.AccountSelect;
            Column2.HeaderText = UserLocalization.Localization.AccountName;
            Column3.HeaderText = UserLocalization.Localization.AccountAlias;
            Column4.HeaderText = UserLocalization.Localization.AccountFactory;
            Column5.HeaderText = UserLocalization.Localization.AccountRegisterTime;

            userButton_login.UIText = UserLocalization.Localization.ButtonEnsure;
        }

        #endregion

        #region Public Property
        /// <summary>
        /// 返回已经选择的账户
        /// </summary>
        public List<UserAccount> SelectAccounts
        {
            get
            {
                return m_result;
            }
        }

        #endregion

        #region Button Click

        private void userButton_login_Click(object sender, EventArgs e)
        {
            m_result = new List<UserAccount>();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow dgvr = dataGridView1.Rows[i];
                if(dgvr.Cells[0].Value != null)
                {
                    if ((bool)dgvr.Cells[0].Value)
                    {
                        m_result.Add((UserAccount)dgvr.Tag);
                    }
                }
            }

            DialogResult = DialogResult.OK;
        }



        #endregion

        #region Private Members


        private List<string> m_selected;
        private List<UserAccount> m_result;
        private string m_condition;

        #endregion

        
    }
}
