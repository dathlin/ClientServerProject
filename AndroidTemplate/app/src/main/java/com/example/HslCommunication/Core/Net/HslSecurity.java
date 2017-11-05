package com.example.HslCommunication.Core.Net;

/**
 * Created by DATHLIN on 2017/11/1.
 */

public class HslSecurity {


    /// <summary>
    /// 加密方法，只对当前的程序集开放
    /// </summary>
    /// <param name="enBytes">等待加密的数据</param>
    /// <returns>加密后的数据</returns>
    public static byte[] ByteEncrypt(byte[] enBytes) {
        if (enBytes == null) return null;
        byte[] result = new byte[enBytes.length];
        for (int i = 0; i < enBytes.length; i++) {
            result[i] = (byte) (enBytes[i] ^ 0xB5);
        }
        return result;
    }


    /// <summary>
    /// 解密方法，只对当前的程序集开放
    /// </summary>
    /// <param name="deBytes">等待解密的数据</param>
    /// <returns>解密后的数据</returns>
    public static byte[] ByteDecrypt(byte[] deBytes) {
        return ByteEncrypt(deBytes);
    }


}
