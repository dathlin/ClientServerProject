# 日志功能 Support By HslCommunication.dll

## Summary
本组件提供了一个日志功能，实现了常用的3中日志模式：
<ul>
<li>单日志文件模式，日志始终都向该文件写入，需要程序中自行清除或手动编写代码清空，不然文件会很大。</li>
<li>根据文件大小存储的多文件模式，一个日志文件写入达到某个数值时，创建新文件写入。</li>
<li>根据时间日期的多文件模式，每条日志将根据写入的时间日志来判断文件名，比如创建按每天存储的日志。</li>
</ul>
我也曾使用过.net中大名鼎鼎的log4net日志组件，一般使用起来确实方便，但是我在诸如实现上述的需求的时候就特别麻烦，而且log4net功能众多，大多数功能并不是我所需要的，我就需要一个实现txt日志的存储方式，简单高效，多种模式即可。于是就自行开发了一个简单高效的日志组件，并集成到了该通信库中，本日志组件还提供了一个分析控件及窗口，可以对一个长长的日志文件进行分析统计（只对本组件生成的日志有效），快速定位需要查找的日志信息，支持使用正则表达式来筛选，本日志组件也提供了分级存储。

## Reference
日志组件所有的功能类都在**HslCommunication.LogNet**命名空间，所以再使用之前先添加
<pre>
<code>
using HslCommunication.LogNet;
</code>
</pre>

## How to  Use

#### 实例化
首先先实例化一个对象，如果您用于整个application的日志存储，可以定义成静态对象。
<pre>
<code>
private ILogNet logNet = new LogNetSingle("D:\\123.txt");
</code>
</pre>
我们通常的做法是日志文件存储在exe程序目录下的Logs文件夹中，无论在服务器端还是客户端都是非常适用的，所以
<pre>
<code>
private ILogNet logNet = new LogNetSingle(Application.StartupPath + "\\Logs\\123.txt");
</code>
</pre>

#### 写日志
接下来你就可以在窗口的其他地方进行写入日志了，本组件提供了5个等级的日志写入功能，名称参考了常规的日志等级，有 **DEBUG** , **INFO**,  **WARN** , **ERROR** , **FATAL** ，根据需要进行存储，还提供了对exception异常的方法支持和自定义的描述化文本写入，该文本不属于日志范畴，在日志分析中会被忽略，如下代码演示几种不同的写入：
<pre>
<code>
logNet.WriteDebug("调试信息");
logNet.WriteInfo("一般信息");
logNet.WriteWarn("警告信息");
logNet.WriteError("错误信息");
logNet.WriteFatal("致命信息");
logNet.WriteException(null, new IndexOutOfRangeException());
</code>
</pre>
写入异常时会被自动赋予 **FATAL** 等级。写入效果如下：

