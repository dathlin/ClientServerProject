using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 软件系统浏览器模版.Models;

namespace 软件系统浏览器模版.Controllers
{


    /******************************************************************************
     * 
     *    Create Time:2017年8月18日 17:44:06
     *    Description:一个用于共享控制器的实现，可以实现一些模块，代码重用、
     *    目前主要包含了消息的分部视图
     * 
     *****************************************************************************/


       


    public class ShareController : Controller
    {
        public ActionResult Message(MessageBoxStyle style, string message)
        {
            ViewData["alertMessage"] = message;
            ViewData["Guid"] = Guid.NewGuid().ToString("N");
            switch (style)
            {
                case MessageBoxStyle.success: return PartialView("_MessageSuccessPartial");
                case MessageBoxStyle.info: return PartialView("_MessageInfoPartial");
                case MessageBoxStyle.warning: return PartialView("_MessageWarningPartial");
                default: return PartialView("_MessageDangerPartial");
            }
        }




    }
}