using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HslCommunicationNetTestTool
{
    public partial class FormCreateClient : Form
    {
        public FormCreateClient()
        {
            InitializeComponent();
        }

        private void FormCreateClient_Load(object sender, EventArgs e)
        {
            textBox3.Text = Guid.Empty.ToString();
        }

        private void userButton1_Click(object sender, EventArgs e)
        {

            try
            {
                Client = new HslCommunication.Enthernet.NetSimplifyClient(
                    textBox1.Text, int.Parse(textBox2.Text))
                {
                    Token = new Guid(textBox3.Text),
                    ConnectTimeOut = int.Parse(textBox4.Text),
                };

                IpAddress = textBox1.Text;
                Port = textBox2.Text;
                Token = textBox3.Text;
                ConnectTimeout = textBox4.Text;

                DialogResult = DialogResult.OK;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public string IpAddress { get; private set; }
        public string Port { get; private set; }
        public string Token { get; private set; }
        public string ConnectTimeout { get; private set; }



        public HslCommunication.Enthernet.NetSimplifyClient Client { get; private set; }
    }
}
