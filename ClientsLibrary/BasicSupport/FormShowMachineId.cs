using CommonLibrary;
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
    public partial class FormShowMachineId : Form
    {
        public FormShowMachineId()
        {
            InitializeComponent();

            Icon = UserSystem.GetFormWindowIcon();
        }

        private void userButton_login_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
        }

        private void FormShowMachineId_Load(object sender, EventArgs e)
        {
            textBox1.Text = UserClient.JsonSettings.SystemInfo;
        }
    }
}
