#undef DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Home.ViewModels;
using FTERPWeb.Models;
using FTERPWeb.Common;
using FTERPCommon;
using FTERPWeb.Common.Filter;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class DepartmentController : Controller
    {
        #region 部门维护

        [UserAuthorize]
        public ActionResult Index()
        {
            return View(new EditDepartmentModel());
        }

        #endregion

        #region 添加下级部门

        [HttpPost]
        [ValidateInput(false)]
        public string AddSub(EditDepartmentModel model)
        {
#if DEBUG
            UserModel user = UserModel.SingleOrDefault(14);
            SysConfig.CurrentUser = user;
#endif

            if (ModelState.IsValid)
            {
                DepartmentModel department = new DepartmentModel();
                department.Name = model.Name;                                                               //部门名称
                department.No = model.No;                                                                   //部门编号

                department.DocDepartment = model.DocDepartment.ToInt();                                     //是否业务部门
                department.Pid = model.Id;                                                                  //上级部门
                department.FullPid = model.FullPid + "-" + model.Id;                                        //所有上级部门

                department.FullName = CommonMethod.GetDepFullName(department.FullPid) + department.Name;    //部门全称
                department.Sortno = model.Sortno;                                                           //排序编号
                department.DepLevel = model.DepLevel + 1;                                                   //部门层级

                department.CreateMan = SysConfig.CurrentUser.Id;                                            //创建人
                department.CreateTime = DateTime.Now;                                                       //创建时间

                int result = (int)department.Insert();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_Department");

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 添加同级部门

        [HttpPost]
        [ValidateInput(false)]
        public string Add(EditDepartmentModel model)
        {
#if DEBUG
            UserModel user = UserModel.SingleOrDefault(14);
            SysConfig.CurrentUser = user;
#endif

            if (ModelState.IsValid)
            {
                DepartmentModel department = new DepartmentModel();
                department.FullPid = model.FullPid == null ? "0" : model.FullPid;     //所有上级部门
                department.Name = model.Name;                                         //部门名称
                department.No = model.No;                                             //部门编号

                department.DocDepartment = model.DocDepartment.ToInt();               //是否单据所属部门
                department.Pid = model.Pid == null ? "0" : model.Pid;                 //上级部门
                department.FullName = CommonMethod.GetDepFullName(department.FullPid) + department.Name;    //部门全称

                department.Sortno = model.Sortno;                                     //排序编号
                department.DepLevel = model.DepLevel == null ? 1 : model.DepLevel;    //部门层级
                department.CreateMan = SysConfig.CurrentUser.Id;                      //创建人
                department.CreateTime = DateTime.Now;                                 //创建时间

                int result = (int)department.Insert();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_Department");

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 编辑部门

        [HttpPost]
        [ValidateInput(false)]
        public string Edit(EditDepartmentModel model)
        {
#if DEBUG
            UserModel user = UserModel.SingleOrDefault(14);
            SysConfig.CurrentUser = user;
#endif

            if (ModelState.IsValid)
            {
                DepartmentModel department = DepartmentModel.SingleOrDefault(model.Id.ToInt());
                if (null == department)
                {
                    return "0";
                }

                department.Name = model.Name;                                 //部门名称
                department.No = model.No;                                     //部门编号
                department.FullName = CommonMethod.GetDepFullName(department.FullPid) + department.Name;    //部门全称

                department.DocDepartment = model.DocDepartment.ToInt();       //是否单据所属部门
                department.Sortno = model.Sortno;                             //排序编号
                department.ModifyMan = SysConfig.CurrentUser.Id;              //修改人
                department.ModifyTime = DateTime.Now;                         //修改时间

                int result = department.Update();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_Department");

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 删除部门

        [HttpPost]
        public string Delete(string depId)
        {
            if (string.IsNullOrWhiteSpace(depId))
            {
                return "0";
            }

            DepartmentModel dep = DepartmentModel.SingleOrDefault(depId);
            DepartmentModel.Delete(string.Format("where ID = {0} or Full_PID like '%{1}%'", depId, depId));


            //记录操作日志
            CommonMethod.Log(SysConfig.CurrentUser.Id, "Delete", "Sys_Department",
                string.Format("删除【{0}】部门", dep == null ? "" : dep.Name));

            return "1";
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获得所有部门
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDepartmentTree()
        {
            List<DepartmentModel> depList = DepartmentModel.Fetch(" where Del_Flag = 0 order by SortNo");


            //DepartmentModel company = new DepartmentModel();
            //company.Name = "公司名称";
            //company.No = "";
            //company.Pid = 0;
            //company.Id = "0";
            //company.Sortno = 1;
            //depList.Insert(0, company);

            return Json(depList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询某部门下是否有人员
        /// </summary>
        /// <param name="depId"></param>
        /// <returns></returns>
        [HttpGet]
        public string HasPeople(string depId)
        {
            List<UserModel> userList = UserModel.Fetch("where Del_Flag = 0 and DepartmentID = @0", depId);
            if (userList == null || userList.Count == 0)
            {
                return "0";
            }
            return "1";
        }

        #endregion
    }
}
