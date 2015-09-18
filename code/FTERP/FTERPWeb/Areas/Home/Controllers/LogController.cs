using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Common;
using FTERPWeb.Common.Filter;
using PetaPoco;
using FTERPCommon;
using FTERPWeb.Home.ViewModels;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class LogController : Controller
    {
        [UserAuthorize]
        [ValidateInput(false)]
        public ActionResult Index(Page<LogIndexModel> model)
        {
            #region 获取操作日志列表

            ViewBag.backType = string.IsNullOrWhiteSpace(Request.QueryString["backType"]) ? "0" : Request.QueryString["backType"];

            StringBuilder sql = new StringBuilder();
            sql.Append(" select u.Code,")
               .Append("        u.Name as UserName,")
               .Append("        l.Remark,")
               .Append("        Convert(nvarchar(16),l.Create_Time,20) as CreateTime ")
               .Append(" from Sys_Log l ")
               .Append(" left join Sys_User u on l.Create_Man = u.ID ")
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

            //操作描述
            if (!string.IsNullOrWhiteSpace(Request.Form["remark"]))
            {
                sql.Append(" and l.Remark like '%").Append(Server.HtmlEncode(Request.Form["remark"])).Append("%' ");

                ViewBag.backType = "1";
            }

            //操作时间
            if (!string.IsNullOrWhiteSpace(Request.Form["startDate"]))
            {
                sql.Append(" and l.Create_Time >= '").Append(Server.HtmlEncode(Request.Form["startDate"])).Append("'");

                ViewBag.backType = "1";
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["endDate"]))
            {
                sql.Append(" and l.Create_Time <= '").Append(Server.HtmlEncode(Request.Form["endDate"])).Append("'");

                ViewBag.backType = "1";
            }

            sql.Append(" order by l.Create_Time desc");

            #region 导出

            if (!string.IsNullOrWhiteSpace(Request["exportFlag"]) && Request["exportFlag"] == "1")
            {
                List<LogIndexModel> logList = LogIndexModel.Fetch(sql.ToString());

                //标题行
                List<string> titleList = new List<string>();
                titleList.Add("Code");
                titleList.Add("UserName");
                titleList.Add("Remark");
                titleList.Add("CreateTime");

                string fileName = Path.Combine("操作日志信息", DateTime.Now.ToString("yyyyMMddHHmmss"));

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
            var items = LogIndexModel.Page(model.CurrentPage, itemsPerPage, sql.ToString());

            ViewBag.CurrentPage = model.CurrentPage;

            return View(items);

            #endregion
        }

    }
}
