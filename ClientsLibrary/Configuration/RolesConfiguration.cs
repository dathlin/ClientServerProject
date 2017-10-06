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
        public RolesConfiguration()
        {
            InitializeComponent();
        }

        private void RolesConfiguration_Load(object sender, EventArgs e)
        {
            // 初始化

            OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.请求角色配置, "");
            if (result.IsSuccess)
            {
                List<RoleItem> roles = JArray.Parse(result.Content).ToObject<List<RoleItem>>();
                
                foreach(var m in roles)
                {
                    listBox1.Items.Add(m);
                }
            }
            else
            {
                MessageBox.Show("请求服务器失败，请稍后重试！");
                userButton4.Enabled = false;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem is RoleItem role)
            {
                listBox2.DataSource = role.Accounts;
            }
        }

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

        private bool CheckRoleWhetherExisting(string role)
        {
            foreach(var m in listBox1.Items)
            {
                if(m.ToString() == role)
                {
                    return true;
                }
            }
            return false;
        }

        private void userButton1_Click(object sender, EventArgs e)
        {
            // add a new role
            using (FormGetInputString form = new FormGetInputString("请输入新的角色名称"))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string role = form.InputString;
                    if(!CheckRoleWhetherExisting(role))
                    {
                        // add
                        listBox1.Items.Add(new RoleItem()
                        {
                            RoleName = role
                        });
                    }
                }
            }
        }

        private void userButton3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is RoleItem role)
            {
                // select account
                using (FormAccountSelect form = new FormAccountSelect(role.Accounts))
                {
                    if(form.ShowDialog() == DialogResult.OK)
                    {
                        role.Accounts = form.SelectAccounts.ConvertAll(m => m.UserName);
                        listBox2.DataSource = role.Accounts;
                    }
                }
            }
        }

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

            OperateResultString result = UserClient.Net_simplify_client.ReadFromServer(
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
    }
}
