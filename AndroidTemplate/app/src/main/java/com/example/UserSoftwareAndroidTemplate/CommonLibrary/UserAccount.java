package com.example.UserSoftwareAndroidTemplate.CommonLibrary;

import com.google.gson.Gson;

import java.util.Date;

/**
 * Created by hsl20 on 2017/11/5.
 */

public class UserAccount {

    /// <summary>
    /// 用户名称，该名称唯一
    /// </summary>
    public String UserName = "";
    /// <summary>
    /// 用户名称的别名，也不可以不使用
    /// </summary>
    public String NameAlias = "";
    /// <summary>
    /// 用户登录的密码
    /// </summary>
    public String Password = "";
    /// <summary>
    /// 账户所属的工厂名称或类别名称
    /// </summary>
    public String Factory = "";
    /// <summary>
    /// 用户的权限等级，目前配置了4个等级
    /// </summary>
    public int Grade = 0;
    /// <summary>
    /// 用户的手机号
    /// </summary>
    public String Phone = "";
    /// <summary>
    /// 用户的电子邮件
    /// </summary>
    public String EMail ="";
    /// <summary>
    /// 该用户的注册日期，一旦注册，应该固定
    /// </summary>
    public Date RegisterTime = new Date();
    /// <summary>
    /// 该用户是否允许登录
    /// </summary>
    public boolean LoginEnable = false;
    /// <summary>
    /// 该用户不允许被登录的原因
    /// </summary>
    public String ForbidMessage = "该账户被管理员禁止登录！";
    /// <summary>
    /// 该用户自注册以来登录的次数
    /// </summary>
    public int LoginFrequency = 0;
    /// <summary>
    /// 该用户上次登录的时间
    /// </summary>
    public Date LastLoginTime = new Date();
    /// <summary>
    /// 该用户上次登录的IP地址
    /// </summary>
    public String LastLoginIpAddress = "";
    /// <summary>
    /// 该用户连续登录失败的计数，可以用来连续五次失败禁止账户登录
    /// </summary>
    public int LoginFailedCount = 0;
    /// <summary>
    /// 上次登录系统的方式，有winform版，wpf版，web版，Android版
    /// </summary>
    public String LastLoginWay = "";
    /// <summary>
    /// 小尺寸头像的MD5码
    /// </summary>
    public String SmallPortraitMD5 = "";
    /// <summary>
    /// 大尺寸头像的MD5码
    /// </summary>
    public String LargePortraitMD5 = "";



    /// <summary>
    /// 用于存储和传送的名称
    /// </summary>
    public static String UserNameText = "UserName";
    /// <summary>
    /// 用于存储和传送的名称
    /// </summary>
    public static String PasswordText = "Password";
    /// <summary>
    /// 用于存储和传送的名称
    /// </summary>
    public static String LoginWayText = "LoginWay";
    /// <summary>
    /// 登录系统的唯一设备ID
    /// </summary>
    public static String DeviceUniqueID = "DeviceUniqueID";
    /// <summary>
    /// 小尺寸头像的MD5传送名称
    /// </summary>
    public static String SmallPortraitText = "SmallPortrait";
    /// <summary>
    /// 大尺寸头像的MD5传送名称
    /// </summary>
    public static String LargePortraitText = "LargePortrait";
    /// <summary>
    /// 系统的框架版本，框架版本不一致，由服务器决定是否允许客户端登录
    /// </summary>
    public static String FrameworkVersion = "FrameworkVersion";



    /// <summary>
    /// 获取本账号的JSON字符串，用于在网络中数据传输
    /// </summary>
    /// <returns></returns>
    public String ToJsonString()
    {
        return new Gson().toJson(this);
    }


    /// <summary>
    /// 获取账号的用户名
    /// </summary>
    /// <returns></returns>
    public String toString()
    {
        return UserName;
    }

}
