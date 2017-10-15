using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication.Controls;

namespace CommonLibrary
{

    /// <summary>
    /// 支持输入一串字符串并执行相应的操作
    /// </summary>
    public partial class FormInputAndAction : Form
    {
        #region Constructor
        
        /// <summary>
        /// 实例化一个窗口支持输出和响应
        /// </summary>
        /// <param name="action">响应的方法</param>
        /// <param name="text_default">默认的数据</param>
        /// <param name="caption">标题</param>
        /// <param name="length">允许输入的文本的最大长度</param>
        public FormInputAndAction(Func<string, bool> action, string text_default = "", string caption = "请输入数据", int length = 1000)
        {
            InitializeComponent();
            ButtonAction = action;
            Caption = caption;
            InputLength = length;
            DefaultStr = text_default;
            Icon = UserSystem.GetFormWindowIcon();
        }


        #endregion


        private void FormInputAndAction_Load(object sender, EventArgs e)
        {
            Text = Caption;

            if (InputLength < int.MaxValue)
            {
                label1.Text += InputLength;
            }
            else
            {
                label1.Text += "无";
            }

            textBox1.Text = DefaultStr;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                textBox1.Focus();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label3.Text = textBox1.Text.Length.ToString();
            if(textBox1.Text.Length>InputLength)
            {
                label2.ForeColor = Color.Red;
                label3.ForeColor = Color.Red;
            }
            else
            {
                label2.ForeColor = Color.DimGray;
                label3.ForeColor = Color.DimGray;
            }
        }

        private void userButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > InputLength)
            {
                MessageBox.Show("字数太多，超出了" + InputLength + "字");
                return;
            }

            if(ButtonAction(textBox1.Text))
            {
                MessageBox.Show("提交成功！");
            }
            else
            {
                MessageBox.Show("提交失败！");
            }
        }

        private void FormInputAndAction_FormClosing(object sender, FormClosingEventArgs e)
        {
            ButtonAction = null;//释放
        }

        private void FormInputAndAction_Shown(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        #region Private Members
        
        private Func<string, bool> ButtonAction = null;
        private string Caption = "";
        private int InputLength = int.MaxValue;
        private string DefaultStr = "";

        #endregion
    }
}
