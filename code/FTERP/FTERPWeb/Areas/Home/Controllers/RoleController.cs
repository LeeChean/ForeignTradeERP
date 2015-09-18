using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Common;
using FTERPWeb.Home.ViewModels;
using FTERPWeb.Models;
using PetaPoco;
using FTERPCommon;
using FTERPWeb.Common.Filter;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class RoleController : Controller
    {
        #region 角色列表

        [UserAuthorize]
        [ValidateInput(false)]
        public ActionResult Index(Page<RoleModel> model)
        {
            #region 查询条件

            StringBuilder strCondition = new StringBuilder(" where Del_Flag = 0");

            //角色名称
            ViewBag.backType = string.IsNullOrWhiteSpace(Request.QueryString["backType"]) ? "0" : Request.QueryString["backType"];

            if (!string.IsNullOrWhiteSpace(Request.Form["roleName"]))
            {
                strCondition.Append(" and Name like '%")
                         .Append(System.Web.HttpUtility.HtmlEncode(Request.Form["roleName"]))
                         .Append("%'");

                ViewBag.backType = "1";
            }

            strCondition.Append(" order by Create_Time desc");

            #endregion

            if (model.CurrentPage == 0)
            {
                model.CurrentPage = 1;
            }
            ViewBag.CurrentPage = model.CurrentPage;

            var itemsPerPage = string.IsNullOrWhiteSpace(Request["itemsPerPage"]) ?
                SysConfig.ItemsPerPage : 0 == Request["itemsPerPage"].ToInt() ? 1 : Request["itemsPerPage"].ToInt();
            var list = RoleModel.Page(model.CurrentPage, itemsPerPage, strCondition.ToString());

            return View(list);
        }

        #endregion

        #region 添加角色

        [UserAuthorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View(new AddRoleModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public string Add(AddRoleModel model)
        {
            if (ModelState.IsValid)
            {
                RoleModel role = new RoleModel();
                role.Name = model.Name;                             //角色名称
                role.Remark = model.Remark;                         //备注
                role.CreateMan = SysConfig.CurrentUser.Id;          //创建人
                role.CreateTime = DateTime.Now;                     //创建时间
                role.DelFlag = 0;                                   //有效状态
                int result = role.Insert().ToInt();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_Role");

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 编辑角色

        [HttpGet]
        public ActionResult Edit()
        {
            string currentPage = string.IsNullOrWhiteSpace(Request["CurrentPage"]) ? "1" : Request["CurrentPage"];
            string backUrl = "/Home/Role/Index?backType=1&CurrentPage=" + currentPage;

            if (string.IsNullOrWhiteSpace(Request["id"]))
            {
                return Redirect(backUrl);
            }

            RoleModel role = RoleModel.SingleOrDefault(Request["id"]);
            if (null == role)
            {
                return Redirect(backUrl);
            }

            AddRoleModel model = new AddRoleModel();
            model.Id = role.Id;
            model.Name = role.Name;
            model.Remark = role.Remark;

            ViewBag.CurrentPage = Request["CurrentPage"];

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public string Edit(AddRoleModel model)
        {
            if (ModelState.IsValid)
            {
                RoleModel role = RoleModel.SingleOrDefault(model.Id.ToInt());
                if (null == role)
                {
                    return "0";
                }
                role.Name = model.Name;                             //角色名称
                role.Remark = model.Remark;                         //备注
                role.ModifyMan = SysConfig.CurrentUser.Id;          //修改人
                role.ModifyTime = DateTime.Now;                     //修改时间
                int result = role.Update().ToInt();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_Role");

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 删除角色

        [HttpPost]
        public string Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return "0";
            }

            if (RoleModel.Delete(string.Format("where ID in ({0})", id)) > 0)
            {
                //记录操作日志
                CommonMethod.Log(SysConfig.CurrentUser.Id, "Delete", "Sys_Role");

                return "1";
            }
            return "0";
        }

        #endregion

        #region 详情

        [HttpGet]
        public ActionResult Details()
        {
            string currentPage = string.IsNullOrWhiteSpace(Request["CurrentPage"]) ? "1" : Request["CurrentPage"];
            string backUrl = "/Home/Role/Index?backType=1&CurrentPage=" + currentPage;
            ViewBag.backUrl = backUrl;
            if (string.IsNullOrWhiteSpace(Request["roleId"]))
            {
                return Redirect(backUrl);
            }

            RoleModel model = RoleModel.SingleOrDefault(Request["roleId"]);
            if (null == model)
            {
                return Redirect(backUrl);
            }

            ViewBag.CurrentPage = Request["CurrentPage"];

            return View(model);
        }

        #endregion

    }
}
