using ClientsLibrary.Configuration;
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
    public partial class FormConfiguration : Form
    {
        public FormConfiguration()
        {
            InitializeComponent();
            Icon = UserClient.GetFormWindowIcon();
        }
        

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            Text = "配置系统的参数";


            treeView1.AfterSelect += TreeView1_AfterSelect;
            TreeNode treeNodeSystem = treeView1.Nodes[0];

            treeNodeSystem.Nodes.Add("General", "常规配置");
            treeNodeSystem.Nodes.Add("Factory", "配置分厂信息");
            treeNodeSystem.Nodes.Add("Client", "客户端信任功能");

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
