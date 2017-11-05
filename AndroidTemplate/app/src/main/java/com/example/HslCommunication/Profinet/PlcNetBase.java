package com.example.HslCommunication.Profinet;

import com.example.HslCommunication.Core.Net.NetBase;

/**
 * Created by DATHLIN on 2017/11/3.
 */

public abstract class PlcNetBase extends NetBase {


    protected int m_PortRead = 1000;
    protected int m_PortReadBackup = -1;
    protected boolean m_IsPortNormal = true;
    protected int m_PortWrite = 1001;



    /// <summary>
    /// 获取访问的端口号
    /// </summary>
    /// <returns></returns>
    protected int GetPort()
    {
        if (m_PortReadBackup <= 0) return m_PortRead;
        return m_IsPortNormal ? m_PortRead : m_PortReadBackup;
    }
    /// <summary>
    /// 更换端口号
    /// </summary>
    protected void ChangePort()
    {
        m_IsPortNormal = !m_IsPortNormal;
    }


    public void setPortRead(int value) {
        m_PortRead = value;
    }


    public int getPortRead()
    {
        return m_PortRead;
    }


    /// <summary>
    /// 控制字节长度，超出选择截断，不够补零
    /// </summary>
    /// <param name="bytes">字节数据</param>
    /// <param name="length">最终需要的目标长度</param>
    /// <returns>处理后的数据</returns>
    protected byte[] ManageBytesLength(byte[] bytes, int length)
    {
        if (bytes == null) return null;
        byte[] temp = new byte[length];
        if (length > bytes.length)
        {
            System.arraycopy(bytes, 0, temp, 0, bytes.length);
        }
        else
        {
            System.arraycopy(bytes, 0, temp, 0, length);
        }
        return temp;
    }




}
