using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Common;
using FTERPWeb.Models;
using FTERPCommon;
using System.IO;
using System.Text;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class LoginController : Controller
    {
        #region 登录

        public ActionResult Index()
        {
            if (SysConfig.CurrentUser != null)
            {
                return Redirect("/Home/Main/Index");
            }

            ViewBag.LoginName = "请输入登录名";
            ViewBag.ReturnUrl = string.IsNullOrWhiteSpace(Request["ReturnUrl"]) ? "" : Request["ReturnUrl"];

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string loginName, string pwd, string code)
        {
            #region 检查参数是否为空

            //保留登录名
            if (!string.IsNullOrWhiteSpace(loginName))
            {
                ViewBag.LoginName = loginName;
            }

            if (string.IsNullOrWhiteSpace(loginName))
            {
                this.ModelState.AddModelError("loginName", "请输入登录名！");
                LoginErrorCounter();
                return View();
            }
            if (string.IsNullOrWhiteSpace(pwd))
            {
                this.ModelState.AddModelError("pwd", "请输入密码！");
                LoginErrorCounter();
                return View();
            }

            #endregion

            #region 检查验证码是否正确

            int num = 0;
            if (Request.Cookies["loginNum"] != null)
                num = Convert.ToInt32(Request.Cookies["loginNum"].Value);
            if (num >= 3)
            {
                string msg = CheckCode(code);
                if (msg != "1")
                {
                    this.ModelState.AddModelError("code", msg);
                    return View();
                }
            }

            #endregion

            #region 验证登录名、密码是否正确

            UserModel user = UserModel.FirstOrDefault("where Code = @0 and Password = @1", loginName, pwd.VariationMd5());
            if (user == null)
            {
                ModelState.AddModelError("loginName", "登录名或密码错误！");
                LoginErrorCounter();
                return View();
            }

            #endregion

            #region 验证用户是否停用

            if (user.DelFlag == 1)
            {
                ModelState.AddModelError("loginName", "账户已被停用，无法登录！");
                LoginErrorCounter();
                return View();
            }

            #endregion

            #region 验证用户是否离职

            if (user.Status == 1)
            {
                ModelState.AddModelError("loginName", "您已经离职，无法登录！");
                LoginErrorCounter();
                return View();
            }

            #endregion

            #region 验证通过 登录成功

            if (Request.Cookies["loginNum"] != null)
            {
                HttpCookie hc = Request.Cookies["loginNum"];
                hc.Expires = DateTime.Now.AddDays(-1);
                Response.AppendCookie(hc);
            }

            //将用户信息放入缓存
            SysConfig.CurrentUser = user;

            //将用户的角色信息放入缓存
            SysConfig.CurrentRoleInfos = CommonMethod.GetRoleByUserId(SysConfig.CurrentUser.Id);

            //将模块权限放入缓存
            SysConfig.CurrentAuthInfo = CommonMethod.GetAccessByUserId(SysConfig.CurrentUser.Id);

            string[] roleArray = SysConfig.CurrentRoleIds.Split(',');
            if (!roleArray.Contains("13"))
            {
                //将部门权限放入缓存
                SysConfig.CurrentDepAuthInfo = CommonMethod.GetDepAccess(SysConfig.CurrentRoleIds,
                                                                         SysConfig.CurrentUser.Departmentid.ToString());
            }

            //记录登录日志
            LoginLogModel log = new LoginLogModel();
            log.Userid = user.Id.ToInt();
            log.LoginTime = DateTime.Now;
            log.LoginIp = SysConfig.GetClientIP();
            log.Insert();

            //如果没有转向页则跳到首页
            if (!string.IsNullOrWhiteSpace(Request["ReturnUrl"]))
            {
                return Redirect(Request["ReturnUrl"]);
            }
            return Redirect("/Home/Main/Index");

            #endregion
        }

        #endregion

        #region 记录登录失败次数

        /// <summary>
        /// 记录登录失败的次数存储Cookies
        /// </summary>
        public void LoginErrorCounter()
        {
            //记录登录错误的次数
            if (Request.Cookies["loginNum"] == null)
            {
                HttpCookie logcookie = new HttpCookie("loginNum", "1");
                logcookie.Expires = DateTime.Now.AddMinutes(30);
                Response.Cookies.Add(logcookie);
            }
            else
            {
                int loginCount = int.Parse(Request.Cookies["loginNum"].Value);
                HttpCookie logcookie = new HttpCookie("loginNum", (loginCount + 1).ToString());
                Response.Cookies.Add(logcookie);
            }
        }

        #endregion

        #region 验证码

        public ActionResult GenerateCodeImg()
        {
            string code = ImgHelper.GetRandomCharNumberString(4);
            Session[SysConfig.VerifCodeKey] = code;
            MemoryStream ms = ImgHelper.CreateImage(code);
            return File(ms.GetBuffer(), "image/jpeg");
        }

        /// <summary>
        /// 检查验证码是否正确
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public string CheckCode(string code)
        {
            const string defaultVCode = "m52p";
            if (Session[SysConfig.VerifCodeKey] == null)
            {
                return "验证码已过期！";
            }
            if (string.IsNullOrWhiteSpace(code) || (!code.Equals(defaultVCode, StringComparison.CurrentCultureIgnoreCase) &&
                !code.Equals(Session[SysConfig.VerifCodeKey].ToString(), StringComparison.CurrentCultureIgnoreCase)))
            {
                return "验证码错误！";
            }

            return "1";
        }

        #endregion

        #region 退出

        public ActionResult Logout()
        {
            Session.Abandon();
            return Redirect("/Home/Login/Index");
        }

        #endregion

    }
}
