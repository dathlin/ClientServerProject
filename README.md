# C-S架构的服务器客户端模版

#### 关于HslCommunication.dll
<p>该组件功能提供了一些基础类和整个C-S项目的核心网络的支持，除此之外，该组件提供了访问三菱PLC和西门子PLC的数据功能。</p>

<p>本组件支持常规的整数的数据读写，支持位数据读写，也支持字符串数据读写，包括中文，具体使用方式请参照下述手册，目前已经完成了一个三菱PLC高并发访问的类，具体交流可以通过以下方式联系我</p>
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

三菱详细手册：<a href="https://github.com/dathlin/C-S-/blob/master/MelsecReadMe.md">三菱PLC数据读写手册</a>

西门子详细手册：<a href="https://github.com/dathlin/C-S-/blob/master/SiemensReadMe.md">西门子PLC数据读写手册</a>

下载地址：<a href="https://github.com/dathlin/C-S-/raw/master/Public/HslCommunication.dll">单独的组件dll文件</a>

文档地址：<a href="https://github.com/dathlin/C-S-/raw/master/Public/HslCommunication.xml">单独的组件xml注释文件</a>

#### 关于本项目模版
<p style="text-indent:2em">本模版基于.Net Framework3.5+C#7.0开发完成，后来新增的wpf版本的客户端基于.Net 4.5实现，所以必须使用Visual studio 2017进行开发，低版本的IDE打开项目将出现语法识别错误。有必要说明下为什么使用.Net Framework3.5，这个版本是xp系统比较方便安装的，在企业部署中会相对容易很多，所以如果你的系统也是应用于企业的，那么强烈建议使用3.5版本，如果是电脑配置相对比较不错的，使用wpf版本可以实现更复杂的动画界面，该模版由三部分的程序组成：</p>
<ul>
<li>公共组件</li>
<li>服务器端</li>
<li>winform客户端+wpf客户端</li>
</ul>

<p style="text-indent:2em">组成部分主要是一个服务端运行的程序，一个客户端运行的程序（可以根据自身的需求选择winform还是wpf），还有一个公共的组件，以及一个json组件和一个网络组件，实现了基础的账户管理功能，版本控制，软件升级，公告管理，消息群发，性能监视，头像管理等等功能。具体的操作方法见演示就行。下面主要介绍下服务端的程序界面和客户端的程序设计模型。
</p>


