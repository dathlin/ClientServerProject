package com.example.UserSoftwareAndroidTemplate;

import com.example.HslCommunication.BasicFramework.SystemVersion;
import com.example.HslCommunication.Enthernet.NetSimplifyClient;

import java.util.UUID;

/**
 * Created by hsl20 on 2017/11/4.
 */

public class UserClient {


    public static SystemVersion CurrentVersion= new SystemVersion("1.0.0.171026");



    public static String ServerIp="117.48.203.204";
    public static int PortSecondary=14568;
    public static UUID Token=UUID.fromString("1275BB9A-14B2-4A96-9673-B0AF0463D474");









    public static NetSimplifyClient Client=new NetSimplifyClient(ServerIp,PortSecondary,Token);

}
