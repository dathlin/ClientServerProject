namespace ClientsLibrary
{
    partial class FormGetInputString
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.userButton_login = new HslCommunication.Controls.UserButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(21, 67);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(383, 29);
            this.textBox1.TabIndex = 0;
            // 
            // userButton_login
            // 
            this.userButton_login.BackColor = System.Drawing.Color.Transparent;
            this.userButton_login.CustomerInformation = "";
            this.userButton_login.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_login.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_login.Location = new System.Drawing.Point(154, 101);
            this.userButton_login.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton_login.Name = "userButton_login";
            this.userButton_login.Size = new System.Drawing.Size(108, 27);
            this.userButton_login.TabIndex = 7;
            this.userButton_login.UIText = "确认";
            this.userButton_login.Click += new System.EventHandler(this.userButton_login_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(18, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(386, 55);
            this.label1.TabIndex = 8;
            this.label1.Text = "label1";
            // 
            // FormGetInputString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(434, 133);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userButton_login);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGetInputString";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "等待输入信息";
            this.Load += new System.EventHandler(this.FormGetInputString_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private HslCommunication.Controls.UserButton userButton_login;
        private System.Windows.Forms.Label label1;
    }
}