package com.example.HslCommunication.Core.Net;

import android.util.Log;

import com.example.HslCommunication.Core.Types.OperateResult;
import com.example.HslCommunication.Core.Utilities.boolWith2Bytes;
import com.example.HslCommunication.Core.Utilities.boolWithBytes;
import com.example.HslCommunication.Core.Utilities.boolWithIntByte;
import com.example.HslCommunication.Core.Utilities.boolWithIntString;
import com.example.HslCommunication.Core.Utilities.Utilities;
import com.example.HslCommunication.Log.LogUtil;
import com.example.HslCommunication.Resources.StringResources;


import java.net.Socket;

/**
 * Created by DATHLIN on 2017/11/2.
 */

public abstract class NetShareBase extends NetBase {


    /// <summary>
    /// [自校验] 发送字节数据并确认对方接收完成数据，如果结果异常，则结束通讯
    /// </summary>
    /// <param name="socket">网络套接字</param>
    /// <param name="headcode">头指令</param>
    /// <param name="customer">用户指令</param>
    /// <param name="send">发送的数据</param>
    /// <param name="result">用于返回的结果</param>
    /// <param name="sendReport">发送的进度报告</param>
    /// <param name="failedString">失败时存储的额外描述信息</param>
    /// <returns></returns>
    protected boolean SendBaseAndCheckReceive(
            Socket socket,
            int headcode,
            int customer,
            byte[] send,
            OperateResult result,
            ProgressReport sendReport,
            String failedString
    )
    {
        // 数据处理
        send = NetSupport.CommandBytes(headcode, customer, KeyToken, send);

        Log.i(Utilities.bytes2HexString(send), "SendBaseAndCheckReceive: ");

        // 发送数据
        if (!SendBytesToSocket(
                socket,                                                // 套接字
                send,                                                  // 发送的字节数据
                result,                                                // 结果信息对象
                failedString                                           // 异常附加对象
        ))
        {
            send = null;
            return false;
        }

        // 确认对方是否接收完成
        int remoteReceive = send.length - HslCommunicationCode.HeadByteLength;
        Log.i("等待接收数据", "SendBaseAndCheckReceive: ");

        if (!CheckRomoteReceived(
                socket,                                                // 套接字
                remoteReceive,                                         // 对方需要接收的长度
                sendReport,                                            // 发送进度报告
                result,                                                // 结果信息对象
                failedString                                           // 异常附加信息
        ))
        {
            send = null;
            return false;
        }

        Log.i("对方接收成功", "SendBaseAndCheckReceive: ");
        // 对方接收成功
        send = null;
        return true;
    }

    /// <summary>
    /// [自校验] 发送字节数据并确认对方接收完成数据，如果结果异常，则结束通讯
    /// </summary>
    /// <param name="socket">网络套接字</param>
    /// <param name="customer">用户指令</param>
    /// <param name="send">发送的数据</param>
    /// <param name="result">用于返回的结果</param>
    /// <param name="sendReport">发送的进度报告</param>
    /// <param name="failedString">异常时记录到日志的附加信息</param>
    /// <returns></returns>
    protected boolean SendBytesAndCheckReceive(
            Socket socket,
            int customer,
            byte[] send,
            OperateResult result,
            ProgressReport sendReport,
            String failedString
    )
    {
        if (SendBaseAndCheckReceive(
                socket,                                           // 套接字
                HslCommunicationCode.Hsl_Protocol_User_Bytes,     // 指示字节数组
                customer,                                         // 用户数据
                send,                                             // 发送数据，该数据还要经过处理
                result,                                           // 结果消息对象
                sendReport,                                       // 发送的进度报告
                failedString                                      // 错误的额外描述
        ))
        {
            return true;
        }
        else
        {
            LogUtil.LogE("SendBytesAndCheckReceive",failedString);
            return false;
        }
    }

