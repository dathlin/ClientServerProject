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

    /**************************************************************************************************
     * 
     *    时间：     2017年10月17日 08:02:15
     *    功能说明： 本窗口提供一个账户选择的功能，比如在分配任务时，需要支持分配给某个部门的人，或是分配给
     *               某个角色权限的人，可以在此处进行账户选择，根据实例化参数的不同来区分
     *    举例：     选择某个工厂的一个账户，需要这么实例化：FormAccountSelect form = new FormAccountSelect("分厂示例一",1,1,null)
     *               选择某个角色的一个账户，需要这么实例化：FormAccountSelect form = new FormAccountSelect("2487f33a8f22408ca4bbca5456a7eecb",1,1,null)
     *               两者区分的条件是判断字符串的长度是否大于等于16
     *               如果允许选择所有的账户的一个账户，需要这么实例化：FormAccountSelect form = new FormAccountSelect("",1,1,null)
     *               当然也支持选择多个账户，直至选择完成所有的账户
     * 
     ***************************************************************************************************/



    /// <summary>
    /// 账户选择器的类，支持从服务器的账户数据上选择我们需要的账户
    /// </summary>
    public partial class FormAccountSelect : Form
    {
        #region Constructor

        /// <summary>
        /// 实例化一个选择服务器账户的窗口，该窗口可以根据工厂属性或是角色属性来筛选
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <param name="minSelected">选择账户数量的下限</param>
        /// <param name="multiSelected">选择账户数量的上限</param>
        /// <param name="selected">已选择的账户名</param>
        public FormAccountSelect(string condition = null, int minSelected = 0, int maxSelected = int.MaxValue, List<string> selected = null)
        {
            InitializeComponent();

            Icon = UserSystem.GetFormWindowIcon();
            m_selected = selected;
            m_condition = condition;
            m_MaxSelected = maxSelected;
            m_MinSelected = minSelected;
        }


        #endregion

        #region Form Load


        private void FormAccountSelect_Load(object sender, EventArgs e)
        {
            // 初始化

            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.获取账户, m_condition);
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
                if (dgvr.Cells[0].Value != null)
                {
                    if ((bool)dgvr.Cells[0].Value)
                    {
                        m_result.Add((UserAccount)dgvr.Tag);
                    }
                }
            }

            if (m_result.Count < m_MinSelected || m_result.Count > m_MaxSelected)
            {
                MessageBox.Show(string.Format(UserLocalization.Localization.FormateBetweenTwoSize, m_MinSelected, m_MaxSelected));
                return;
            }

            DialogResult = DialogResult.OK;
        }



        #endregion

        #region Private Members


        private List<string> m_selected;
        private List<UserAccount> m_result;
        private string m_condition;
        private int m_MaxSelected = int.MaxValue;
        private int m_MinSelected = 0;

        #endregion

        
    }
}
