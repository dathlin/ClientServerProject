namespace ClientsLibrary.FileSupport
{ 
    partial class FileItemShow
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
            this.label_download_times = new System.Windows.Forms.Label();
            this.label_upload_name = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.linkLabel_download = new System.Windows.Forms.LinkLabel();
            this.linkLabel_delete = new System.Windows.Forms.LinkLabel();
            this.label_file_date = new System.Windows.Forms.Label();
            this.label_file_size = new System.Windows.Forms.Label();
            this.label_file_mark = new System.Windows.Forms.Label();
            this.label_file_name = new System.Windows.Forms.Label();
            this.pictureBox_file = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_file)).BeginInit();
            this.SuspendLayout();
            // 
            // label_download_times
            // 
            this.label_download_times.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_download_times.AutoSize = true;
            this.label_download_times.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_download_times.Location = new System.Drawing.Point(390, 20);
            this.label_download_times.Name = "label_download_times";
            this.label_download_times.Size = new System.Drawing.Size(56, 17);
            this.label_download_times.TabIndex = 19;
            this.label_download_times.Text = "下载数：";
            // 
            // label_upload_name
            // 
            this.label_upload_name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_upload_name.AutoSize = true;
            this.label_upload_name.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_upload_name.Location = new System.Drawing.Point(276, 20);
            this.label_upload_name.Name = "label_upload_name";
            this.label_upload_name.Size = new System.Drawing.Size(56, 17);
            this.label_upload_name.TabIndex = 18;
            this.label_upload_name.Text = "上传人：";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(4, 37);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(492, 4);
            this.progressBar1.TabIndex = 17;
            // 
            // linkLabel_download
            // 
            this.linkLabel_download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel_download.AutoSize = true;
            this.linkLabel_download.Enabled = false;
            this.linkLabel_download.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel_download.Location = new System.Drawing.Point(508, 20);
            this.linkLabel_download.Name = "linkLabel_download";
            this.linkLabel_download.Size = new System.Drawing.Size(32, 17);
            this.linkLabel_download.TabIndex = 16;
            this.linkLabel_download.TabStop = true;
            this.linkLabel_download.Text = "下载";
            this.linkLabel_download.Click += new System.EventHandler(this.linkLabel_download_Click);
            // 
            // linkLabel_delete
            // 
            this.linkLabel_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel_delete.AutoSize = true;
            this.linkLabel_delete.Enabled = false;
            this.linkLabel_delete.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel_delete.Location = new System.Drawing.Point(508, 4);
            this.linkLabel_delete.Name = "linkLabel_delete";
            this.linkLabel_delete.Size = new System.Drawing.Size(32, 17);
            this.linkLabel_delete.TabIndex = 15;
            this.linkLabel_delete.TabStop = true;
            this.linkLabel_delete.Text = "删除";
            this.linkLabel_delete.Click += new System.EventHandler(this.linkLabel_delete_Click);
            // 
            // label_file_date
            // 
            this.label_file_date.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_file_date.AutoSize = true;
            this.label_file_date.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_file_date.Location = new System.Drawing.Point(390, 4);
            this.label_file_date.Name = "label_file_date";
            this.label_file_date.Size = new System.Drawing.Size(110, 17);
            this.label_file_date.TabIndex = 14;
            this.label_file_date.Text = "日期：2016-09-01";
            // 
            // label_file_size
            // 
            this.label_file_size.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_file_size.AutoSize = true;
            this.label_file_size.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_file_size.Location = new System.Drawing.Point(277, 4);
            this.label_file_size.Name = "label_file_size";
            this.label_file_size.Size = new System.Drawing.Size(44, 17);
            this.label_file_size.TabIndex = 13;
            this.label_file_size.Text = "大小：";
            // 
            // label_file_mark
            // 
            this.label_file_mark.AutoSize = true;
            this.label_file_mark.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_file_mark.Location = new System.Drawing.Point(27, 20);
            this.label_file_mark.Name = "label_file_mark";
            this.label_file_mark.Size = new System.Drawing.Size(68, 17);
            this.label_file_mark.TabIndex = 12;
            this.label_file_mark.Text = "文件备注：";
            // 
            // label_file_name
            // 
            this.label_file_name.AutoSize = true;
            this.label_file_name.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_file_name.Location = new System.Drawing.Point(27, 4);
            this.label_file_name.Name = "label_file_name";
            this.label_file_name.Size = new System.Drawing.Size(68, 17);
            this.label_file_name.TabIndex = 11;
            this.label_file_name.Text = "文件名称：";
            // 
            // pictureBox_file
            // 
            this.pictureBox_file.Image = global::ClientsLibrary.Properties.Resources.file;
            this.pictureBox_file.Location = new System.Drawing.Point(4, 6);
            this.pictureBox_file.Name = "pictureBox_file";
            this.pictureBox_file.Size = new System.Drawing.Size(18, 18);
            this.pictureBox_file.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_file.TabIndex = 10;
            this.pictureBox_file.TabStop = false;
            // 
            // FileItemShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.label_download_times);
            this.Controls.Add(this.label_upload_name);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.linkLabel_download);
            this.Controls.Add(this.linkLabel_delete);
            this.Controls.Add(this.label_file_date);
            this.Controls.Add(this.label_file_size);
            this.Controls.Add(this.label_file_mark);
            this.Controls.Add(this.label_file_name);
            this.Controls.Add(this.pictureBox_file);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FileItemShow";
            this.Size = new System.Drawing.Size(543, 45);
            this.Load += new System.EventHandler(this.FileItemShow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_file)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_download_times;
        private System.Windows.Forms.Label label_upload_name;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.LinkLabel linkLabel_download;
        private System.Windows.Forms.LinkLabel linkLabel_delete;
        private System.Windows.Forms.Label label_file_date;
        private System.Windows.Forms.Label label_file_size;
        private System.Windows.Forms.Label label_file_mark;
        private System.Windows.Forms.Label label_file_name;
        private System.Windows.Forms.PictureBox pictureBox_file;
    }
}