    /// <summary>
    /// [自校验] 直接发送字符串数据并确认对方接收完成数据，如果结果异常，则结束通讯
    /// </summary>
    /// <param name="socket">网络套接字</param>
    /// <param name="customer">用户指令</param>
    /// <param name="send">发送的数据</param>
    /// <param name="result">用于返回的结果</param>
    /// <param name="sendReport">发送的进度报告</param>
    /// <param name="failedString"></param>
    /// <returns></returns>
    protected boolean SendStringAndCheckReceive(
            Socket socket,
            int customer,
            String send,
            OperateResult result,
            ProgressReport sendReport,
            String failedString
    )
    {
        byte[] data =null;

        if(send == null ||send.length() <= 0)
        {

        }
        else
        {
            data = send.getBytes();
        }

        if (!SendBaseAndCheckReceive(
                socket,                                           // 套接字
                HslCommunicationCode.Hsl_Protocol_User_String,    // 指示字符串数据
                customer,                                         // 用户数据
                data,                                             // 字符串的数据
                result,                                           // 结果消息对象
                sendReport,                                       // 发送的进度报告
                failedString                                      // 错误的额外描述
        ))
        {
            return false;
        }
        return true;
    }

/*
    /// <summary>
    /// [自校验] 将文件数据发送至套接字，具体发送细节将在继承类中实现，如果结果异常，则结束通讯
    /// </summary>
    /// <param name="socket">套接字</param>
    /// <param name="filename">文件名称，文件必须存在</param>
    /// <param name="servername">远程端的文件名称</param>
    /// <param name="filetag">文件的额外标签</param>
    /// <param name="fileupload">文件的上传人</param>
    /// <param name="result">操作结果对象</param>
    /// <param name="sendReport">发送进度报告</param>
    /// <param name="failedString"></param>
    /// <returns></returns>
    protected boolean SendFileAndCheckReceive(
            Socket socket,
            String filename,
            String servername,
            String filetag,
            String fileupload,
            OperateResult result,
            ProgressReport sendReport,
            String failedString
    )
    {
        // 发送文件名，大小，标签
        File info = new File(filename);

        //FileInfo info = new FileInfo(filename);

        if (!info.exists())
        {
            // 如果文件不存在
            if (!SendStringAndCheckReceive(socket, 0, "", result, null, failedString)) return false;
            else
            {
                result.Message = "找不到该文件，请重新确认文件！";
                CloseSocket(socket);
                return false;
            }
        }

        // 文件存在的情况
        Newtonsoft.Json.Linq.JObject json = new Newtonsoft.Json.Linq.JObject
        {
            { "FileName", new Newtonsoft.Json.Linq.JValue(servername) },
            { "FileSize", new Newtonsoft.Json.Linq.JValue(info.Length) },
            { "FileTag", new Newtonsoft.Json.Linq.JValue(filetag) },
            { "FileUpload", new Newtonsoft.Json.Linq.JValue(fileupload) }
        };

        if (!SendStringAndCheckReceive(socket, 1, json.ToString(), result, null, failedString)) return false;


        if (!SendFileStreamToSocket(socket, filename, info.Length, result, sendReport, failedString))
        {
            return false;
        }

        // 检查接收
        // if (!CheckRomoteReceived(socket, info.Length, sendReport, result, failedString))
        // {
        //    return false;
        // }

        return true;
    }

    /// <summary>
    /// [自校验] 将流数据发送至套接字，具体发送细节将在继承类中实现，如果结果异常，则结束通讯
    /// </summary>
    /// <param name="socket">套接字</param>
    /// <param name="stream">文件名称，文件必须存在</param>
    /// <param name="servername">远程端的文件名称</param>
    /// <param name="filetag">文件的额外标签</param>
    /// <param name="fileupload">文件的上传人</param>
    /// <param name="result">操作结果对象</param>
    /// <param name="sendReport">发送进度报告</param>
    /// <param name="failedString"></param>
    /// <returns></returns>
    protected boolean SendFileAndCheckReceive(
            Socket socket,
            Stream stream,
            String servername,
            String filetag,
            String fileupload,
            OperateResult result,
            ProgressReport sendReport,
            String failedString
    )
    {
        // 文件存在的情况
        Newtonsoft.Json.Linq.JObject json = new Newtonsoft.Json.Linq.JObject
        {
            { "FileName", new Newtonsoft.Json.Linq.JValue(servername) },
            { "FileSize", new Newtonsoft.Json.Linq.JValue(stream.Length) },
            { "FileTag", new Newtonsoft.Json.Linq.JValue(filetag) },
            { "FileUpload", new Newtonsoft.Json.Linq.JValue(fileupload) }
        };

        if (!SendStringAndCheckReceive(socket, 1, json.ToString(), result, null, failedString)) return false;


        try
        {
            NetSupport.WriteSocketFromStream(socket, stream, stream.Length, sendReport, true);
        }
        catch(Exception ex)
        {
            CloseSocket(socket);
            if(LogNet!=null) LogNet.WriteException(failedString, ex);
            result.Message = CombineExceptionString(failedString, ex.getLocalizedMessage());
            return false;
        }

        // 检查接收
        // if (!CheckRomoteReceived(socket, info.Length, sendReport, result, failedString))
        // {
        //    return false;
        // }

        return true;
    }*/




