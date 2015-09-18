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
using PetaPoco;
using System.Text;
using FTERPWeb.Common.Filter;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Data;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class ClientController : Controller
    {
        #region 客户列表

        [UserAuthorize]
        [ValidateInput(false)]
        public ActionResult Index(Page<ClientIndexModel> model)
        {
            #region 获取客户列表

            StringBuilder sql = new StringBuilder(2048);
            sql.Append("select cl.ID,")
               .Append("       cl.No,")
               .Append("       cl.Type,")
               .Append("       cl.Ch_Name as ChName,")
               .Append("       cl.En_Name as EnName,")
               .Append("       cl.Short_Name as ShortName,")
               .Append("       ct.ChName as Country,")
               .Append("       cl.Address,")
               .Append("       cl.Phone,")
               .Append("       u2.Name as BelongsMan,")
               .Append("       d.Full_Name as BelongsDepartment,")
               .Append("       cl.Email,")
               .Append("       case when cl.Del_Flag = 0 then '是'")
               .Append("            when cl.Del_Flag = 1 then '否' end as DelFlag,")
               .Append("       u1.Name as CreateMan,")
               .Append("       CONVERT(nvarchar(16),cl.Create_Time,20) as CreateTime,")
               .Append("       case when cl.Approval_Status = 1 then '待提交'")
               .Append("            when cl.Approval_Status = 2 then '变更待提交'")
               .Append("            when cl.Approval_Status = 3 then '已提交'")
               .Append("            when cl.Approval_Status = 4 then '已生效'")
               .Append("            when cl.Approval_Status = 5 then '已退回' end as ApprovalStatus ")
               .Append("from sys_client cl ")
               .Append("left join Sys_Country ct on cl.CountryID = ct.ID and ct.Del_Flag = 0 ")
               .Append("left join Sys_User u1 on cl.Create_Man = u1.ID and u1.Del_Flag = 0 ")
               .Append("left join Sys_User u2 on cl.Belongs_Man = u2.ID and u2.Del_Flag = 0 ")
               .Append("left join Sys_Department d on cl.Belongs_Department = d.ID and d.Del_Flag = 0 ")
               .Append("where cl.Del_Flag = 0");

            //管理员可以看到所有单据 其他用户只能看到3种数据：本人创建的、属于本人的、有权限组织的单据
            string[] roleArray = SysConfig.CurrentRoleNames.Split(',');
            if (!roleArray.Contains("管理员"))
            {
                sql.Append(" and (")
                          .Append("cl.Create_Man in(")
                                              .Append("select ID from Sys_User where DepartmentID in(")
                                                       .Append(SysConfig.CurrentDepAuthInfo).Append(") ")
                                              .Append("union ")
                                              .Append("select ").Append(SysConfig.CurrentUser.Id)
                                          .Append(")")
                         .Append(" or cl.Belongs_Man in(")
                                              .Append("select ID from Sys_User where DepartmentID in(")
                                                       .Append(SysConfig.CurrentDepAuthInfo).Append(") ")
                                              .Append("union ")
                                              .Append("select ").Append(SysConfig.CurrentUser.Id)
                                          .Append(")")
                         .Append(")");
            }

            #region 查询条件

            ViewBag.backType = string.IsNullOrWhiteSpace(Request.QueryString["backType"]) ? "0" : Request.QueryString["backType"];

            //客户编号
            if (!string.IsNullOrWhiteSpace(Request.Form["no"]))
            {
                sql.Append(" and cl.No like '%").Append(Server.HtmlEncode(Request.Form["no"])).Append("%'");

                ViewBag.backType = "1";
            }
            //中文名称
            if (!string.IsNullOrWhiteSpace(Request.Form["chName"]))
            {
                sql.Append(" and cl.Ch_Name like '%").Append(Server.HtmlEncode(Request.Form["chName"])).Append("%'");

                ViewBag.backType = "1";
            }
            //客户简称
            if (!string.IsNullOrWhiteSpace(Request.Form["shortName"]))
            {
                sql.Append(" and cl.Short_Name like '%").Append(Server.HtmlEncode(Request.Form["shortName"])).Append("%'");

                ViewBag.backType = "1";
            }
            //英文名称
            if (!string.IsNullOrWhiteSpace(Request.Form["enName"]))
            {
                sql.Append(" and cl.En_Name like '%").Append(Server.HtmlEncode(Request.Form["enName"])).Append("%'");

                ViewBag.backType = "1";
            }
            //制单人
            if (!string.IsNullOrWhiteSpace(Request.Form["createMan"]))
            {
                sql.Append(" and cl.Create_Man like '%").Append(Server.HtmlEncode(Request.Form["createMan"])).Append("%'");

                ViewBag.backType = "1";
            }
            //审批状态
            if (!string.IsNullOrWhiteSpace(Request.Form["approvalStatus"]))
            {
                sql.Append(" and cl.Approval_Status = ").Append(Request.Form["approvalStatus"]);

                ViewBag.backType = "1";
            }
            //是否状态
            if (!string.IsNullOrWhiteSpace(Request.Form["delFlag"]))
            {
                sql.Append(" and cl.Del_Flag = ").Append(Request.Form["delFlag"]);

                ViewBag.backType = "1";
            }

            sql.Append(" order by cl.Create_Time desc");

            #endregion

            #region 导出

            if (!string.IsNullOrWhiteSpace(Request["exportFlag"]) && Request["exportFlag"] == "1")
            {
                List<ClientIndexModel> clientList = ClientIndexModel.Fetch(sql.ToString());

                //标题行
                List<string> titleList = new List<string>();
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

                string fileName = Path.Combine("客户信息", DateTime.Now.ToString("yyyyMMddHHmmss"));

                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                ExcelHelper.DataToExcel(clientList, "", titleList, fileName, this.HttpContext);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            #endregion

            if (model.CurrentPage == 0)
            {
                model.CurrentPage = 1;
            }

            var itemsPerPage = string.IsNullOrWhiteSpace(Request["itemsPerPage"]) ?
                SysConfig.ItemsPerPage : 0 == Request["itemsPerPage"].ToInt() ? 1 : Request["itemsPerPage"].ToInt();
            var items = ClientIndexModel.Page(model.CurrentPage, itemsPerPage, sql.ToString());

            ViewBag.CurrentPage = model.CurrentPage;

            return View(items);

            #endregion
        }

        #endregion

        #region 客户添加

        [UserAuthorize]
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Add()
        {
            ViewBag.Country = CommonMethod.GetCountryList();

            return View(new ClientViewModel());
        }

        [HttpPost]
        public string Add(ClientViewModel model)
        {
#if DEBUG
            SysConfig.CurrentUser = UserModel.SingleOrDefault("3");
#endif
            if (ModelState.IsValid)
            {
                ClientModel client = new ClientModel();
                client.No = CommonMethod.GetLatestSerialNo(SysConfig.SerialNo.ClientNo);    //客户编号
                client.Type = model.Type;                                                   //客户类别

                client.ChName = model.ChName;                                               //中文名称
                client.EnName = model.EnName;                                               //英文名称
                client.ShortName = model.ShortName;                                         //简称

                client.Countryid = model.Countryid.ToInt();                                 //国家
                client.Address = model.Address;                                             //地址
                client.Phone = model.Phone;                                                 //电话

                client.Email = model.Email;                                                 //邮箱
                client.Linkman = model.Linkman;                                             //联系人
                client.Remark = model.Remark;                                               //备注

                client.BelongsMan = CommonMethod.GetBelongsMan(                             //业务员
                    SysConfig.CurrentUser.Departmentid.ToString());
                client.BelongsDepartment = CommonMethod.GetBelongsDepartment(               //所属部门
                    SysConfig.CurrentUser.Departmentid.ToString());


                ApprovalRuleModel rule = ApprovalRuleModel.FirstOrDefault(@"where Del_Flag = 0 and Doc_Type = 1 
                                                                                and DepartmentID = @0", client.BelongsDepartment);
                //如果没有审批规则 则直接生效
                if (rule == null)
                {
                    client.ApprovalStatus = "4";                                            //审批状态  4：已生效
                }
                //如果有审批规则 则待提交
                else
                {
                    client.ApprovalStatus = "1";                                            //审批状态  1：待提交
                }

                client.DelFlag = 0;                                                         //有效状态
                client.CreateMan = SysConfig.CurrentUser.Id;                                //制单人
                client.CreateTime = DateTime.Now;                                           //制单时间
                int result = client.Insert().ToInt();

                if (result > 0)
                {
                    //更新客户最大序列号
                    CommonMethod.UpdateSerialNo(SysConfig.SerialNo.ClientNo);

                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_Client");
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_SerialNo");

                    return "1";
                }
            }
            return "0";
        }

        #endregion

        #region 客户编辑

        [HttpGet]
        public ActionResult Edit(string id)
        {
            #region 检查参数

            string currentPage = string.IsNullOrWhiteSpace(Request["CurrentPage"]) ? "1" : Request["CurrentPage"];
            string backUrl = "/Home/Client/Index?backType=1&CurrentPage=" + currentPage;
            ViewBag.backUrl = backUrl;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect(backUrl);
            }

            #endregion

            ViewBag.Country = CommonMethod.GetCountryList();

            #region 获取客户信息

            ClientModel client = ClientModel.SingleOrDefault(id);
            if (null == client)
            {
                return Redirect(backUrl);
            }

            ClientViewModel model = new ClientViewModel(client);

            return View(model);

            #endregion
        }

        [HttpPost]
        [ValidateInput(false)]
        public string Edit(ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                ClientModel client = ClientModel.SingleOrDefault(model.Id);
                client.Type = model.Type;                               //客户类别
                client.ChName = model.ChName;                           //中文名称
                client.EnName = model.EnName;                           //英文名称

                client.ShortName = model.ShortName;                     //简称
                client.Countryid = model.Countryid.ToInt();             //国家
                client.Address = model.Address;                         //地址

                client.Phone = model.Phone;                             //电话
                client.Email = model.Email;                             //邮箱
                client.Linkman = model.Linkman;                         //联系人

                //如果当前的审批状态是“已提交”  则编辑后自动变成“变更待提交”
                if (model.ApprovalStatus == "3")
                {
                    client.ApprovalStatus = "2";

                    #region 同时终止旧的单据审批记录

                    ApprovalDocumentModel.Update("set Next_RoleID = '-' where Table_Name = 'Sys_Client' and DocID = @0", model.Id);

                    #endregion
                }
                //如果当前的审批状态是“已退回” 则编辑后自动变成“待提交”
                if (model.ApprovalStatus == "5")
                {
                    client.ApprovalStatus = "1";
                }

                client.Remark = model.Remark;                           //备注
                client.ModifyMan = SysConfig.CurrentUser.Id;            //修改人
                client.ModifyTime = DateTime.Now;                       //修改时间

                int result = client.Update();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_Client");

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 客户详情

        public ActionResult ClientDetails(string id)
        {
            #region 检查参数

            string currentPage = string.IsNullOrWhiteSpace(Request["CurrentPage"]) ? "1" : Request["CurrentPage"];
            string backUrl = "/Home/Client/Index?backType=1&CurrentPage=" + currentPage;
            ViewBag.backUrl = backUrl;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect(backUrl);
            }

            #endregion

            #region 获取客户信息

            ClientModel client = ClientModel.SingleOrDefault(id);
            if (null == client)
            {
                return Redirect(backUrl);
            }

            //国家
            CountryModel country = CountryModel.SingleOrDefault(client.Countryid);
            ViewBag.Country = country == null ? "" : country.Chname;

            //审批状态
            switch (client.ApprovalStatus)
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
            ViewBag.BelongsMan = CommonMethod.GetUserNameByID(client.BelongsMan.ToString());

            //单据所属部门
            ViewBag.BelongsDepartment = CommonMethod.GetDepNameById(client.BelongsDepartment.ToString());

            //制单人
            ViewBag.CreateMan = CommonMethod.GetUserNameByID(client.CreateMan);

            //制单时间
            ViewBag.CreateTime = client.CreateTime.Value.ToString("yyyy-MM-dd");

            return View(client);

            #endregion
        }

        #endregion

        #region 详情Tab页

        public ActionResult Details(string id)
        {
            ViewBag.id = id;
            return View();
        }

        #endregion

        #region 客户删除

        [HttpPost]
        public string Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return "0";
            }

            if (ClientModel.Update(string.Format("set Del_Flag = 1 where ID in ({0})", id)) > 0)
            {
                //记录操作日志
                CommonMethod.Log(SysConfig.CurrentUser.Id, "Delete", "Sys_Client",
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
            return CommonMethod.Submit("1",
                CommonMethod.GetBelongsDepartment(SysConfig.CurrentUser.Departmentid.ToString()).ToString(),
                "Sys_Client", id);
        }

        #endregion

        #region 审批

        [HttpGet]
        public ActionResult Approve(string aId, string docId, string roleId)
        {
            ClientModel client = ClientModel.SingleOrDefault(docId);
            if (client == null)
            {
                return Redirect("/Home/Main/UnapprovedIndex");
            }

            //国家
            CountryModel country = CountryModel.SingleOrDefault(client.Countryid);
            ViewBag.Country = country == null ? "" : country.Chname;

            //审批状态
            switch (client.ApprovalStatus)
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
            ViewBag.BelongsMan = CommonMethod.GetUserNameByID(client.BelongsMan.ToString());

            //业务部门
            ViewBag.BelongsDepartment = CommonMethod.GetDepNameById(client.BelongsDepartment.ToString());

            //制单人
            ViewBag.CreateMan = CommonMethod.GetUserNameByID(client.CreateMan);

            //制单时间
            ViewBag.CreateTime = client.CreateTime.Value.ToString("yyyy-MM-dd");

            ViewBag.aId = aId;
            ViewBag.roleId = roleId;

            return View(client);
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

            return CommonMethod.Approve("1", "Sys_Client", Request["docId"], Request["aId"], Request["type"],
                                        Request["roleId"],
                                        string.IsNullOrWhiteSpace(Request["remark"]) ? "" : Request["remark"]);
        }

        #endregion
    }
}
