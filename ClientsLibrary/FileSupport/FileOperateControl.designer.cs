namespace ClientsLibrary.FileSupport
{
    partial class FileOperateControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label_filename = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_now_info = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label_filesize = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(5, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件名称：";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(62, 42);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(433, 12);
            this.progressBar1.TabIndex = 1;
            // 
            // label_filename
            // 
            this.label_filename.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_filename.Location = new System.Drawing.Point(69, 3);
            this.label_filename.Name = "label_filename";
            this.label_filename.Size = new System.Drawing.Size(427, 38);
            this.label_filename.TabIndex = 2;
            this.label_filename.Text = "文件名称文件名称文件名称文件名称文件名称文件名称文件名称文件名称文件名称文件名称文件名称";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(5, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "进度：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(5, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "当前操作：";
            // 
            // label_now_info
            // 
            this.label_now_info.AutoSize = true;
            this.label_now_info.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_now_info.Location = new System.Drawing.Point(69, 60);
            this.label_now_info.Name = "label_now_info";
            this.label_now_info.Size = new System.Drawing.Size(56, 17);
            this.label_now_info.TabIndex = 5;
            this.label_now_info.Text = "当前操作";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(389, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "重新上传";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_filesize
            // 
            this.label_filesize.AutoSize = true;
            this.label_filesize.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_filesize.Location = new System.Drawing.Point(248, 60);
            this.label_filesize.Name = "label_filesize";
            this.label_filesize.Size = new System.Drawing.Size(27, 17);
            this.label_filesize.TabIndex = 10;
            this.label_filesize.Text = "0 B";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(184, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "文件大小：";
            // 
            // FileOperateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.label_filesize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label_now_info);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_filename);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Name = "FileOperateControl";
            this.Size = new System.Drawing.Size(505, 82);
            this.Load += new System.EventHandler(this.FileOperateControl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FileOperateControl_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label_filename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_now_info;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label_filesize;
        private System.Windows.Forms.Label label5;
    }
}
