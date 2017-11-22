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
    public partial class RolesConfiguration : UserControl
    {
        #region Constructor


        public RolesConfiguration()
        {
            InitializeComponent();
        }


        #endregion

        #region Control Load


        private void RolesConfiguration_Load(object sender, EventArgs e)
        {
            UILocalization();
            // 初始化

            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.请求角色配置, "");
            if (result.IsSuccess)
            {
                List<RoleItem> roles = JArray.Parse(result.Content).ToObject<List<RoleItem>>();
                roles.ForEach(m => listBox1.Items.Add(m));
            }
            else
            {
                MessageBox.Show("请求服务器失败，请稍后重试！");
                userButton4.Enabled = false;
            }
        }


        #endregion

        #region Role Select

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem is RoleItem role)
            {
                listBox2.DataSource = role.Accounts;
                textBox1.Text = role.RoleCode;
                textBox2.Text = role.Description;
            }
        }

        #endregion

        #region Role Delete


        private void userButton2_Click(object sender, EventArgs e)
        {
            // delete list item
            if (listBox1.SelectedItem != null)
            {
                if (MessageBox.Show("是否真的删除该角色信息？", "删除确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    listBox1.Items.Remove(listBox1.SelectedItem);
                    listBox2.DataSource = null;
                }
            }
        }


        #endregion

        #region Role Exist Check


        private bool CheckRoleWhetherExisting(string roleName)
        {
            foreach(var m in listBox1.Items)
            {
                if(m.ToString() == roleName)
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckRoleWhetherExisting(RoleItem role, string roleName)
        {
            foreach (var m in listBox1.Items)
            {
                if (!ReferenceEquals(m, role))
                {
                    if (m.ToString() == roleName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        #endregion

        #region Role Add
        
        private void userButton1_Click(object sender, EventArgs e)
        {
            using (FormInputNewRole form = new FormInputNewRole())
            {
                P1:
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RoleItem role = form.RoleItem;
                    if (CheckRoleWhetherExisting(role.RoleName))
                    {
                        MessageBox.Show("该角色名称已经存在，不允许添加。");
                        goto P1;
                    }

                    // add
                    listBox1.Items.Add(role);
                }
            }
        }

        #endregion

        #region Role Edit


        private void userButton5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is RoleItem role)
            {
                // edit
                using (FormInputNewRole form = new FormInputNewRole())
                {
                    P1:
                    if (form.ShowDialog(role) == DialogResult.OK)
                    {
                        if (CheckRoleWhetherExisting(role, form.RoleName))
                        {
                            MessageBox.Show("该角色名称已经存在，不允许添加。");
                            goto P1;
                        }

                        // edit
                        role.RoleName = form.RoleName;
                        role.Description = form.RoleDescription;

                        textBox1.Text = role.RoleCode;
                        textBox2.Text = role.Description;
                        
                        // refresh
                        //for (int i = 0; i < listBox1.Items.Count; i++)
                        //    listBox1.Items[i] = listBox1.Items[i];
                        listBox1.Items[listBox1.SelectedIndex] = listBox1.Items[listBox1.SelectedIndex];
                    }
                }
            }
        }

        #endregion

        #region Role Account Edit


        private void userButton3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is RoleItem role)
            {
                // select account
                using (FormAccountSelect form = new FormAccountSelect(null, 0, int.MaxValue, role.Accounts))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        role.Accounts = form.SelectAccounts.ConvertAll(m => m.UserName);
                        listBox2.DataSource = role.Accounts;
                    }
                }
            }
        }

        #endregion

        #region Role Upload Server
        
        private void userButton4_Click(object sender, EventArgs e)
        {
            // save
            List<RoleItem> roles = new List<RoleItem>();
            foreach(var m in listBox1.Items)
            {
                if(m is RoleItem item)
                {
                    roles.Add(item);
                }
            }

            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(
                CommonHeadCode.SimplifyHeadCode.上传角色配置, JArray.FromObject(roles).ToString());
            if (result.IsSuccess)
            {
                MessageBox.Show("上传数据成功！");
            }
            else
            {
                MessageBox.Show("上传数据失败："+result.Message);
            }
        }

        #endregion

        #region Localization Support

        /// <summary>
        /// 本地化显示的操作，还未完成
        /// </summary>
        private void UILocalization()
        {

            label1.Text = UserLocalization.Localization.AccountRoleNameList;
            label2.Text = UserLocalization.Localization.AccountRoleAccountList;
            label3.Text = UserLocalization.Localization.GeneralUniqueID;
            label4.Text = UserLocalization.Localization.GeneralDescription;

            userButton1.UIText = UserLocalization.Localization.ButtonAdd;
            userButton5.UIText = UserLocalization.Localization.ButtonEdit;
            userButton2.UIText = UserLocalization.Localization.ButtonDelete;

            userButton3.UIText = UserLocalization.Localization.ButtonEdit;
            userButton4.UIText = UserLocalization.Localization.ButtonSave;
        }


        #endregion
    }
}
