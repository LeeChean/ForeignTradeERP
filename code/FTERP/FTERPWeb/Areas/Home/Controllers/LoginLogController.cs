using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Common.Filter;
using PetaPoco;
using FTERPWeb.Home.ViewModels;
using System.Text;
using FTERPWeb.Common;
using System.Net;
using FTERPCommon;
using System.IO;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class LoginLogController : Controller
    {
        [UserAuthorize]
        [ValidateInput(false)]
        public ActionResult Index(Page<LoginLogIndexModel> model)
        {
            #region 获取登录日志列表

            ViewBag.backType = string.IsNullOrWhiteSpace(Request.QueryString["backType"]) ? "0" : Request.QueryString["backType"];

            StringBuilder sql = new StringBuilder();
            sql.Append(" select u.Code,")
               .Append("       u.Name as UserName,")
               .Append("       d.Full_Name as Department,")
               .Append("       ll.Login_IP as LoginIP,")
               .Append("       Convert(nvarchar(16),ll.Login_Time,20) as LoginTime ")
               .Append(" from Sys_Login_Log ll ")
               .Append(" left join Sys_User u on ll.UserID = u.ID ")
               .Append(" left join Sys_Department d on u.DepartmentID = d.ID ")
               .Append(" where 1 = 1 ");

            //登录名
            if (!string.IsNullOrWhiteSpace(Request.Form["code"]))
            {
                sql.Append(" and u.Code like '%").Append(Server.HtmlEncode(Request.Form["code"])).Append("%' ");

                ViewBag.backType = "1";
            }
            //姓名
            if (!string.IsNullOrWhiteSpace(Request.Form["userName"]))
            {
                sql.Append(" and u.Name like '%").Append(Server.HtmlEncode(Request.Form["userName"])).Append("%' ");

                ViewBag.backType = "1";
            }

            //部门
            if (!string.IsNullOrWhiteSpace(Request.Form["department"]))
            {
                sql.Append(" and d.Name like '%").Append(Server.HtmlEncode(Request.Form["department"])).Append("%' ");

                ViewBag.backType = "1";
            }

            //登录IP
            if (!string.IsNullOrWhiteSpace(Request.Form["loginIP"]))
            {
                sql.Append(" and ll.Login_IP like '%").Append(Server.HtmlEncode(Request.Form["loginIP"])).Append("%' ");

                ViewBag.backType = "1";
            }
            //登录时间
            if (!string.IsNullOrWhiteSpace(Request.Form["startDate"]))
            {
                sql.Append(" and ll.Login_Time >= '").Append(Server.HtmlEncode(Request.Form["startDate"])).Append("'");

                ViewBag.backType = "1";
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["endDate"]))
            {
                sql.Append(" and ll.Login_Time <= '").Append(Server.HtmlEncode(Request.Form["endDate"])).Append("'");

                ViewBag.backType = "1";
            }

            sql.Append(" order by ll.Login_Time desc");

            #region 导出

            if (!string.IsNullOrWhiteSpace(Request["exportFlag"]) && Request["exportFlag"] == "1")
            {
                List<LoginLogIndexModel> logList = LoginLogIndexModel.Fetch(sql.ToString());

                //标题行
                List<string> titleList = new List<string>();
                titleList.Add("Code");
                titleList.Add("UserName");
                titleList.Add("Department");
                titleList.Add("LoginIP");
                titleList.Add("LoginTime");

                string fileName = Path.Combine("登录日志信息", DateTime.Now.ToString("yyyyMMddHHmmss"));

                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                ExcelHelper.DataToExcel(logList, "", titleList, fileName, this.HttpContext);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            #endregion

            if (model.CurrentPage == 0)
            {
                model.CurrentPage = 1;
            }

            var itemsPerPage = string.IsNullOrWhiteSpace(Request["itemsPerPage"]) ?
                SysConfig.ItemsPerPage : 0 == Request["itemsPerPage"].ToInt() ? 1 : Request["itemsPerPage"].ToInt();
            var items = LoginLogIndexModel.Page(model.CurrentPage, itemsPerPage, sql.ToString());

            ViewBag.CurrentPage = model.CurrentPage;

            return View(items);

            #endregion
        }

    }
}
