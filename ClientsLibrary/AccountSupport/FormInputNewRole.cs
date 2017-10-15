using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonLibrary;

namespace ClientsLibrary
{

    /// <summary>
    /// 用户输入或者编辑新的角色内容
    /// </summary>
    public partial class FormInputNewRole : Form
    {
        #region Constructor


        public FormInputNewRole()
        {
            InitializeComponent();

            Icon = UserSystem.GetFormWindowIcon();
        }

        #endregion

        #region Localization Support

        /// <summary>
        /// 本地化显示的操作，还未完成
        /// </summary>
        private void UILocalization()
        {
            if(m_RoleItem == null)
            {
                Text = UserLocalization.Localization.AccountRoleAdd;
            }
            else
            {
                Text = UserLocalization.Localization.AccountRoleEdit;
            }

            label1.Text = UserLocalization.Localization.GeneralUniqueID + "：";
            label2.Text = UserLocalization.Localization.GeneralName + "：";
            label3.Text = UserLocalization.Localization.GeneralDescription + "：";

            userButton_login.UIText = UserLocalization.Localization.ButtonEnsure;
        }


        #endregion

        #region Form Load Show


        private void FormInputNewRole_Load(object sender, EventArgs e)
        {
            if (m_RoleItem == null)
            {
                textBox1.Text = Guid.NewGuid().ToString("N");
            }
        }

        private void FormInputNewRole_Shown(object sender, EventArgs e)
        {
            UILocalization();
        }


        #endregion

        #region Show Dialog


        public DialogResult ShowDialog(RoleItem roleItem)
        {
            // 此处是编辑
            m_RoleItem = roleItem;
            
            if (m_RoleItem != null)
            {
                textBox1.Text = m_RoleItem.RoleCode;
                textBox2.Text = m_RoleItem.RoleName;
                textBox3.Text = m_RoleItem.Description;
            }

            return ShowDialog();
        }

        #endregion
        
        #region Public Property

        public RoleItem RoleItem
        {
            get { return m_RoleItem; }
        }

        public string RoleName
        {
            get { return textBox2.Text; }
        }

        public string RoleDescription
        {
            get { return textBox3.Text; }
        }

        #endregion

        #region Private Members

        private RoleItem m_RoleItem;

        #endregion

        #region Save


        private void userButton_login_Click(object sender, EventArgs e)
        {
            if(m_RoleItem == null)
            {
                m_RoleItem = new RoleItem()
                {
                    RoleCode = textBox1.Text,
                    RoleName = textBox2.Text,
                    Description = textBox3.Text,
                };

            }
            
            DialogResult = DialogResult.OK;
        }

        #endregion


    }
}
