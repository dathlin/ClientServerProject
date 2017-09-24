# 西门子PLC的详细的数据访问功能
<ul>
<li>技术支持QQ群：<strong>592132877</strong></li>
<li>邮箱：<strong>hsl200909@163.com</strong></li>
<li>概述：接下来详细讲解读写数据及字符串的代码</li>
</ul>

#### 配置西门子PLC的以太网模块

<strong>环境</strong>：此处使用了STEP 7V5.5 sp4编程软件作为示例，在添加以太网模块(6GK7 343-1EX30-0E0 CP343-1)到组态中时，可以设置IP地址及子网掩码， 此处测试使用，所以不使用路由器，如果您的西门子需要连接到内网中的话，需要配置路由器。目前只支持M,I,Q数据的读写。 然后点击新建，创建一个Ethernet(1)网络。以太网参数配置如下图：

![](https://github.com/dathlin/C-S-/raw/master/img/plc_siemens_1.jpg)

将以太网的模块添加到机架中以后，现在打开<strong>网络组态</strong> ，打开后点击组态上的PLC模块。会出现如下界面，在箭头出进行双击操作，可以弹出对话框，并进行一系列操作：

![](https://github.com/dathlin/C-S-/raw/master/img/plc_siemens_2.jpg)
![](https://github.com/dathlin/C-S-/raw/master/img/plc_siemens_3.jpg)
![](https://github.com/dathlin/C-S-/raw/master/img/plc_siemens_4.jpg)
![](https://github.com/dathlin/C-S-/raw/master/img/plc_siemens_5.jpg)
![](https://github.com/dathlin/C-S-/raw/master/img/plc_siemens_6.jpg)

按照上面一套操作下来，创建了一个读取的端口，端口号为2000，后面有用，需要记住， 按照上述的步骤再创建一个写入的端口，只有最后一步不一致，如下：

![](https://github.com/dathlin/C-S-/raw/master/img/plc_siemens_7.jpg)

配置完之后的效果图如下，新建了两个端口，一个用于读取数据，一个用于写入数据。 <strong>注意：设置完成后一定要写入到PLC才算真的完成。</strong>

![](https://github.com/dathlin/C-S-/raw/master/img/plc_siemens_8.jpg)

如上图所示，共配置了2000,2001两个端口号，配置完成后需要进行重启PLC，端口的配置原则如下：

<strong>端口号设置规则:</strong>
<ul>
<li>为了不与原先存在的系统发生冲突，您在添加自己的端口时尽量使用您自己的端口。</li>
<li>如果读写都需要，尽可能的将读取端口和写入端口区分开来，这样做比较高性能。</li>
<li>如果您的网络状态不是特别稳定，读取端口使用2个，一个受阻切换另一个读取可以提升系统的稳定性。</li>
</ul>

西门子PLC的数据种类其实只有一种，就是byte数据，更大的数据用多个byte来组合，位数据就是byte上的位，所以会比三菱的简单一点点，好处理一点。

#### 初始化访问PLC对象

如果想使用本组件的数据读取功能，必须先初始化数据访问对象，根据实际情况进行数据的填入。 下面仅仅是测试中的数据：
<pre>
<code>
	private SiemensNet siemens_net = new SiemensNet();
	private void FormPlcTest_Load(object sender, EventArgs e)
	{
		//初始化，此处的值参考了上面的配置参数
		siemens_net.ConnectTimeout = 500;
		siemens_net.PortRead = 2000;
		siemens_net.PortReadBackup = 2002;//备用读端口，也可以不指定，默认负数，不会切换负数端口
		siemens_net.PortWrite = 2001;
		siemens_net.PLCIpAddress = System.Net.IPAddress.Parse("192.168.0.6");
	}
</code>
</pre>
说明：对象应该放在窗体类下面，此处仅仅针对读取一台设备的plc，也可以在访问的方法中实例化局部对象， 初始化数据，然后读取，该对象几乎不损耗内存，内存垃圾由CLR进行自动回收。此处测试方便，窗体的多个按钮均连接同一台PLC 设备，所以本窗体实例化一个对象即可。

#### M,I,Q数据读写

由于西门子这三个数据种类相同，操作时一模一样，所以此处只展示读写M寄存器的例子。

如下方法演示读取了M100-M101这2个M的值，返回值解析：如果读取正常则共返回2个字节的数据，以下示例数据
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
错误说明：有可能因为站号网络号没有配置正确返回有错误代号没有错误信息， 也有可能因为网络问题导致没有连接上，此时会有连接不上的错误信息。

下面展示的是后台线程循环读取的情况，事实上在实际的使用过程中经常会碰见的情况。下面的方法需要 放到单独的线程中，同理，访问D数据时也是按照下面循环就行，此处不再赘述。
<pre>
<code>
	//后台循环读取PLC数据 M200开始10个字 也即是M200-M209
	while (true)
	{
		OperateResultBytes read = siemens_net.ReadFromPLC(SiemensDataType.M, 100, 2);
		if (read.IsSuccess)
		{
			//成功读取，委托显示
			textBox2.BeginInvoke(new Action(delegate
			{
				textBox4.Text = "M100:" + read.Content[0] + " M101:" + read.Content[1];
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

#### 整数数据读写(一个数据由2个byte组成)

虽然上述实现了M数据的读写，但是只能表示0-255的数据，想要支持更大的数据，需要自己指定规则， 这就需要你对数据和字节原理非常清晰才能实现，为了方便，此处提供了读写双字节数据的功能，先演示读取M100-M106 的数据，对应有三个双字节数据，代码如下：
<pre>
<code>
	private void button15_Click(object sender, EventArgs e)
	{
		//整数读取
		OperateResultBytes read = siemens_net.ReadFromPLC(SiemensDataType.M, 100, 6);
		if (read.IsSuccess)
		{
			short[] result = siemens_net.GetArrayFromBytes(read.Content);
			textBox4.Text = "M100:" + result[0] + ",M102:" + result[1] + ",M104:" + result[2];
			//或者下述的方法或许，可以发现和三菱的D数据获取是一模一样的
			//short m100 = BitConverter.ToInt16(read.Content, 0);
			//short m102 = BitConverter.ToInt16(read.Content, 2);
			//short m104 = BitConverter.ToInt16(read.Content, 4);
		}
		else
		{
			MessageBox.Show(read.ToMessageShowString());
		}
	}
</code>
</pre>

实际使用中，如果有不明白的地方，可以咨询我，接下来介绍写入整数数据的操作，例如，我们要使得M100,M101=2456，M102,M103=4567，M104,M105=-124，那么代码如下：
<pre>
<code>
	private void button16_Click(object sender, EventArgs e)
	{
		//整数写入，数组的长度为x，那么占用的M字节数为x乘以2
		OperateResultBytes write = siemens_net.WriteIntoPLC(SiemensDataType.M, 100, new short[] { 2456, 4567, -124 });
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

#### ASCII字符串数据的读写

在实际项目中，有可能会碰到PLC存储了规格数据，或是条码数据，这些数据是以ASCII编码形式存在， 我们需要把数据进行读取出来用于显示，保存等操作。下面演示读取指定长度的条码数据，数据的数据存放在M100-M109中， 长度应该为存储条码的最大长度，也即是占用了10个M，<strong>一个M可以存储1个ASCII码字符：</strong>
<pre>
<code>
	private void button12_Click(object sender, EventArgs e)
	{
		//读取字符串，长度为10
		OperateResultBytes read = siemens_net.ReadFromPLC(SiemensDataType.M, 100, 10);
		if (read.IsSuccess)
		{
			textBox4.Text = Encoding.ASCII.GetString(read.Content);
		}
		else
		{
			MessageBox.Show(read.ToMessageShowString());
		}
	}
</code>
</pre>

下面演示写入条码数据，地址在M100-M109中，所以需要写入10个字符：

<pre>
<code>
	private void button14_Click(object sender, EventArgs e)
	{
		//写字符串，字符串的长度决定了需要占用多少个M字节，两者是相等的
		OperateResultBytes write = siemens_net.WriteAsciiStringIntoPLC(SiemensDataType.M, 100, "K123456789");
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

需要注意的是，如果第一次在M100-M109中写入了"K123456789"，第二次写入了"K6666"，那么读取M100-M109的条码数据会读取到K666656789，如果要避免这种情况，则需要在写入条码的时候，指定总长度，<strong>该长度 可单数可偶数</strong>，具体的使用方法如下：

<pre>
<code>
	private void button14_Click(object sender, EventArgs e)
	{
		//写字符串
		OperateResultBytes write = siemens_net.WriteAsciiStringIntoPLC(SiemensDataType.M, 100, "K6666", 10);
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

#### 中文及特殊字符的读写

在需要读写复杂的字符数据时，上述的ASCII编码已经不能满足要求，虽然使用读写的基础方法可以实现任意数据的读写， 但是此处为了方便，还是提供了一个方便的方法来读写中文数据，采用Unicode编码的字符， 该编码下的一个字符占用两个M来存储。如下将演示，读写方法，基本用途和上述 ASCII编码的读写一致。

<pre>
<code>
	private void button11_Click(object sender, EventArgs e)
	{
		//读取中文 指定读取的长度必须双数，否则解码失败
		OperateResultBytes read = siemens_net.ReadFromPLC(SiemensDataType.M, 200, 12);
		if (read.IsSuccess)
		{
			textBox4.Text = Encoding.Unicode.GetString(read.Content);
		}
		else
		{
			MessageBox.Show(read.ToMessageShowString());
		}
	}
</code>
</pre>

在写入的过程中，只演示写入指定长度的(实际中也应该使用这个方法)，指定长度的意思为多少个中文。

<pre>
<code>
	private void button13_Click(object sender, EventArgs e)
	{
		//中文写入
		OperateResultBytes write = siemens_net.WriteUnicodeStringIntoPLC(SiemensDataType.M, 200, "测试de东西", 10);
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

#### 一个实际中复杂的例子演示

实际中可能碰到的情况会很复杂，一台设备中需要上传的数据包含了温度，压力，产量，规格等等信息，在一串数据中 会包含各种各样的不同的数据，所以此处做一个完整示例的演示，假设我们需要读取 M100-M116的数据，假设M100,M101存放了温度数据，55.1℃在M中为551，M102,M103存放了压力数据，1.23MPa在M中存放为123，M104存放了 设备状态，0为停止，1为运行，M105,M106存放了产量，1000就是指1000个，M107-M116存放了规格，以下代码演示如何去解析数据：

<pre>
<code>
	private void button29_Click(object sender, EventArgs e)
	{
		//解析复杂数据
		OperateResultBytes read = siemens_net.ReadFromPLC(SiemensDataType.M, 200, 17);
		if (read.IsSuccess)
		{
			double 温度 = BitConverter.ToInt16(read.Content, 0) / 10d;//索引很重要
			double 压力 = BitConverter.ToInt16(read.Content, 2) / 100d;
			bool IsRun = read.Content[4] == 1;
			int 产量 = BitConverter.ToInt16(read.Content, 5);
			string 规格 = Encoding.ASCII.GetString(read.Content, 7, 10);
		}
		else
		{
			MessageBox.Show(read.ToMessageShowString());
		}
	}
</code>
</pre>

#### 高并发时的高性能访问方法

还未完成，等待发布。
