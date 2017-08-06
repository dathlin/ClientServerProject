using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace 软件系统浏览器模版.Models.Account
{
    public class ModelAccount
    {
    }

    /// <summary>
    /// 验证系统是否登录成功的特性
    /// </summary>
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if ((filterContext.HttpContext.Session[SessionItemsDescription.UserAccount] as UserAccount) != null)
            {
                //授权成功
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
        }
    }

    /// <summary>
    /// 验证系统的账户是否符合管理员的信息
    /// </summary>
    public class AuthorizeAdminAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session[SessionItemsDescription.UserAccount] is UserAccount account)
            {
                if (account.Grade < AccountGrade.SuperAdministrator)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "LackOfAuthority" }));
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
        }
    }
}