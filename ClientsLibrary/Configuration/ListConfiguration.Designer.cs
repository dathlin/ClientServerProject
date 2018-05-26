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
            this.userButton_add = new HslCommunication.Controls.UserButton();
            this.userButton_delete = new HslCommunication.Controls.UserButton();
            this.userButton_save = new HslCommunication.Controls.UserButton();
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
            // userButton_add
            // 
            this.userButton_add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.userButton_add.BackColor = System.Drawing.Color.Transparent;
            this.userButton_add.CustomerInformation = "";
            this.userButton_add.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_add.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_add.Location = new System.Drawing.Point(3, 417);
            this.userButton_add.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton_add.Name = "userButton_add";
            this.userButton_add.Size = new System.Drawing.Size(78, 25);
            this.userButton_add.TabIndex = 7;
            this.userButton_add.UIText = "新增";
            this.userButton_add.Click += new System.EventHandler(this.userButton_Add_Click);
            // 
            // userButton_delete
            // 
            this.userButton_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.userButton_delete.BackColor = System.Drawing.Color.Transparent;
            this.userButton_delete.CustomerInformation = "";
            this.userButton_delete.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_delete.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_delete.Location = new System.Drawing.Point(87, 417);
            this.userButton_delete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton_delete.Name = "userButton_delete";
            this.userButton_delete.Size = new System.Drawing.Size(78, 25);
            this.userButton_delete.TabIndex = 8;
            this.userButton_delete.UIText = "删除";
            this.userButton_delete.Click += new System.EventHandler(this.userButton1_Click);
            // 
            // userButton_save
            // 
            this.userButton_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.userButton_save.BackColor = System.Drawing.Color.Transparent;
            this.userButton_save.CustomerInformation = "";
            this.userButton_save.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_save.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_save.Location = new System.Drawing.Point(419, 417);
            this.userButton_save.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton_save.Name = "userButton_save";
            this.userButton_save.Size = new System.Drawing.Size(78, 25);
            this.userButton_save.TabIndex = 9;
            this.userButton_save.UIText = "保存";
            this.userButton_save.Click += new System.EventHandler(this.userButton2_Click);
            // 
            // ArrayConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.userButton_save);
            this.Controls.Add(this.userButton_delete);
            this.Controls.Add(this.userButton_add);
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
        private HslCommunication.Controls.UserButton userButton_add;
        private HslCommunication.Controls.UserButton userButton_delete;
        private HslCommunication.Controls.UserButton userButton_save;
    }
}
