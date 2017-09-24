# 三菱PLC的详细的数据访问功能
<ul>
<li>技术支持QQ群：<strong>592132877</strong></li>
<li>邮箱：<strong>hsl200909@163.com</strong></li>
<li>概述：接下来详细讲解读写数据及字符串的代码</li>
</ul>

#### 配置三菱PLC的以太网模块

<strong>环境</strong>：此处以GX Works2为示例，添加以太网模块，型号为<strong>QJ71E71-100</strong>，组态里添加完成后进行以太网的参数配置，此处需要注意的是：<strong style="color:#ff0000">参数的配置对接下来的代码中配置参数要一一对应</strong>
![](https://github.com/dathlin/C-S-/raw/master/img/plc_melsec_1.jpg)

<strong><font color=#ff0000>注意：在PLC的以太网模块的配置中，无法设置网络号为0，也无法设置站号为0， 所以此处均设置为1，在C#程序中也使用上述的配置，在代码中均配置为0，如果您自定义设置为网络2， 站号8，那么在代码中就要写对应的数据。如果仍然通信失败，重新测试0，0。</font></strong>

<strong>打开设置</strong>：在上图中的打开设置选项，进行其他参数的配置，下图只是举了一个例子，开通了4个端口来支持读写操作：
![](https://github.com/dathlin/C-S-/raw/master/img/plc_melsec_2.jpg)

<strong>端口号设置规则:</strong>
<ul>
<li>为了不与原先存在的系统发生冲突，您在添加自己的端口时尽量使用您自己的端口。</li>
<li>如果读写都需要，尽可能的将读取端口和写入端口区分开来，这样做比较高性能。</li>
<li>如果您的网络状态不是特别稳定，读取端口使用2个，一个受阻切换另一个读取可以提升系统的稳定性。</li>
</ul>
本文档仅作组件的测试，所以只用了一个端口作为读写。如果你的程序也使用了一个端口，那么你在读取数据时候， 刚好也在写入（异步操作可能发生这样的情况），那么写入会失败！

三菱PLC的数据主要由两类数据组成，位数据和字数据，在位数据中，例如X,Y,M,L都是位数据，字数据例如D,W。 两类的数据在读取解码上存在一点小差别。（事实上也可以先将16个M先赋值给一个D，读取D数据再进行解析， 在读取M的数量比较多的时候，这样操作效率更高）

#### 初始化访问PLC对象

如果想使用本组件的数据读取功能，必须先初始化数据访问对象，根据实际情况进行数据的填入。 下面仅仅是测试中的数据：
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
说明：对象应该放在窗体类下面，此处仅仅针对读取一台设备的plc，也可以在访问的方法中实例化局部对象， 初始化数据，然后读取，该对象几乎不损耗内存，内存垃圾由CLR进行自动回收。此处测试方便，窗体的多个按钮均连接同一台PLC 设备，所以本窗体实例化一个对象即可。

#### X,Y,M,L位数据的读写说明

本小节将展示四种位数据的读取，虽然更多的时候只是读取D数据即可，或者是将位数据批量挪到D数据中， 但是在此处仍然进行介绍单独的读取X,Y,M,L，由于这四种读取手法一致，故针对M数据进行介绍，其他的您可以自己测试。

如下方法演示读取了M200-M209这10个M的值，注意：读取长度必须为偶数，即时写了奇数，也会补齐至偶数，<strong style="color:#ff0000">读取和写入的最大长度为7168，否则报错。如需实际需求确实大于7168的，请分批次读取。</strong><br />
返回值解析：如果读取正常则共返回10个字节的数据，以下示例数据
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

错误说明：有可能因为站号网络号没有配置正确返回有错误代号没有错误信息， 也有可能因为网络问题导致没有连接上，此时会有连接不上的错误信息。


下面展示的是后台线程循环读取的情况，事实上在实际的使用过程中经常会碰见的情况。下面的方法需要 放到单独的线程中，同理，访问D数据时也是按照下面循环就行，此处不再赘述。
<pre>
<code>
	//后台循环读取PLC数据 M200开始10个字 也即是M200-M209
	while (true)
	{
		OperateResultBytes read = melsec_net.ReadFromPLC(MelsecDataType.M, 200, 10);
		if (read.IsSuccess)
		{
			//成功读取，委托显示
			textBox2.BeginInvoke(new Action(delegate
			{
				textBox2.Text = "M201:" + (read.Content[1] == 1 ? "通" : "断");
			}));
		}
		else
		{
			//失败读取，应该对失败信息进行日志记录，不应该显示，测试访问时才适合显示错误信息
			LogHelper.save(read.ToMessageShowString());
		}
		System.Threading.Thread.Sleep(1000);//决定了访问的频率
	}
</code>
</pre>

#### D,W字数据的读写操作

此处读取针对中间存在整数数据的情况，因为两者读取方式相同，故而只演示一种数据读取， <strong style="color:#ff0000">使用本组件读取数据，一次最多读取或写入960个字，超出则失败。 如果读取的长度确实超过限制，请考虑分批读取。</strong>
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

#### ASCII字符串数据的读写

在实际项目中，有可能会碰到PLC存储了规格数据，或是条码数据，这些数据是以ASCII编码形式存在， 我们需要把数据进行读取出来用于显示，保存等操作。下面演示读取指定长度的条码数据，数据的数据存放在D2000-D2004中， 长度应该为存储条码的最大长度，也即是占用了5个D，<strong style="color:#ff0000">一个D可以存储2个ASCII码字符：</strong>
<pre>
	<code>
	private void button7_Click(object sender, EventArgs e)
	{
		//读取字符串数据，共计10个字节长度
		OperateResultBytes read = melsec_net.ReadFromPLC(MelsecDataType.D, 2000, 5);
		if (read.IsSuccess)
		{
			//成功读取
			textBox2.Text = Encoding.ASCII.GetString(read.Content);
		}
		else
		{
			//失败读取
			MessageBox.Show(read.ToMessageShowString());
		}
	}
	private void button8_Click(object sender, EventArgs e)
	{
		//写字符串，如果写入K12345678这9个字符，读取出来时末尾会补0
		OperateResultBytes write = melsec_net.WriteAsciiStringIntoPLC(MelsecDataType.D, 2000, "K123456789");
		if (write.IsSuccess)
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

需要注意的是，如果第一次在D2000-D2004中写入了"K123456789"，第二次写入了"K6666"，那么读取D2000-D2004的条码数据会读取到 K666656789，如果要避免这种情况，<strong style="color:#ff0000">则需要在写入条码的时候，指定总长度，该长度必须为偶数， 不然也会自动补0，小于该长度时，自动补零，大于该长度时，自动截断数据，</strong>具体的使用方法如下：
<pre>
<code>
	private void button8_Click(object sender, EventArgs e)
	{
		//写字符串，本次写入指定了10个长度的字符，其余的D的数据将被清空，是一种安全的写入方式
		OperateResultBytes write = melsec_net.WriteAsciiStringIntoPLC(MelsecDataType.D, 2000, "K6666", 10);
		if (write.IsSuccess)
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

#### 中文及特殊字符的读写

在需要读写复杂的字符数据时，上述的ASCII编码已经不能满足要求，虽然使用读写的基础方法可以实现任意数据的读写， 但是此处为了方便，还是提供了一个方便的方法来读写中文数据，采用Unicode编码的字符， 该编码下的一个字符占用一个D或W来存储。如下将演示，读写方法，基本用途和上述 ASCII编码的读写一致。
<pre>
<code>
	private void button9_Click(object sender, EventArgs e)
	{
		//读中文，存储在D3000-D3009
		OperateResultBytes read = melsec_net.ReadFromPLC(MelsecDataType.D, 3000, 10);
		if (read.IsSuccess)
		{
			//解析数据
			textBox2.Text = Encoding.Unicode.GetString(read.Content);
		}
		else
		{
			MessageBox.Show(read.ToMessageShowString());
		}
	}
	private void button10_Click(object sender, EventArgs e)
	{
		//写中文 D3000-D3009，该10含义为中文字符数
		OperateResultBytes write = melsec_net.WriteUnicodeStringIntoPLC(MelsecDataType.D, 3000, "测试数据test", 10);
		if (write.IsSuccess)
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

#### 一个实际中复杂的例子演示

实际中可能碰到的情况会很复杂，一台设备中需要上传的数据包含了温度，压力，产量，规格等等信息，在一串数据中 会包含各种各样的不同的数据，上述的读取D，读取M，读取条码的方式不太好用，所以此处做一个完整示例的演示，假设我们需要读取 D4000-D4009的数据，假设D4000存放了温度数据，55.1℃在D中为551，D4001存放了压力数据，1.23MPa在D中存放为123，D4002存放了 设备状态，0为停止，1为运行，D4003存放了产量，1000就是指1000个，D4004备用，D4005-D4009存放了规格，以下代码演示如何去解析数据：
<pre>
<code>
	private void button29_Click(object sender, EventArgs e)
	{
		//解析复杂数据
		OperateResultBytes read = melsec_net.ReadFromPLC(MelsecDataType.D, 4000, 10);
		if (read.IsSuccess)
		{
			double 温度 = BitConverter.ToInt16(read.Content, 0) / 10d;//索引很重要
			double 压力 = BitConverter.ToInt16(read.Content, 2) / 100d;
			bool IsRun = BitConverter.ToInt16(read.Content, 4) == 1;
			int 产量 = BitConverter.ToInt16(read.Content, 6);
			string 规格 = Encoding.ASCII.GetString(read.Content, 10, 10);
		}
		else
		{
			MessageBox.Show(read.ToMessageShowString());
		}
	}
</code>
</pre>

#### 高并发时的高性能访问方法

实际中可能会碰到需要读取100台设备的PLC上的数据，读取的数据地址是一致的，如果选择开100条线程去读取，将会极大的浪费系统资源，且又效率低下，即使使用了线程池技术，也无法保证所有的数据进行同步处理。所以本系统提供了一个类来高性能的访问多台设备，<strong style="color:#ff0000">注意：该类只针对访问相同数据块的设备。</strong>
<pre>
<code>
        /*************************************************************************************************
         * 
         *    以下展示一个高性能访问多台PLC数据的类，即使同时访问100台设备，性能也是非常高
         * 
         *    该类没有仔细的在现场环境测试过，不保证完全可用
         * 
         *************************************************************************************************/

        HslCommunication.Profinet.MelsecNetMultiAsync MelsecMulti { get; set; }

        private void MelsecNetMultiInnitialization()
        {
            List<System.Net.IPEndPoint> IpEndPoints = new List<System.Net.IPEndPoint>();
            //增加100台需要访问的三菱设备，指定所有设备IP和端口，注意：顺序很重要
            for (int i = 1; i < 100; i++)
            {
                IpEndPoints.Add(new System.Net.IPEndPoint(System.Net.IPAddress.Parse("192.168.10." + i), 6000));
            }

            //每隔1秒钟访问一次
            MelsecMulti = new HslCommunication.Profinet.MelsecNetMultiAsync(0, 0, HslCommunication.Profinet.MelsecDataType.D, 6000, 20, 700, 1000, IpEndPoints.ToArray());
            MelsecMulti.OnReceivedData += MelsecMulti_OnReceivedData;//所有机台的数据都返回时触发
        }

        private void MelsecMulti_OnReceivedData(byte[] object1)
        {
            /*********************************************************************************************
             * 
             *    正常情况下，一秒触发一次，object1包含了所有机台读取到的数据
             *    比如每台设备读取D6000开始20个D，如上述指令所示
             *    那么每台设备数据长度为20*2+2=42个byte，100台设备就是4200字节长度
             *    也就是说，object1的0-41字节是第一台设备的，以此类推
             *    每台设备的前两个字节都为0才说明本次数据访问正常，为0x00,0x01说明连接失败，其他说明说明三菱本身的异常
             * 
             ********************************************************************************************/
            for (int i = 0; i < 100; i++)
            {
                int startIndex = i * 42;
                ushort netState = BitConverter.ToUInt16(object1, startIndex);
                //为0，说明数据正常，不为0说明网络访问失败或是指令出错
            }
        }
</code>
</pre>
