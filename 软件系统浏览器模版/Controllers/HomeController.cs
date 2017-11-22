using ClientsLibrary;
using CommonLibrary;
using HslCommunication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 软件系统浏览器模版.Models;
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

        //Get
        /// <summary>
        /// 权限不足时显示的界面
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        public ActionResult LackOfAuthority()
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


        //POST
        /// <summary>
        /// 获取意见反馈的界面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public ActionResult AdviceFeedback(string advice)
        {
            if (Request.IsAjaxRequest())
            {
                //对建议进行保存
                HslCommunication.OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.意见反馈, UserClient.UserAccount.UserName + ":" + advice);
                if (result.IsSuccess)
                {
                    return PartialViewMessage(MessageBoxStyle.success, "建议提交成功！");
                }
                else
                {
                    return PartialViewMessage(MessageBoxStyle.danger, result.Message);
                }
            }
            else
            {
                return PartialViewMessage(MessageBoxStyle.danger, "请求无效！");
            }
        }

        //GET
        /// <summary>
        /// 设置新的公告的页面
        /// </summary>
        [HttpGet]
        [AuthorizeUser]
        public ActionResult ChangeAnnouncement()
        {
            return View();
        }


        //POST
        /// <summary>
        /// 设置新的公告内容的界面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public ActionResult SetAnnouncement(FormCollection fc)
        {
            if (Request.IsAjaxRequest())
            {
                string announcement = fc["Announcement"];
                UserAccount account = Session[SessionItemsDescription.UserAccount] as UserAccount;

                if (announcement.Length > 1000)
                {
                    return PartialViewMessage(MessageBoxStyle.warning, "公告的字数超过了1000字！");
                }


                OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.更新公告, announcement);
                if (result.IsSuccess)
                {
                    UserClient.Announcement = announcement;
                    return PartialViewMessage(MessageBoxStyle.success, "公告更改成功！");
                }
                else
                {
                    return PartialViewMessage(MessageBoxStyle.danger, result.Message);
                }
            }
            else
            {
                return PartialViewMessage(MessageBoxStyle.danger, "请求无效！");
            }
        }



        //GET
        /// <summary>
        /// 获取账号管理的界面
        /// </summary>
        [HttpGet]
        [AuthorizeUser]
        public ActionResult ManagementAccount()
        {
            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.获取账户);
            if(result.IsSuccess)
            {
                ViewData["accounts"] = result.Content;
            }
            else
            {
                ViewData["accounts"] = "数据获取失败：" + result.ToMessageShowString();
            }
            return View();
        }


        //POST
        /// <summary>
        /// 设置新的账户的请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public ActionResult SetManagementAccount(FormCollection fc)
        {
            if (Request.IsAjaxRequest())
            {
                string Accounts = fc["NewAccounts"];
                UserAccount account = Session[SessionItemsDescription.UserAccount] as UserAccount;
                

                OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.更细账户, Accounts);
                if (result.IsSuccess)
                {
                    return PartialViewMessage(MessageBoxStyle.success, "账户更改成功！");
                }
                else
                {
                    return PartialViewMessage(MessageBoxStyle.danger, result.Message);
                }
            }
            else
            {
                return PartialViewMessage(MessageBoxStyle.danger, "请求无效！");
            }
        }


        //GET
        /// <summary>
        /// 获取发送消息的界面
        /// </summary>
        [HttpGet]
        [AuthorizeUser]
        public ActionResult SendMessage()
        {
            return View();
        }


        //POST
        /// <summary>
        /// 设置新的消息发送的界面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(FormCollection fc)
        {
            if (Request.IsAjaxRequest())
            {
                string SendMessage = fc["SendMessage"];
                UserAccount account = Session[SessionItemsDescription.UserAccount] as UserAccount;

                if (SendMessage.Length > 1000)
                {
                    return PartialViewMessage(MessageBoxStyle.warning, "需要发送的字数超过了1000字！");
                }


                OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.群发消息, SendMessage);
                if (result.IsSuccess)
                {
                    UserClient.Announcement = SendMessage;
                    return PartialViewMessage(MessageBoxStyle.success, "消息群发成功！");
                }
                else
                {
                    return PartialViewMessage(MessageBoxStyle.danger, result.Message);
                }
            }
            else
            {
                return PartialViewMessage(MessageBoxStyle.danger, "请求无效！");
            }
        }



        private ActionResult PartialViewMessage(MessageBoxStyle style, string message)
        {
            return RedirectToAction("Message", "Share", new { style = style, message = message });
        }

    }
}