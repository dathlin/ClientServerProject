# 共享文件支持 Support By HslCommunication.dll

## Summary
该功能来源的需求在于软件主要使用人员希望将自己整理的一些报表资料，共享给其他人下载查看，于是就设计了这个功能，因为要存储文件的诸多信息，比如文件名，文件的额外描述，上传人的命名，下载次数等等信息，其他人可以根据需要进行自己下载。

于是就有了这个功能，方便的上传下载文件，还可以自己使用类库在手动实现自己需要的逻辑代码。

## Reference
日志组件所有的功能类都在 **HslCommunication** 和 **HslCommunication.Enthernet** 命名空间，所以再使用之前先添加
<pre>
<code>
using HslCommunication;
using HslCommunication.Enthernet;
</code>
</pre>

## Server
服务器端的文件都存放在一个临时的列表里，存储在服务器exe的目录下面，在服务器端我们要先实现实例化操作，配置相应的信息，以保证客户端可以正常的访问，代码如下，放到服务器的form下面即可：
<pre>
<code>
        // 共享文件服务器引擎
        private SimpleFileServer net_simple_file_server { get; set; } = null;
        // 共享文件服务引擎初始化，放到窗口的启动界面或使用按钮点击界面，必须执行才真正的启动文件管理
        private void Simple_File_Initiaization()
        {
            try
            {
                net_simple_file_server = new SimpleFileServer()
                {
                    //文件信息存储路径
                    FileListName = Application.StartupPath + @"\files.txt"
                };
                net_simple_file_server.KeyToken = Guid.Empty;//设置安全令牌
                net_simple_file_server.ReadFromFile();//读取上次存储的文件列表
                net_simple_file_server.LogNet =new LogNetSingle(LogSavePath + @"\share_file_log.txt");//设置日志存储对象
                net_simple_file_server.LogNet.SetMessageDegree(HslMessageDegree.DEBUG);//默认debug及以上级别日志均进行存储，根据需要自行选择
                //文件存储路径
                net_simple_file_server.FilesDirectoryPath = Application.StartupPath + @"\Files";
                net_simple_file_server.FilesDirectoryPathTemp = Application.StartupPath + @"\Temp";
                net_simple_file_server.FileChange += Net_simple_file_server_FileChange;//文件数量发生变化的时候触发事件
                net_simple_file_server.ServerStart(12458);//此处随便定义一个端口
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动失败，原因：" + ex.Message);
            }
        }

        private void Net_simple_file_server_FileChange()
        {
            // 获取当前文件的数量，可以选择发送给客户端实时显示。也可以不操作
            int fileCount = net_simple_file_server.File_Count();
        }
</code>
</pre>

只要服务器的初始化方法执行，那么服务器的文件引擎也就启动了，关键的操作都是客户端了。

## 客户端

我们最终需要在客户端进行上传文件，下载文件，删除文件，所有的操作，在组件里提供了标准的实现控件可以方便的实现操作，当然，如果你想自己实现自己的操作，组件里也公开了底层的类，方便实现，包括上传进度，下载进度等等。

#### 组件自带实现
文件上传

----------
文件上传使用组件实现起来非常方便，实例化一个窗口即可，然后传入相关的参数显示就OK了。
<pre>
<code>
            // 上传文件
            using (FormSimplyFileUpload upload = new FormSimplyFileUpload(
                Guid.Empty, // 安全令牌必须和服务器一致
                null,// 如果需要日志记录，可以传入ILogNet接口的对象
                "127.0.0.1",// 服务器本机的地址
                12458, // 服务器的端口
                UserClient.UserAccount.UserName))
            {
                upload.ShowDialog();
            }
</code>
</pre>
窗口的样子如下：
![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client12.png) 

