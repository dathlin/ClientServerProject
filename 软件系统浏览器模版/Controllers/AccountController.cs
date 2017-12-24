using ClientsLibrary;
using CommonLibrary;
using HslCommunication;
using HslCommunication.BasicFramework;
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



    /// <summary>
    /// 账户相关的控制器
    /// </summary>
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }


        // GET: LoginPage
        public ActionResult Login(string Message)
        {
            ViewData["Message"] = Message;
            return View();
        }



        //POST 用于系统的账户登录
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection fc)
        {
            //请求指令头数据，该数据需要更具实际情况更改
            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.维护检查);
            if (result.IsSuccess)
            {
                //例如返回结果为1说明允许登录，0则说明服务器处于维护中，并将信息显示
                if (result.Content != "1")
                {
                    return Login(result.Message);
                }
            }
            else
            {
                //访问失败
                return Login(result.Message);
            }

            //检查账户
            //包装数据
            JObject json = new JObject
            {
                { UserAccount.UserNameText, new JValue(fc["UserName"]) },
                { UserAccount.PasswordText, new JValue(fc["UserPassword"]) },
                { UserAccount.LoginWayText, new JValue("webApp") },
                { UserAccount.DeviceUniqueID, new JValue(UserClient.JsonSettings.SystemInfo) },        // 客户端唯一ID
                { UserAccount.FrameworkVersion, new JValue(SoftBasic.FrameworkVersion.ToString()) }    // 客户端框架版本
            };
            result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.账户检查, json.ToString());
            if (result.IsSuccess)
            {
                //服务器应该返回账户的信息
                UserAccount account = JObject.Parse(result.Content).ToObject<UserAccount>();
                if (!account.LoginEnable)
                {
                    //不允许登录
                    return Login(account.ForbidMessage);
                }
                Session[SessionItemsDescription.UserAccount] = account;
                //UserClient.UserAccount = account;
            }
            else
            {
                //访问失败
                return Login(result.Message);
            }


            result = UserClient.Net_simplify_client.ReadFromServer(CommonLibrary.CommonHeadCode.SimplifyHeadCode.更新检查);
            if (result.IsSuccess)
            {
                //服务器应该返回服务器的版本号
                SystemVersion sv = new SystemVersion(result.Content);
                //系统账户跳过低版本检测
                if (fc["UserName"] != "admin")
                {
                    if (UserClient.CurrentVersion != sv)
                    {
                        return Login("当前版本号不正确，需要联系管理员更新服务器才允许登录。");
                    }
                }
                else
                {
                    if (UserClient.CurrentVersion < sv)
                    {
                        return Login("版本号过时，需要联系管理员更新服务器才允许登录。");
                    }
                }
            }
            else
            {
                //访问失败
                return Login(result.Message);
            }


            result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.参数下载);
            if (result.IsSuccess)
            {
                //服务器返回初始化的数据，此处进行数据的提取，有可能包含了多个数据
                json = JObject.Parse(result.Content);
                //例如公告数据
                UserClient.Announcement = SoftBasic.GetValueFromJsonObject(json, nameof(UserClient.Announcement), "");
                CommonLibrary.DataBaseSupport.SqlServerSupport.ConnectionString = SoftBasic.GetValueFromJsonObject(json, nameof(ServerSettings.SqlServerStr), "");
            }
            else
            {
                //访问失败
                //访问失败
                return Login(result.Message);
            }

            //允许登录，并记录到Session
            return RedirectToAction("Index", "Home");
        }

        //POST 用于退出系统的账户
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关
        /// <summary>
        /// 注销账户的方法
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(FormCollection fc)
        {
            Session[SessionItemsDescription.UserAccount] = null;
            return RedirectToAction("Login", "Account");
        }

        //GET 查看账号详细信息的界面
        /// <summary>
        /// 查看账户的详细信息
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        public ActionResult AccountDetail()
        {
            ViewData[SessionItemsDescription.UserAccount] = Session[SessionItemsDescription.UserAccount];
            return View();
        }


        //GET
        /// <summary>
        /// 更改账户的密码的页面，更改密码之前需要对旧的密码进行验证
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }


        //POST 这是一个Ajax的请求
        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public ActionResult CheckPassword(string InputPassword)
        {
            if (Request.IsAjaxRequest())
            {
                UserAccount account = Session[SessionItemsDescription.UserAccount] as UserAccount;
                if (InputPassword == account.Password)
                {
                    //允许修改密码
                    return PartialView("SetPasswordPartial");
                }
                else
                {
                    return PartialViewMessage(MessageBoxStyle.warning, "密码验证失败！");
                }
            }
            else
            {
                return PartialViewMessage(MessageBoxStyle.danger, "请求无效！");
            }
        }


        //POST 这是一个Ajax的请求
        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public ActionResult SetNewPassword(string inputPassword1, string inputPassword2)
        {
            if (Request.IsAjaxRequest())
            {
                UserAccount account = Session[SessionItemsDescription.UserAccount] as UserAccount;
                if (inputPassword1 != inputPassword2)
                {
                    return PartialViewMessage(MessageBoxStyle.warning, "两次密码不一致！");
                }
                if (inputPassword1.Length < 5 || inputPassword1.Length > 8)
                {
                    return PartialViewMessage(MessageBoxStyle.warning, "密码位数错误，应该在5-8位！");
                }

                if(!System.Text.RegularExpressions.Regex.IsMatch(inputPassword1, "^[A-Za-z0-9]+$"))
                {
                    return PartialViewMessage(MessageBoxStyle.warning, "密码包含了特殊字符，只能是字母数字。");
                }

                JObject json = new JObject
                    {
                        { UserAccount.UserNameText, UserClient.UserAccount.UserName },
                        { UserAccount.PasswordText, inputPassword1 }
                    };

                OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.密码修改, json.ToString());
                if (result.IsSuccess)
                {
                    return PartialViewMessage(MessageBoxStyle.success, "密码修改成功！");
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
        /// 注册新的账户界面
        /// </summary>
        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult RegisterAccount()
        {
            return View();
        }


        //POST
        /// <summary>
        /// 在用户点击注册账户之后显示的界面
        /// </summary>
        [HttpPost]
        [AuthorizeAdmin]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterAccount(FormCollection fc)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    UserAccount account = new UserAccount();
                    account.UserName = fc["username"];
                    account.NameAlias = fc["alias"];
                    account.Password = fc["password"];
                    account.Factory = fc["factory"];
                    account.Grade = int.Parse(fc["grade"]);
                    account.LoginEnable = bool.Parse(fc["loginEnable"]);
                    account.ForbidMessage = fc["reason"];
                    OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.注册账号, account.ToJsonString());
                    if (result.IsSuccess && result.Content == "1")
                    {
                        return PartialViewMessage(MessageBoxStyle.success, "账户注册成功！");
                    }
                    else
                    {
                        return PartialViewMessage(MessageBoxStyle.warning, "账户注册失败！");
                    }
                }
                catch
                {
                    return PartialViewMessage(MessageBoxStyle.danger, "数据异常！");
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