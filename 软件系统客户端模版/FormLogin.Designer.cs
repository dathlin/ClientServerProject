using HslCommunication.Controls;

namespace 软件系统客户端模版
{
    partial class FormLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_userName = new System.Windows.Forms.TextBox();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.userButton_login = new HslCommunication.Controls.UserButton();
            this.label_version = new System.Windows.Forms.Label();
            this.label_copyright = new System.Windows.Forms.Label();
            this.label_status = new System.Windows.Forms.Label();
            this.checkBox_remeber = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(35, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("微软雅黑", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(396, 36);
            this.label2.TabIndex = 1;
            this.label2.Text = "[软件系统的名称]";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_userName
            // 
            this.textBox_userName.Location = new System.Drawing.Point(115, 57);
            this.textBox_userName.Name = "textBox_userName";
            this.textBox_userName.Size = new System.Drawing.Size(189, 26);
            this.textBox_userName.TabIndex = 2;
            this.textBox_userName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_userName_KeyDown);
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(115, 98);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(189, 26);
            this.textBox_password.TabIndex = 4;
            this.textBox_password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_password_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(35, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 21);
            this.label3.TabIndex = 3;
            this.label3.Text = "密   码：";
            // 
            // userButton_login
            // 
            this.userButton_login.BackColor = System.Drawing.Color.Transparent;
            this.userButton_login.CustomerInformation = "";
            this.userButton_login.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_login.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.userButton_login.Location = new System.Drawing.Point(318, 57);
            this.userButton_login.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.userButton_login.Name = "userButton_login";
            this.userButton_login.Size = new System.Drawing.Size(76, 67);
            this.userButton_login.TabIndex = 5;
            this.userButton_login.UIText = "登录";
            this.userButton_login.Click += new System.EventHandler(this.userButton_login_Click);
            // 
            // label_version
            // 
            this.label_version.AutoSize = true;
            this.label_version.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_version.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_version.ForeColor = System.Drawing.Color.Gray;
            this.label_version.Location = new System.Drawing.Point(1, 176);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(44, 17);
            this.label_version.TabIndex = 6;
            this.label_version.Text = "版本：";
            this.label_version.Click += new System.EventHandler(this.label_version_Click);
            // 
            // label_copyright
            // 
            this.label_copyright.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_copyright.ForeColor = System.Drawing.Color.Gray;
            this.label_copyright.Location = new System.Drawing.Point(215, 176);
            this.label_copyright.Name = "label_copyright";
            this.label_copyright.Size = new System.Drawing.Size(206, 17);
            this.label_copyright.TabIndex = 7;
            this.label_copyright.Text = "本软件著作权归某某某所有";
            this.label_copyright.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label_status
            // 
            this.label_status.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_status.ForeColor = System.Drawing.Color.Blue;
            this.label_status.Location = new System.Drawing.Point(15, 151);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(393, 19);
            this.label_status.TabIndex = 8;
            this.label_status.Text = "[登录的消息提示]";
            this.label_status.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // checkBox_remeber
            // 
            this.checkBox_remeber.AutoSize = true;
            this.checkBox_remeber.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_remeber.Location = new System.Drawing.Point(115, 127);
            this.checkBox_remeber.Name = "checkBox_remeber";
            this.checkBox_remeber.Size = new System.Drawing.Size(87, 21);
            this.checkBox_remeber.TabIndex = 9;
            this.checkBox_remeber.Text = "记住密码？";
            this.checkBox_remeber.UseVisualStyleBackColor = true;
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(420, 194);
            this.Controls.Add(this.checkBox_remeber);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.label_copyright);
            this.Controls.Add(this.label_version);
            this.Controls.Add(this.userButton_login);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_userName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统登录";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLogin_FormClosing);
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.Shown += new System.EventHandler(this.FormLogin_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_userName;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label3;
        private UserButton userButton_login;
        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.Label label_copyright;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.CheckBox checkBox_remeber;
    }
}