文件显示使用了组件的文件控件实现的，需要传入相关的信息才可以，这部分和代码结合的比较紧密，以下展示了从服务器请求了一个文件列表后需要进行显示，在panel2中进行动态控件的排版代码：
<pre>
<code>
        // 该参数来源于服务器，需要先手动向服务器请求，具体请参照CS项目模版：ClientServerProject 
        private void SetFilesShow(List&lt;HslSoftFile> files)
        {
            panel2.SuspendLayout();
            //清楚panel2中原来的文件控件
            ClearControls();

            if (files?.Count > 0 && panel2.Width > 20)
            {
                int location_y = 2 - panel2.VerticalScroll.Value;
                //添加子控件
                foreach(var m in files)
                {
                    FileItemShow item = new FileItemShow(
                        Guid.Empty,  // 和服务器一致的安全令牌
                        null, // 如果需要日志记录，可以传入ILogNet接口的对象
                        "127.0.0.1", // 服务器本机的地址
                        12458, // 服务器的端口，和之前的配置一致
                        () =>
                        {
                            if (m.UploadName != UserClient.UserAccount.UserName)
                            {
                                MessageBox.Show("无法删除不是自己上传的文件。");
                                return false;
                            }
                            else
                            {
                                return MessageBox.Show("请确认是否真的删除？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                            }
                        });
                    panel2.Controls.Add(item);                                            // 添加显示
                    item.BackColor = Color.White;                                         // 白色的背景
                    item.BorderStyle = BorderStyle.FixedSingle;                                 // 边框样式
                    item.SetFile(m, () => m.UploadName == UserClient.UserAccount.UserName);    // 设置文件显示并提供一个删除使能的权限,此处设置是登录名和上传人不一致时,删除键不能点击
                    item.Location = new Point(2, location_y);                                 // 控件的位置
                    int width = panel2.VerticalScroll.Visible ? panel2.Width - 4 - SystemInformation.VerticalScrollBarWidth : panel2.Width - 4; // 控件的宽度
                    item.Size = new Size(width, item.Size.Height);  // 控件的大小
                    item.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;// 控件随窗口变化的样式

                    location_y += item.Height + 4; // 位置偏移
                    FilesControls.Push(item);// 控件压入堆栈
                }
            }

            panel2.ResumeLayout();
        }
</code>
</pre>

而文件的删除和下载已经集成到了控件中，已经不需要进行额外的代码编写了。


#### 手动编码实现

----------
也就是说，如果你想通过自己的代码实现文件的下载和上传，或是删除，或者将这些功能集成到自己的窗口中实现时，可以按照如下的代码运行，无论是上传，下载和删除都要使用一个类的对象，以下是实例化操作：
<pre>
<code>
            SimpleFileClient client = new SimpleFileClient()
            {
                KeyToken = Guid.Empty,//设置安全令牌
                null, // 如果需要日志记录，可以传入ILogNet接口的对象
                ServerIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12458),//服务器的ip地址和端口号
            };
</code>
</pre>
配置和上述服务器的配置是一致的，但是在实际的开发中，最后设置一个安全的令牌，提升系统的安全性，接下来就是用上面实例化的对象来上传文件操作，
<pre>
<code>
        private void UploadFileToServer(string filePathName)
        {
            FileInfo fInfo = new FileInfo(filePathName);
            OperateResult result = client.UploadFile(
                filePathName,                        // 文件的本地完整路径，带路径名和文件名
                fInfo.Name,                          // 文件的名称，带后缀，主要用户服务器存储和显示给其他用户看的，和下载要用
                "这个文件是这个月所有人的奖金明细",      // 文件的额外描述
                "张三",                              // 文件的上传人
                ReportProgress                       // 用于报告进度的委托，如果不需要显示，直接赋值为空
                );

            if (result.IsSuccess)
            {
                MessageBox.Show("文件上传成功！");
            }
            else
            {
                MessageBox.Show("文件上传失败，原因：" + result.Message);
            }
        }

        /// &lt;summary&gt;
        /// 用于报告进度的方法，该方法是线程安全的
        /// &lt;/summary&gt;
        /// &lt;param name="current"&gt;&lt;/param&gt;
        /// &lt;param name="count"&gt;&lt;/param&gt;
        private void ReportProgress(long current, long count)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    ReportProgress(current, count);
                }));
                return;
            }

            long percent = 0;
            if (count > 0)
            {
                percent = current * 100 / count;
            }
            
            label1.Text = "已上传文件" + percent + "%";  // 用于显示上传进度
        }
</code>
</pre>

注意，上述的方法 **UploadFileToServer** 应该放到后台线程来执行，不然会卡死界面，而且一般文件都需要上传好一会，这是个很不好的体验，至于报告进度的方法已经考虑了线程安全，所以不需要在更改了，label1只是个用来显示上传进度的显示罢了，你也可以用进度条控件。

**下载实现**
<pre>
<code>
        // 此处的fileName是指服务器保存的文件名，也即是上面上传时的fInfo.Name参数
        private void DownloadFileFromServer(string fileName)
        {
            string save_file_name = Application.StartupPath + "\\download\\files";// 定义保存路径
            if (!Directory.Exists(save_file_name))
            {
                Directory.CreateDirectory(save_file_name);
            }
            save_file_name += "\\" + fileName;


            OperateResult result = client.DownloadFile(
                fileName,           // 服务器文件的名称
                ReportProgress,     // 报告下载进度的委托，如果不需要，可以为空
                save_file_name      // 本地保存的文件路径，带文件名，如果本地文件已经存在，会覆盖，如果本地文件存在且使用中，会异常
                );

            
             if(result.IsSuccess)
             {
                if (MessageBox.Show("下载完成，路径为：" + save_file_name + Environment.NewLine +
                    "是否打开文件路径？", "打开确认", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", @"/select," + save_file_name);
                }
            }
            else
            {
                MessageBox.Show("下载失败，错误原因：" + result.Message);
            }
        }
        /// &lt;summary&gt;
        /// 用于报告进度的方法，该方法是线程安全的
        /// &lt;/summary&gt;
        /// &lt;param name="current"&gt;&lt;/param&gt;
        /// &lt;param name="count"&gt;&lt;/param&gt;
        private void ReportProgress(long current, long count)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    ReportProgress(current, count);
                }));
                return;
            }

            long percent = 0;
            if (count > 0)
            {
                percent = current * 100 / count;
            }
            
            label1.Text = "已下载文件" + percent + "%";  // 用于显示下传进度
        }
</code>
</pre>
下载的方法也应该放到后台线程比较稳妥，界面也更加流畅。
下面是删除操作，删除操作不需要放到后台线程中，前台即可，这样可以根据返回值进行判断
<pre>
<code>
        // 此处的fileName是指服务器保存的文件名，也即是上面上传时的fInfo.Name参数
        private void DeleteFile(string fileName)
        {
            OperateResult result = client.DeleteFile(fileName);
            
            if(result.IsSuccess)
            {
                MessageBox.Show("删除成功！");
            }
            else
            {
                MessageBox.Show("删除失败！错误：" + result.Message);
            }
        }
</code>
</pre>

通过上述所有的示例，已经充分展示了共享文件块的上传下载的便利，以及支持在代码中自由组合实现更高级的操作，当然。也可以选择组件自带的实现，更加的快速。