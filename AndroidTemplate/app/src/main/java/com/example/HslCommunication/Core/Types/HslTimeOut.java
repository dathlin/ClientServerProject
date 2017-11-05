package com.example.HslCommunication.Core.Types;

import java.net.Socket;
import java.util.Date;

/**
 * Created by DATHLIN on 2017/11/1.
 */

public class HslTimeOut {


    public HslTimeOut()
    {
        StartTime = new Date();
        IsSuccessful = false;
    }

    public Date StartTime=null;

    public boolean IsSuccessful=false;

    public int DelayTime=5000;

    public Socket WorkSocket=null;
}
