using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Home.ViewModels;
using FTERPWeb.Models;
using FTERPCommon;
using FTERPWeb.Common.Filter;
using FTERPWeb.Common;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class AccessController : Controller
    {
        [UserAuthorize]
        public ActionResult Index()
        {
            #region 获取所有角色

            List<SelectListItem> roleList = new List<SelectListItem>();
            List<RoleModel> list = RoleModel.Fetch("where Del_Flag = 0");

            roleList.Add(new SelectListItem() { Text = "请选择", Value = "" });
            foreach (RoleModel item in list)
            {
                roleList.Add(new SelectListItem() { Text = item.Name, Value = item.Id });
            }
            ViewBag.roleList = roleList;

            #endregion

            return View();
        }

        [HttpGet]
        public ActionResult GetFuncTree(string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            string sql = @"select distinct f.ID,
                                  f.PID,
                                  f.Title,
                                  case when a.id is null then 'false' else 'true' end as Checked,
                                  f.SortNo
                           from Sys_Func f
                           left join Sys_Access a on f.ID = a.FuncID and a.RoleID = @0
                           where f.Del_Flag = 0 order by f.SortNo";

            List<RoleAccessModel> model = RoleAccessModel.Fetch(sql, roleId);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Add()
        {
            if (string.IsNullOrWhiteSpace(Request["roleId"]))
            {
                return "0";
            }

            string[] funcIds = Request["funcIds"] == null ? null : Request["funcIds"].Split(',');

            AccessModel.Delete(" where RoleID = @0", Request["roleId"]);
            foreach (string item in funcIds)
            {
                AccessModel access = new AccessModel();
                access.Roleid = Request["roleId"].ToInt();
                access.Funcid = item.ToInt();
                access.CreateMan = SysConfig.CurrentUser.Id;
                access.CreateTime = DateTime.Now;
                int result = access.Insert().ToInt();

                if (result <= 0)
                {
                    return "0";
                }
            }

            //记录操作日志
            CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_Access");

            return "1";
        }

    }
}
