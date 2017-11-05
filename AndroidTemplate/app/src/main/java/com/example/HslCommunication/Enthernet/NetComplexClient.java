package com.example.HslCommunication.Enthernet;

import com.example.HslCommunication.Core.Net.HslCommunicationCode;
import com.example.HslCommunication.Core.Net.NetShareBase;
import com.example.HslCommunication.Core.Net.NetSupport;
import com.example.HslCommunication.Core.Types.NetHandle;
import com.example.HslCommunication.Core.Types.OperateResult;
import com.example.HslCommunication.Core.Utilities.Utilities;
import com.example.HslCommunication.Core.Utilities.boolWith2Bytes;
import com.example.HslCommunication.Core.Utilities.boolWithSocket;
import com.example.HslCommunication.Log.LogUtil;
import com.example.HslCommunication.Resources.StringResources;

import java.net.Socket;
import java.util.Date;

/**
 * Created by hsl20 on 2017/11/4.
 */

public class NetComplexClient extends NetShareBase {



    /// <summary>
    /// 客户端的核心连接块
    /// </summary>
    private AsyncStateOne stateone = new AsyncStateOne();
    /// <summary>
    /// 客户端系统是否启动
    /// </summary>
    public boolean Is_Client_Start = false;

    /// <summary>
    /// 重连接失败的次数
    /// </summary>
    public int Connect_Failed_Count  = 0;
    /// <summary>
    /// 指示客户端是否处于正在连接服务器中
    /// </summary>
    private boolean Is_Client_Connecting = false;
    /// <summary>
    /// 登录服务器的判断锁
    /// </summary>
    private Object lock_connecting = new Object();
    /// <summary>
    /// 客户端登录的标识名称，可以为ID号，也可以为登录名
    /// </summary>
    public String ClientAlias = "";
    /// <summary>
    /// 远程服务器的IP地址和端口
    /// </summary>
    public String ServerIp = "127.0.0.1";
    /*
        远程服务器的端口
     */
    public int ServerPort = 10000;

    /// <summary>
    /// 服务器的时间，自动实现和服务器同步
    /// </summary>
    private Date ServerTime  = new Date();
    /// <summary>
    /// 系统与服务器的延时时间，单位毫秒
    /// </summary>
    private int DelayTime =0;



    public void LoginSuccess()
    {

    }

    public  void LoginFailed(int times)
    {

    }

    public void MessageAlerts(String msg)
    {

    }

    public void BeforReConnected()
    {

    }

    public void AcceptString(NetHandle handle, String msg)
    {

    }

    public void AcceptByte( NetHandle handle, byte[] msg)
    {

    }


    private boolean IsQuie = false;

