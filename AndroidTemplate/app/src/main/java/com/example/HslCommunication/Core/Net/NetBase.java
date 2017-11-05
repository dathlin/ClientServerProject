package com.example.HslCommunication.Core.Net;
import android.util.Log;

import com.example.HslCommunication.Core.Types.HslTimeOut;
import com.example.HslCommunication.Core.Types.OperateResult;
import com.example.HslCommunication.Core.Utilities.boolWithBytes;
import com.example.HslCommunication.Core.Utilities.boolWithSocket;
import com.example.HslCommunication.Log.LogUtil;
import com.example.HslCommunication.Resources.StringResources;


import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.DataOutputStream;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.UUID;


/**
 * Created by DATHLIN on 2017/11/1.
 * 该类提供基础的网络操作服务，读写字节流，读写文件流
 */



public abstract class NetBase {

    /// <summary>
    /// 用于通信工作的核心对象
    /// </summary>
    protected Socket WorkSocket = null;
    /// <summary>
    /// 分次接收的数据长度
    /// </summary>
    public int SegmentationLength = 1024;

    /// <summary>
    /// 检查超时的子线程
    /// </summary>
    /// <param name="obj"></param>
    public void ThreadPoolCheckConnect(Object obj) {
        HslTimeOut timeout = (HslTimeOut) obj;
        if (timeout != null) {
            NetSupport.ThreadPoolCheckConnect(timeout, ConnectTimeout);
        }
    }


    /// <summary>
    /// 网络访问中的超时时间，单位：毫秒，默认值5000
    /// </summary>
    public int ConnectTimeout = 5000;

    /// <summary>
    /// 当前对象的身份令牌，用来在网络通信中双向认证的依据
    /// </summary>
    public UUID KeyToken = UUID.randomUUID();


    /****************************************************************************
     *
     *    1. 创建并连接套接字
     *    2. 接收指定长度的字节数据
     *    3. 发送字节数据到套接字
     *    4. 检查对方是否接收完成
     *    5. 检查头子节令牌是否通过
     *    6. 将文件流写入套接字
     *    7. 从套接字接收文件流
     *
     ****************************************************************************/


