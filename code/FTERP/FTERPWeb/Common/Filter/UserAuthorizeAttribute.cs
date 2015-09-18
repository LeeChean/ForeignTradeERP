using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Models;

namespace FTERPWeb.Common.Filter
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string reLoginUrl = "/Home/Main/Timeout";
            const string noAuthUrl = "/Home/Main/NoAuth";

            //如果没有登录
            if (SysConfig.CurrentUser == null)
            {
                filterContext.Result = new RedirectResult(reLoginUrl);
                return;
            }

            var accessInfo = SysConfig.CurrentAuthInfo as List<FuncModel>;
            var level2Info = accessInfo.Where(s => s.FuncLevel == 2).ToList();
            var level3Info = accessInfo.Where(s => s.FuncLevel == 3).ToList();
            var action = filterContext.RouteData.Values["action"].ToString().ToLower();
            var controller = filterContext.RouteData.Values["controller"].ToString().ToLower();

            var level2 = level2Info.FirstOrDefault(s => s.Name.ToLower() == controller);

            //没有权限
            if (level2 == null)
            {
                filterContext.Result = new RedirectResult(noAuthUrl);
                return;
            }

            var level3 = level3Info.FirstOrDefault(s => s.Name.ToLower() == action && s.Pid == Convert.ToInt32(level2.Id));

            //没有权限
            if (level3 == null)
            {
                filterContext.Result = new RedirectResult(noAuthUrl);
                return;
            }
        }
    }
}