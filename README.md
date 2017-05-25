# C-S架构的服务器客户端模版

#### 关于HslCommunication.dll
<p>该组件功能提供了一些基础类和整个C-S项目的核心网络的支持，除此之外，该组件提供了访问三菱PLC和西门子PLC的数据功能。</p>

<br />
<br />

#### 关于本项目模版
<p style="text-indent:2em">本模版基于.Net Framework3.5+C#7.0开发完成，所以必须使用Visual studio 2017进行开发，低版本的IDE打开项目将出现语法识别错误。有必要说明下为什么使用.Net Framework3.5，这个版本是xp系统比较方便安装的，在企业部署中会相对容易很多，所以如果你的系统也是应用于企业的，那么强烈建议使用3.5版本，该模版由三部分的程序组成：</p>
<ul>
<li>公共组件</li>
<li>服务器端</li>
<li>客户端</li>
</ul>

<p style="text-indent:2em">组成部分主要是一个服务端运行的程序，一个客户端运行的程序，还有一个公共的组件，以及一个json组件和一个网络组件，实现了基础的账户管理功能，版本控制，软件升级，公告管理，消息群发功能。具体的操作方法见演示就行。下面主要介绍下服务端的程序界面和客户端的程序界面。
</p>


#### 整个系统的架构设计如下
![](https://github.com/dathlin/C-S-/raw/master/软件系统服务端模版/screenshots/design.png)  
<br />

#### 服务器端程序界面如下：
![](https://github.com/dathlin/C-S-/raw/master/软件系统服务端模版/screenshots/server.png)  
功能菜单都在设置中，可以尝试其他操作。
<br />

#### 客户端的程序界面
##### 登录窗口
![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client1.png)  
<br />

##### 主界面
![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client2.png)  

<br />
<br />
<br />
<br />

# 本网络组件支持使用工业以太网技术访问PLC
<p>本组件支持常规的整数的数据读写，也支持字符串数据读写，包括中文，以下只是简单的举例，目前已经完成了一个三菱PLC高并发访问的类，具体交流可以通过以下方式联系我</p>
<ul>
<li>技术支持QQ群：<strong>592132877</strong></li>
<li>邮箱：<strong>hsl200909@163.com</strong></li>
</ul>
<p>如果要使用本组件访问PLC，需要引用命名空间，如下</p>
<pre>
<code>
using HslCommunication.Profinet;
</code>
</pre>

#### 三菱PLC访问数据读写

详细手册：<a href="https://github.com/dathlin/C-S-/blob/master/MelsecReadMe.md">三菱PLC数据读写手册</a>

##### 初始化对象
<pre>
<code>
        private MelsecNet melsec_net = new MelsecNet();
        private void Form1_Load(object sender, EventArgs e)
        {
            //初始化
            melsec_net.PLCIpAddress = System.Net.IPAddress.Parse("192.168.0.7");//IP
            melsec_net.PortRead = 6000;//端口
            melsec_net.PortReadBackup = 6001;//备用读端口，也可以不指定，默认负数，不会切换负数端口
            melsec_net.PortWrite = 6000;//写入端口，也可以和读取一样
            melsec_net.NetworkNumber = 0;//网络号
            melsec_net.NetworkStationNumber = 0;//网络站号
            melsec_net.ConnectTimeout = 500;//连接超时时间
        }
</code>
</pre>
##### 读写D寄存器数据
<pre>
<code>
        private void button1_Click(object sender, EventArgs e)
        {
            //读取PLC数据 D6000开始21个字 也即是D6000-D6020 最大长度980
            OperateResultBytes read = melsec_net.ReadFromPLC(MelsecDataType.D, 6000, 21);
            if(read.IsSuccess)
            {
                //成功读取
                textBox2.Text = "D6000:" + melsec_net.GetShortFromBytes(read.Content, 0);
                //textBox2.Text = "D6001:" + melsec_net.GetShortFromBytes(read.Content, 1);
                //textBox2.Text = "D6002:" + melsec_net.GetShortFromBytes(read.Content, 2);
                //textBox2.Text = "D6003:" + melsec_net.GetShortFromBytes(read.Content, 3);
                //textBox2.Text = "D6004:" + melsec_net.GetShortFromBytes(read.Content, 4);
                //================================================================================
                //这两种方式一样的，
                //textBox2.Text = "D6000:" + BitConverter.ToInt16(read.Content, 0);
                //textBox2.Text = "D6001:" + BitConverter.ToInt16(read.Content, 2);
                //textBox2.Text = "D6002:" + BitConverter.ToInt16(read.Content, 4);
                //textBox2.Text = "D6003:" + BitConverter.ToInt16(read.Content, 6);
                //textBox2.Text = "D6004:" + BitConverter.ToInt16(read.Content, 8);
            }
            else
            {
                //失败读取
                MessageBox.Show(read.ToMessageShowString());
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            short[] values = new short[4] { 1335, 8765, 1234, 4567 };//决定了写多少长度的D
            //写入PLC数据 D6000为1234,D6001为8765,D6002为1234,D6003为4567
            OperateResultBytes write = melsec_net.WriteIntoPLC(MelsecDataType.D, 6000, values);
            if(write.IsSuccess)
            {
                textBox2.Text = "写入成功";
            }
            else
            {
                MessageBox.Show(write.ToMessageShowString());//显示失败原因
            }
        }
</code>
</pre>
##### 读写M寄存器数据
<pre>
<code>
        private void button100_Click(object sender, EventArgs e)
        {
            //后台循环读取PLC数据 M200开始10个字 也即是M200-M209
            OperateResultBytes read = melsec_net.ReadFromPLC(MelsecDataType.M, 200, 10);
            if (read.IsSuccess)
            {
                textBox2.Text = "M200:" + (read.Content[0] == 1 ? "通" : "断");
                //textBox2.Text = "M201:" + (read.Content[1] == 1 ? "通" : "断");
                //textBox2.Text = "M202:" + (read.Content[2] == 1 ? "通" : "断");
                //textBox2.Text = "M203:" + (read.Content[3] == 1 ? "通" : "断");
                //textBox2.Text = "M204:" + (read.Content[4] == 1 ? "通" : "断");
                //textBox2.Text = "M205:" + (read.Content[5] == 1 ? "通" : "断");
                //textBox2.Text = "M206:" + (read.Content[6] == 1 ? "通" : "断");
                //textBox2.Text = "M207:" + (read.Content[7] == 1 ? "通" : "断");
                //textBox2.Text = "M208:" + (read.Content[8] == 1 ? "通" : "断");
                //textBox2.Text = "M209:" + (read.Content[9] == 1 ? "通" : "断");
            }
            else
            {
                //失败读取，显示失败信息
                MessageBox.Show(read.ToMessageShowString());
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //此处写入后M200:通 M201:断 M202:断 M203:通
            bool[] values = new bool[] { true, false, false, true };
            OperateResultBytes write = melsec_net.WriteIntoPLC(MelsecDataType.M, 200, values);
            if(write.IsSuccess)
            {
                textBox2.Text = "写入成功";
            }
            else
            {
                MessageBox.Show(write.ToMessageShowString());
            }
        }
</code>
</pre>

#### 西门子PLC访问数据读写

详细手册：<a href="https://github.com/dathlin/C-S-/blob/master/SiemensReadMe.md">西门子PLC数据读写手册</a>

##### 初始化对象
<pre>
<code>
        //西门子PLC读取块
        private SiemensNet siemens_net = new SiemensNet();
        private void Form1_Load(object sender, EventArgs e)
        {
            siemens_net.ConnectTimeout = 500;//超时时间
            siemens_net.PortRead = 2000;//读端口
            siemens_net.PortReadBackup = 2002;//备用读端口，也可以不指定，默认负数，不会切换负数端口
            siemens_net.PortWrite = 2001;//写端口
            siemens_net.PLCIpAddress = System.Net.IPAddress.Parse("192.168.0.6");//ip地址
        }
</code>
</pre>

##### 读写M寄存器数据
<pre>
<code>
        private void button3_Click(object sender, EventArgs e)
        {
            OperateResultBytes read = siemens_net.ReadFromPLC(SiemensDataType.M, 100, 2);
            if(read.IsSuccess)
            {
                textBox4.Text = "M100:" + read.Content[0] + " M101:" + read.Content[1];
                bool M100_0 = (read.Content[0] & 0x01) == 0x01;//M100.0的通断
                bool M100_1 = (read.Content[0] & 0x02) == 0x02;//M100.1的通断
                bool M100_2 = (read.Content[0] & 0x04) == 0x04;//M100.2的通断
                bool M100_3 = (read.Content[0] & 0x08) == 0x08;//M100.3的通断
                bool M100_4 = (read.Content[0] & 0x10) == 0x10;//M100.4的通断
                bool M100_5 = (read.Content[0] & 0x20) == 0x20;//M100.5的通断
                bool M100_6 = (read.Content[0] & 0x40) == 0x40;//M100.6的通断
                bool M100_7 = (read.Content[0] & 0x80) == 0x80;//M100.7的通断
            }
            else
            {
                MessageBox.Show(read.ToMessageShowString());
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //写入结果是M100:81  M101:22  M102:124
            OperateResultBytes write = siemens_net.WriteIntoPLC(SiemensDataType.M, 100, new byte[] { 81, 22, 124 });
            if (write.IsSuccess)
            {
                textBox4.Text = "写入成功";
            }
            else
            {
                MessageBox.Show(write.ToMessageShowString());
            }
        }
</code>
</pre>


# License:
Copyright (c) Richard.Hu. All rights reserved.
Licensed under the MIT License.
