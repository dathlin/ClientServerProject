using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 软件系统浏览器模版.Models.Account;

namespace 软件系统浏览器模版.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 网站的主界面
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 网站的关于界面
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// 网站的联系人界面
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// 系统更新日志
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        public ActionResult UpdateLog()
        {
            return View();
        }

        /// <summary>
        /// 版本号说明
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        public ActionResult VersionInformation()
        {
            return View();
        }

        /// <summary>
        /// 一个错误的消息界面
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ActionResult ErrorPage(string message)
        {
            ViewBag.Message = message;
            return View("Error");
        }


        //GET
        /// <summary>
        /// 获取意见反馈的界面
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        public ActionResult AdviceFeedback()
        {
            return View();
        }

    }
}