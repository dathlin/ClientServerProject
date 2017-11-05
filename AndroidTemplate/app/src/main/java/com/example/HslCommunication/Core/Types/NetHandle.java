package com.example.HslCommunication.Core.Types;

import com.example.HslCommunication.Core.Utilities.Utilities;

/**
 * Created by DATHLIN on 2017/11/2.
 */

public final class NetHandle {


    /// <summary>
    /// 初始化一个暗号对象
    /// </summary>
    public NetHandle(int value)
    {
        byte[] buffer = Utilities.int2Bytes(value);

        m_CodeMajor = buffer[3];
        m_CodeMinor = buffer[2];
        m_CodeIdentifier = Utilities.byte2Short(buffer,0);


        m_CodeValue = value;
    }


    /// <summary>
    /// 根据三个值来初始化暗号对象
    /// </summary>
    /// <param name="major">主暗号</param>
    /// <param name="minor">次暗号</param>
    /// <param name="identifier">暗号编号</param>
    public NetHandle(int major, int minor, int identifier)
    {
        m_CodeValue = 0;

        byte[] buffer_major=Utilities.int2Bytes(major);
        byte[] buffer_minor=Utilities.int2Bytes(minor);
        byte[] buffer_identifier=Utilities.int2Bytes(identifier);

        m_CodeMajor = buffer_major[0];
        m_CodeMinor = buffer_minor[0];
        m_CodeIdentifier = Utilities.byte2Short(buffer_identifier,0);

        byte[] buffer = new byte[4];
        buffer[3] = m_CodeMajor;
        buffer[2] = m_CodeMinor;
        buffer[1] = buffer_identifier[1];
        buffer[0] = buffer_identifier[0];

        m_CodeValue = Utilities.bytes2Int(buffer);
    }


    /// <summary>
    /// 完整的暗号值
    /// </summary>
    private int m_CodeValue;

    /// <summary>
    /// 主暗号分类0-255
    /// </summary>
    private byte m_CodeMajor;

    /// <summary>
    /// 次要的暗号分类0-255
    /// </summary>
    private byte m_CodeMinor;

    /// <summary>
    /// 暗号的编号分类0-65535
    /// </summary>
    private short m_CodeIdentifier;



    /// <summary>
    /// 完整的暗号值
    /// </summary>
    public int get_CodeValue(){
        return  m_CodeValue;
    }

    /// <summary>
    /// 主暗号分类0-255
    /// </summary>
    public byte get_CodeMajor() {
        return m_CodeMajor;
    }

    /// <summary>
    /// 次要的暗号分类0-255
    /// </summary>
    public byte get_CodeMinor() {
        return m_CodeMinor;
    }

    /// <summary>
    /// 暗号的编号分类0-65535
    /// </summary>
    public short get_CodeIdentifier() {
        return m_CodeIdentifier;
    }




}
