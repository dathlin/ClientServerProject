package com.example.HslCommunication.Core.Net;

import com.example.HslCommunication.Core.Types.HslTimeOut;
import com.example.HslCommunication.Core.Utilities.Utilities;

import java.io.DataOutputStream;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.Date;
import java.io.DataInputStream;
import java.util.UUID;

/**
 * Created by DATHLIN on 2017/11/1.
 */

public class NetSupport {

    private static final int SocketBufferSize = 4096;



    public static void ThreadPoolCheckConnect(HslTimeOut timeout, int millisecond) {
        while (!timeout.IsSuccessful) {
            if ((new Date().getTime() - timeout.StartTime.getTime()) > millisecond) {
                // 连接超时或是验证超时
                if (!timeout.IsSuccessful) {
                    try {
                        if (timeout.WorkSocket != null) {
                            timeout.WorkSocket.close();
                        }
                    } catch (java.io.IOException ex) {
                        // 不处理，放弃
                    }
                }
                break;
            }
        }
    }

    public static boolean IsTwoBytesEquel(byte[] b1, int start1, byte[] b2, int start2, int length) {
        if (b1 == null || b2 == null) return false;
        for (int i = 0; i < length; i++) {
            if (b1[i + start1] != b2[i + start2]) {
                return false;
            }
        }
        return true;
    }

    public static boolean CheckTokenEquel(byte[] head, UUID token)
    {
        return IsTwoBytesEquel(head, 12, Utilities.UUID2Byte(token), 0, 16);
    }



    public static byte[] ReadBytesFromSocket(Socket socket, int receive, ProgressReport report, boolean reportByPercent, boolean response) throws java.io.IOException
    {
        byte[] bytes_receive = new byte[receive];
        int count_receive = 0;
        long percent = 0;

        DataInputStream input = new DataInputStream(socket.getInputStream());
        DataOutputStream output = new DataOutputStream(socket.getOutputStream());

        while (count_receive < receive) {
            // 分割成4KB来接收数据
            int receive_length = 0;
            if ((receive - count_receive) >= SocketBufferSize) {
                receive_length = SocketBufferSize;
            } else {
                receive_length = (receive - count_receive);
            }

            count_receive += input.read(bytes_receive, count_receive, receive_length);

            if (reportByPercent) {
                long percentCurrent = (long) count_receive * 100 / receive;
                if (percent != percentCurrent) {
                    percent = percentCurrent;
                    // 报告进度
                    if (report != null) report.Report(count_receive, receive);
                }
            } else {
                // 报告进度
                if (report != null) report.Report(count_receive, receive);
            }

            // 回发进度
            if (response) output.write(Utilities.long2Bytes((long) count_receive));
        }

        return bytes_receive;
    }



    public static byte[] ReadBytesFromSocket(Socket socket, int receive) throws java.io.IOException {
        return ReadBytesFromSocket(socket, receive, null, false, false);
    }



    /// <summary>
    /// 读取套接字并且写入流
    /// </summary>
    /// <param name="stream">文件流</param>
    /// <param name="socket">连接的套接字</param>
    /// <param name="length">返回的文件长度</param>
    /// <param name="report">发送的进度报告</param>
    /// <param name="reportByPercent"></param>
    public static void WriteStreamFromSocket(Socket socket, OutputStream stream, long receive, ProgressReport report, boolean reportByPercent) throws java.io.IOException
    {
        byte[] buffer = new byte[SocketBufferSize];
        long count_receive = 0;
        long percent = 0;

        DataInputStream input = new DataInputStream(socket.getInputStream());
        DataOutputStream output = new DataOutputStream(socket.getOutputStream());

        while (count_receive < receive)
        {
            // 分割成4KB来接收数据
            int current = input.read(buffer, 0, SocketBufferSize);

            count_receive += current;
            stream.write(buffer, 0, current);
            if (reportByPercent)
            {
                long percentCurrent = count_receive * 100 / receive;
                if (percent != percentCurrent)
                {
                    percent = percentCurrent;
                    // 报告进度
                    if (report != null) report.Report(count_receive, receive);
                }
            }
            else
            {
                // 报告进度
                if (report != null) report.Report(count_receive, receive);
            }
            // 回发进度
            output.write(Utilities.long2Bytes(count_receive));
        }
        buffer = null;
    }




