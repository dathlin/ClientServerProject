package com.example.UserSoftwareAndroidTemplate;


import com.example.UserSoftwareAndroidTemplate.CommonLibrary.UserAccount;

import HslCommunication.BasicFramework.SystemVersion;
import HslCommunication.Enthernet.SimplifyNet.NetSimplifyClient;


import java.util.List;
import java.util.UUID;

/**
 * Created by hsl20 on 2017/11/4.
 */

public class UserClient {


    public static SystemVersion FrameworkVersion = new SystemVersion("1.7.5");
    public static SystemVersion CurrentVersion = new SystemVersion("1.0.0.171026");
    public static String SettingsFileName = "Settings";


    public static String ServerIp = "192.168.1.110";
    public static int PortSecondary = 14568;
    public static UUID Token = UUID.fromString("1275BB9A-14B2-4A96-9673-B0AF0463D474");


    public static List<String> SystemFactories = null; // 分厂
    public static String Announcement = ""; // 公告
    public static NetSimplifyClient Net_simplify_client = new NetSimplifyClient(ServerIp, PortSecondary,Token);
    public static UserAccount UserAccount = null;
}