    /// <summary>
    /// 关闭该客户端引擎
    /// </summary>
    public void ClientClose() {
        IsQuie = true;
        if (Is_Client_Start)
            SendBytes(stateone, NetSupport.CommandBytes(HslCommunicationCode.Hsl_Protocol_Client_Quit, 0, KeyToken, null));

        if (thread_heart_check != null) thread_heart_check.interrupt();
        Is_Client_Start = false;


        ThreadSleep(10);

        CloseSocket(stateone.WorkSocket);
        LogUtil.LogD("ClientClose","Client Close.");
    }
    /// <summary>
    /// 启动客户端引擎，连接服务器系统
    /// </summary>
    public void ClientStart()
    {
        if (Is_Client_Start) return;
        Thread thread_login = new Thread(new Runnable() {
            @Override
            public void run() {
                ThreadLogin();
            }
        });
        thread_login.start();
        LogUtil.LogD("ClientStart","Client Start.");

        if (thread_heart_check == null)
        {
            thread_heart_check = new Thread(new Runnable() {
                @Override
                public void run() {
                    ThreadHeartCheck();
                }
            });
            thread_heart_check.start();
        }
    }
    private void ThreadLogin()
    {
        synchronized (lock_connecting)
        {
            if (Is_Client_Connecting) return;
            Is_Client_Connecting = true;
        }


        if (Connect_Failed_Count == 0)
        {
            MessageAlerts("正在连接服务器...");
        }
        else
        {
            int count = 10;
            while (count > 0)
            {
                if (IsQuie) return;
                MessageAlerts("连接断开，等待" + count-- + "秒后重新连接");
                ThreadSleep(1000);
            }
            MessageAlerts("正在尝试第" + Connect_Failed_Count + "次连接服务器...");
        }


        stateone.setHeartTime(new Date());
        LogUtil.LogD("ThreadLogin","Begin Connect Server, Times: " + Connect_Failed_Count);


        OperateResult result = new OperateResult();

        Socket socket=null;
        boolWithSocket valueSocket = CreateSocketAndConnect(ServerIp,ServerPort,result);
        if(!valueSocket.Result)
        {
            Connect_Failed_Count++;
            Is_Client_Connecting = false;
            LoginFailed(Connect_Failed_Count);
            LogUtil.LogW("ThreadLogin","Connected Failed, Times: " + Connect_Failed_Count);
            // 连接失败，重新连接服务器
            ReconnectServer();
            return;
        }

        socket = valueSocket.Socket;

        // 连接成功，发送数据信息
        if(!SendStringAndCheckReceive(
                socket,
                1,
                ClientAlias,
                result,
                null,
                null
        ))
        {
            Connect_Failed_Count++;
            Is_Client_Connecting = false;
            LogUtil.LogD("ThreadLogin","Login Server Failed, Times: " + Connect_Failed_Count);
            LoginFailed(Connect_Failed_Count);
            // 连接失败，重新连接服务器
            ReconnectServer();
            return;
        }

        // 登录成功
        Connect_Failed_Count = 0;
        //stateone.IpEndPoint = (IPEndPoint)socket.RemoteEndPoint;
        stateone.setClientType(ClientAlias);
        stateone.WorkSocket = socket;
        //stateone.WorkSocket.BeginReceive(stateone.BytesHead, stateone.AlreadyReceivedHead,
                //stateone.BytesHead.Length - stateone.AlreadyReceivedHead, SocketFlags.None,
                //new AsyncCallback(HeadReceiveCallback), stateone);

        Thread receive = new Thread(new Runnable() {
            @Override
            public void run() {
                ThreadReceiveBackground();
            }
        });
        receive.start();



        // 发送一条验证消息
        byte[] bytesTemp = new byte[16];

        //BitConverter.GetBytes(DateTime.Now.Ticks).CopyTo(bytesTemp, 0);
        SendBytes(stateone, NetSupport.CommandBytes(HslCommunicationCode.Hsl_Protocol_Check_Secends, 0, KeyToken, bytesTemp));


        stateone.setHeartTime(new Date());
        Is_Client_Start = true;
        LoginSuccess();

        LogUtil.LogD("ThreadLogin","Login Server Success, Times: " + Connect_Failed_Count);

        Is_Client_Connecting = false;

        ThreadSleep(1000);
    }



    private void ThreadReceiveBackground()
    {
        while (true)
        {
            OperateResult result = new OperateResult();
            boolWith2Bytes value = ReceiveAndCheckBytes(stateone.WorkSocket, result, null,null);

            if (!value.Result)
            {
                continue;
            }

            // 数据处理
            byte[] buffer1= new byte[4];
            buffer1[0] = value.Content[0];
            buffer1[1] = value.Content[1];
            buffer1[2] = value.Content[2];
            buffer1[3] = value.Content[3];

            byte[] buffer2= new byte[4];
            buffer2[0] = value.Content[4];
            buffer2[1] = value.Content[5];
            buffer2[2] = value.Content[6];
            buffer2[3] = value.Content[7];

            int protocol = Utilities.bytes2Int(buffer1);
            int customer = Utilities.bytes2Int(buffer2);

            DataProcessingCenter(null,protocol,customer,value.Content2);
        }
    }


    // private bool Is_reconnect_server = false;
    // private object lock_reconnect_server = new object();


    private void ReconnectServer()
    {
        // 是否连接服务器中，已经在连接的话，则不再连接
        if (Is_Client_Connecting) return;
        // 是否退出了系统，退出则不再重连
        if (IsQuie) return;

        LogUtil.LogI("ReconnectServer","Prepare ReConnect Server.");

        // 触发连接失败，重连系统前错误
        BeforReConnected();
        CloseSocket(stateone.WorkSocket);

        Thread thread_login = new Thread(new Runnable() {
            @Override
            public void run() {
                ThreadLogin();
            }
        });
        thread_login.start();
    }


