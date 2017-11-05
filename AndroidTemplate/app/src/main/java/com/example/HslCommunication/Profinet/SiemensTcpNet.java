package com.example.HslCommunication.Profinet;

import com.example.HslCommunication.Core.Net.NetSupport;
import com.example.HslCommunication.Core.Types.OperateResult;
import com.example.HslCommunication.Core.Types.OperateResultBytes;
import com.example.HslCommunication.Core.Utilities.Utilities;
import com.example.HslCommunication.Core.Utilities.boolWithBytes;
import com.example.HslCommunication.Core.Utilities.boolWithSiemens;
import com.example.HslCommunication.Core.Utilities.boolWithSocket;

import java.io.OutputStream;
import java.net.Socket;
import java.util.ArrayList;

/**
 * Created by DATHLIN on 2017/11/3.
 */

public final class SiemensTcpNet extends PlcNetBase {

    /// <summary>
    /// 实例化一个数据通信的对象，需要指定访问哪种Plc
    /// </summary>
    /// <param name="siemens"></param>
    public SiemensTcpNet(SiemensPLCS siemens)
    {
        m_PortRead = 102;
        m_PortWrite = 102;
        CurrentPlc = siemens;

        switch (siemens)
        {
            case S1200: plcHead1[18] = 1; break;
            case S300: plcHead1[18] = 2; break;
            case Smart200:plcHead1[18] = 1; break;
            default: plcHead1[18] = 3; break;
        }
    }


    /// <summary>
    /// 可以手动设置PLC类型，用来测试原本不支持的数据访问功能
    /// </summary>
    /// <param name="type"></param>
    public void SetPlcType(byte type)
    {
        plcHead1[18] = type;
    }

    private String ipAddress="";

    public String getIpAddress() {
        return ipAddress;
    }

    public void setIpAddress(String ipAddress) {
        this.ipAddress = ipAddress;
    }


    private boolWithBytes ReceiveBytesFromSocket(Socket socket)
    {
        boolWithBytes value =new boolWithBytes();
        try
        {
            // 先接收4个字节的数据
            byte[] head = NetSupport.ReadBytesFromSocket(socket, 4);
            int receive = head[2] * 256 + head[3];
            value.Content = new byte[receive];
            System.arraycopy(head,0,value.Content,0, 4);
            byte[] data = NetSupport.ReadBytesFromSocket(socket, receive - 4);
            System.arraycopy(data,0,value.Content,4,data.length);
            value.Result=true;
            return value;
        }
        catch(Exception ex)
        {
            CloseSocket(socket);
            return value;
        }
    }

    private boolean SendBytesToSocket(Socket socket, byte[] data)
    {
        try
        {
            if (data != null)
            {
                OutputStream outputStream = socket.getOutputStream();
                outputStream.write(data);
            }
            return true;
        }
        catch(Exception ex)
        {
            CloseSocket(socket);
            return false;
        }
    }


    private int CalculateAddressStarted(String address)
    {
        if (address.indexOf('.') < 0)
        {
            return Integer.parseInt(address) * 8;
        }
        else
        {
            String[] temp = address.split("\\.");
            return Integer.parseInt(temp[0]) * 8 + Integer.parseInt(temp[1]);
        }
    }

    /// <summary>
    /// 解析数据地址
    /// </summary>
    /// <param name="address">数据地址</param>
    /// <param name="type">类型</param>
    /// <param name="startAddress">其实地址</param>
    /// <param name="dbAddress">DB块地址</param>
    /// <param name="result">结果数据对象</param>
    /// <returns></returns>
    private boolWithSiemens AnalysisAddress(String address, OperateResult result)
    {
        boolWithSiemens value = new boolWithSiemens();
        try
        {
            value.dbAddress = 0;
            if (address.charAt(0) == 'I')
            {
                value.type = -127;
                value.startAddress = CalculateAddressStarted(address.substring(1));
            }
            else if (address.charAt(0) == 'Q')
            {
                value.type = -126;
                value.startAddress = CalculateAddressStarted(address.substring(1));
            }
            else if (address.charAt(0) == 'M')
            {
                value.type = -125;
                value.startAddress = CalculateAddressStarted(address.substring(1));
            }
            else if (address.charAt(0) == 'D' || address.substring(0, 2) == "DB")
            {
                value.type = -124;
                String[] adds = address.split("\\.");
                if (address.charAt(1) == 'B')
                {
                    value.dbAddress = Short.parseShort(adds[0].substring(2));
                }
                else
                {
                    value.dbAddress = Short.parseShort(adds[0].substring(1));
                }

                value.startAddress = CalculateAddressStarted(address.substring(address.indexOf('.') + 1));
            }
            else
            {
                result.Message = "不支持的数据类型";
                value.type = 0;
                value.startAddress = 0;
                value.dbAddress = 0;
                return value;
            }
        }
        catch (Exception ex)
        {
            result.Message = ex.getMessage();
            return value;
        }

        value.Result=true;
        return value;
    }