    /// <summary>
    /// 读取流并将数据写入socket
    /// </summary>
    /// <param name="stream">文件流</param>
    /// <param name="socket">连接的套接字</param>
    /// <param name="length">返回的文件长度</param>
    /// <param name="report">发送的进度报告</param>
    /// <param name="reportByPercent"></param>
    public static void WriteSocketFromStream(Socket socket, InputStream stream, long length, ProgressReport report, boolean reportByPercent) throws java.io.IOException
    {
        byte[] buffer = new byte[SocketBufferSize];
        long count_send = 0;
        long percent = 0;

        DataInputStream input = new DataInputStream(socket.getInputStream());
        DataOutputStream output = new DataOutputStream(socket.getOutputStream());

        while (count_send < length)
        {
            int count = stream.read(buffer, 0, SocketBufferSize);
            count_send += count;


            output.write(buffer, 0, count);

            while (count_send != Utilities.bytes2Long(ReadBytesFromSocket(socket, 8)));

            long received = count_send;

            if (reportByPercent)
            {
                long percentCurrent = received * 100 / length;
                if (percent != percentCurrent)
                {
                    percent = percentCurrent;
                    // 报告进度
                    if (report != null) report.Report(received, length);
                }
            }
            else
            {
                // 报告进度
                if (report != null) report.Report(received, length);
            }

            // 双重接收验证
            if (count == 0)
            {
                break;
            }
        }

        buffer = null;
    }



    public static void CheckSendBytesReceived(Socket socket, long length, ProgressReport report, boolean reportByPercent) throws java.io.IOException
    {
        long remoteNeedReceive = 0;
        long percent = 0;

        // 确认服务器的数据是否接收完成
        while (remoteNeedReceive < length)
        {
            remoteNeedReceive = Utilities.bytes2Long(ReadBytesFromSocket(socket, 8));
            if (reportByPercent)
            {
                long percentCurrent = remoteNeedReceive * 100 / length;
                if (percent != percentCurrent)
                {
                    percent = percentCurrent;
                    // 报告进度
                    if (report != null) report.Report(remoteNeedReceive, length);
                }
            }
            else
            {
                // 报告进度
                if (report != null) report.Report(remoteNeedReceive, length);
            }
        }
    }


    /*

        发送字节数据的最终命令

     */

    public static byte[] CommandBytes(int customer, UUID token, byte[] data)
    {
        return CommandBytes(HslCommunicationCode.Hsl_Protocol_User_Bytes, customer, token, data);
    }


    /*

        生成发送文本数据的最终命令

     */

    public static byte[] CommandBytes(int customer, UUID token, String data)
    {
        if (data == null) return CommandBytes(HslCommunicationCode.Hsl_Protocol_User_String, customer, token, null);
        else return CommandBytes(HslCommunicationCode.Hsl_Protocol_User_String, customer, token, Utilities.string2Byte(data));
    }

    /*

    生成最终的发送命令

     */

    public static byte[] CommandBytes(int command, int customer, UUID token, byte[] data)
    {
        byte[] _temp = null;
        int _zipped = HslCommunicationCode.Hsl_Protocol_NoZipped;
        int _sendLength = 0;
        if (data == null)
        {
            _temp = new byte[HslCommunicationCode.HeadByteLength];
        }
        else
        {
            // 加密
            data = HslSecurity.ByteEncrypt(data);
            if (data.length > 10240)
            {
                // 10K以上的数据，进行数据压缩
                data = HslZipped.CompressBytes(data);
                _zipped = HslCommunicationCode.Hsl_Protocol_Zipped;
            }
            _temp = new byte[HslCommunicationCode.HeadByteLength + data.length];
            _sendLength = data.length;
        }

        Utilities.int2Bytes(command);

        System.arraycopy(Utilities.int2Bytes(command),0,_temp,0,4);
        System.arraycopy(Utilities.int2Bytes(customer),0,_temp,4,4);
        System.arraycopy(Utilities.int2Bytes(_zipped),0,_temp,8,4);
        System.arraycopy(Utilities.UUID2Byte(token),0,_temp,12,16);
        System.arraycopy(Utilities.int2Bytes(_sendLength),0,_temp,28,4);
        if (_sendLength > 0)
        {
            System.arraycopy(data,0,_temp,32,_sendLength);
        }
        return _temp;
    }


    /*

    从接收的数据命令开始解析

     */

    public static byte[] CommandAnalysis(byte[] head, byte[] content)
    {
        if (content != null)
        {
            byte[] buffer = new byte[4];
            buffer[0] = head[8];
            buffer[1] = head[9];
            buffer[2] = head[10];
            buffer[3] = head[11];

            int _zipped = Utilities.bytes2Int(buffer);
            // 先进行解压
            if (_zipped == HslCommunicationCode.Hsl_Protocol_Zipped)
            {
                content = HslZipped.Decompress(content);
            }
            // 进行解密
            return HslSecurity.ByteDecrypt(content);
        }
        else
        {
            return null;
        }
    }

}
