# 同步网络功能 Support By HslCommunication.dll


## Summary
一个用于同步数据交互的网络通信类，在实际的程序开发中，我们经常会碰到这样的需要，我们需要向服务器请求一些数据，然后接收从服务器返回的数据，数据类型通常为 **byte[]** 或是 **string** ,类型，所以通常在服务器端会配置一个数据处理总站，每次请求都会带有一个信号头，用于服务器区分不同的信息机制，而我们可以使用组件中的 **NetSimplifyServer** 和 **NetSimplifyClient** 类来完成功能。

## Reference
日志组件所有的功能类都在 **HslCommunication** 和 **HslCommunication.Enthernet** 命名空间，所以再使用之前先添加
<pre>
<code>
using HslCommunication;
using HslCommunication.Enthernet;
</code>
</pre>

## How to  Use

#### 服务器端
我们先要在服务器端进行创建网络监听，这样才能让客户端连接到服务器，服务器需要先实例化及初始化代码如下，代码直接在服务器主窗口下面：
<pre>
<code>
        // 用户同步数据传送的引擎
        private NetSimplifyServer net_simplify_server = new NetSimplifyServer(); //实例化
        // 同步传送数据的初始化
        private void Net_Simplify_Server_Initialization()
        {
            try
            {
                net_simplify_server.KeyToken = Guid.Empty;//设置身份令牌，本质就是一个GUID码，验证客户端使用
                net_simplify_server.LogNet = new LogNetSingle(LogSavePath + @"\simplify_log.txt");//日志路径
                net_simplify_server.LogNet.SetMessageDegree(HslMessageDegree.DEBUG);//默认debug及以上级别日志均进行存储，根据需要自行选择
                net_simplify_server.ReceiveStringEvent += Net_simplify_server_ReceiveStringEvent;//接收到字符串触发
                net_simplify_server.ReceivedBytesEvent += Net_simplify_server_ReceivedBytesEvent;//接收到字节触发
                net_simplify_server.ServerStart(17432);//网络端口，此处使用了一个随便填写的端口
                net_simplify_server.ConnectTimeout = 5200;
            }
            catch (Exception ex)
            {
                SoftBasic.ShowExceptionMessage(ex);
            }
        }

        /// &lt;summary>
        /// 接收来自客户端的字节数据
        /// &lt;/summary>
        /// &lt;param name="state">网络状态&lt;/param>
        /// &lt;param name="customer">字节数据，根据实际情况选择是否使用&lt;/param>
        /// &lt;param name="data">来自客户端的字节数据&lt;/param>
        private void Net_simplify_server_ReceivedBytesEvent(AsyncStateOne state, NetHandle customer, byte[] data)
        {
            if(customer==1000)
            {
				// 收到指令为1000的请求时，返回1000长度的字节数组
                net_simplify_server.SendMessage(state, customer, new byte[1000]);
            }
            else
            {
                net_simplify_server.SendMessage(state, customer, data);
            }
        }




        /***********************************************************************************************
         * 
         *    方法说明：    当接收到来自客户端的数据的时候触发的方法
         *    特别注意：    如果你的数据处理中引发了异常，应用程序将会奔溃，SendMessage异常系统将会自动处理
         * 
         ************************************************************************************************/


        /// &lt;summary>
        /// 接收到来自客户端的字符串数据，然后将结果发送回客户端，注意：必须回发结果
        /// &lt;/summary>
        /// &lt;param name="state">客户端的地址&lt;/param>
        /// &lt;param name="handle">用于自定义的指令头，可不用，转而使用data来区分&lt;/param>
        /// &lt;param name="data">接收到的服务器的数据v/param>
        private void Net_simplify_server_ReceiveStringEvent(AsyncStateOne state, NetHandle handle, string data)
        {

            /*******************************************************************************************
             * 
             *     说明：同步消息处理总站，应该根据不同的消息设置分流到不同的处理方法
             *     
             *     注意：处理完成后必须调用 net_simplify_server.SendMessage(state, customer, "处理结果字符串，可以为空");
             *
             *******************************************************************************************/

            if (handle == 1)
            {
                net_simplify_server.SendMessage(state, handle, "测试数据一");
            }
            else if (handle == 2)
            {
                net_simplify_server.SendMessage(state, handle, "测试数据二");
            }
			else if (handle == 3)
            {
                net_simplify_server.SendMessage(state, handle, "测试数据三");
            }
            else
            {
                net_simplify_server.SendMessage(state, handle, data);
            }
        }


</code>
</pre>

