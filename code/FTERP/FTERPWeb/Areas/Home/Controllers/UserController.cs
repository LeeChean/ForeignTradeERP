using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Home.ViewModels;
using FTERPWeb.Models;
using FTERPCommon;
using System.Text;
using PetaPoco;
using FTERPWeb.Common;
using FTERPWeb.Common.Filter;
using System.Net;
using System.IO;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class UserController : Controller
    {
        #region 用户列表

        [UserAuthorize]
        [ValidateInput(false)]
        public ActionResult Index(Page<UserIndexModel> model)
        {
            #region 获取角色列表

            List<SelectListItem> roleList = new List<SelectListItem>();
            List<RoleModel> roleModel = RoleModel.Fetch("where Del_Flag = 0");

            foreach (var item in roleModel)
            {
                roleList.Add(new SelectListItem() { Text = item.Name, Value = item.Id });
            }

            ViewBag.Role = roleList;

            #endregion

            #region 获取用户列表

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from(")
               .Append("    select distinct u.ID,")
               .Append("                    u.Code,")
               .Append("                    u.Name,")
               .Append("                    d.Full_Name as DepartmentName,")
               .Append("                    u.Phone,")
               .Append("                    case u.Gender when 0 then '女'")
               .Append("                                  when 1 then '男' end as Gender,")
               .Append("                    case u.Status when 0 then '在职'")
               .Append("                                  when 1 then '离职'")
               .Append("                                  when 2 then '休假' end as Status,")
               .Append("                    Convert(nvarchar(16),u.Create_Time,20) as CreateTime,")
               .Append("                    case u.Del_Flag when 0 then '是'")
               .Append("                                    when 1 then '否' end as DelFlag ")
               .Append("    from Sys_User u ")
               .Append("    left join Sys_Department d on u.DepartmentID = d.ID and d.Del_Flag = 0 ")
               .Append("    left join Sys_User_Role ur on u.ID = ur.UserID ")
               .Append("    where u.Del_Flag = 0");

            #region 查询条件

            ViewBag.backType = string.IsNullOrWhiteSpace(Request.QueryString["backType"]) ? "0" : Request.QueryString["backType"];

            //登录名
            if (!string.IsNullOrWhiteSpace(Request.Form["code"]))
            {
                sql.Append(" and u.Code like '%").Append(Server.HtmlEncode(Request.Form["code"])).Append("%'");

                ViewBag.backType = "1";
            }
            //姓名
            if (!string.IsNullOrWhiteSpace(Request.Form["name"]))
            {
                sql.Append(" and u.Name like '%").Append(Server.HtmlEncode(Request.Form["name"])).Append("%'");

                ViewBag.backType = "1";
            }
            //所属部门
            if (!string.IsNullOrWhiteSpace(Request.Form["department"]))
            {
                sql.Append(" and d.Full_Name like '%").Append(Server.HtmlEncode(Request.Form["department"])).Append("%'");

                ViewBag.backType = "1";
            }
            //角色
            if (!string.IsNullOrWhiteSpace(Request.Form["role"]))
            {
                sql.Append(" and ur.RoleID = ").Append(Request.Form["role"]);

                ViewBag.backType = "1";
            }
            //性别
            if (!string.IsNullOrWhiteSpace(Request.Form["gender"]))
            {
                sql.Append(" and u.Gender = ").Append(Request.Form["gender"]);

                ViewBag.backType = "1";
            }
            //用户状态
            if (!string.IsNullOrWhiteSpace(Request.Form["status"]))
            {
                sql.Append(" and u.Status = ").Append(Request.Form["status"]);

                ViewBag.backType = "1";
            }
            //有效状态
            if (!string.IsNullOrWhiteSpace(Request.Form["delFlag"]))
            {
                sql.Append(" and u.Del_Flag = ").Append(Request.Form["delFlag"]);

                ViewBag.backType = "1";
            }

            sql.Append(") a");
            sql.Append(" order by CreateTime desc ");

            #endregion


            #region 导出

            if (!string.IsNullOrWhiteSpace(Request["exportFlag"]) && Request["exportFlag"] == "1")
            {
                List<UserIndexModel> userList = UserIndexModel.Fetch(sql.ToString());

                //标题行
                List<string> titleList = new List<string>();
                titleList.Add("Code");
                titleList.Add("Name");
                titleList.Add("DepartmentName");
                titleList.Add("RoleName");
                titleList.Add("Phone");
                titleList.Add("Gender");
                titleList.Add("Status");
                titleList.Add("CreateTime");
                titleList.Add("DelFlag");

                string fileName = Path.Combine("用户信息", DateTime.Now.ToString("yyyyMMddHHmmss"));

                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                ExcelHelper.DataToExcel(userList, "", titleList, fileName, this.HttpContext);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            #endregion

            if (model.CurrentPage == 0)
            {
                model.CurrentPage = 1;
            }

            var itemsPerPage = string.IsNullOrWhiteSpace(Request["itemsPerPage"]) ?
                SysConfig.ItemsPerPage : 0 == Request["itemsPerPage"].ToInt() ? 1 : Request["itemsPerPage"].ToInt();
            var items = UserIndexModel.Page(model.CurrentPage, itemsPerPage, sql.ToString());

            ViewBag.CurrentPage = model.CurrentPage;

            return View(items);

            #endregion
        }

        #endregion

        #region 添加用户

        [UserAuthorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View(new AddUserModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public string Add(AddUserModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = new UserModel();
                user.Code = model.Code;                             //登录名
                user.Password = "123456".VariationMd5();            //默认密码

                user.Name = model.Name;                             //姓名
                user.Phone = model.Phone;                           //电话
                user.No = model.No;                                 //用户编号

                user.Departmentid = model.Departmentid.ToInt();     //部门
                user.Gender = model.Gender;                         //性别
                user.Age = model.Age;                               //年龄

                user.Status = model.Status;                         //用户状态
                user.Email = model.Email;                           //邮箱

                user.DelFlag = 0;                                   //有效状态
                user.CreateMan = SysConfig.CurrentUser.Id;          //创建人
                user.CreateTime = DateTime.Now;                     //创建时间
                int result = user.Insert().ToInt();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_User");
                    return "1";
                }
            }
            return "0";
        }

        #endregion

        #region 编辑用户

        [HttpGet]
        public ActionResult Edit(string id)
        {
            #region 检查参数

            string currentPage = string.IsNullOrWhiteSpace(Request["CurrentPage"]) ? "1" : Request["CurrentPage"];
            string backUrl = "/Home/User/Index?backType=1&CurrentPage=" + currentPage;
            ViewBag.backUrl = backUrl;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect(backUrl);
            }

            #endregion

            #region 获取用户信息

            UserModel user = UserModel.SingleOrDefault(id);
            if (null == user)
            {
                return Redirect(backUrl);
            }

            AddUserModel model = new AddUserModel();
            model.Id = user.Id;
            model.Code = user.Code;
            model.Name = user.Name;
            model.Phone = user.Phone;
            model.No = user.No;
            model.Age = user.Age;
            model.Gender = user.Gender;
            model.Email = user.Email;
            model.Status = user.Status;
            model.Departmentid = user.Departmentid.ToString();

            string depIdList = "";
            ViewBag.depList = CommonMethod.GetFullDepartmentList(model.Departmentid.ToString(), out depIdList);
            ViewBag.depIdList = depIdList;

            return View(model);

            #endregion
        }

        [HttpPost]
        [ValidateInput(false)]
        public string Edit(AddUserModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = UserModel.SingleOrDefault(model.Id);
                user.Code = model.Code;
                user.Name = model.Name;
                user.Age = model.Age;
                user.Gender = model.Gender;
                user.Phone = model.Phone;
                user.No = model.No;
                user.Departmentid = model.Departmentid.ToInt();
                user.Status = model.Status;
                user.Email = model.Email;
                user.ModifyMan = SysConfig.CurrentUser.Id;
                user.ModifyTime = DateTime.Now;

                int result = user.Update();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_User");

                    //更新缓存
                    SysConfig.CurrentUser = UserModel.SingleOrDefault(model.Id);

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 用户详情

        public ActionResult Details(string id)
        {
            UserModel user = UserModel.SingleOrDefault(id);
            string currentPage = string.IsNullOrWhiteSpace(Request["CurrentPage"]) ? "1" : Request["CurrentPage"];
            string backUrl = "/Home/User/Index?backType=1&CurrentPage=" + currentPage;
            ViewBag.backUrl = backUrl;
            if (user == null)
            {
                return Redirect(backUrl);
            }

            AddUserModel model = new AddUserModel();
            model.Name = user.Name;
            model.Code = user.Code;
            model.Phone = user.Phone;
            model.No = user.No;
            model.Email = user.Email;
            model.Gender = user.Gender;
            model.Age = user.Age;
            model.Status = user.Status;

            DepartmentModel departmentModel = DepartmentModel.SingleOrDefault(user.Departmentid);
            ViewBag.department = departmentModel == null ? "" : departmentModel.FullName;

            return View(model);
        }

        #endregion

        #region 重置密码

        [HttpPost]
        public string ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return "0";
            }

            if (UserModel.Update(string.Format("set Password = {0},Modify_Man = {1},Modify_Time = {2} where ID in ({3})",
                                               "123456".VariationMd5(), SysConfig.CurrentUser.Id, DateTime.Now, id)) > 0)
            {
                //记录操作日志
                CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_User",
                              string.Format("将主键为{0}的记录的密码重置为123456", id));

                return "1";
            }
            return "0";
        }

        #endregion

        #region 删除用户

        [HttpPost]
        public string Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return "0";
            }

            if (UserModel.Update(string.Format("set Del_Flag = 1 where ID in ({0})", id)) > 0)
            {
                //记录操作日志
                CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_User",
                              string.Format("将主键为{0}的记录置为无效", id));

                return "1";
            }
            return "0";

        }

        #endregion

        #region 辅助方法

        public ActionResult SubDepartment(string id, string depId)
        {
            List<SelectListItem> department = new List<SelectListItem>();
            List<DepartmentModel> departmentModel = DepartmentModel.Fetch("where Del_Flag = 0 and PID = @0 order by SortNo", depId);

            if (departmentModel != null && departmentModel.Count > 0)
            {
                department.Add(new SelectListItem() { Text = "请选择", Value = "" });
                foreach (DepartmentModel item in departmentModel)
                {
                    department.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.Id
                    });
                }
            }

            ViewBag.Id = id;
            ViewData["department"] = department;

            return View();
        }

        #endregion

        #region 绑定角色

        [HttpGet]
        public ActionResult BindRole(string userId)
        {
            #region 获取所有角色

            List<RoleModel> model = RoleModel.Fetch("where Del_Flag = 0");
            ViewBag.userId = userId;

            #endregion

            #region 获取当前用户所有的角色

            List<UserRoleModel> userRoleModel = UserRoleModel.Fetch("where UserID = @0", userId);
            List<string> roleList = new List<string>();
            foreach (UserRoleModel item in userRoleModel)
            {
                roleList.Add(item.Roleid.ToString());
            }
            ViewBag.roleIds = string.Join(",", roleList.ToArray());

            #endregion

            return View(model);
        }

        [HttpPost]
        public string SaveUserRole(string userId, string roleIds)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleIds))
            {
                return "0";
            }

            UserRoleModel.Delete("where UserID = @0", userId);

            string[] roleList = roleIds.Split(',');
            foreach (string role in roleList)
            {
                UserRoleModel model = new UserRoleModel();
                model.Userid = userId.ToInt();
                model.Roleid = role.ToInt();
                model.CreateMan = SysConfig.CurrentUser.Id;
                model.CreateTime = DateTime.Now;

                model.Insert();
            }

            //记录操作日志
            CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_User_Role",
                          string.Format("给主键为{0}的用户绑定主键为{1}的角色", userId, roleIds));

            return "1";
        }

        #endregion
    }
}
