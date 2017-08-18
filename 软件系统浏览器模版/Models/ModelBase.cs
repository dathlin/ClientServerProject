using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 软件系统浏览器模版.Models
{


    /***********************************************************************************
     * 
     *    基础的数据核心
     * 
     ***********************************************************************************/




    public class ModelBase
    {


    }



    /// <summary>
    /// 消息框的状态
    /// </summary>
    public enum MessageBoxStyle
    {
        /// <summary>
        /// 成功的消息
        /// </summary>
        success,
        /// <summary>
        /// 信息提示
        /// </summary>
        info,
        /// <summary>
        /// 警告消息
        /// </summary>
        warning,
        /// <summary>
        /// 错误消息
        /// </summary>
        danger
    }

    


    /// <summary>
    /// Session的数据集合的描述
    /// </summary>
    public class SessionItemsDescription
    {
        public static string UserAccount { get; set; } = "SoftUserAccount";
    }
}