#### 整个系统的架构设计如下
![](https://github.com/dathlin/C-S-/raw/master/img/Design1.png)  
<br />

#### 系统的登录设计
<ol>
<li>状态检查，检测服务器的维护状态设置，如果处于维护中，则显示不能登录系统原因。</li>
<li>账户检查，服务器对登录账户全面检查，用户名是否存在，密码是否正确，是否允许登录，并对登录ip，时间，频次进行记录。</li>
<li>版本检查，服务器返回最新的版本号，客户端检测后根据自己的需求选择是否启动更新程序。</li>
<li>参数下载，上述所有检查通过以后，进行运行数据的初始化，比如将公告数据传送到客户端，您也可以添加自己的数据。采用json数据进行封装，客户端解析的时候请参照示例。</li>
<li>上述所有检测通过之后，启动客户端的主界面程序。但凡有一项检测失败，或者参数下载失败，均不允许登录，并且提示相关错误。</li>
</ol>

![](https://github.com/dathlin/C-S-/raw/master/img/Design2.png)  

#### 系统的异常处理模型设计
![](https://github.com/dathlin/C-S-/raw/master/img/Design3.png)  

#### 系统的账户头像设计
![](https://github.com/dathlin/C-S-/raw/master/img/Design6.png) 

#### 系统的其他工具设计
![](https://github.com/dathlin/C-S-/raw/master/img/Design4.png)  

#### 一个基于此模版的示例项目设计模型
![](https://github.com/dathlin/C-S-/raw/master/img/Design5.png)  


# 服务器端程序界面如下：

![](https://github.com/dathlin/C-S-/raw/master/软件系统服务端模版/screenshots/server.png)  

###### 下述服务器端的功能说明均来自服务器的菜单点击

1. 服务器端的版本控制，更新新的版本号，按照实际需求来更新您的版本号，门牌为【设置】-【版本控制】

![](https://github.com/dathlin/C-S-/raw/master/软件系统服务端模版/screenshots/server1.png) 

2. 服务器端的维护状态控制，比如系统维护阶段，不允许所有客户端登录，门牌为【设置】-【维护切换】

![](https://github.com/dathlin/C-S-/raw/master/软件系统服务端模版/screenshots/server2.png) 

3. 消息群发，您也可以在代码中自动触发群发，代码参考此处的手动群发，门牌为【设置】-【消息发送】

![](https://github.com/dathlin/C-S-/raw/master/软件系统服务端模版/screenshots/server3.png) 

4. 账户管理，客户端的界面和这个一致，该管理属于底层的json数据管理，任意更改数据，门牌为【设置】-【账户管理】

![](https://github.com/dathlin/C-S-/raw/master/软件系统服务端模版/screenshots/server4.png)

5. 关于本系统，框架版本号以本github发布的版本号为准，门牌为【关于】-【关于软件】

![](https://github.com/dathlin/C-S-/raw/master/软件系统服务端模版/screenshots/server5.png)

6. 一键断开，如遇到紧急情况，或是切换维护之前，可以选择强制关闭所有的客户端。门牌为【设置】-【一键断开】

<br />
<br />
<br />

# 客户端的程序界面

###### 登录窗口

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client1.png)  
<br />

###### 登录主界面（此处点击了关于菜单）

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client2.png)  

###### 更改公告，此处没有设置权限过滤，门牌为【管理员】-【更改公告】

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client3.png) 

###### 日志查看，本系统集成了非常实用的日志功能，所有的网络组件均支持日志的记录，方便调试。门牌为【管理员】-【日志查看】

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client4.png) 

###### 远程更新，成功部署本系统后，支持远程客户端的版本更新，此功能应开发人员拥有。门牌为【管理员】-【远程更新】

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client5.png) 

###### 密码更改，当账户需要更改密码时，需要对密码进行验证。门牌为【设置】-【修改密码】

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client6.png) 

###### 更新日志，当客户端更新了新的版本后，初次运行程序时就会自动弹出如下窗口，具体的更新内容应该写入到文件中。手动门牌为【关于】-【更新日志】

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client7.png) 

###### 反馈意见，人性化的软件允许用户支持提交修改建议，功能使用反馈等。门牌为【关于】-【意见反馈】

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client8.png) 

###### 快速注册账号，支持管理员快速注册账号，该界面允许更改。门牌为【管理员】-【注册账号】

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client9.png) 

###### 共享文件，本系统支持一个小型的文件共享，包含了上传下载删除过滤。门牌为主界面的【文件数量】

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client10.png) 
![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client12.png) 

###### 本系统集成了一个小型的简单群聊天系统，缓存消息200条。门牌为主界面的【设置】-【留言板】

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client13.png)

###### 监视服务器的对象内存消耗，门牌为【管理员】-【开发中心】

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client14.png)

###### 修改账户的头像，门牌为【设备】-【更换头像】

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端模版/screenshots/client15.png)

<br />


# Wpf版客户端的程序界面

###### 登录窗口

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端Wpf/screenshots/client1.png)  

###### 主窗口，还未实现文件功能

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端Wpf/screenshots/client2.png) 

###### 主窗口的暗色主题

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端Wpf/screenshots/client3.png) 

###### 主题选择界面

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端Wpf/screenshots/client4.png)


###### 共享文件界面

![](https://github.com/dathlin/C-S-/raw/master/软件系统客户端Wpf/screenshots/client5.png)

###### 其他功能界面使用了winform的窗口技术，此处不在赘述。


<br />

# Web版客户端的程序界面

###### 登录界面，背景图片可以自定义

![](https://github.com/dathlin/C-S-/raw/master/软件系统浏览器模版/screenshots/browser1.png) 

###### 主界面，很多功能还在完善中

![](https://github.com/dathlin/C-S-/raw/master/软件系统浏览器模版/screenshots/browser2.png) 


<br />


<br />



# License:
###### Copyright (c) Richard.Hu. All rights reserved.
###### Licensed under the MIT License.
###### WeChat:工业智能软件开发项目组
###### Wpf模版使用了一个开源项目，<a href="https://github.com/ButchersBoy/MaterialDesignInXamlToolkit">https://github.com/ButchersBoy/MaterialDesignInXamlToolkit</a>
