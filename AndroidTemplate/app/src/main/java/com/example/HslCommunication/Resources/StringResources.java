package com.example.HslCommunication.Resources;

/**
 * Created by DATHLIN on 2017/11/2.
 */

public class StringResources {


    /***********************************************************************************
            *
            *    一般的错误信息
         *
                 ************************************************************************************/


    public static final String ConnectedFailed = "连接失败";
    public static final String UnknownError = "未知错误";
    public static final String ErrorCode = "错误代号";
    public static final String TextDescription = "文本描述";
    public static final String ExceptionMessage = "错误信息：";
    public static final String ExceptionStackTrace = "错误堆栈：";
    public static final String ExceptopnTargetSite = "错误方法：";
    public static final String ExceprionCustomer = "用户自定义方法出错：";



    /***********************************************************************************
     *
     *    系统相关的错误信息
     *
     ************************************************************************************/

    public static final String SystemInstallOperater = "安装新系统：IP为";
    public static final String SystemUpdateOperater = "更新新系统：IP为";


    /***********************************************************************************
     *
     *    套接字相关的信息描述
     *
     ************************************************************************************/

    public static final String SocketIOException = "套接字传送数据异常：";
    public static final String SocketSendException = "同步数据发送异常：";
    public static final String SocketHeadReceiveException = "指令头接收异常：";
    public static final String SocketContentReceiveException = "内容数据接收异常：";
    public static final String SocketContentRemoteReceiveException = "对方内容数据接收异常：";
    public static final String SocketAcceptCallbackException = "异步接受传入的连接尝试";
    public static final String SocketReAcceptCallbackException = "重新异步接受传入的连接尝试";
    public static final String SocketSendAsyncException = "异步数据发送出错:";
    public static final String SocketEndSendException = "异步数据结束挂起发送出错";
    public static final String SocketReceiveException = "异步数据发送出错:";
    public static final String SocketEndReceiveException = "异步数据结束接收指令头出错";
    public static final String SocketRemoteCloseException = "远程主机强迫关闭了一个现有的连接";


    /***********************************************************************************
     *
     *    文件相关的信息
     *
     ************************************************************************************/


    public static final String FileDownloadSuccess = "文件下载成功";
    public static final String FileDownloadFailed = "文件下载异常";
    public static final String FileUploadFailed = "文件上传异常";
    public static final String FileUploadSuccess = "文件上传成功";
    public static final String FileDeleteFailed = "文件删除异常";
    public static final String FileDeleteSuccess = "文件删除成功";
    public static final String FileReceiveFailed = "确认文件接收异常";
    public static final String FileNotExist = "文件不存在";
    public static final String FileSaveFailed = "文件存储失败";
    public static final String FileLoadFailed = "文件加载失败";
    public static final String FileSendClientFailed = "文件发送的时候发生了异常";
    public static final String FileWriteToNetFailed = "文件写入网络异常";
    public static final String FileReadFromNetFailed = "从网络读取文件异常";

    /***********************************************************************************
     *
     *    服务器的引擎相关数据
     *
     ************************************************************************************/

    public static final String TokenCheckFailed = "接收验证令牌不一致";
    public static final String TokenCheckTimeout = "接收验证超时:";
    public static final String NetClientAliasFailed = "客户端的别名接收失败：";
    public static final String NetEngineStart = "启动引擎";
    public static final String NetEngineClose = "关闭引擎";
    public static final String NetClientOnline = "上线";
    public static final String NetClientOffline = "下线";
    public static final String NetClientBreak = "异常掉线";
    public static final String NetClientFull = "服务器承载上限，收到超出的请求连接。";
    public static final String NetClientLoginFailed = "客户端登录中错误：";
}