![](https://github.com/dathlin/C-S-/raw/master/img/log_1.jpg)

下面再说明写入描述文本的时候发生的事情：
<pre>
<code>
logNet.WriteDescrition("这是一条描述文本");
logNet.WriteDebug("调试信息");
logNet.WriteInfo("一般信息");
logNet.WriteWarn("警告信息");
logNet.WriteError("错误信息");
logNet.WriteFatal("致命信息");
logNet.WriteException(null, new IndexOutOfRangeException());
</code>
</pre>

![](https://github.com/dathlin/C-S-/raw/master/img/log_2.jpg)

#### 设置等级

描述性文本的前面会再新增一行作为明显的区分。下面再来说明下如何设置日志存储等级。比如我们在开发app的过程中，肯定要经常调试之类的，为了顺利的调试和快速的找到BUG，我们会对系统运行状态，一些关键变量进行输出，但是这些输出来部署运行时是不需要的（当前有些还是必须的，可以方便的追踪问题），如果没有日志分级，我们就需要对部分的日志输出代码进行注释，显然浪费人力物力，在此处我们可以将调试时查看的一些关键信息使用 **DEBUG** 的等级存储，然后在部署时，重新设置日志的存储等级为 **INFO** 等级，那么所有的 **DEBUG** 等级的日志都不会被记录到日志。使用方法：

<pre>
<code>
logNet.SetMessageDegree(HslMessageDegree.DEBUG);//所有等级存储
logNet.SetMessageDegree(HslMessageDegree.INFO);//除DEBUG外，都存储
logNet.SetMessageDegree(HslMessageDegree.WARN);//除DEBUG和INFO外，都存储
logNet.SetMessageDegree(HslMessageDegree.ERROR);//只存储ERROR和FATAL
logNet.SetMessageDegree(HslMessageDegree.FATAL);//只存储FATAL
logNet.SetMessageDegree(HslMessageDegree.None);//不存储任何等级
</code>
</pre>

#### 自定义事件

组件默认存储所有的等级，如果需要设置，在实例化后即可设置等级。日志组件支持一个事件，在所有的日志进行存储前（被日志等级过滤掉的不会触发）会报告事件，可以用于其他操作或是控制台的显示等等，注意： **如果在事件关联方法中直接访问UI线程，会异常** 
<pre>
<code>
logNet.BeforeSaveToFile += LogNet_BeforeSaveToFile;
</code>
</pre>
<pre>
<code>
private void LogNet_BeforeSaveToFile(object sender, HslEventArgs e)
{
    // 如果需要UI显示，就要取消注释下方的代码

    //if(InvokeRequired)
    //{
    //    Invoke(new Action(() =>{
    //        LogNet_BeforeSaveToFile(sender, e);
    //    }));
    //    return;
    //}

    string degree = e.HslMessage.Degree.ToString();//获取等级
    DateTime time = e.HslMessage.Time;//获取时间
    string text = e.HslMessage.Text;//日志文本
    int threadId = e.HslMessage.ThreadId;//记录日志的线程id
}
</code>
</pre>

#### 按文件大小存储的实例化

若要按照文件大小进行存储，例如日志存储2M后，自动生成新的文件，然后存满2M后生成新文件，如此重复，则需要指定文件的 **存储路径** 和 **大小** ，这种方式存储的文件名称不可控制，自动定义为 **Logs_20170903170604.txt** 的格式，会以当前时间自动命名，如下举例了实例化一个2M大小的对象:
<pre>
<code>
ILogNet logNet = new LogNetFileSize(Application.StartupPath + "\\customer1", 2 * 1024 * 1024);
</code>
</pre>
只要定义了对象，就可以按照上述写入日志的代码来写了。

#### 按时间日期存储的实例化
也是一种多文件的存储机制，和按照大小的存储非常类似，此处可以配置按照每小时生成新的文件，每天新的文件，每月新的文件，每季度新的文件，每年新的文件，生成的文件名称也是固定的，需要指定 **路径** 和 **存储模式** ：
<pre>
<code>
ILogNet logNet = new LogNetDateTime(Application.StartupPath + "\\customer2",GenerateMode.ByEveryHour);//按每小时
ILogNet logNet = new LogNetDateTime(Application.StartupPath + "\\customer2", GenerateMode.ByEveryDay);//按每天
ILogNet logNet = new LogNetDateTime(Application.StartupPath + "\\customer2", GenerateMode.ByEveryMonth);//按每月
ILogNet logNet = new LogNetDateTime(Application.StartupPath + "\\customer2", GenerateMode.ByEverySeason);//按每季度
ILogNet logNet = new LogNetDateTime(Application.StartupPath + "\\customer2", GenerateMode.ByEveryYear);//按每年
</code>
</pre>

#### 单文件模式
单文件的模式在上述已经作为演示说明过了，但是单文件模式提供了两个额外的方法：
<ul>
<li>获取该文件日志中所有的内容</li>
<li>清空该文件的所有数据</li>
</ul>
具体的代码如下所示：
<pre>
<code>
LogNetSingle logNetSingle = logNet as LogNetSingle;
if (logNetSingle != null)
{
    string logData = logNetSingle.GetAllSavedLog();//获取所有的日志信息
    logNetSingle.ClearLog();//清除所有的日志信息
}
</code>
</pre>


## Log View
如果只提供了一个日志的写入而没有分析工具，那么本组件就是没什么竞争力的，本日志组件提供了一个大杀器，日志分析控件！您可以集成到您自己的系统中，该控件只需要接受一个 **日志源字符串** （就是日志文件的所有字符串数据，服务器读取文件发送给远程客户端就可以现实远程查看日志分析！棒不棒！）原先的 **C-S-** 项目中已经带有了2个日志查看器，一个服务器端使用了本组件提供的标准form窗口，客户端使用了日志分析控件实现远程查看功能，具体代码可以参照 **C-S-** 项目的代码。

#### 功能概述

<ul>
<li>对日志文件中的所有等级日志进行分析，每种等级多少个</li>
<li>可以同时根据日志等级和时间区段来筛选日志，比如查看某一时间段的 <strong>DEBUG</strong> 等级日志</li>
<li>可以进行可视化分析，查看日志数据的时间分布情况</li>
<li>在可视化的界面，如果某个区间段的某日数量特别高，鼠标移动上去后还可以自动跳转</li>
</ul>

#### 效果上图
查看日志信息

![](https://github.com/dathlin/C-S-/raw/master/img/log_3.jpg)

点击了分布视图，中间的细线是光标移动处

![](https://github.com/dathlin/C-S-/raw/master/img/log_4.jpg)

上图中点击了 **信息** 按钮后，显示的图形发生了变化，只显示了 **信息** 等级日志

![](https://github.com/dathlin/C-S-/raw/master/img/log_5.jpg)

光标移动到某一区间后，下方会有该区间的日志数量和时间范围，双击后的操作就您自己去发现了。^_^