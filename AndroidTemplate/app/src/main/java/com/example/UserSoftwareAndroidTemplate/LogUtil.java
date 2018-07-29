package com.example.UserSoftwareAndroidTemplate;

import android.util.Log;

public class LogUtil {
    public static final int VerBose = 1;

    public static final int Debug = 2;

    public static final int Info = 3;

    public static final int Warn = 4;

    public static final int Error = 5;

    public static final int Nothing = 6;

    public static int Level = VerBose;

    /**
        记录零碎的日志
     **/
    public static void LogV(String tag, String msg) {
        if (Level <= VerBose) Log.v(tag, msg);
    }

    /*
        记录调试日志
     */
    public static void LogD(String tag, String msg) {
        if (Level <= Debug) Log.d(tag, msg);
    }

    /*
        记录信息日志
     */
    public static void LogI(String tag, String msg) {
        if (Level <= Info) Log.i(tag, msg);
    }

    /*
        记录警告日志
     */
    public static void LogW(String tag, String msg) {
        if (Level <= Warn) Log.w(tag, msg);
    }

    /*
        记录一般错误日志
     */
    public static void LogE(String tag, String msg) {
        if (Level <= Error) Log.e(tag, msg);
    }

    /*
        记录带信息异常的错误日志
     */
    public static void LogE(String tag, String msg, Exception ex) {
        if (Level <= Error) Log.e(tag, msg, ex);
    }
}