    private boolean InitilizationConnect(Socket socket,OperateResult result)
    {
        // 发送初始化信息
        if(!SendBytesToSocket(socket,plcHead1))
        {
            result.Message = "初始化信息发送失败";
            return false;
        }

        if(!ReceiveBytesFromSocket(socket).Result)
        {
            result.Message = "初始化信息接收失败";
            return false;
        }

        if(!SendBytesToSocket(socket,plcHead2))
        {
            result.Message = "初始化信息发送失败";
            return false;
        }

        if(!ReceiveBytesFromSocket(socket).Result)
        {
            result.Message = "初始化信息接收失败";
            return false;
        }

        return true;
    }


    /// <summary>
    /// 从PLC读取数据，地址格式为I100，Q100，DB20.100，M100，以字节为单位
    /// </summary>
    /// <param name="address">起始地址，格式为I100，M100，Q100，DB20.100</param>
    /// <param name="count">读取的数量，以字节为单位</param>
    /// <returns></returns>
    public OperateResultBytes ReadFromPLC(String address, short count)
    {
        return ReadFromPLC(new String[] { address }, new short[] { count });
    }


    /// <summary>
    /// 一次性从PLC获取所有的数据，按照先后顺序返回一个统一的Buffer，需要按照顺序处理，两个数组长度必须一致
    /// </summary>
    /// <param name="address">起始地址数组</param>
    /// <param name="count">数据长度数组</param>
    /// <returns></returns>
    public OperateResultBytes ReadFromPLC(String[] address, short[] count)
    {
        OperateResultBytes result = new OperateResultBytes();

        if (address == null)
        {
            result.Message = "地址不能为空";
            return result;
        }


        if (count == null)
        {
            result.Message = "数量不能为空";
            return result;
        }
        if (address.length != count.length) {
            result.Message = "两个参数的个数不统一";
            return result;
        }

        Socket socket;
        boolWithSocket value = CreateSocketAndConnect(ipAddress,getPortRead(), result);
        if (!value.Result)
        {
            ChangePort();
            return result;
        }

        socket = value.Socket;
        if (!InitilizationConnect(socket, result))
        {
            return result;
        }


        // 分批次进行读取，计算总批次
        int times = address.length / 255;
        if (address.length % 255 > 0)
        {
            times++;
        }

        // 缓存所有批次的结果
        ArrayList<byte[]> arrays_bytes = new ArrayList<byte[]>();

        for (int jj = 0; jj < times; jj++)
        {
            // 计算本批次需要读取的数据
            int startIndex = jj * 255;
            int readCount = address.length - startIndex;
            if (readCount > 255)
            {
                readCount = 255;
            }

            byte[] _PLCCommand = new byte[19 + readCount * 12];
            // 报文头
            _PLCCommand[0] = 0x03;
            _PLCCommand[1] = 0x00;
            // 长度
            _PLCCommand[2] = (byte)(_PLCCommand.length / 256);
            _PLCCommand[3] = (byte)(_PLCCommand.length % 256);
            // 固定
            _PLCCommand[4] = 0x02;
            _PLCCommand[5] = -16;
            _PLCCommand[6] = -128;
            _PLCCommand[7] = 0x32;
            // 命令：发
            _PLCCommand[8] = 0x01;
            // 标识序列号
            _PLCCommand[9] = 0x00;
            _PLCCommand[10] = 0x00;
            _PLCCommand[11] = 0x00;
            _PLCCommand[12] = 0x01;
            // 命令数据总长度
            _PLCCommand[13] = (byte)((_PLCCommand.length - 17) / 256);
            _PLCCommand[14] = (byte)((_PLCCommand.length - 17) % 256);

            _PLCCommand[15] = 0x00;
            _PLCCommand[16] = 0x00;

            // 命令起始符
            _PLCCommand[17] = 0x04;
            // 读取数据块个数
            _PLCCommand[18] = (byte)readCount;

            int receiveCount = 0;
            for (int ii = 0; ii < readCount; ii++)
            {
                receiveCount += count[ii + 255 * jj];

                boolWithSiemens siemens = AnalysisAddress(address[ii + 255 * jj], result);
                // 填充数据
                if (!siemens.Result)
                {
                    CloseSocket(socket);
                    return result;
                }

                // 读取地址的前缀
                _PLCCommand[19 + ii * 12] = 0x12;
                _PLCCommand[20 + ii * 12] = 0x0A;
                _PLCCommand[21 + ii * 12] = 0x10;
                _PLCCommand[22 + ii * 12] = 0x02;
                // 访问数据的个数
                _PLCCommand[23 + ii * 12] = (byte)(count[ii + 255 * jj] / 256);
                _PLCCommand[24 + ii * 12] = (byte)(count[ii + 255 * jj] % 256);
                // DB块编号，如果访问的是DB块的话
                _PLCCommand[25 + ii * 12] = (byte)(siemens.dbAddress / 256);
                _PLCCommand[26 + ii * 12] = (byte)(siemens.dbAddress % 256);
                // 访问数据类型
                _PLCCommand[27 + ii * 12] = siemens.type;
                // 偏移位置
                _PLCCommand[28 + ii * 12] = (byte)(siemens.startAddress / 256 / 256);
                _PLCCommand[29 + ii * 12] = (byte)(siemens.startAddress / 256);
                _PLCCommand[30 + ii * 12] = (byte)(siemens.startAddress % 256);
            }


            if (!SendBytesToSocket(socket, _PLCCommand))
            {
                result.Message = "发送读取信息失败";
                return result;
            }

            boolWithBytes content=ReceiveBytesFromSocket(socket);
            if (!content.Result)
            {
                result.Message = "接收信息失败";
                return result;
            }

            if (content.Content.length != receiveCount + readCount * 4 + 21)
            {
                CloseSocket(socket);
                result.Message = "数据长度校验失败";
                result.Content = content.Content;
                return result;
            }

            // 分次读取成功
            byte[] buffer = new byte[receiveCount];
            int kk = 21;
            int ll = 0;
            for (int ii = 0; ii < readCount; ii++)
            {
                // 将数据挪回正确的地方
                System.arraycopy(content, kk + 4, buffer, ll, count[ii + 255 * jj]);
                kk += count[ii + 255 * jj] + 4;
                ll += count[ii + 255 * jj];
            }
            arrays_bytes.add(buffer);
        }


        if (arrays_bytes.size() == 1)
        {
            result.Content = arrays_bytes.get(0);
        }
        else
        {
            int length = 0;
            int offset = 0;

            // 获取长度并生成缓冲数据

            for(int ii=0;ii<arrays_bytes.size();ii++)
            {
                length += arrays_bytes.get(ii).length;
            }

            result.Content = new byte[length];

            // 复制数据操作
            for(int ii=0;ii<arrays_bytes.size();ii++)
            {
                System.arraycopy(arrays_bytes.get(ii),0,result.Content,offset,arrays_bytes.get(ii).length);
                offset+=arrays_bytes.get(ii).length;
            }
            arrays_bytes.clear();
        }
        result.IsSuccess = true;
        CloseSocket(socket);
        //所有的数据接收完成，进行返回
        return result;
    }

