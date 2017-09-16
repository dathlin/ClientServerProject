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
        }
        

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            Text = "配置系统的参数";

            treeView1.ShowRootLines = false;
            treeView1.Nodes.Clear();


            TreeNode root = new TreeNode("配置系统数据");

            

            treeView1.Nodes.Add(root);
        }
        
    }
}
