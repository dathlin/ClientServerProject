namespace ClientsLibrary
{
    partial class FormShowMachineId
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormShowMachineId));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.userButton_login = new HslCommunication.Controls.UserButton();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(29, 26);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(294, 29);
            this.textBox1.TabIndex = 0;
            // 
            // userButton_login
            // 
            this.userButton_login.BackColor = System.Drawing.Color.Transparent;
            this.userButton_login.CustomerInformation = "";
            this.userButton_login.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_login.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_login.Location = new System.Drawing.Point(125, 65);
            this.userButton_login.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton_login.Name = "userButton_login";
            this.userButton_login.Size = new System.Drawing.Size(108, 27);
            this.userButton_login.TabIndex = 6;
            this.userButton_login.UIText = "复制到剪贴板";
            this.userButton_login.Click += new System.EventHandler(this.userButton_login_Click);
            // 
            // FormShowMachineId
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(356, 105);
            this.Controls.Add(this.userButton_login);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormShowMachineId";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "显示机器码";
            this.Load += new System.EventHandler(this.FormShowMachineId_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private HslCommunication.Controls.UserButton userButton_login;
    }
}