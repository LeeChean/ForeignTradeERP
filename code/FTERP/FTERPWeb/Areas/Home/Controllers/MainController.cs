using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Common;
using FTERPWeb.Home.ViewModels;
using FTERPWeb.Models;
using FTERPWeb.Common.Filter;
using FTERPCommon;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class MainController : Controller
    {
        [TimeoutAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NoAuth()
        {
            ViewData["time"] = 3;
            return View();
        }

        public ActionResult Timeout()
        {
            ViewData["time"] = 3;
            return View();
        }

        #region 修改密码

        [HttpGet]
        public ActionResult EditPwd()
        {
            return View(new EditPwdViewModel());
        }

        [HttpPost]
        public string EditPwd(string NewPwd)
        {
            if (ModelState.IsValid)
            {
                UserModel user = SysConfig.CurrentUser;
                user.Password = NewPwd.VariationMd5();
                user.ModifyMan = SysConfig.CurrentUser.Id;
                user.ModifyTime = DateTime.Now;
                user.Update();

                //记录操作日志
                CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_User", "修改密码");

                return "1";
            }
            return "0";
        }

        #endregion

        [HttpGet]
        public ActionResult UnapprovedIndex()
        {
            IndexModel model = new IndexModel();

            //获取单据类别列表
            //model.DocTypeList = DocumentTypeModel.Fetch("where Del_Flag = 0");

            //获取当前用户的待审批列表
            model.UnapprovedList = CommonMethod.GetUnapprovedList(SysConfig.CurrentUser.Departmentid.ToInt(), SysConfig.CurrentRoleIds);

            return View(model);
        }
    }
}
