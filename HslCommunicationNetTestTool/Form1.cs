using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using HslCommunication;

namespace HslCommunicationNetTestTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Tag is HslCommunication.Enthernet.NetSimplifyClient client)
            {
                CurrentClient = client;
                label7.Text = "选择连接：" + treeView1.SelectedNode.Text;
            }
        }

        private HslCommunication.Enthernet.NetSimplifyClient CurrentClient { get; set; }
        private List<ClassNetSettings> List { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 加载配置，如果存在的话
            string saveStr = Settings1.Default.Connects;
            if (!string.IsNullOrEmpty(saveStr))
            {
                ClassNetSettings[] settings = JArray.Parse(saveStr).ToObject<ClassNetSettings[]>();

                TreeNode root = treeView1.Nodes[0];

                foreach (var m in settings)
                {
                    string name = $"{m.IpAddress} [{m.IpPort}]";
                    TreeNode node = new TreeNode(name);
                    node.Tag = new HslCommunication.Enthernet.NetSimplifyClient(
                        m.IpAddress,
                        int.Parse(m.IpPort))
                    {
                        ConnectTimeOut = int.Parse(m.TimeOut),
                        Token = new Guid(m.Token),
                    };
                    node.ToolTipText = m.Guid;

                    root.Nodes.Add(node);
                }

                root.Expand();
                List = new List<ClassNetSettings>(settings);
            }

            if (List == null) List = new List<ClassNetSettings>();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings1.Default.Connects = JArray.FromObject(List).ToString();
            Settings1.Default.Save();

        }

        private void userButton1_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes[0].IsSelected)
            {
                // 新增端口操作
                using (FormCreateClient form = new FormCreateClient())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        HslCommunication.Enthernet.NetSimplifyClient client = form.Client;

                        ClassNetSettings setting = new ClassNetSettings()
                        {
                            Guid = Guid.NewGuid().ToString(),
                            IpAddress = form.IpAddress,
                            IpPort = form.Port,
                            Token = form.Token,
                            TimeOut = form.ConnectTimeout,
                        };

                        string name = $"{form.IpAddress} [{form.Port}]";
                        TreeNode node = new TreeNode(name);
                        node.Tag = client;
                        treeView1.Nodes[0].Nodes.Add(node);

                        List.Add(setting);
                    }
                }
            }
        }

        private void userButton2_Click(object sender, EventArgs e)
        {
            // 删除端口操作
            if (ReferenceEquals(treeView1.SelectedNode.Parent, treeView1.Nodes[0]))
            {
                if (treeView1.SelectedNode.Tag is HslCommunication.Enthernet.NetSimplifyClient client)
                {
                    List.RemoveAll(match => match.Guid == treeView1.SelectedNode.ToolTipText);
                    treeView1.SelectedNode.Parent.Nodes.Remove(treeView1.SelectedNode);
                }
            }
        }

        private void userButton3_Click(object sender, EventArgs e)
        {
            if (CurrentClient != null)
            {
                try
                {
                    NetHandle handle = 0;
                    if (textBox1.Text.IndexOf(".") < 0)
                    {
                        // 纯数字
                        handle = int.Parse(textBox1.Text);
                    }
                    else
                    {
                        string[] values = textBox1.Text.Split('.');
                        handle = new NetHandle(byte.Parse(values[0]), byte.Parse(values[1]), ushort.Parse(values[2]));
                    }

                    int count = int.Parse(textBox4.Text);

                    DateTime start = DateTime.Now;

                    for (int i = 0; i < count; i++)
                    {

                        OperateResult<string> resultString = CurrentClient.ReadFromServer(handle, textBox2.Text);

                        if (resultString.IsSuccess)
                        {
                            textBox3.AppendText(resultString.Content + Environment.NewLine);
                        }
                        else
                        {
                            textBox3.AppendText("读取失败：" + resultString.Message + Environment.NewLine);
                        }
                    }

                    label_timeSpend.Text = (DateTime.Now - start).TotalSeconds.ToString("F3");

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void userButton4_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
        }
    }
}