    /// <summary>
    /// 将数据写入到PLC数据，地址格式为I100，Q100，DB20.100，M100，以字节为单位
    /// </summary>
    /// <param name="address">起始地址，格式为I100，M100，Q100，DB20.100</param>
    /// <param name="data">写入的数据，长度根据data的长度来指示</param>
    /// <returns></returns>
    public OperateResult WriteIntoPLC(String address, byte[] data)
    {
        OperateResult result = new OperateResult();


        Socket socket;
        boolWithSocket value = CreateSocketAndConnect(ipAddress,getPortRead(), result);
        if (!value.Result)
        {
            ChangePort();
            return result;
        }

        socket = value.Socket;
        if (!InitilizationConnect(socket, result))
        {
            return result;
        }


        if (data == null) data = new byte[0];

        boolWithSiemens siemens = AnalysisAddress(address, result);
        // 填充数据
        if (!siemens.Result)
        {
            CloseSocket(socket);
            return result;
        }


        byte[] _PLCCommand = new byte[35 + data.length];
        _PLCCommand[0] = 0x03;
        _PLCCommand[1] = 0x00;
        // 长度
        _PLCCommand[2] = (byte)((35 + data.length) / 256);
        _PLCCommand[3] = (byte)((35 + data.length) % 256);
        // 固定
        _PLCCommand[4] = 0x02;
        _PLCCommand[5] = -16;
        _PLCCommand[6] = -128;
        _PLCCommand[7] = 0x32;
        // 命令 发
        _PLCCommand[8] = 0x01;
        // 标识序列号
        _PLCCommand[9] = 0x00;
        _PLCCommand[10] = 0x00;
        _PLCCommand[11] = 0x00;
        _PLCCommand[12] = 0x01;
        // 固定
        _PLCCommand[13] = 0x00;
        _PLCCommand[14] = 0x0E;
        // 写入长度+4
        _PLCCommand[15] = (byte)((4 + data.length) / 256);
        _PLCCommand[16] = (byte)((4 + data.length) % 256);
        // 命令起始符
        _PLCCommand[17] = 0x05;
        // 写入数据块个数
        _PLCCommand[18] = 0x01;
        // 固定，返回数据长度
        _PLCCommand[19] = 0x12;
        _PLCCommand[20] = 0x0A;
        _PLCCommand[21] = 0x10;
        // 写入方式，1是按位，2是按字
        _PLCCommand[22] = 0x02;
        // 写入数据的个数
        _PLCCommand[23] = (byte)(data.length / 256);
        _PLCCommand[24] = (byte)(data.length % 256);
        // DB块编号，如果访问的是DB块的话
        _PLCCommand[25] = (byte)(siemens.dbAddress / 256);
        _PLCCommand[26] = (byte)(siemens.dbAddress % 256);
        // 写入数据的类型
        _PLCCommand[27] = siemens.type;
        // 偏移位置
        _PLCCommand[28] = (byte)(siemens.startAddress / 256 / 256);;
        _PLCCommand[29] = (byte)(siemens.startAddress / 256);
        _PLCCommand[30] = (byte)(siemens.startAddress % 256);
        // 按字写入
        _PLCCommand[31] = 0x00;
        _PLCCommand[32] = 0x04;
        // 按位计算的长度
        _PLCCommand[33] = (byte)(data.length * 8 / 256);
        _PLCCommand[34] = (byte)(data.length * 8 % 256);

        System.arraycopy(data,0,_PLCCommand,35,data.length);

        if(!SendBytesToSocket(socket,_PLCCommand))
        {
            result.Message = "发送写入信息失败";
            return result;
        }

        boolWithBytes value2=ReceiveBytesFromSocket(socket);
        if (!value2.Result)
        {
            result.Message = "接收信息失败";
            return result;
        }

        if (value2.Content[value2.Content.length - 1] != 0xFF)
        {
            // 写入异常
            CloseSocket(socket);
            result.Message = "写入数据异常";
            return result;
        }

        CloseSocket(socket);
        result.IsSuccess = true;
        return result;
    }


