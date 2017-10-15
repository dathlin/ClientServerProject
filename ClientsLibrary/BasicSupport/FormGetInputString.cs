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
    public partial class FormGetInputString : Form
    {
        public FormGetInputString(
            string info,
            string defaultValue = "",
            string title = "等待输入信息",
            int maxlength = 100
            )
        {
            InitializeComponent();

            Icon = UserSystem.GetFormWindowIcon();
            Info = info;
            DefaultValue = defaultValue;
            Title = title;
            MaxLength = maxlength;
        }

        private void FormGetInputString_Load(object sender, EventArgs e)
        {
            textBox1.Text = DefaultValue;
            textBox1.MaxLength = MaxLength;
            Text = Title;
            label1.Text = Info;
        }




        private string Info;
        private string DefaultValue;
        private string Title;
        private int MaxLength = 100;
        private string m_outPut;

        public string InputString { get => m_outPut; private set => m_outPut = value; }

        private void userButton_login_Click(object sender, EventArgs e)
        {
            m_outPut = textBox1.Text;
            DialogResult = DialogResult.OK;
        }
    }
}