    /// <summary>
    /// 通信出错后的处理
    /// </summary>
    /// <param name="receive"></param>
    /// <param name="ex"></param>
    void SocketReceiveException(AsyncStateOne receive, Exception ex)
    {
        if (ex.getMessage().contains(StringResources.SocketRemoteCloseException))
        {
            // 异常掉线
            ReconnectServer();
        }
        else
        {
            // MessageAlerts?.Invoke("数据接收出错：" + ex.Message);
        }

        LogUtil.LogD("SocketReceiveException","Socket Excepiton Occured.");
    }


    /// <summary>
    /// 服务器端用于数据发送文本的方法
    /// </summary>
    /// <param name="customer">用户自定义的命令头</param>
    /// <param name="str">发送的文本</param>
    public void Send(NetHandle customer, String str)
    {
        if (Is_Client_Start)
        {
            SendBytes(stateone, NetSupport.CommandBytes(customer.get_CodeValue(), KeyToken, str));
        }
    }
    /// <summary>
    /// 服务器端用于发送字节的方法
    /// </summary>
    /// <param name="customer">用户自定义的命令头</param>
    /// <param name="bytes">实际发送的数据</param>
    public void Send(NetHandle customer, byte[] bytes)
    {
        if (Is_Client_Start)
        {
            SendBytes(stateone, NetSupport.CommandBytes(customer.get_CodeValue(), KeyToken, bytes));
        }
    }

    private void SendBytes(final AsyncStateOne stateone, final byte[] content)
    {
        Thread thread_login = new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    stateone.WorkSocket.getOutputStream().write(content);
                } catch (Exception ex) {

                }
            }
        });
        thread_login.start();
        //SendBytesAsync(stateone, content);
    }

    /// <summary>
    /// 客户端的数据处理中心
    /// </summary>
    /// <param name="receive"></param>
    /// <param name="protocol"></param>
    /// <param name="customer"></param>
    /// <param name="content"></param>
    void DataProcessingCenter(AsyncStateOne receive, int protocol, int customer, byte[] content)
    {
        if (protocol == HslCommunicationCode.Hsl_Protocol_Check_Secends)
        {
            //Date dt = new Date(BitConverter.ToInt64(content, 0));
            //ServerTime = new Date(BitConverter.ToInt64(content, 8));
            //DelayTime = (int)(new Date() - dt).TotalMilliseconds;
            stateone.setHeartTime(new Date());
            // MessageAlerts?.Invoke("心跳时间：" + DateTime.Now.ToString());
        }
        else if (protocol == HslCommunicationCode.Hsl_Protocol_Client_Quit)
        {
            // 申请了退出
        }
        else if (protocol == HslCommunicationCode.Hsl_Protocol_User_Bytes)
        {
            // 接收到字节数据
            AcceptByte(new NetHandle(customer), content);
        }
        else if (protocol == HslCommunicationCode.Hsl_Protocol_User_String)
        {
            // 接收到文本数据
            String str = Utilities.byte2String(content);
            AcceptString(new NetHandle(customer), str);
        }
    }


    private Thread thread_heart_check = null;

    /// <summary>
    /// 心跳线程的方法
    /// </summary>
    private void ThreadHeartCheck()
    {

        ThreadSleep(2000);
        while (true)
        {
            ThreadSleep(1000);

            if (!IsQuie)
            {
                byte[] send = new byte[16];
                System.arraycopy(Utilities.long2Bytes(new Date().getTime()),0,send,0,8);
                SendBytes(stateone, NetSupport.CommandBytes(HslCommunicationCode.Hsl_Protocol_Check_Secends, 0, KeyToken, send));
                double timeSpan = (new Date().getTime() - stateone.getHeartTime().getTime())/1000;
                if (timeSpan > 1 * 8)//8次没有收到失去联系
                {
                    LogUtil.LogD("ThreadHeartCheck","Heart Check Failed int "+timeSpan+" Seconds.");
                    ReconnectServer();
                    ThreadSleep(1000);
                }
            }
            else
            {
                break;
            }
        }
    }


}
