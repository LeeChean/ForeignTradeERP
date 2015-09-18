#undef DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Common;
using FTERPWeb.Home.ViewModels;
using FTERPWeb.Models;
using FTERPCommon;
using PetaPoco;
using System.Text;
using FTERPWeb.Common.Filter;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Data;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class SupplierController : Controller
    {
        #region 供应商列表

        [UserAuthorize]
        [ValidateInput(false)]
        public ActionResult Index(Page<SupplierIndexModel> model)
        {
            ViewBag.Country = CommonMethod.GetCountryList();

            #region 获取供应商列表

            StringBuilder sql = new StringBuilder();
            sql.Append("select s.ID,")
               .Append("       s.No,")
               .Append("       s.Type,")
               .Append("       s.Ch_Name as ChName,")
               .Append("       s.En_Name as EnName,")
               .Append("       s.Short_Name as ShortName,")
               .Append("       c.ChName as Country,")
               .Append("       s.Address,")
               .Append("       s.Phone,")
               .Append("       u2.Name as BelongsMan,")
               .Append("       d.Full_Name as BelongsDepartment,")
               .Append("       s.Email,")
               .Append("       case when s.Del_Flag = 0 then '是'")
               .Append("            when s.Del_Flag = 1 then '否' end as DelFlag,")
               .Append("       u1.Name as CreateMan,")
               .Append("       CONVERT(nvarchar(16),s.Create_Time,20) as CreateTime,")
               .Append("       case when s.Approval_Status = 1 then '待提交'")
               .Append("            when s.Approval_Status = 2 then '变更待提交'")
               .Append("            when s.Approval_Status = 3 then '已提交'")
               .Append("            when s.Approval_Status = 4 then '已生效'")
               .Append("            when s.Approval_Status = 5 then '已退回' end as ApprovalStatus ")
               .Append("from Sys_Supplier s ")
               .Append("left join Sys_Country c on s.CountryID = c.ID and c.Del_Flag = 0 ")
               .Append("left join Sys_User u1 on s.Create_Man = u1.ID and u1.Del_Flag = 0 ")
               .Append("left join Sys_User u2 on s.Belongs_Man = u2.ID and u2.Del_Flag = 0 ")
               .Append("left join Sys_Department d on s.Belongs_Department = d.ID and d.Del_Flag = 0 ")
               .Append("where s.Del_Flag = 0");

            //管理员可以看到所有单据 其他用户只能看到3种数据：本人创建的、属于本人的、有权限组织的单据
            string[] roleArray = SysConfig.CurrentRoleNames.Split(',');
            if (!roleArray.Contains("管理员"))
            {
                sql.Append(" and (")
                          .Append("s.Create_Man in(")
                                              .Append("select ID from Sys_User where DepartmentID in(")
                                                       .Append(SysConfig.CurrentDepAuthInfo).Append(") ")
                                              .Append("union ")
                                              .Append("select ").Append(SysConfig.CurrentUser.Id)
                                          .Append(")")
                         .Append(" or s.Belongs_Man in(")
                                              .Append("select ID from Sys_User where DepartmentID in(")
                                                       .Append(SysConfig.CurrentDepAuthInfo).Append(") ")
                                              .Append("union ")
                                              .Append("select ").Append(SysConfig.CurrentUser.Id)
                                          .Append(")")
                         .Append(")");
            }

            #region 查询条件

            ViewBag.backType = string.IsNullOrWhiteSpace(Request.QueryString["backType"]) ? "0" : Request.QueryString["backType"];

            //供应商编号
            if (!string.IsNullOrWhiteSpace(Request.Form["no"]))
            {
                sql.Append(" and s.No like '%").Append(Server.HtmlEncode(Request.Form["no"])).Append("%'");

                ViewBag.backType = "1";
            }
            //中文名称
            if (!string.IsNullOrWhiteSpace(Request.Form["chName"]))
            {
                sql.Append(" and s.Ch_Name like '%").Append(Server.HtmlEncode(Request.Form["chName"])).Append("%'");

                ViewBag.backType = "1";
            }
            //国家
            if (!string.IsNullOrWhiteSpace(Request.Form["country"]))
            {
                sql.Append(" and c.ID = ").Append(Request.Form["country"]);

                ViewBag.backType = "1";
            }
            //英文名称
            if (!string.IsNullOrWhiteSpace(Request.Form["enName"]))
            {
                sql.Append(" and s.En_Name like '%").Append(Server.HtmlEncode(Request.Form["enName"])).Append("%'");

                ViewBag.backType = "1";
            }
            //审批状态
            if (!string.IsNullOrWhiteSpace(Request.Form["approvalStatus"]))
            {
                sql.Append(" and s.Approval_Status = ").Append(Request.Form["approvalStatus"]);

                ViewBag.backType = "1";
            }
            //是否状态
            if (!string.IsNullOrWhiteSpace(Request.Form["delFlag"]))
            {
                sql.Append(" and s.Del_Flag = ").Append(Request.Form["delFlag"]);

                ViewBag.backType = "1";
            }

            sql.Append(" order by s.Create_Time desc");

            #endregion

            #region 导出

            if (!string.IsNullOrWhiteSpace(Request["exportFlag"]) && Request["exportFlag"] == "1")
            {
                List<SupplierIndexModel> supplierList = SupplierIndexModel.Fetch(sql.ToString());

                //标题行
                List<string> titleList = new List<string>();
                titleList.Add("Type");
                titleList.Add("No");
                titleList.Add("ShortName");
                titleList.Add("ChName");
                titleList.Add("EnName");
                titleList.Add("Country");
                titleList.Add("Address");
                titleList.Add("Phone");
                titleList.Add("Email");
                titleList.Add("CreateMan");
                titleList.Add("BelongsMan");
                titleList.Add("BelongsDepartment");

                string fileName = Path.Combine("供应商信息", DateTime.Now.ToString("yyyyMMddHHmmss"));

                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                ExcelHelper.DataToExcel(supplierList, "", titleList, fileName, this.HttpContext);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            #endregion

            if (model.CurrentPage == 0)
            {
                model.CurrentPage = 1;
            }

            var itemsPerPage = string.IsNullOrWhiteSpace(Request["itemsPerPage"]) ?
                SysConfig.ItemsPerPage : 0 == Request["itemsPerPage"].ToInt() ? 1 : Request["itemsPerPage"].ToInt();
            var items = SupplierIndexModel.Page(model.CurrentPage, SysConfig.ItemsPerPage, sql.ToString());

            ViewBag.CurrentPage = model.CurrentPage;

            return View(items);

            #endregion
        }

        #endregion

        #region 供应商添加

        [UserAuthorize]
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Add()
        {
            ViewBag.Country = CommonMethod.GetCountryList();

            return View(new SupplierViewModel());
        }

        [HttpPost]
        public string Add(SupplierViewModel model)
        {
#if DEBUG
            SysConfig.CurrentUser = UserModel.SingleOrDefault("3");
#endif
            if (ModelState.IsValid)
            {
                SupplierModel supplier = new SupplierModel();
                supplier.No = CommonMethod.GetLatestSerialNo(SysConfig.SerialNo.SupplierNo);        //供应商编号
                supplier.Type = model.Type;                                                         //供应商类别

                supplier.ChName = model.ChName;                                                     //中文名称
                supplier.EnName = model.EnName;                                                     //英文名称
                supplier.ShortName = model.ShortName;                                               //简称

                supplier.Countryid = model.Countryid.ToInt();                                       //国家
                supplier.Address = model.Address;                                                   //地址
                supplier.Phone = model.Phone;                                                       //电话

                supplier.Email = model.Email;                                                       //邮箱
                supplier.Linkman = model.Linkman;                                                   //联系人
                supplier.Remark = model.Remark;                                                     //备注

                supplier.BelongsMan = CommonMethod.GetBelongsMan(                                   //业务员
                    SysConfig.CurrentUser.Departmentid.ToString());
                supplier.BelongsDepartment = CommonMethod.GetBelongsDepartment(                     //业务部门
                    SysConfig.CurrentUser.Departmentid.ToString());

                ApprovalRuleModel rule = ApprovalRuleModel.FirstOrDefault(@"where Del_Flag = 0 and Doc_Type = 2 
                                                                                and DepartmentID = @0", supplier.BelongsDepartment);
                //如果没有审批规则 则直接生效
                if (rule == null)
                {
                    supplier.ApprovalStatus = "4";                                                  //审批状态  4：已生效
                }
                //如果有审批规则 则待提交
                else
                {
                    supplier.ApprovalStatus = "1";                                                  //审批状态  1：待提交
                }

                supplier.OpeningBank = model.OpeningBank;                                           //开户行
                supplier.BankAccount = model.BankAccount;                                           //开户行账号
                supplier.DelFlag = 0;                                                               //有效状态

                supplier.CreateMan = SysConfig.CurrentUser.Id;                                      //制单人
                supplier.CreateTime = DateTime.Now;                                                 //制单时间

                int result = supplier.Insert().ToInt();

                if (result > 0)
                {
                    //更新供应商最大序列号
                    CommonMethod.UpdateSerialNo(SysConfig.SerialNo.SupplierNo);

                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_Supplier");
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_SerialNo");

                    return "1";
                }
            }
            return "0";
        }

        #endregion

        #region 供应商编辑

        [HttpGet]
        public ActionResult Edit(string id)
        {
            #region 检查参数

            string currentPage = string.IsNullOrWhiteSpace(Request["CurrentPage"]) ? "1" : Request["CurrentPage"];
            string backUrl = "/Home/Supplier/Index?backType=1&CurrentPage=" + currentPage;
            ViewBag.backUrl = backUrl;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect(backUrl);
            }

            #endregion

            ViewBag.Country = CommonMethod.GetCountryList();

            #region 获取供应商信息

            SupplierModel supplier = SupplierModel.SingleOrDefault(id);
            if (null == supplier)
            {
                return Redirect(backUrl);
            }

            SupplierViewModel model = new SupplierViewModel(supplier);

            return View(model);

            #endregion
        }

        [HttpPost]
        [ValidateInput(false)]
        public string Edit(SupplierViewModel model)
        {
#if DEBUG
            SysConfig.CurrentUser = UserModel.SingleOrDefault(3);
#endif
            if (ModelState.IsValid)
            {
                SupplierModel supplier = SupplierModel.SingleOrDefault(model.Id);

                supplier.Type = model.Type;                               //供应商类别
                supplier.ChName = model.ChName;                           //中文名称
                supplier.EnName = model.EnName;                           //英文名称

                supplier.ShortName = model.ShortName;                     //简称
                supplier.Countryid = model.Countryid.ToInt();             //国家
                supplier.Address = model.Address;                         //地址

                supplier.Phone = model.Phone;                             //电话
                supplier.Email = model.Email;                             //邮箱
                supplier.Linkman = model.Linkman;                         //联系人

                //如果当前的审批状态是“已提交”  则编辑后自动变成“变更待提交”
                if (model.ApprovalStatus == "3")
                {
                    supplier.ApprovalStatus = "2";

                    #region 同时终止旧的单据审批记录

                    ApprovalDocumentModel.Update("set Next_RoleID = '-' where Table_Name = 'Sys_Supplier' and DocID = @0", model.Id);

                    #endregion
                }
                //如果当前的审批状态是“已退回” 则编辑后自动变成“待提交”
                if (model.ApprovalStatus == "5")
                {
                    supplier.ApprovalStatus = "1";
                }

                supplier.Remark = model.Remark;                           //备注
                supplier.OpeningBank = model.OpeningBank;                 //开户行
                supplier.BankAccount = model.BankAccount;                 //开户行账号

                supplier.ModifyMan = SysConfig.CurrentUser.Id;            //修改人
                supplier.ModifyTime = DateTime.Now;                       //修改时间
                int result = supplier.Update();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_Supplier");

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 供应商详情

        public ActionResult Details(string id)
        {
            #region 检查参数

            string currentPage = string.IsNullOrWhiteSpace(Request["CurrentPage"]) ? "1" : Request["CurrentPage"];
            string backUrl = "/Home/Supplier/Index?backType=1&CurrentPage=" + currentPage;
            ViewBag.backUrl = backUrl;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect(backUrl);
            }

            #endregion

            #region 获取供应商信息

            SupplierModel supplier = SupplierModel.SingleOrDefault(id);
            if (null == supplier)
            {
                return Redirect(backUrl);
            }

            //国家
            CountryModel country = CountryModel.SingleOrDefault(supplier.Countryid);
            ViewBag.Country = country == null ? "" : country.Chname;

            //审批状态
            switch (supplier.ApprovalStatus)
            {
                case "1":
                    ViewBag.Status = "未提交";
                    break;
                case "2":
                    ViewBag.Status = "变更未提交";
                    break;
                case "3":
                    ViewBag.Status = "已提交";
                    break;
                case "4":
                    ViewBag.Status = "已生效";
                    break;
                case "5":
                    ViewBag.Status = "已退回";
                    break;

            }

            //业务员
            ViewBag.BelongsMan = CommonMethod.GetUserNameByID(supplier.BelongsMan.ToString());

            //业务部门
            ViewBag.BelongsDepartment = CommonMethod.GetDepNameById(supplier.BelongsDepartment.ToString());

            //制单人
            ViewBag.CreateMan = CommonMethod.GetUserNameByID(supplier.CreateMan);

            //制单时间
            ViewBag.CreateTime = supplier.CreateTime.Value.ToString("yyyy-MM-dd");

            return View(supplier);

            #endregion
        }

        #endregion

        #region 供应商删除

        [HttpPost]
        public string Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return "0";
            }

            if (SupplierModel.Update(string.Format("set Del_Flag = 1 where ID in ({0})", id)) > 0)
            {
                //记录操作日志
                CommonMethod.Log(SysConfig.CurrentUser.Id, "Delete", "Sys_Supplier",
                              string.Format("将主键为{0}的记录置为无效", id));

                return "1";
            }
            return "0";

        }

        #endregion

        #region 提交

        [HttpPost]
        public string Submit(string id)
        {
            return CommonMethod.Submit("2",
                CommonMethod.GetBelongsDepartment(SysConfig.CurrentUser.Departmentid.ToString()).ToString(),
                "Sys_Supplier", id);
        }

        #endregion

        #region 审批

        [HttpGet]
        public ActionResult Approve(string aId, string docId, string roleId)
        {
            SupplierModel supplier = SupplierModel.SingleOrDefault(docId);
            if (supplier == null)
            {
                return Redirect("/Home/Main/UnapprovedIndex");
            }

            //国家
            CountryModel country = CountryModel.SingleOrDefault(supplier.Countryid);
            ViewBag.Country = country == null ? "" : country.Chname;

            //审批状态
            switch (supplier.ApprovalStatus)
            {
                case "1":
                    ViewBag.Status = "未提交";
                    break;
                case "2":
                    ViewBag.Status = "变更未提交";
                    break;
                case "3":
                    ViewBag.Status = "已提交";
                    break;
                case "4":
                    ViewBag.Status = "已生效";
                    break;
                case "5":
                    ViewBag.Status = "已退回";
                    break;
            }

            //业务员
            ViewBag.BelongsMan = CommonMethod.GetUserNameByID(supplier.BelongsMan.ToString());

            //业务部门
            ViewBag.BelongsDepartment = CommonMethod.GetDepNameById(supplier.BelongsDepartment.ToString());

            //制单人
            ViewBag.CreateMan = CommonMethod.GetUserNameByID(supplier.CreateMan);

            //制单时间
            ViewBag.CreateTime = supplier.CreateTime.Value.ToString("yyyy-MM-dd");

            ViewBag.aId = aId;
            ViewBag.roleId = roleId;

            return View(supplier);
        }

        [HttpPost]
        public string Approve()
        {
            #region 检查参数

            if (string.IsNullOrWhiteSpace(Request["docId"]) || string.IsNullOrWhiteSpace(Request["aId"]) ||
               string.IsNullOrWhiteSpace(Request["roleId"]) || string.IsNullOrWhiteSpace(Request["type"]))
            {
                return "0";
            }

            #endregion

            return CommonMethod.Approve("1", "Sys_Supplier", Request["docId"], Request["aId"], Request["type"],
                                        Request["roleId"],
                                        string.IsNullOrWhiteSpace(Request["remark"]) ? "" : Request["remark"]);
        }

        #endregion
    }
}