    /// <summary>
    /// 创建socket对象并尝试连接终结点，如果异常，则结束通信
    /// </summary>
    /// <param name="socket">网络套接字</param>
    /// <param name="iPEndPoint">网络终结点</param>
    /// <param name="result">结果对象</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    protected boolWithSocket CreateSocketAndConnect(
            String ipAddress,
            int port,
            OperateResult result
    ) {
        boolWithSocket value = new boolWithSocket();
        try {
            // create the socket object
            value.Socket = new Socket(ipAddress, port);
            value.Result = true;
            return value;
        } catch (Exception ex) {
            result.Message = StringResources.ConnectedFailed;
            LogUtil.LogE("CreateSocketAndConnect", StringResources.ConnectedFailed, ex);
            CloseSocket(value.Socket);
            return value;
        }
    }


    /// <summary>
    /// 仅仅接收一定长度的字节数据，如果异常，则结束通信
    /// </summary>
    /// <param name="socket">套接字</param>
    /// <param name="bytes">字节数据</param>
    /// <param name="length">长度</param>
    /// <param name="result">结果对象</param>
    /// <param name="receiveStatus">接收状态</param>
    /// <param name="reportByPercent">是否根据百分比报告进度</param>
    /// <param name="response">是否回发进度</param>
    /// <param name="checkTimeOut">是否进行超时检查</param>
    /// <param name="exceptionMessage">假设发生异常，应该携带什么信息</param>
    /// <returns></returns>
    protected boolWithBytes ReceiveBytesFromSocket(
            Socket socket,
            int length,
            OperateResult result,
            ProgressReport receiveStatus,
            boolean reportByPercent,
            boolean response,
            boolean checkTimeOut,
            String exceptionMessage
    ) {
        boolWithBytes value = new boolWithBytes();

        try {
            value.Content = NetSupport.ReadBytesFromSocket(socket, length, receiveStatus, reportByPercent, response);
        } catch (Exception ex) {
            CloseSocket(socket);
            result.Message = CombineExceptionString(exceptionMessage, ex.getMessage());
            LogUtil.LogE("ReceiveBytesFromSocket", exceptionMessage, ex);
            Log.e("接收数据异常", "ReceiveBytesFromSocket: ", ex);
            return value;
        }
        value.Result = true;
        return value;
    }

    /// <summary>
    /// 仅仅将数据发送到socket对象上去，如果异常，则结束通信
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="send"></param>
    /// <param name="result"></param>
    /// <param name="exceptionMessage"></param>
    /// <returns></returns>
    protected boolean SendBytesToSocket(
            Socket socket,
            byte[] send,
            OperateResult result,
            String exceptionMessage
    ) {
        try {
            DataOutputStream output = new DataOutputStream(socket.getOutputStream());
            output.write(send);
        } catch (Exception ex) {
            result.Message = CombineExceptionString(exceptionMessage, ex.getLocalizedMessage());
            LogUtil.LogE("SendBytesToSocket", exceptionMessage, ex);
            CloseSocket(socket);
            send = null;
            return false;
        }
        return true;
    }


    /// <summary>
    /// 确认对方是否已经接收完成数据，如果异常，则结束通信
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="length"></param>
    /// <param name="report"></param>
    /// <param name="result"></param>
    /// <param name="exceptionMessage"></param>
    /// <returns></returns>
    protected boolean CheckRomoteReceived(
            Socket socket,
            long length,
            ProgressReport report,
            OperateResult result,
            String exceptionMessage
    ) {
        try {
            NetSupport.CheckSendBytesReceived(socket, length, report, true);
            return true;
        } catch (Exception ex) {
            result.Message = CombineExceptionString(exceptionMessage, ex.getLocalizedMessage());
            LogUtil.LogE("CheckRomoteReceived", exceptionMessage, ex);
            CloseSocket(socket);
            return false;
        }
    }


    /// <summary>
    /// 检查令牌是否正确，如果不正确，结束网络通信
    /// </summary>
    /// <param name="socket">套接字</param>
    /// <param name="head">头子令</param>
    /// <param name="token">令牌</param>
    /// <param name="result">结果对象</param>
    /// <returns></returns>
    protected boolean CheckTokenPermission(
            Socket socket,
            byte[] head,
            UUID token,
            OperateResult result
    ) {
        if (NetSupport.CheckTokenEquel(head, KeyToken)) {
            return true;
        } else {
            result.Message = StringResources.TokenCheckFailed;
            LogUtil.LogE("CheckTokenPermission", StringResources.TokenCheckFailed + " Ip:" + socket.getInetAddress().toString());
            CloseSocket(socket);
            return false;
        }
    }

    /// <summary>
    /// 将文件数据发送至套接字，如果结果异常，则结束通讯
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="filename"></param>
    /// <param name="filelength"></param>
    /// <param name="result"></param>
    /// <param name="report"></param>
    /// <param name="exceptionMessage"></param>
    /// <returns></returns>
    protected boolean SendFileStreamToSocket(
            Socket socket,
            String filename,
            long filelength,
            OperateResult result,
            ProgressReport report,
            String exceptionMessage
    ) {
        try {
            InputStream inputStream = new BufferedInputStream(new FileInputStream(filename));
            NetSupport.WriteSocketFromStream(socket, inputStream, filelength, report, true);
            inputStream.close();
            return true;
        } catch (Exception ex) {
            CloseSocket(socket);
            LogUtil.LogE("SendFileStreamToSocket", exceptionMessage, ex);
            result.Message = CombineExceptionString(exceptionMessage, ex.getLocalizedMessage());
            return false;
        }
    }


    /// <summary>
    /// 从套接字中接收一个文件数据，如果结果异常，则结束通讯
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="filename"></param>
    /// <param name="receive"></param>
    /// <param name="report"></param>
    /// <param name="result"></param>
    /// <param name="exceptionMessage"></param>
    /// <returns></returns>
    protected boolean ReceiveFileSteamFromSocket(
            Socket socket,
            String filename,
            long receive,
            ProgressReport report,
            OperateResult result,
            String exceptionMessage
    ) {
        try {
            OutputStream outputStream = new BufferedOutputStream(new FileOutputStream(filename));
            NetSupport.WriteStreamFromSocket(socket, outputStream, receive, report, true);
            outputStream.close();
            return true;
        } catch (Exception ex) {
            result.Message = CombineExceptionString(exceptionMessage, ex.getLocalizedMessage());
            LogUtil.LogE("ReceiveFileSteamFromSocket", exceptionMessage, ex);
            CloseSocket(socket);
            return false;
        }
    }

    /// <summary>
    /// 获取错误的用于显示的信息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    protected String CombineExceptionString(String message, String exception) {
        return message + "\r\n原因：" + exception;
    }



    protected void CloseSocket(Socket socket) {
        if (socket != null) {
            try {
                socket.close();
            } catch (Exception ex2) {
                LogUtil.LogE("CloseSocket","",ex2);
            }
        }
    }


    protected void ThreadSleep(int milltime)
    {
        try
        {
            Thread.sleep(milltime);
        }
        catch (Exception ex)
        {

        }
    }
}
