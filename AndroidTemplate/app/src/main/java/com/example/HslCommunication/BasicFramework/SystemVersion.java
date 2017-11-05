package com.example.HslCommunication.BasicFramework;

/**
 * Created by hsl20 on 2017/11/4.
 */

public class SystemVersion {

    /// <summary>
    /// 根据格式化字符串的版本号初始化
    /// </summary>
    /// <param name="VersionString">格式化的字符串，例如：1.0.0或1.0.0.0503</param>
    public SystemVersion(String VersionString)
    {
        String[] temp = VersionString.split("\\.");
        if (temp.length >= 3)
        {
            m_MainVersion = Integer.parseInt(temp[0]);
            m_SecondaryVersion = Integer.parseInt(temp[1]);
            m_EditVersion = Integer.parseInt(temp[2]);

            if (temp.length >= 4)
            {
                m_InnerVersion = Integer.parseInt(temp[3]);
            }
        }
    }
    /// <summary>
    /// 根据指定的数字实例化一个对象
    /// </summary>
    /// <param name="main">主版本</param>
    /// <param name="sec">次版本</param>
    /// <param name="edit">修订版</param>
    public SystemVersion(int main, int sec, int edit)
    {
        m_MainVersion = main;
        m_SecondaryVersion = sec;
        m_EditVersion = edit;
    }
    /// <summary>
    /// 根据指定的数字实例化一个对象
    /// </summary>
    /// <param name="main">主版本</param>
    /// <param name="sec">次版本</param>
    /// <param name="edit">修订版</param>
    /// <param name="inner">内部版本号</param>
    public SystemVersion(int main, int sec, int edit, int inner)
    {
        m_MainVersion = main;
        m_SecondaryVersion = sec;
        m_EditVersion = edit;
        m_InnerVersion = inner;
    }
    private int m_MainVersion = 2;
    /// <summary>
    /// 主版本
    /// </summary>
    public int MainVersion()
    {
        return m_MainVersion;
    }
    private int m_SecondaryVersion = 0;
    /// <summary>
    /// 次版本
    /// </summary>
    public int SecondaryVersion() {
        return m_SecondaryVersion;
    }

    private int m_EditVersion = 0;
    /// <summary>
    /// 修订版
    /// </summary>
    public int EditVersion() {
        return m_EditVersion;
    }
    private int m_InnerVersion = 0;
    /// <summary>
    /// 内部版本号，或者是版本号表示为年月份+内部版本的表示方式
    /// </summary>
    public int InnerVersion()
    {
        return m_InnerVersion;
    }

    /// <summary>
    /// 根据格式化为支持返回的不同信息的版本号
    /// C返回1.0.0.0
    /// N返回1.0.0
    /// S返回1.0
    /// </summary>
    /// <param name="format">格式化信息</param>
    /// <returns></returns>
    public String toString(String format)
    {
        if(format == "C")
        {
            return MainVersion()+"."+SecondaryVersion()+"."+EditVersion()+"."+InnerVersion();
        }

        if(format == "N")
        {
            return MainVersion()+"."+SecondaryVersion()+"."+EditVersion();
        }

        if(format == "S")
        {
            return MainVersion()+"."+SecondaryVersion();
        }

        return toString();
    }



    @Override
    public String toString() {
        if(InnerVersion() == 0)
        {
            return MainVersion()+"."+SecondaryVersion()+"."+EditVersion();
        }
        else
        {
            return MainVersion()+"."+SecondaryVersion()+"."+EditVersion()+"."+InnerVersion();
        }
    }


    public boolean IsSameVersion(SystemVersion sv)
    {
        if(this.m_MainVersion!=sv.m_MainVersion)
        {
            return  false;
        }

        if(this.m_SecondaryVersion!=sv.m_SecondaryVersion)
        {
            return false;
        }

        if(this.m_EditVersion!=sv.m_EditVersion)
        {
            return false;
        }

        if(this.m_InnerVersion!=sv.m_InnerVersion)
        {
            return false;
        }

        return true;
    }



}
