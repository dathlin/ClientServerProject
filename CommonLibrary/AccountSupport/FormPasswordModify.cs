using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonLibrary
{
    /// <summary>
    /// 一个简单的修改密码的类
    /// </summary>
    public partial class FormPasswordModify : Form
    {
        /// <summary>
        /// 实例化一个密码修改窗口，需要指定密码修改的方法
        /// </summary>
        /// <param name="password_old">旧密码，需要用来验证权限</param>
        /// <param name="submitMethod">修改密码的真正方法</param>
        /// <param name="passwordLengthMin">指定密码长度最小值，可不提供</param>
        /// <param name="passwordLengthMax">指定密码长度最大值，可不提供</param>
        public FormPasswordModify(string password_old,
            Func<string, bool> submitMethod,
            int passwordLengthMin = 4,
            int passwordLengthMax = 8)
        {
            InitializeComponent();
            PasswordLengthMin = passwordLengthMin;
            PasswordLengthMax = passwordLengthMax;
            PasswordOriginal = password_old;
            SubmitMethod = submitMethod;
        }

        private void FormPasswordModify_Load(object sender, EventArgs e)
        {
            Text = "密码修改 [务必不要使用简单的密码] ";
            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            label4.Text = $"剩余修改时间：{WaittingTime}秒";
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (WaittingTime > 0)
            {
                label4.Text = $"剩余修改时间：{WaittingTime--}秒";
            }
            else
            {
                WaittingTime = 120;
                panel1.Visible = false;
                label4.Text = $"剩余修改时间：{WaittingTime}秒";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox1.Enabled = true;
                userButton1.Enabled = true;
                timer1.Stop();
            }
        }

        private int WaittingTime { get; set; } = 120;

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                userButton1.PerformClick();
            }
        }

        private void FormPasswordModify_Shown(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox3.Focus();
            }
        }


        private void userButton1_Click(object sender, EventArgs e)
        {
            //验证原密码
            if (textBox1.Text == PasswordOriginal)
            {
                textBox1.Enabled = false;
                userButton1.Enabled = false;
                panel1.Visible = true;
                textBox2.Focus();
                timer1.Start();
            }
            else
            {
                MessageBox.Show("密码验证错误！");
            }
        }


        private string PasswordOriginal { get; set; } = "";
        private int PasswordLengthMin { get; set; } = 4;
        private int PasswordLengthMax { get; set; } = 8;
        private Func<string, bool> SubmitMethod = null;
        private void userButton2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length >= PasswordLengthMin &&
                textBox2.Text.Length <= PasswordLengthMax)
            {
                if (textBox2.Text != textBox3.Text)
                {
                    MessageBox.Show("两次密码不一致，请重新输入");
                }
                else
                {
                    if (SubmitMethod == null)
                    {
                        MessageBox.Show("该功能未实现！");
                    }
                    else
                    {
                        if (SubmitMethod(textBox2.Text))
                        {
                            MessageBox.Show("密码更新成功！");
                        }
                        else
                        {
                            MessageBox.Show("密码更新失败！请稍候重试！");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show($"密码长度不正确，请控制在{PasswordLengthMin}-{PasswordLengthMax}位之间");
            }
        }

        private void FormPasswordModify_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }
    }
}
