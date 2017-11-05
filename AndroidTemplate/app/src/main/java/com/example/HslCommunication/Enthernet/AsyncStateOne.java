package com.example.HslCommunication.Enthernet;

import com.example.HslCommunication.Core.Net.HslCommunicationCode;

import java.net.Socket;
import java.util.Date;
import java.util.UUID;

/**
 * Created by hsl20 on 2017/11/4.
 */

public class AsyncStateOne {


    /// <summary>
    /// 实例化一个对象
    /// </summary>
    public AsyncStateOne() {
        ClientUniqueID = UUID.randomUUID().toString();
    }

    /// <summary>
    /// IP地址
    /// </summary>
    private String IpAddress = "";

    public String getIpAddress() {
        return IpAddress;
    }

    private void setIpAddress(String ipAddress) {
        IpAddress = ipAddress;
    }


    /*
        端口号
     */
    private int IpPort = 10000;

    public int getIpPort() {
        return IpPort;
    }

    private void setIpPort(int ipPort) {
        IpPort = ipPort;
    }


    /*
        登录的别名
     */
    private String LoginAlias = "";

    public String getLoginAlias() {
        return LoginAlias;
    }

    public void setLoginAlias(String loginAlias) {
        LoginAlias = loginAlias;
    }


    /*
        心跳验证的时间点
     */
    private Date HeartTime=new Date();
    public Date getHeartTime() {
        return HeartTime;
    }

    public void setHeartTime(Date heartTime) {
        HeartTime = heartTime;
    }


    /*
        客户端类别
     */
    private String ClientType="";
    public void setClientType(String clientType) {
        ClientType = clientType;
    }


    public String getClientType() {
        return ClientType;
    }


    /*
        客户端的唯一标识
     */
    private String ClientUniqueID="";
    public String getClientUniqueID() {
        return ClientUniqueID;
    }

    public void setClientUniqueID(String clientUniqueID) {
        ClientUniqueID = clientUniqueID;
    }



    /// <summary>
    /// 指令头缓存
    /// </summary>
    byte[] BytesHead = new byte[HslCommunicationCode.HeadByteLength];
    /// <summary>
    /// 已经接收的指令头长度
    /// </summary>
    int AlreadyReceivedHead = 0;
    /// <summary>
    /// 数据内容缓存
    /// </summary>
    byte[] BytesContent = null;
    /// <summary>
    /// 已经接收的数据内容长度
    /// </summary>
    int AlreadyReceivedContent = 0;

    /// <summary>
    /// 清除本次的接收内容
    /// </summary>
    void Clear() {
        BytesHead = new byte[HslCommunicationCode.HeadByteLength];
        AlreadyReceivedHead = 0;
        BytesContent = null;
        AlreadyReceivedContent = 0;
    }


    Socket WorkSocket=null;
}