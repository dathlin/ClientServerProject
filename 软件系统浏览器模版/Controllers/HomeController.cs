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
            ViewBag.Message = "Your application description page.";

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


    }
}