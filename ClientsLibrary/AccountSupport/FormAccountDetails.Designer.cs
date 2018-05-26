namespace ClientsLibrary
{
    partial class FormAccountDetails
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("我的文件（下载中）");
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox_UserPortrait = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_LoginFailedCount = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_LastLoginWay = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_LastLoginIpAddress = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_LastLoginTime = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_LoginFrequency = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_LoginEnable = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_RegisterTime = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_GradeDescription = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Factory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_NameAlias = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_UserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_file_uploadTime = new System.Windows.Forms.TextBox();
            this.label_uploadTime = new System.Windows.Forms.Label();
            this.textBox_file_size = new System.Windows.Forms.TextBox();
            this.label_fileSize = new System.Windows.Forms.Label();
            this.textBox_file_name = new System.Windows.Forms.TextBox();
            this.label_fileName = new System.Windows.Forms.Label();
            this.userButton_delete = new HslCommunication.Controls.UserButton();
            this.userButton_download = new HslCommunication.Controls.UserButton();
            this.userButton_upload = new HslCommunication.Controls.UserButton();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_UserPortrait)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.pictureBox_UserPortrait);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBox_LoginFailedCount);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.textBox_LastLoginWay);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.textBox_LastLoginIpAddress);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBox_LastLoginTime);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBox_LoginFrequency);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBox_LoginEnable);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox_RegisterTime);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox_GradeDescription);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_Factory);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_NameAlias);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_UserName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 492);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "账户信息";
            // 
            // pictureBox_UserPortrait
            // 
            this.pictureBox_UserPortrait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_UserPortrait.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_UserPortrait.Location = new System.Drawing.Point(153, 22);
            this.pictureBox_UserPortrait.Name = "pictureBox_UserPortrait";
            this.pictureBox_UserPortrait.Size = new System.Drawing.Size(136, 136);
            this.pictureBox_UserPortrait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_UserPortrait.TabIndex = 23;
            this.pictureBox_UserPortrait.TabStop = false;
            this.pictureBox_UserPortrait.Click += new System.EventHandler(this.pictureBox_UserPortrait_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(20, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 17);
            this.label12.TabIndex = 22;
            this.label12.Text = "头像：";
            // 
            // textBox_LoginFailedCount
            // 
            this.textBox_LoginFailedCount.BackColor = System.Drawing.Color.White;
            this.textBox_LoginFailedCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_LoginFailedCount.Location = new System.Drawing.Point(153, 459);
            this.textBox_LoginFailedCount.Name = "textBox_LoginFailedCount";
            this.textBox_LoginFailedCount.ReadOnly = true;
            this.textBox_LoginFailedCount.Size = new System.Drawing.Size(136, 23);
            this.textBox_LoginFailedCount.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 462);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 17);
            this.label11.TabIndex = 20;
            this.label11.Text = "登录失败次数：";
            // 
            // textBox_LastLoginWay
            // 
            this.textBox_LastLoginWay.BackColor = System.Drawing.Color.White;
            this.textBox_LastLoginWay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_LastLoginWay.Location = new System.Drawing.Point(153, 428);
            this.textBox_LastLoginWay.Name = "textBox_LastLoginWay";
            this.textBox_LastLoginWay.ReadOnly = true;
            this.textBox_LastLoginWay.Size = new System.Drawing.Size(136, 23);
            this.textBox_LastLoginWay.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 431);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 17);
            this.label10.TabIndex = 18;
            this.label10.Text = "上次登录方式：";
            // 
            // textBox_LastLoginIpAddress
            // 
            this.textBox_LastLoginIpAddress.BackColor = System.Drawing.Color.White;
            this.textBox_LastLoginIpAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_LastLoginIpAddress.Location = new System.Drawing.Point(153, 398);
            this.textBox_LastLoginIpAddress.Name = "textBox_LastLoginIpAddress";
            this.textBox_LastLoginIpAddress.ReadOnly = true;
            this.textBox_LastLoginIpAddress.Size = new System.Drawing.Size(136, 23);
            this.textBox_LastLoginIpAddress.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 401);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 17);
            this.label9.TabIndex = 16;
            this.label9.Text = "上次登录地址：";
            // 
            // textBox_LastLoginTime
            // 
            this.textBox_LastLoginTime.BackColor = System.Drawing.Color.White;
            this.textBox_LastLoginTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_LastLoginTime.Location = new System.Drawing.Point(153, 368);
            this.textBox_LastLoginTime.Name = "textBox_LastLoginTime";
            this.textBox_LastLoginTime.ReadOnly = true;
            this.textBox_LastLoginTime.Size = new System.Drawing.Size(136, 23);
            this.textBox_LastLoginTime.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 371);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "上次登录时间：";
            // 
            // textBox_LoginFrequency
            // 
            this.textBox_LoginFrequency.BackColor = System.Drawing.Color.White;
            this.textBox_LoginFrequency.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_LoginFrequency.Location = new System.Drawing.Point(153, 338);
            this.textBox_LoginFrequency.Name = "textBox_LoginFrequency";
            this.textBox_LoginFrequency.ReadOnly = true;
            this.textBox_LoginFrequency.Size = new System.Drawing.Size(136, 23);
            this.textBox_LoginFrequency.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 341);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 17);
            this.label7.TabIndex = 12;
            this.label7.Text = "总登录次数：";
            // 
            // textBox_LoginEnable
            // 
            this.textBox_LoginEnable.BackColor = System.Drawing.Color.White;
            this.textBox_LoginEnable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_LoginEnable.Location = new System.Drawing.Point(153, 308);
            this.textBox_LoginEnable.Name = "textBox_LoginEnable";
            this.textBox_LoginEnable.ReadOnly = true;
            this.textBox_LoginEnable.Size = new System.Drawing.Size(136, 23);
            this.textBox_LoginEnable.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 311);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "是否允许登录：";
            // 
            // textBox_RegisterTime
            // 
            this.textBox_RegisterTime.BackColor = System.Drawing.Color.White;
            this.textBox_RegisterTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_RegisterTime.Location = new System.Drawing.Point(153, 279);
            this.textBox_RegisterTime.Name = "textBox_RegisterTime";
            this.textBox_RegisterTime.ReadOnly = true;
            this.textBox_RegisterTime.Size = new System.Drawing.Size(136, 23);
            this.textBox_RegisterTime.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 282);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "注册时间：";
            // 
            // textBox_GradeDescription
            // 
            this.textBox_GradeDescription.BackColor = System.Drawing.Color.White;
            this.textBox_GradeDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_GradeDescription.Location = new System.Drawing.Point(153, 251);
            this.textBox_GradeDescription.Name = "textBox_GradeDescription";
            this.textBox_GradeDescription.ReadOnly = true;
            this.textBox_GradeDescription.Size = new System.Drawing.Size(136, 23);
            this.textBox_GradeDescription.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 254);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "权限：";
            // 
            // textBox_Factory
            // 
            this.textBox_Factory.BackColor = System.Drawing.Color.White;
            this.textBox_Factory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Factory.Location = new System.Drawing.Point(153, 223);
            this.textBox_Factory.Name = "textBox_Factory";
            this.textBox_Factory.ReadOnly = true;
            this.textBox_Factory.Size = new System.Drawing.Size(136, 23);
            this.textBox_Factory.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "工厂：";
            // 
            // textBox_NameAlias
            // 
            this.textBox_NameAlias.BackColor = System.Drawing.Color.White;
            this.textBox_NameAlias.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_NameAlias.Location = new System.Drawing.Point(153, 194);
            this.textBox_NameAlias.Name = "textBox_NameAlias";
            this.textBox_NameAlias.ReadOnly = true;
            this.textBox_NameAlias.Size = new System.Drawing.Size(136, 23);
            this.textBox_NameAlias.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "别名：";
            // 
            // textBox_UserName
            // 
            this.textBox_UserName.BackColor = System.Drawing.Color.White;
            this.textBox_UserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_UserName.Location = new System.Drawing.Point(153, 164);
            this.textBox_UserName.Name = "textBox_UserName";
            this.textBox_UserName.ReadOnly = true;
            this.textBox_UserName.Size = new System.Drawing.Size(136, 23);
            this.textBox_UserName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.textBox_file_uploadTime);
            this.groupBox2.Controls.Add(this.label_uploadTime);
            this.groupBox2.Controls.Add(this.textBox_file_size);
            this.groupBox2.Controls.Add(this.label_fileSize);
            this.groupBox2.Controls.Add(this.textBox_file_name);
            this.groupBox2.Controls.Add(this.label_fileName);
            this.groupBox2.Controls.Add(this.userButton_delete);
            this.groupBox2.Controls.Add(this.userButton_download);
            this.groupBox2.Controls.Add(this.userButton_upload);
            this.groupBox2.Controls.Add(this.treeView1);
            this.groupBox2.Location = new System.Drawing.Point(322, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(373, 492);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "个人文件，支持拖拽上传，双击下载";
            // 
            // textBox_file_uploadTime
            // 
            this.textBox_file_uploadTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_file_uploadTime.BackColor = System.Drawing.Color.White;
            this.textBox_file_uploadTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_file_uploadTime.Location = new System.Drawing.Point(252, 432);
            this.textBox_file_uploadTime.Name = "textBox_file_uploadTime";
            this.textBox_file_uploadTime.ReadOnly = true;
            this.textBox_file_uploadTime.Size = new System.Drawing.Size(111, 23);
            this.textBox_file_uploadTime.TabIndex = 16;
            // 
            // label_uploadTime
            // 
            this.label_uploadTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_uploadTime.AutoSize = true;
            this.label_uploadTime.Location = new System.Drawing.Point(182, 434);
            this.label_uploadTime.Name = "label_uploadTime";
            this.label_uploadTime.Size = new System.Drawing.Size(68, 17);
            this.label_uploadTime.TabIndex = 15;
            this.label_uploadTime.Text = "上传日期：";
            // 
            // textBox_file_size
            // 
            this.textBox_file_size.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_file_size.BackColor = System.Drawing.Color.White;
            this.textBox_file_size.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_file_size.Location = new System.Drawing.Point(66, 432);
            this.textBox_file_size.Name = "textBox_file_size";
            this.textBox_file_size.ReadOnly = true;
            this.textBox_file_size.Size = new System.Drawing.Size(110, 23);
            this.textBox_file_size.TabIndex = 14;
            // 
            // label_fileSize
            // 
            this.label_fileSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_fileSize.AutoSize = true;
            this.label_fileSize.Location = new System.Drawing.Point(7, 434);
            this.label_fileSize.Name = "label_fileSize";
            this.label_fileSize.Size = new System.Drawing.Size(44, 17);
            this.label_fileSize.TabIndex = 13;
            this.label_fileSize.Text = "大小：";
            // 
            // textBox_file_name
            // 
            this.textBox_file_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_file_name.BackColor = System.Drawing.Color.White;
            this.textBox_file_name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_file_name.Location = new System.Drawing.Point(66, 405);
            this.textBox_file_name.Name = "textBox_file_name";
            this.textBox_file_name.ReadOnly = true;
            this.textBox_file_name.Size = new System.Drawing.Size(297, 23);
            this.textBox_file_name.TabIndex = 12;
            // 
            // label_fileName
            // 
            this.label_fileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_fileName.AutoSize = true;
            this.label_fileName.Location = new System.Drawing.Point(7, 407);
            this.label_fileName.Name = "label_fileName";
            this.label_fileName.Size = new System.Drawing.Size(56, 17);
            this.label_fileName.TabIndex = 11;
            this.label_fileName.Text = "文件名：";
            // 
            // userButton_delete
            // 
            this.userButton_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.userButton_delete.BackColor = System.Drawing.Color.Transparent;
            this.userButton_delete.CustomerInformation = "";
            this.userButton_delete.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_delete.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_delete.Location = new System.Drawing.Point(297, 460);
            this.userButton_delete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton_delete.Name = "userButton_delete";
            this.userButton_delete.Size = new System.Drawing.Size(65, 23);
            this.userButton_delete.TabIndex = 10;
            this.userButton_delete.UIText = "删除";
            this.userButton_delete.Click += new System.EventHandler(this.userButton2_Click);
            // 
            // userButton_download
            // 
            this.userButton_download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.userButton_download.BackColor = System.Drawing.Color.Transparent;
            this.userButton_download.CustomerInformation = "";
            this.userButton_download.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_download.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_download.Location = new System.Drawing.Point(81, 460);
            this.userButton_download.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton_download.Name = "userButton_download";
            this.userButton_download.Size = new System.Drawing.Size(65, 23);
            this.userButton_download.TabIndex = 9;
            this.userButton_download.UIText = "下载";
            this.userButton_download.Click += new System.EventHandler(this.userButton1_Click);
            // 
            // userButton_upload
            // 
            this.userButton_upload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.userButton_upload.BackColor = System.Drawing.Color.Transparent;
            this.userButton_upload.CustomerInformation = "";
            this.userButton_upload.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton_upload.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton_upload.Location = new System.Drawing.Point(10, 460);
            this.userButton_upload.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton_upload.Name = "userButton_upload";
            this.userButton_upload.Size = new System.Drawing.Size(65, 23);
            this.userButton_upload.TabIndex = 8;
            this.userButton_upload.UIText = "上传";
            this.userButton_upload.Click += new System.EventHandler(this.userButton_upload_Click);
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.Location = new System.Drawing.Point(10, 24);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "files_root";
            treeNode1.Text = "我的文件（下载中）";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView1.ShowLines = false;
            this.treeView1.Size = new System.Drawing.Size(353, 377);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDoubleClick);
            // 
            // FormAccountDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(706, 516);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(722, 555);
            this.MinimumSize = new System.Drawing.Size(722, 555);
            this.Name = "FormAccountDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AccountDetails";
            this.Load += new System.EventHandler(this.AccountDetails_Load);
            this.Shown += new System.EventHandler(this.AccountDetails_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_UserPortrait)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_LoginFailedCount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_LastLoginWay;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_LastLoginIpAddress;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_LastLoginTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_LoginFrequency;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_LoginEnable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_RegisterTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_GradeDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_Factory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_NameAlias;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_UserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TreeView treeView1;
        private HslCommunication.Controls.UserButton userButton_delete;
        private HslCommunication.Controls.UserButton userButton_download;
        private HslCommunication.Controls.UserButton userButton_upload;
        private System.Windows.Forms.PictureBox pictureBox_UserPortrait;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_file_uploadTime;
        private System.Windows.Forms.Label label_uploadTime;
        private System.Windows.Forms.TextBox textBox_file_size;
        private System.Windows.Forms.Label label_fileSize;
        private System.Windows.Forms.TextBox textBox_file_name;
        private System.Windows.Forms.Label label_fileName;
    }
}