    /// <summary>
    /// [自校验] 接收一条完整的同步数据，包含头子节和内容字节，基础的数据，如果结果异常，则结束通讯
    /// </summary>
    /// <param name="socket">套接字</param>
    /// <param name="head">头子节</param>
    /// <param name="content">内容字节</param>
    /// <param name="result">结果</param>
    /// <param name="receiveReport">接收进度反馈</param>
    /// <param name="failedString">失败时用于显示的字符串</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">result</exception>
    protected boolWith2Bytes ReceiveAndCheckBytes(
            Socket socket,
            OperateResult result,
            ProgressReport receiveReport,
            String failedString
    )
    {
        boolWith2Bytes value2=new boolWith2Bytes();

        // 30秒超时接收验证
//        HslTimeOut hslTimeOut = new HslTimeOut()
//        {
//            DelayTime = 30000,
//            IsSuccessful = false,
//            StartTime = DateTime.Now,
//            WorkSocket = socket,
//        };
//
//        ThreadPool.QueueUserWorkItem(new WaitCallback(NetSupport.ThreadPoolCheckTimeOut), hslTimeOut);

        Log.i("准备接收头指令", "ReceiveAndCheckBytes: ");

        boolWithBytes value = ReceiveBytesFromSocket(
                socket,                                             // 套接字
                HslCommunicationCode.HeadByteLength,                // 头指令长度
                result,                                             // 结果消息对象
                null,                                 // 不报告进度
                false,                             // 报告是否按照百分比报告
                false,                                    // 不回发接收长度
                true,                               // 检查是否超时
                failedString                                       // 异常时的附加文本描述
        );

        if(!value.Result)
        {
            return value2;
        }

        value2.Content = value.Content;

        Log.i("准备检查令牌", "ReceiveAndCheckBytes: ");

        // 检查令牌
        if (!CheckTokenPermission(socket, value.Content, KeyToken, result))
        {
            Log.i("令牌检查失败", "ReceiveAndCheckBytes: ");
            result.Message = StringResources.TokenCheckFailed;
            return value2;
        }

        // 接收内容
        byte[] buffer = new byte[4];
        buffer[0]=value.Content[28];
        buffer[1]=value.Content[29];
        buffer[2]=value.Content[30];
        buffer[3]=value.Content[31];
        int contentLength = Utilities.bytes2Int(buffer);

        Log.i("准备接收内容，长度为："+contentLength, "ReceiveAndCheckBytes: ");

        value =ReceiveBytesFromSocket(
                socket,                                     // 套接字
                contentLength,                              // 内容数据长度
                result,                                     // 结果消息对象
                receiveReport,                              // 接收进度报告委托
                true,                                       // 按照百分比进行报告数据
                true,                                       // 回发已经接收的数据长度
                false,                                      // 不进行超时检查
                failedString                                // 异常时附加的文本描述
        );

        if(!value.Result)
        {
            return value2;
        }
        Log.i("内容接收成功", "ReceiveAndCheckBytes: ");

        value2.Content2 = NetSupport.CommandAnalysis(value2.Content, value.Content);
        value2.Result=true;
        return value2;
    }



