package com.example.HslCommunication.Core.Net;

/**
 * Created by DATHLIN on 2017/11/1.
 */

public class HslCommunicationCode {


    /// <summary>
    /// 规定所有的网络传输指令头都为32字节
    /// </summary>
    public static final int HeadByteLength = 32;


    /// <summary>
    /// 用于心跳程序的暗号信息
    /// </summary>
    public static final int Hsl_Protocol_Check_Secends = 1;
    /// <summary>
    /// 客户端退出消息
    /// </summary>
    public static final int Hsl_Protocol_Client_Quit = 2;
    /// <summary>
    /// 因为客户端达到上限而拒绝登录
    /// </summary>
    public static final int Hsl_Protocol_Client_Refuse_Login = 3;
    /// <summary>
    /// 允许客户端登录到服务器
    /// </summary>
    public static final int Hsl_Protocol_Client_Allow_Login = 4;




    /// <summary>
    /// 说明发送的只是文本信息
    /// </summary>
    public static final int Hsl_Protocol_User_String = 1001;
    /// <summary>
    /// 发送的数据就是普通的字节数组
    /// </summary>
    public static final int Hsl_Protocol_User_Bytes = 1002;
    /// <summary>
    /// 发送的数据就是普通的图片数据
    /// </summary>
    public static final int Hsl_Protocol_User_Bitmap = 1003;
    /// <summary>
    /// 发送的数据是一条异常的数据，字符串为异常消息
    /// </summary>
    public static final int Hsl_Protocol_User_Exception = 1004;




    /// <summary>
    /// 请求文件下载的暗号
    /// </summary>
    public static final int Hsl_Protocol_File_Download = 2001;
    /// <summary>
    /// 请求文件上传的暗号
    /// </summary>
    public static final int Hsl_Protocol_File_Upload = 2002;
    /// <summary>
    /// 请求删除文件的暗号
    /// </summary>
    public static final int Hsl_Protocol_File_Delete = 2003;
    /// <summary>
    /// 文件校验成功
    /// </summary>
    public static final int Hsl_Protocol_File_Check_Right = 2004;
    /// <summary>
    /// 文件校验失败
    /// </summary>
    public static final int Hsl_Protocol_File_Check_Error = 2005;
    /// <summary>
    /// 文件保存失败
    /// </summary>
    public static final int Hsl_Protocol_File_Save_Error = 2006;
    /// <summary>
    /// 请求文件列表的暗号
    /// </summary>
    public static final int Hsl_Protocol_File_Directory_Files = 2007;
    /// <summary>
    /// 请求子文件的列表暗号
    /// </summary>
    public static final int Hsl_Protocol_File_Directories = 2008;




    /// <summary>
    /// 不压缩数据字节
    /// </summary>
    public static final int Hsl_Protocol_NoZipped = 3001;
    /// <summary>
    /// 压缩数据字节
    /// </summary>
    public static final int Hsl_Protocol_Zipped  = 3002;
}
