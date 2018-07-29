# C-S架构的服务器客户端模版

[![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2017-red.svg)](https://www.visualstudio.com/zh-hans/) [![Language](https://img.shields.io/badge/Language-C%23%207.0-orange.svg)](https://blogs.msdn.microsoft.com/dotnet/2016/08/24/whats-new-in-csharp-7-0/) [![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/dathlin/ClientServerProject/blob/master/LICENSE)
[![Android Studio](https://img.shields.io/badge/Android%20Studio-3.1-green.svg)](https://www.visualstudio.com/zh-hans/)

## Summary
一个基于中小型提炼的C-S开发框架，覆盖电脑端，web端，手机端的全平台系统模版，在大多数的一对多的系统设计中会包含一些常用的重复的功能代码，
比如网络通信机制，客户端版本控制，账户控制管理，密码修改，公告管理，服务器配置，各种常用窗口等等，而且大多数的中小型系统只是需要到简单的权限管理即可。

<br />
本框架包含了四种客户端的模式，第一种常用的winform客户端，第二种为wpf客户端，第三种为asp.net mvc模式，
第四种为安卓平台的客户端，也就是说你可以在winform和wpf客户端上选择一种模式，然后您的系统提供一些功能（诸如报表查看）到asp.net上去，
然后提供一个手机端使用的Android App，如果服务器假设在云端，所有的人都可以随时随地的进行交互，数据流通，所有的账户模型都是统一的，
浏览器还方便一些只需要查看报表用户的人员不需要在部署客户端了。当然，客户端可以提供更加强大的功能。


A CS development framework based on small-to-medium sized refining, covering computer-side, web-side, and mobile-side full-platform system templates,
will include some commonly used repetitive function codes, such as network communication mechanisms, 
in most one-to-many system designs. Client version control, account control management, password modification, 
announcement management, server configuration, various common windows, etc., and most small and medium-sized systems 
just need simple rights management.


 This framework contains four client-side modes, the first commonly used winform client, the second is the wpf client, 
the third is the asp.net mvc mode, and the fourth is the client of the Android platform. You can choose a mode on winform and wpf clients, 
then your system provides some functions (such as report viewing) to asp.net, and then provide an Android app for mobile phone use, 
if the server is assumed in the cloud, all People can interact with anytime, anywhere, data flow, all account models are unified, 
the browser is also convenient for some people who only need to view the report users do not need to deploy the client. Of course, 
the client can provide more powerful features.


## Features included
<ul>
<li>一个简单的账户管理功能，包含了账户注册，密码修改，客户端登录账户记录,账户注销,账户包含了一些基础信息</li>
<li>一个简单的客户端登录控制功能，可以手动控制允许哪些客户端进行登录，只需要打开窗口配置一下即可</li>
<li>一个简单的公告管理功能，允许有权限的账户针对公告进行更改，未来将支持公告更改记录</li>
<li>一个简单的意见反馈功能，允许客户端反馈软件的意见或是BUG，方便开发人员更改</li>
<li>一个简单的右下角消息框弹出功能，在公告更改和新发消息的时候可以自由控制</li>
<li>一个简单的版本日志提示窗口，在新版本更新后就会自动提示显示</li>
<li>一个简单的角色管理器功能，对每个角色可以配置任意数量的账户名</li>
</ul>
<ul>
<li>服务器端的配置实时保存，以防止服务器电脑突然关机，断电造成的数据丢失问题</li>
<li>一个完善的网络通信框架，包含一对多控制的tcp网络（服务器对客户端进行控制，并方便的群发数据），单独请求数据的同步网络，udp网络</li>
<li>完善的自动升级的部署机制，服务器部署新版本后，所有客户端都将一键自动更新</li>
<li>客户端提供开发人员远程更新服务器程序的能力，方便开发人员的操作</li>
<li>完善的日志记录功能，所有的网络功能和文件功能都提供了日志记录，所有客户端的异常都会发送至服务器记录，客户端也可以非常方便的查看所有的日志信息，您也可以很方便的将其他信息记录到日志中</li>
<li>一个简单的局域网聊天功能，用于所有的在线账户进行聊天的功能，所有的消息做了一定的缓存</li>
<li>提供了一个文件共享平台，大多数的软件系统都会共享一些特殊的文件资料，允许方便的下载，管理，上传</li>
<li>提供所有账户自身的头像功能，未来还将支持多账户同步</li>
<li>提供一个简单的开发中心，允许客户端实时监视服务器程序的对象内存使用情况</li>
<li>客户端提供了一个统一的配置中心，可以用来配置服务器各种参数。</li>
</ul>
<ul>
<li>Wpf版本的客户端程序额外提供了一个主题颜色设置的功能</li>
</ul>

## Environment

<ul>
<li>IDE: Visual Studio 2017
<ul>

<li>winform 服务器：.NET Framework 3.5</li>
<li>winform 客户端：.NET Framework 3.5</li>
<li>wpf客户端： .NET Framework 4.5</li>
<li>asp.net mvc服务器：.NET Framework 4.5</li>
</ul>
</li>
</ul>

## Getting Started
<ul>
<li>重新生成 <b>CommonLibrary</b> 项目</li>
<li>请确认 <b>ClientsLibrary</b> 项目文件 <b>UserClient.cs</b> 的类 <b>UserClient</b> 的属性 <b>ServerIp</b> 是否为127.0.0.1，如果不是，请修改</li>
<li>重新生成 <b>ClientsLibrary</b> 项目</li>
<li>重新生成 <b>软件系统服务端模版</b> 并运行该exe</li>
<li>选择一个你想调试的客户端版本程序，例如winform就启动 <b>软件系统客户端模版</b> 项目</li>
<li>输入一个默认的账户admin,密码123456</li>
<li>接下来就可以体验所有的功能了</li>
</ul>
安卓端的程序在文件夹AndroidTemplate中，请使用Android Studio打开该文件夹，并修改连接的服务器地址。（该模版还在完善中...）

## Quick Experience
如果觉得下载源代码比较麻烦，又想快速体验客户端功能，那就点击<a href="https://github.com/dathlin/ClientServerProject/raw/master/QuickExperience/软件自动更新.exe">软件自动更新.exe</a>下载程序，放置到任意位置，推荐桌面，双击安装程序，输入默认的账户密码即可体验完整的最新版本的客户端，当服务器的客户端版本更新时，你再打开本程序时也会自动升级。然后桌面的 **软件自动更新.exe** 就可以删除了。此处目前仅仅体验wpf程序。

至于卸载软件，只要删除桌面的快捷方式和安装目录的文件即可，其他位置不会有任何的文件残留。

## Secondary Development
基于本模版可以方便的进行二次开发，具体可以开发示例如下（如下是我个人的实践经验，欢迎补充）：
<ul>
<li>基于现场监视控制的上位机系统，可方便实现一对多的同步监视</li>
<li>用于部门人员的项目管理系统</li>
<li>用于设备资料档案管理的设备管理系统</li>
<li>用于管理备品备件的ERP系统</li>
<li>多客户端之间需要进行复杂数据交互的系统</li>
</ul>

二次开发时需要特别注意的是在项目 **CommonLibrary** -> **UserSystem.cs** 中的参数需要根据实际全部修改，注意事项已在文件里说明。

## Contribute
如果你也喜欢这个项目，可以点击右上角的star或是fork，如果发现了一些BUG或是需要更改的地方也可以直接发起pull request，当然也可以联系技术支持QQ群来联系我本人，或是发送邮件，具体参考下面。

## Contact
<ul>
<li>技术支持QQ群：<strong>592132877</strong></li>
<li>邮箱：<strong>hsl200909@163.com</strong></li>
</ul>

## Disclaimer
<ul>
<li>使用了<a href="http://www.newtonsoft.com/json">json.net组件</a></li>
<li>Wpf模版使用了一个开源项目，<a href="https://github.com/ButchersBoy/MaterialDesignInXamlToolkit">https://github.com/ButchersBoy/MaterialDesignInXamlToolkit</a></li>
<li>文件共享功能的图标来源<a href="http://fileicons.chromefans.org/">免费文件图标</a></li>
</ul>


## HslCommunication.dll [![NuGet Status](https://img.shields.io/nuget/v/HslCommunication.svg)](https://www.nuget.org/packages/HslCommunication/) ![NuGet Download](https://img.shields.io/nuget/dt/HslCommunication.svg) 
<p>本C-S项目的核心组件，该组件功能提供了一些基础功能类和整个C-S项目的网络支持，除此之外，该组件提供了访问三菱PLC，西门子PLC，欧姆龙PLC，Modbus的数据功能。
关于这个库的项目介绍地址如下：</p>

[http://www.cnblogs.com/dathlin/p/7703805.html](http://www.cnblogs.com/dathlin/p/7703805.html)

在Nuget控制台输入下面的指令即可安装，或者使用VS2017的Nuget包管理器来方便的下载组件，如果不清楚怎么使用NuGet可以参考网上教程。

关于该库的急速体验Demo程序下载地址：

[HslCommunicationDemo.zip](https://github.com/dathlin/HslCommunication/raw/master/Download/HslCommunicationDemo.zip)

<pre>
<code>
Install-Package HslCommunication
</code>
</pre>


# 整个系统的架构设计如下

#### 核心架构的设计机制

![](https://github.com/dathlin/ClientServerProject/raw/master/img/Design1.png)  
<br />

#### 系统的登录设计
<ol>
<li>状态检查，检测服务器的维护状态设置，如果处于维护中，则显示不能登录系统原因。</li>
<li>账户检查，服务器对登录账户全面检查，用户名是否存在，密码是否正确，是否允许登录，并对登录ip，时间，频次进行记录。</li>
<li>版本检查，服务器返回最新的版本号，客户端检测后根据自己的需求选择是否启动更新程序。</li>
<li>参数下载，上述所有检查通过以后，进行运行数据的初始化，比如将公告数据传送到客户端，您也可以添加自己的数据。采用json数据进行封装，客户端解析的时候请参照示例。</li>
<li>上述所有检测通过之后，启动客户端的主界面程序。但凡有一项检测失败，或者参数下载失败，均不允许登录，并且提示相关错误。</li>
</ol>

![](https://github.com/dathlin/ClientServerProject/raw/master/img/Design2.png)  

#### 系统的权限角色模型设计
![](https://github.com/dathlin/ClientServerProject/raw/master/img/Design7.png) 

#### 系统的异常处理模型设计
![](https://github.com/dathlin/ClientServerProject/raw/master/img/Design3.png)  

#### 系统的账户头像设计
![](https://github.com/dathlin/ClientServerProject/raw/master/img/Design6.png) 

#### 系统的其他工具设计
![](https://github.com/dathlin/ClientServerProject/raw/master/img/Design4.png)  

#### 一个基于此模版的示例项目设计模型
![](https://github.com/dathlin/ClientServerProject/raw/master/img/Design5.png)  


# 服务器端程序界面如下：

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统服务端模版/screenshots/server.png)  

###### 下述服务器端的功能说明均来自服务器的菜单点击

1. 服务器端的版本控制，更新新的版本号，按照实际需求来更新您的版本号，门牌为【设置】-【版本控制】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统服务端模版/screenshots/server1.png) 

2. 服务器端的维护状态控制，比如系统维护阶段，不允许所有客户端登录，门牌为【设置】-【维护切换】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统服务端模版/screenshots/server2.png) 

3. 消息群发，您也可以在代码中自动触发群发，代码参考此处的手动群发，门牌为【设置】-【消息发送】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统服务端模版/screenshots/server3.png) 

4. 账户管理，客户端的界面和这个一致，该管理属于底层的json数据管理，任意更改数据，门牌为【设置】-【账户管理】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统服务端模版/screenshots/server4.png)

5. 关于本系统，框架版本号以本github发布的版本号为准，门牌为【关于】-【关于软件】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统服务端模版/screenshots/server5.png)

6. 一键断开，如遇到紧急情况，或是切换维护之前，可以选择强制关闭所有的客户端。门牌为【设置】-【一键断开】

<br />
<br />
<br />

# 客户端的程序界面

###### 登录窗口

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client1.png)  
<br />

###### 登录主界面（此处点击了关于菜单）

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client2.png)  

###### 更改公告，此处没有设置权限过滤，门牌为【管理员】-【更改公告】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client3.png) 

###### 日志查看，本系统集成了非常实用的日志功能，所有的网络组件均支持日志的记录，方便调试。门牌为【管理员】-【日志查看】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client4.png) 

###### 远程更新，成功部署本系统后，支持远程客户端的版本更新，此功能应开发人员拥有。门牌为【管理员】-【远程更新】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client5.png) 

###### 密码更改，当账户需要更改密码时，需要对密码进行验证。门牌为【设置】-【修改密码】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client6.png) 

###### 更新日志，当客户端更新了新的版本后，初次运行程序时就会自动弹出如下窗口，具体的更新内容应该写入到文件中。手动门牌为【关于】-【更新日志】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client7.png) 

###### 反馈意见，人性化的软件允许用户支持提交修改建议，功能使用反馈等。门牌为【关于】-【意见反馈】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client8.png) 

###### 快速注册账号，支持管理员快速注册账号，该界面允许更改。门牌为【管理员】-【注册账号】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client9.png) 

###### 共享文件，本系统支持一个小型的文件共享，包含了上传下载删除过滤。门牌为主界面的【文件数量】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client10.png) 
![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client12.png) 

###### 本系统集成了一个小型的简单群聊天系统，缓存消息200条。门牌为主界面的【设置】-【留言板】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client13.png)

###### 监视服务器的对象内存消耗，门牌为【管理员】-【开发中心】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client14.png)

###### 修改账户的头像，门牌为【设置】-【我的信息】-点击头像

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client15.png)

###### 我的账户信息及个人文件，门牌为【设置】-【我的信息】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client16.png)

###### 统一的系统配置界面，门牌为【管理员】-【系统配置】

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client17.png)

##### 中英文双语支持，目前仅先适配我的信息和系统配置界面

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端模版/screenshots/client18.png)

<br />


# Wpf版客户端的程序界面

###### 登录窗口

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端Wpf/screenshots/client1.png)  

###### 主窗口，还未实现文件功能

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端Wpf/screenshots/client2.png) 

###### 主窗口的暗色主题

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端Wpf/screenshots/client3.png) 

###### 主题选择界面

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端Wpf/screenshots/client4.png)


###### 共享文件界面

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统客户端Wpf/screenshots/client5.png)

###### 其他功能界面使用了winform的窗口技术，此处不在赘述。


<br />

# Web版客户端的程序界面

###### 登录界面，背景图片可以自定义

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统浏览器模版/screenshots/browser1.png) 

###### 主界面，很多功能还在完善中

![](https://github.com/dathlin/ClientServerProject/raw/master/软件系统浏览器模版/screenshots/browser2.png) 


<br />

# Android 客户端模版（还在完善中...）

![](https://github.com/dathlin/ClientServerProject/raw/master/img/andriod1.png) 

![](https://github.com/dathlin/ClientServerProject/raw/master/img/andriod2.png) 

<br />



# License:
##### Copyright (c) Richard.Hu. All rights reserved.
##### Licensed under the MIT License.
##### WeChat:工业智能软件开发项目组