    /// <summary>
    /// [自校验] 从网络中接收一个字符串数据，如果结果异常，则结束通讯
    /// </summary>
    /// <param name="socket">套接字</param>
    /// <param name="customer">接收的用户数据</param>
    /// <param name="receive">接收的字节数据</param>
    /// <param name="result">结果信息对象</param>
    /// <param name="receiveReport">接收数据时的进度报告</param>
    /// <param name="failedString">失败时记录日志的字符串</param>
    /// <returns></returns>
    protected boolWithIntString ReceiveStringFromSocket(
            Socket socket,
            OperateResult result,
            ProgressReport receiveReport,
            String failedString
    )
    {
        boolWithIntString valueString = new boolWithIntString();

        boolWith2Bytes value2Bytes = ReceiveAndCheckBytes(socket,result, receiveReport, failedString);

        if (!value2Bytes.Result)
        {
            return valueString;
        }

        // check

        byte[] buffer = new byte[4];
        buffer[0]=value2Bytes.Content[0];
        buffer[1]=value2Bytes.Content[1];
        buffer[2]=value2Bytes.Content[2];
        buffer[3]=value2Bytes.Content[3];
        if (Utilities.bytes2Int(buffer) != HslCommunicationCode.Hsl_Protocol_User_String)
        {
            result.Message = "数据头校验失败！";
            LogUtil.LogE("ReceiveStringFromSocket","数据头校验失败！");
            CloseSocket(socket);
            return valueString;
        }

        buffer[0]=value2Bytes.Content[4];
        buffer[1]=value2Bytes.Content[5];
        buffer[2]=value2Bytes.Content[6];
        buffer[3]=value2Bytes.Content[7];
        // 分析数据
        valueString.DataInt = Utilities.bytes2Int(buffer);
        valueString.DataString = Utilities.byte2String(value2Bytes.Content2);
        valueString.Result = true;
        return valueString;
    }

