using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Home.ViewModels;
using FTERPWeb.Models;
using FTERPCommon;
using FTERPWeb.Common;
using FTERPWeb.Common.Filter;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class DepAccessController : Controller
    {
        [UserAuthorize]
        public ActionResult Index()
        {
            #region 获取所有角色

            List<SelectListItem> roleList = new List<SelectListItem>();
            List<RoleModel> list = RoleModel.Fetch("where Del_Flag = 0");

            foreach (RoleModel item in list)
            {
                roleList.Add(new SelectListItem() { Text = item.Name, Value = item.Id });
            }
            ViewBag.roleList = roleList;

            #endregion

            return View();
        }

        [HttpGet]
        public ActionResult GetDepTree(string belongDepId, string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId) || string.IsNullOrWhiteSpace(belongDepId))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            string sql = @"select distinct d.ID as Id,
                                           d.PID as Pid,
                                           d.Name,
                                           case when da.ID is null then 'false' else 'true' end as Checked,
                                           d.SortNo
                           from Sys_Department d
                           left join Sys_DepAccess da on d.ID = da.DepID and da.BelongDepID = @0 
                             and da.RoleID = @1
                           where d.Del_Flag = 0 order by d.SortNo";

            List<DepartmentAccessModel> model = DepartmentAccessModel.Fetch(sql, belongDepId, roleId);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Add()
        {
            if (string.IsNullOrWhiteSpace(Request["roleId"]) || string.IsNullOrWhiteSpace(Request["belongDepId"]))
            {
                return "0";
            }

            string[] depIds = Request["depIds"] == null ? null : Request["depIds"].Split(',');

            DepaccessModel.Delete(" where RoleID = @0 and BelongDepID = @1", Request["roleId"], Request["belongDepId"]);
            foreach (string item in depIds)
            {
                DepaccessModel depAccess = new DepaccessModel();
                depAccess.Belongdepid = Request["belongDepId"].ToInt();
                depAccess.Roleid = Request["roleId"].ToInt();
                depAccess.Depid = item.ToInt();
                depAccess.CreateMan = SysConfig.CurrentUser.Id;
                depAccess.CreateTime = DateTime.Now;
                int result = depAccess.Insert().ToInt();

                if (result <= 0)
                {
                    return "0";
                }
            }

            //记录操作日志
            CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_DepAccess");

            return "1";
        }
    }
}
