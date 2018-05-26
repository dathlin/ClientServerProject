namespace ClientsLibrary.Configuration
{
    partial class ClientConfiguration
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.userButton_delete = new HslCommunication.Controls.UserButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.userButton1 = new HslCommunication.Controls.UserButton();
            this.userButton2 = new HslCommunication.Controls.UserButton();
            this.userButton3 = new HslCommunication.Controls.UserButton();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(3, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(183, 21);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "是否强制启用指定客户端登录";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.label1.Location = new System.Drawing.Point(0, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "允许登录列表：";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 17;
            this.listBox1.Location = new System.Drawing.Point(3, 51);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(552, 344);
            this.listBox1.TabIndex = 2;
            // 
            // userButton_delete
            // 
            this.userButton_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userButton_delete.BackColor = System.Drawing.Color.Transparent;
            this.userButton_delete.CustomerInformation = "";
            this.userButton_delete.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_delete.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_delete.Location = new System.Drawing.Point(444, 23);
            this.userButton_delete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton_delete.Name = "userButton_delete";
            this.userButton_delete.Size = new System.Drawing.Size(111, 25);
            this.userButton_delete.TabIndex = 9;
            this.userButton_delete.UIText = "删除选中项";
            this.userButton_delete.Click += new System.EventHandler(this.userButton_delete_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(3, 406);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(552, 23);
            this.textBox1.TabIndex = 10;
            // 
            // userButton1
            // 
            this.userButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.userButton1.BackColor = System.Drawing.Color.Transparent;
            this.userButton1.CustomerInformation = "";
            this.userButton1.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton1.Location = new System.Drawing.Point(129, 436);
            this.userButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton1.Name = "userButton1";
            this.userButton1.Size = new System.Drawing.Size(120, 25);
            this.userButton1.TabIndex = 11;
            this.userButton1.UIText = "添加客户端标识";
            this.userButton1.Click += new System.EventHandler(this.userButton1_Click);
            // 
            // userButton2
            // 
            this.userButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.userButton2.BackColor = System.Drawing.Color.Transparent;
            this.userButton2.CustomerInformation = "";
            this.userButton2.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton2.Location = new System.Drawing.Point(3, 436);
            this.userButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton2.Name = "userButton2";
            this.userButton2.Size = new System.Drawing.Size(120, 25);
            this.userButton2.TabIndex = 12;
            this.userButton2.UIText = "获取本机标识";
            this.userButton2.Click += new System.EventHandler(this.userButton2_Click);
            // 
            // userButton3
            // 
            this.userButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.userButton3.BackColor = System.Drawing.Color.Transparent;
            this.userButton3.CustomerInformation = "";
            this.userButton3.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton3.Location = new System.Drawing.Point(477, 436);
            this.userButton3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton3.Name = "userButton3";
            this.userButton3.Size = new System.Drawing.Size(78, 25);
            this.userButton3.TabIndex = 13;
            this.userButton3.UIText = "保存";
            this.userButton3.Click += new System.EventHandler(this.userButton3_Click);
            // 
            // ClientConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.userButton3);
            this.Controls.Add(this.userButton2);
            this.Controls.Add(this.userButton1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.userButton_delete);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ClientConfiguration";
            this.Size = new System.Drawing.Size(558, 465);
            this.Load += new System.EventHandler(this.ClientConfiguration_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private HslCommunication.Controls.UserButton userButton_delete;
        private System.Windows.Forms.TextBox textBox1;
        private HslCommunication.Controls.UserButton userButton1;
        private HslCommunication.Controls.UserButton userButton2;
        private HslCommunication.Controls.UserButton userButton3;
    }
}