    /// <summary>
    /// [自校验] 从网络中接收一串字节数据，如果结果异常，则结束通讯
    /// </summary>
    /// <param name="socket">套接字</param>
    /// <param name="customer">接收的用户数据</param>
    /// <param name="data">接收的字节数据</param>
    /// <param name="result">结果信息对象</param>
    /// <param name="failedString">失败时记录日志的字符串</param>
    /// <returns></returns>
    protected boolWithIntByte ReceiveContentFromSocket(
            Socket socket,
            OperateResult result,
            ProgressReport receiveReport,
            String failedString
    )
    {

        boolWithIntByte value = new boolWithIntByte();

        boolWith2Bytes value2Bytes = ReceiveAndCheckBytes(socket,result,null, failedString);

        if (!value2Bytes.Result)
        {
            return value;
        }

        byte[] buffer = new byte[4];
        buffer[0]=value2Bytes.Content[0];
        buffer[1]=value2Bytes.Content[1];
        buffer[2]=value2Bytes.Content[2];
        buffer[3]=value2Bytes.Content[3];
        if (Utilities.bytes2Int(buffer) != HslCommunicationCode.Hsl_Protocol_User_String)
        {
            result.Message = "数据头校验失败！";
            LogUtil.LogE("ReceiveContentFromSocket","数据头校验失败！");
            CloseSocket(socket);
            return value;
        }

        buffer[0]=value2Bytes.Content[4];
        buffer[1]=value2Bytes.Content[5];
        buffer[2]=value2Bytes.Content[6];
        buffer[3]=value2Bytes.Content[7];
        // 分析数据
        value.DataInt = Utilities.bytes2Int(buffer);
        value.Content = value2Bytes.Content2;
        value.Result = true;
        return value;
    }

/*

    /// <summary>
    /// [自校验] 从套接字中接收文件头信息
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="filename"></param>
    /// <param name="size"></param>
    /// <param name="filetag"></param>
    /// <param name="fileupload"></param>
    /// <param name="result"></param>
    /// <param name="failedString"></param>
    /// <returns></returns>
    protected bool ReceiveFileHeadFromSocket(
            Socket socket,
            out string filename,
            out long size,
            out string filetag,
            out string fileupload,
            OperateResult result,
            string failedString = null
    )
    {
        // 先接收文件头信息
        if (!ReceiveStringFromSocket(socket, out int customer, out string filehead, result, null, failedString))
        {
            filename = null;
            size = 0;
            filetag = null;
            fileupload = null;
            return false;
        }

        // 判断文件是否存在
        if (customer == 0)
        {
            LogNet?.WriteWarn("对方文件不存在，无法接收！");
            result.Message = StringResources.FileNotExist;
            filename = null;
            size = 0;
            filetag = null;
            fileupload = null;
            socket?.Close();
            return false;
        }

        // 提取信息
        Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(filehead);
        filename = SoftBasic.GetValueFromJsonObject(json, "FileName", "");
        size = SoftBasic.GetValueFromJsonObject(json, "FileSize", 0L);
        filetag = SoftBasic.GetValueFromJsonObject(json, "FileTag", "");
        fileupload = SoftBasic.GetValueFromJsonObject(json, "FileUpload", "");

        return true;
    }


    /// <summary>
    /// [自校验] 从网络中接收一个文件，如果结果异常，则结束通讯
    /// </summary>
    /// <param name="socket">网络套接字</param>
    /// <param name="savename">接收文件后保存的文件名</param>
    /// <param name="filename">文件在对方电脑上的文件名</param>
    /// <param name="size">文件大小</param>
    /// <param name="filetag">文件的标识</param>
    /// <param name="fileupload">文件的上传人</param>
    /// <param name="result">结果信息对象</param>
    /// <param name="receiveReport">接收进度报告</param>
    /// <param name="failedString">失败时的记录日志字符串</param>
    /// <returns></returns>
    protected bool ReceiveFileFromSocket(
            Socket socket,
            string savename,
            out string filename,
            out long size,
            out string filetag,
            out string fileupload,
            OperateResult result,
            Action<long, long> receiveReport,
            string failedString = null
    )
    {
        // 先接收文件头信息
        if (!ReceiveFileHeadFromSocket(
                socket,
                out filename,
                out size,
                out filetag,
                out fileupload,
                result,
                failedString
        ))
        {
            return false;
        }

        //// 先接收文件头信息
        //if (!ReceiveStringFromSocket(socket, out int customer, out string filehead, result, null, failedString))
        //{
        //    filename = null;
        //    size = 0;
        //    filetag = null;
        //    fileupload = null;
        //    return false;
        //}

        //// 判断文件是否存在
        //if (customer == 0)
        //{
        //    LogNet?.WriteWarn("对方文件不存在，无法接收！");
        //    result.Message = StringResources.FileNotExist;
        //    filename = null;
        //    size = 0;
        //    filetag = null;
        //    fileupload = null;
        //    socket?.Close();
        //    return false;
        //}

        //// 提取信息
        //Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(filehead);
        //filename = SoftBasic.GetValueFromJsonObject(json, "FileName", "");
        //size = SoftBasic.GetValueFromJsonObject(json, "FileSize", 0L);
        //filetag = SoftBasic.GetValueFromJsonObject(json, "FileTag", "");
        //fileupload = SoftBasic.GetValueFromJsonObject(json, "FileUpload", "");

        // 接收文件消息
        if (!ReceiveFileSteamFromSocket(socket, savename, size, receiveReport, result, failedString))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// [自校验] 从网络中接收一个文件，写入数据流，如果结果异常，则结束通讯
    /// </summary>
    /// <param name="socket">网络套接字</param>
    /// <param name="stream">等待写入的数据流</param>
    /// <param name="filename">文件在对方电脑上的文件名</param>
    /// <param name="size">文件大小</param>
    /// <param name="filetag">文件的标识</param>
    /// <param name="fileupload">文件的上传人</param>
    /// <param name="result">结果信息对象</param>
    /// <param name="receiveReport">接收进度报告</param>
    /// <param name="failedString">失败时的记录日志字符串</param>
    /// <returns></returns>
    protected bool ReceiveFileFromSocket(
            Socket socket,
            Stream stream,
            out string filename,
            out long size,
            out string filetag,
            out string fileupload,
            OperateResult result,
            Action<long, long> receiveReport,
            string failedString = null
    )
    {
        // 先接收文件头信息
        if (!ReceiveFileHeadFromSocket(
                socket,
                out filename,
                out size,
                out filetag,
                out fileupload,
                result,
                failedString
        ))
        {
            return false;
        }


        try
        {
            NetSupport.WriteStreamFromSocket(socket, stream, size, receiveReport, true);
            return true;
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            socket?.Close();
            return false;
        }
    }
*/



}
