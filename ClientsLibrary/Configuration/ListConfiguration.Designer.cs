namespace ClientsLibrary.Configuration
{
    partial class ArrayConfiguration
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Factory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userButton_login = new HslCommunication.Controls.UserButton();
            this.userButton1 = new HslCommunication.Controls.UserButton();
            this.userButton2 = new HslCommunication.Controls.UserButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Factory});
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(500, 408);
            this.dataGridView1.TabIndex = 0;
            // 
            // Factory
            // 
            this.Factory.HeaderText = "分厂名称";
            this.Factory.Name = "Factory";
            this.Factory.Width = 400;
            // 
            // userButton_login
            // 
            this.userButton_login.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.userButton_login.BackColor = System.Drawing.Color.Transparent;
            this.userButton_login.CustomerInformation = "";
            this.userButton_login.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_login.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_login.Location = new System.Drawing.Point(3, 417);
            this.userButton_login.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton_login.Name = "userButton_login";
            this.userButton_login.Size = new System.Drawing.Size(78, 25);
            this.userButton_login.TabIndex = 7;
            this.userButton_login.UIText = "新增";
            this.userButton_login.Click += new System.EventHandler(this.userButton_login_Click);
            // 
            // userButton1
            // 
            this.userButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.userButton1.BackColor = System.Drawing.Color.Transparent;
            this.userButton1.CustomerInformation = "";
            this.userButton1.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton1.Location = new System.Drawing.Point(87, 417);
            this.userButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton1.Name = "userButton1";
            this.userButton1.Size = new System.Drawing.Size(78, 25);
            this.userButton1.TabIndex = 8;
            this.userButton1.UIText = "删除";
            this.userButton1.Click += new System.EventHandler(this.userButton1_Click);
            // 
            // userButton2
            // 
            this.userButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.userButton2.BackColor = System.Drawing.Color.Transparent;
            this.userButton2.CustomerInformation = "";
            this.userButton2.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton2.Location = new System.Drawing.Point(419, 417);
            this.userButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton2.Name = "userButton2";
            this.userButton2.Size = new System.Drawing.Size(78, 25);
            this.userButton2.TabIndex = 9;
            this.userButton2.UIText = "保存";
            this.userButton2.Click += new System.EventHandler(this.userButton2_Click);
            // 
            // ArrayConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.userButton2);
            this.Controls.Add(this.userButton1);
            this.Controls.Add(this.userButton_login);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ArrayConfiguration";
            this.Size = new System.Drawing.Size(500, 446);
            this.Load += new System.EventHandler(this.FactoryConfiguration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Factory;
        private HslCommunication.Controls.UserButton userButton_login;
        private HslCommunication.Controls.UserButton userButton1;
        private HslCommunication.Controls.UserButton userButton2;
    }
}
