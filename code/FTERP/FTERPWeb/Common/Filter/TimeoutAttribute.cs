using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FTERPWeb.Common.Filter
{
    public class TimeoutAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string reLoginUrl = "/Home/Login/Index";

            //如果没有登录
            if (SysConfig.CurrentUser == null)
            {
                filterContext.Result = new RedirectResult(reLoginUrl);
                return;
            }
        }
    }
}