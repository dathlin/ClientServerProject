using ClientsLibrary.Configuration;
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
    public partial class FormConfiguration : Form
    {
        public FormConfiguration()
        {
            InitializeComponent();
            Icon = UserSystem.GetFormWindowIcon();
        }
        

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            Text = UserLocalization.Localization.SettingsText;


            treeView1.AfterSelect += TreeView1_AfterSelect;
            TreeNode treeNodeSystem = treeView1.Nodes[0];
            treeNodeSystem.Text = UserLocalization.Localization.SettingsSystem;

            treeNodeSystem.Nodes.Add("General", UserLocalization.Localization.SettingsGeneral);
            treeNodeSystem.Nodes.Add("Factory", UserLocalization.Localization.SettingsAccountFactory);
            treeNodeSystem.Nodes.Add("Client", UserLocalization.Localization.SettingsTrustClient);
            treeNodeSystem.Nodes.Add("Roles", UserLocalization.Localization.SettingsRoleAssign);

            treeNodeSystem.Expand();
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name == "System") return;

            if (m_Current != null)
            {
                if (m_Current.Tag.ToString() == e.Node.Name)
                {
                    return;
                }
            }
            
            m_Current?.Dispose();
            UserControl control;

            if (e.Node.Name == "General")
            {
                control = new GeneralConfiguration();
            }
            else if (e.Node.Name == "Factory")
            {
                control = new ArrayConfiguration(
                    CommonLibrary.CommonHeadCode.SimplifyHeadCode.请求分厂,
                    CommonLibrary.CommonHeadCode.SimplifyHeadCode.上传分厂,
                    e.Node.Text);
            }
            else if(e.Node.Name == "Client")
            {
                control = new ClientConfiguration();
            }
            else if (e.Node.Name == "Roles")
            {
                control = new RolesConfiguration();
            }
            else
            {
                control = new UserControl();
            }



            control.Width = panel1.Width;
            control.Height = panel1.Height;
            control.Location = new Point(0, 0);
            control.Parent = panel1;
            control.Tag = e.Node.Name;
            m_Current = control;
        }
        

        private UserControl m_Current = null;
    }
}