    /// <summary>
    /// 写入PLC的一个位，例如"M100.6"，"I100.7"，"Q100.0"，"DB20.100.0"，如果只写了"M100"默认为"M100.0
    /// </summary>
    /// <param name="address"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public OperateResult WriteIntoPLC(String address, boolean data)
    {
        OperateResult result = new OperateResult();

        Socket socket;
        boolWithSocket value = CreateSocketAndConnect(getIpAddress(),m_PortWrite, result);
        if (!value.Result)
        {
            ChangePort();
            return result;
        }

        socket = value.Socket;
        if (!InitilizationConnect(socket, result))
        {
            return result;
        }



        byte[] buffer = new byte[1];
        buffer[0] = data ? (byte)0x01 : (byte)0x00;

        boolWithSiemens siemens = AnalysisAddress(address, result);
        // 填充数据
        if (!siemens.Result)
        {
            CloseSocket(socket);
            return result;
        }



        byte[] _PLCCommand = new byte[35 + buffer.length];
        _PLCCommand[0] = 0x03;
        _PLCCommand[1] = 0x00;
        // 长度
        _PLCCommand[2] = (byte)((35 + buffer.length) / 256);
        _PLCCommand[3] = (byte)((35 + buffer.length) % 256);
        // 固定
        _PLCCommand[4] = 0x02;
        _PLCCommand[5] = -16;
        _PLCCommand[6] = -128;
        _PLCCommand[7] = 0x32;
        // 命令 发
        _PLCCommand[8] = 0x01;
        // 标识序列号
        _PLCCommand[9] = 0x00;
        _PLCCommand[10] = 0x00;
        _PLCCommand[11] = 0x00;
        _PLCCommand[12] = 0x01;
        // 固定
        _PLCCommand[13] = 0x00;
        _PLCCommand[14] = 0x0E;
        // 写入长度+4
        _PLCCommand[15] = (byte)((4 + buffer.length) / 256);
        _PLCCommand[16] = (byte)((4 + buffer.length) % 256);
        // 命令起始符
        _PLCCommand[17] = 0x05;
        // 写入数据块个数
        _PLCCommand[18] = 0x01;
        _PLCCommand[19] = 0x12;
        _PLCCommand[20] = 0x0A;
        _PLCCommand[21] = 0x10;
        // 写入方式，1是按位，2是按字
        _PLCCommand[22] = 0x01;
        // 写入数据的个数
        _PLCCommand[23] = (byte)(buffer.length / 256);
        _PLCCommand[24] = (byte)(buffer.length % 256);
        // DB块编号，如果访问的是DB块的话
        _PLCCommand[25] = (byte)(siemens.dbAddress / 256);
        _PLCCommand[26] = (byte)(siemens.dbAddress % 256);
        // 写入数据的类型
        _PLCCommand[27] = siemens.type;
        // 偏移位置
        _PLCCommand[28] = (byte)(siemens.startAddress / 256 / 256);
        _PLCCommand[29] = (byte)(siemens.startAddress / 256);
        _PLCCommand[30] = (byte)(siemens.startAddress % 256);
        // 按位写入
        _PLCCommand[31] = 0x00;
        _PLCCommand[32] = 0x03;
        // 按位计算的长度
        _PLCCommand[33] = (byte)(buffer.length / 256);
        _PLCCommand[34] = (byte)(buffer.length % 256);

        System.arraycopy(buffer,0,_PLCCommand,35,buffer.length);

        if(!SendBytesToSocket(socket,_PLCCommand))
        {
            result.Message = "发送写入信息失败";
            return result;
        }

        boolWithBytes value2=ReceiveBytesFromSocket(socket);
        if (!value2.Result)
        {
            result.Message = "接收信息失败";
            return result;
        }

        if (value2.Content[value2.Content.length - 1] != 0xFF)
        {
            // 写入异常
            CloseSocket(socket);
            result.Message = "写入数据异常";
            return result;
        }

        CloseSocket(socket);
        result.IsSuccess = true;
        return result;
    }

