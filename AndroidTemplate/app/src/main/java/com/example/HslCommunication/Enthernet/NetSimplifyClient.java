package com.example.HslCommunication.Enthernet;

import com.example.HslCommunication.Core.Net.HslCommunicationCode;
import com.example.HslCommunication.Core.Net.NetShareBase;
import com.example.HslCommunication.Core.Net.ProgressReport;
import com.example.HslCommunication.Core.Utilities.boolWith2Bytes;
import com.example.HslCommunication.Core.Utilities.Utilities;
import com.example.HslCommunication.Core.Types.NetHandle;
import com.example.HslCommunication.Core.Types.OperateResultBytes;
import com.example.HslCommunication.Core.Types.OperateResultString;
import com.example.HslCommunication.Core.Utilities.boolWithSocket;
import com.example.HslCommunication.Resources.StringResources;

import java.io.InputStream;
import java.net.Socket;
import java.util.UUID;

/**
 * Created by DATHLIN on 2017/11/2.
 */

public final class NetSimplifyClient extends NetShareBase {



    /// <summary>
    /// 实例化一个客户端的对象，用于和服务器通信
    /// </summary>
    public NetSimplifyClient(String ipAddress, int port, UUID token)
    {
        m_ipAddress = ipAddress;
        m_port = port;
        KeyToken = token;
    }

    private String m_ipAddress="127.0.0.1";
    private int m_port=10000;

    /// <summary>
    /// 客户端向服务器进行请求，请求字符串数据
    /// </summary>
    /// <param name="customer">用户的指令头</param>
    /// <param name="send">发送数据</param>
    /// <param name="sendStatus">发送数据时的进度报告</param>
    /// <param name="receiveStatus">接收数据时的进度报告</param>
    /// <returns></returns>
    public OperateResultString ReadFromServer(
            NetHandle customer,
            String send,
            ProgressReport sendStatus,
            ProgressReport receiveStatus
    )
    {
        OperateResultString result = new OperateResultString();
        byte[] data = Utilities.string2Byte(send);
        OperateResultBytes temp = ReadFromServerBase(HslCommunicationCode.Hsl_Protocol_User_String, customer.get_CodeValue(), data, sendStatus, receiveStatus);
        result.IsSuccess = temp.IsSuccess;
        result.ErrorCode = temp.ErrorCode;
        result.Message = temp.Message;
        if (temp.IsSuccess)
        {
            result.Content = Utilities.byte2String(temp.Content);
        }
        temp = null;
        return result;
    }


    /// <summary>
    /// 客户端向服务器进行请求，请求字节数据
    /// </summary>
    /// <param name="customer">用户的指令头</param>
    /// <param name="send"></param>
    /// <param name="sendStatus">发送数据的进度报告</param>
    /// <param name="receiveStatus">接收数据的进度报告</param>
    /// <returns></returns>
    public OperateResultBytes ReadFromServer(
            NetHandle customer,
            byte[] send,
            ProgressReport sendStatus,
            ProgressReport receiveStatus
    )
    {
        return ReadFromServerBase(HslCommunicationCode.Hsl_Protocol_User_Bytes, customer.get_CodeValue(), send, sendStatus, receiveStatus);
    }

    /// <summary>
    /// 需要发送的底层数据
    /// </summary>
    /// <param name="headcode">数据的指令头</param>
    /// <param name="customer">用户的指令头</param>
    /// <param name="send">需要发送的底层数据</param>
    /// <param name="sendStatus">发送状态的进度报告，用于显示上传进度</param>
    /// <param name="receiveStatus">接收状态的进度报告，用于显示下载进度</param>
    /// <returns></returns>
    private OperateResultBytes ReadFromServerBase(
            int headcode,
            int customer,
            byte[] send,
            ProgressReport sendStatus,
            ProgressReport receiveStatus)
    {
        OperateResultBytes result = new OperateResultBytes();

        // 创建并连接套接字
        Socket socket=null;
        boolWithSocket valueSocket=CreateSocketAndConnect(m_ipAddress, m_port,result);
        if(!valueSocket.Result)
        {
            return result;
        }

        socket = valueSocket.Socket;

        // 发送并检查数据是否发送完成
        if (!SendBaseAndCheckReceive(socket, headcode, customer, send, result, sendStatus,null))
        {
            return result;
        }

        // 接收头数据和内容数据

        boolWith2Bytes value = ReceiveAndCheckBytes(socket, result, receiveStatus,null);

        if (!value.Result)
        {
            return result;
        }

        CloseSocket(socket);
        result.Content = value.Content2;
        result.IsSuccess = true;
        return result;
    }


}