服务端的主要代码都在上面的代码段了，也没多少代码，关键是支持的请求多了之后，不停的使用 **if...else** 代码会显得很多很乱，所以此处的 **Nethandle** 这个值类型就是为了解决这个问题而设计的，它本质上是一个 **int** 数据，我们知道一个 **int** 是由4个字节组成，那么byte[0]byte[1]byte[2]byte[3]，那么我可以用byte[3]（最高位）来作为指令大类。byte[2]来作为指令小类，byte[0]和byte[1]组成的 **ushort** 数据来作为指令编号，所以上述的方法 **Net_simplify_server_ReceiveStringEvent** 中的细节可以改成下面：
<pre>
<code>

        /// &lt;summary>
        /// 接收到来自客户端的字符串数据，然后将结果发送回客户端，注意：必须回发结果
        /// &lt;/summary>
        /// &lt;param name="state">客户端的地址&lt;/param>
        /// &lt;param name="handle">用于自定义的指令头，可不用，转而使用data来区分&lt;/param>
        /// &lt;param name="data">接收到的服务器的数据&lt;/param>
        private void Net_simplify_server_ReceiveStringEvent(AsyncStateOne state, NetHandle handle, string data)
        {

            /*******************************************************************************************
             * 
             *     说明：同步消息处理总站，应该根据不同的消息设置分流到不同的处理方法
             *     
             *     注意：处理完成后必须调用 net_simplify_server.SendMessage(state, customer, "处理结果字符串，可以为空");
             *
             *******************************************************************************************/

            if (handle.CodeMajor == 1)
            {
                ProcessCodeMajorOne(state, handle, data);
            }
            else if (handle.CodeMajor == 2)
            {
                ProcessCodeMajorTwo(state, handle, data);
            }
			else if (handle.CodeMajor == 3)
            {
                ProcessCodeMajorThree(state, handle, data);
            }
            else
            {
                net_simplify_server.SendMessage(state, handle, data);
            }
        }

        private vode ProcessCodeMajorOne(AsyncStateOne state, NetHandle handle, string data)
        {
            if (handle.CodeIdentifier == 1)
            {
                // 下面可以再if..else
                net_simplify_server.SendMessage(state, handle, "测试数据大类1，命令1，接收到的数据是：" + data);
            }
            else
            {
                net_simplify_server.SendMessage(state, handle, data);
            }
        }

        private vode ProcessCodeMajorTwo(AsyncStateOne state, NetHandle handle, string data)
        {
            if (handle.CodeIdentifier == 1)
            {
                // 下面可以再if..else
                net_simplify_server.SendMessage(state, handle, "测试数据大类2，命令1，接收到的数据是：" + data);
            }
            else
            {
                net_simplify_server.SendMessage(state, handle, data);
            }
        }

        private vode ProcessCodeMajorThree(AsyncStateOne state, NetHandle handle, string data)
        {
            if (handle.CodeIdentifier == 1)
            {
                // 下面可以再if..else
                net_simplify_server.SendMessage(state, handle, "测试数据大类3，命令1，接收到的数据是：" + data);
            }
            else
            {
                net_simplify_server.SendMessage(state, handle, data);
            }
        }

		
</code>
</pre>

指令根据不同的功能进行归类，会使代码简洁很多。


#### 客户端

客户端的程序相对简单很多，只需要实例化一下就可以使用了，而且该实例化对象的方法是线程安全的，所以在定义成静态对象，在代码的任何地方都可以使用，不需要再重复实例化，如下代码是实例化：
<pre>
<code>
        /// 用于访问服务器数据的网络对象类，必须修改这个端口参数，否则运行失败
        public static NetSimplifyClient Net_simplify_client { get; set; } = new NetSimplifyClient(
            new IPEndPoint(IPAddress.Parse("127.0.0.1"), 17432))  // 指定服务器的ip，和服务器设置的端口
        {
            KeyToken = Guid.Empty, // 这个guid码必须要服务器的一致，否则服务器会拒绝连接
            ConnectTimeout = 5000,
        };
</code>
</pre>


接下来就是读取数据的展示了，返回的结果关联到一个类 **OperateResultString** 这个类只包含了几个公开的数据属性，没什么实际的含义，一看就明白了。
<pre>
<code>
            OperateResultString result = Net_simplify_client.ReadFromServer(
                new NetHandle(1,0,1), "发送的数据"); // 指示了大类1，子类0，编号1

            if (result.IsSuccess)
            {
                // 按照上面服务器的代码，此处显示数据为："上传成功！返回的数据：测试数据大类1，命令1，接收到的数据是：发送的数据"
                MessageBox.Show("操作成功！返回的数据：" + result.Content);
            }
            else
            {
                MessageBox.Show("操作失败！原因：" + result.Message);
            }
</code>
</pre>


#### 失败说明
失败的原因通常来自网络异常，当你把服务器架设在云端时，或是其他的服务器电脑，如果访问老是失败，就要检查防火墙是否允许指定端口网络通信了。