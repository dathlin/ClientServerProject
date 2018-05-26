using HslCommunication.Controls;

namespace 软件系统客户端模版.UIControls
{
    partial class GroupFilesRender
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.userButton_refresh = new HslCommunication.Controls.UserButton();
            this.userButton_upload = new HslCommunication.Controls.UserButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.userButton_refresh);
            this.panel1.Controls.Add(this.userButton_upload);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(706, 35);
            this.panel1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(513, 5);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(187, 23);
            this.textBox1.TabIndex = 9;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(462, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "搜索：";
            // 
            // userButton_refresh
            // 
            this.userButton_refresh.BackColor = System.Drawing.Color.Transparent;
            this.userButton_refresh.CustomerInformation = "";
            this.userButton_refresh.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_refresh.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_refresh.Location = new System.Drawing.Point(80, 4);
            this.userButton_refresh.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.userButton_refresh.Name = "userButton_refresh";
            this.userButton_refresh.Size = new System.Drawing.Size(71, 25);
            this.userButton_refresh.TabIndex = 7;
            this.userButton_refresh.UIText = "刷新";
            this.userButton_refresh.Click += new System.EventHandler(this.userButton_refresh_Click);
            // 
            // userButton_upload
            // 
            this.userButton_upload.BackColor = System.Drawing.Color.Transparent;
            this.userButton_upload.CustomerInformation = "";
            this.userButton_upload.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_upload.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_upload.Location = new System.Drawing.Point(3, 4);
            this.userButton_upload.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.userButton_upload.Name = "userButton_upload";
            this.userButton_upload.Size = new System.Drawing.Size(71, 25);
            this.userButton_upload.TabIndex = 6;
            this.userButton_upload.UIText = "上传";
            this.userButton_upload.Click += new System.EventHandler(this.userButton_upload_Click);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel2.Location = new System.Drawing.Point(0, 35);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(706, 427);
            this.panel2.TabIndex = 1;
            // 
            // GroupFilesRender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "GroupFilesRender";
            this.Size = new System.Drawing.Size(706, 462);
            this.Load += new System.EventHandler(this.ShareFilesRender_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private UserButton userButton_refresh;
        private UserButton userButton_upload;
    }
}
