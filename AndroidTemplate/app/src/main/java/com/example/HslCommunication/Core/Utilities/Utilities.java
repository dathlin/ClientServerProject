package com.example.HslCommunication.Core.Utilities;

import com.example.HslCommunication.Log.LogUtil;

import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.util.UUID;

/**
 * Created by DATHLIN on 2017/11/1.
 */



public class Utilities {

    public static byte[] int2Bytes(int num) {
        byte[] byteNum = new byte[4];
        for (int ix = 0; ix < 4; ++ix) {
            int offset = 32 - (ix + 1) * 8;
            byteNum[3-ix] = (byte) ((num >> offset) & 0xff);
        }
        return byteNum;
    }

    public static int bytes2Int(byte[] byteNum) {
        int num = 0;
        for (int ix = 0; ix < 4; ++ix) {
            num <<= 8;
            num |= (byteNum[3-ix] & 0xff);
        }
        return num;
    }

    public static byte int2OneByte(int num) {
        return (byte) (num & 0x000000ff);
    }

    public static int oneByte2Int(byte byteNum) {
        //针对正数的int
        return byteNum > 0 ? byteNum : (128 + (128 + byteNum));
    }

    public static byte[] long2Bytes(long num) {
        byte[] byteNum = new byte[8];
        for (int ix = 0; ix < 8; ++ix) {
            int offset = 64 - (ix + 1) * 8;
            byteNum[7-ix] = (byte) ((num >> offset) & 0xff);
        }
        return byteNum;
    }

    public static long bytes2Long(byte[] byteNum) {
        long num = 0;
        for (int ix = 0; ix < 8; ++ix) {
            num <<= 8;
            num |= (byteNum[7-ix] & 0xff);
        }
        return num;
    }

    public static UUID Byte2UUID(byte[] data) {
        if (data.length != 16) {
            throw new IllegalArgumentException("Invalid UUID byte[]");
        }
        long msb = 0;
        long lsb = 0;
        for (int i = 0; i < 8; i++)
            msb = (msb << 8) | (data[i] & 0xff);
        for (int i = 8; i < 16; i++)
            lsb = (lsb << 8) | (data[i] & 0xff);

        return new UUID(msb, lsb);
    }

    public static byte[] UUID2Byte(UUID uuid) {
        ByteArrayOutputStream ba = new ByteArrayOutputStream(16);
        DataOutputStream da = new DataOutputStream(ba);
        try {
            da.writeLong(uuid.getMostSignificantBits());
            da.writeLong(uuid.getLeastSignificantBits());
        }
        catch (IOException e) {
            e.printStackTrace();
        }
        byte[] buffer = ba.toByteArray();
        // 进行错位
        byte temp=buffer[0];
        buffer[0] = buffer[3];
        buffer[3] =temp;
        temp=buffer[1];
        buffer[1]=buffer[2];
        buffer[2]=temp;

        temp = buffer[4];
        buffer[4]=buffer[5];
        buffer[5] =temp;

        temp = buffer[6];
        buffer[6]=buffer[7];
        buffer[7] =temp;

        return buffer;
    }

    public static short byte2Short(byte[] b, int index) {
        return (short) (((b[index + 0] << 8) | b[index + 1] & 0xff));
    }


    public static byte[] short2Byte(short s) {
        byte[] targets = new byte[2];
        for (int i = 0; i < 2; i++) {
            int offset = (targets.length - 1 - i) * 8;
            targets[1-i] = (byte) ((s >>> offset) & 0xff);
        }
        return targets;
    }

    public static byte[] string2Byte(String str) {
        if (str == null) {
            return null;
        }
        byte[] byteArray;
        try {
            byteArray = str.getBytes("unicode");
        } catch (Exception ex) {
            byteArray = str.getBytes();
            LogUtil.LogE("string2Byte","unicode编码转换错误",ex);
        }
//
//        for(int i=0;i<byteArray.length;i++)
//        {
//            byte temp=byteArray[i];
//            byteArray[i]=byteArray[i+1];
//            byteArray[i+1] =temp;
//            i++;
//        }
        return byteArray;
    }

    public static String byte2String(byte[] byteArray) {
        if (byteArray == null) {
            return null;
        }

        for (int i = 0; i < byteArray.length; i++) {
            byte temp = byteArray[i];
            byteArray[i] = byteArray[i + 1];
            byteArray[i + 1] = temp;
            i++;
        }
        String str;
        try {
            str = new String(byteArray, "unicode");
        } catch (Exception ex) {
            str = new String(byteArray);
        }
        return str;
    }

    private static final char[] HEX_CHAR = {'0', '1', '2', '3', '4', '5',
            '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};
    public static String bytes2HexString(byte[] bytes) {
        char[] buf = new char[bytes.length * 2];
        int index = 0;
        for(byte b : bytes) { // 利用位运算进行转换，可以看作方法一的变种
            buf[index++] = HEX_CHAR[b >>> 4 & 0xf];
            buf[index++] = HEX_CHAR[b & 0xf];
        }

        return new String(buf);
    }
}
