package com.example.HslCommunication.Core.Types;

/**
 * Created by DATHLIN on 2017/11/1.
 */

public class OperateResult {

    /// <summary>
    /// 指示本次访问是否成功
    /// </summary>
    public boolean IsSuccess =false;


    /// <summary>
    /// 具体的错误描述
    /// </summary>
    public String Message  = "Unknown Errors";
    /// <summary>
    /// 具体的错误代码
    /// </summary>
    public int ErrorCode = 10000;

    /// <summary>
    /// 消息附带的额外信息
    /// </summary>
    public Object Tag =null;

    /// <summary>
    /// 获取错误代号及文本描述
    /// </summary>
    /// <returns></returns>
    public String ToMessageShowString()
    {
        return "错误代码："+ErrorCode +"\r\n错误信息："+Message;
    }

}