    /// <summary>
    /// 从返回的西门子数组中获取short数组数据，已经内置高地位转换
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public short[] GetArrayFromBytes(byte[] bytes)
    {
        short[] temp = new short[bytes.length / 2];
        for (int i = 0; i < temp.length; i++)
        {
            byte[] buffer = new byte[2];
            buffer[0] = bytes[i * 2 + 1];
            buffer[1] = bytes[i * 2];
            temp[i] = Utilities.byte2Short(buffer, 0);
        }
        return temp;
    }

    /// <summary>
    /// 从返回的西门子数组中获取int数组数据，已经内置高地位转换
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public int[] GetIntArrayFromBytes(byte[] bytes)
    {
        int[] temp = new int[bytes.length / 4];
        for (int i = 0; i < temp.length; i++)
        {
            byte[] buffer = new byte[4];
            buffer[0] = bytes[i * 4 + 0];
            buffer[1] = bytes[i * 4 + 1];
            buffer[2] = bytes[i * 4 + 2];
            buffer[3] = bytes[i * 4 + 3];
            temp[i] = Utilities.bytes2Int(buffer);
        }
        return temp;
    }

    /// <summary>
    /// 根据索引位转换获取short数据
    /// </summary>
    /// <param name="content"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public short GetShortFromBytes(byte[] content, int index)
    {
        byte[] buffer = new byte[2];
        buffer[0] = content[index + 0];
        buffer[1] = content[index + 1];
        return Utilities.byte2Short(buffer,0);
    }


    /// <summary>
    /// 根据索引位转换获取int数据
    /// </summary>
    /// <param name="content"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetIntFromBytes(byte[] content, int index)
    {
        byte[] buffer = new byte[4];
        buffer[0] = content[index + 0];
        buffer[1] = content[index + 1];
        buffer[2] = content[index + 2];
        buffer[3] = content[index + 3];
        return Utilities.bytes2Int(buffer);
    }

    private byte[] plcHead1 =
    {
        0x03,  // 报文头
                0x00,
                0x00,  // 数据长度
                0x16,
                0x11,
                -32,
                0x00,
                0x00,
                0x00,
                0x01,
                0x00,
                -63,
                0x02,
                0x10,
                0x00,
                -62,
                0x02,
                0x03,
                0x01,  // 指示cpu
                -64,
                0x01,
                0x0A
    };
    private byte[] plcHead2 =
            {
                    0x03,
                    0x00,
                    0x00,
                    0x19,
                    0x02,
                    -16,
                    -128,
                    0x32,
                    0x01,
                    0x00,
                    0x00,
                    -52,
                    -63,
                    0x00,
                    0x08,
                    0x00,
                    0x00,
                    -16,
                    0x00,
                    0x00,
                    0x01,
                    0x00,
                    0x01,
                    0x03,
                    -64
            };

    private SiemensPLCS CurrentPlc = SiemensPLCS.S1200;


}
