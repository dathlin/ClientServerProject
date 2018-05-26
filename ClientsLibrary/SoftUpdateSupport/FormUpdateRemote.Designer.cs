using HslCommunication.Controls;

namespace ClientsLibrary
{
    partial class FormUpdateRemote
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
            this.userButton_file = new HslCommunication.Controls.UserButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.userButton_version = new HslCommunication.Controls.UserButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(24, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "第一步：更新所有新版本的文件。";
            // 
            // userButton_file
            // 
            this.userButton_file.BackColor = System.Drawing.Color.Transparent;
            this.userButton_file.CustomerInformation = "";
            this.userButton_file.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_file.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_file.Location = new System.Drawing.Point(216, 62);
            this.userButton_file.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.userButton_file.Name = "userButton_file";
            this.userButton_file.Size = new System.Drawing.Size(94, 37);
            this.userButton_file.TabIndex = 13;
            this.userButton_file.UIText = "选择文件";
            this.userButton_file.Click += new System.EventHandler(this.userButton_file_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(141, 181);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(245, 23);
            this.textBox1.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(24, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "第二步：更新服务器的版本号。";
            // 
            // userButton_version
            // 
            this.userButton_version.BackColor = System.Drawing.Color.Transparent;
            this.userButton_version.CustomerInformation = "";
            this.userButton_version.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_version.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_version.Location = new System.Drawing.Point(216, 236);
            this.userButton_version.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.userButton_version.Name = "userButton_version";
            this.userButton_version.Size = new System.Drawing.Size(94, 37);
            this.userButton_version.TabIndex = 16;
            this.userButton_version.UIText = "提交版本号";
            this.userButton_version.Click += new System.EventHandler(this.userButton_version_Click);
            // 
            // FormUpdateRemote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(569, 311);
            this.Controls.Add(this.userButton_version);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.userButton_file);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(585, 350);
            this.MinimumSize = new System.Drawing.Size(585, 350);
            this.Name = "FormUpdateRemote";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "远程更新客户端";
            this.Load += new System.EventHandler(this.FormUpdateRemote_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private UserButton userButton_file;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private UserButton userButton_version;
    }
}