using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetaPoco;
using FTERPWeb.Home.ViewModels;
using System.Text;
using FTERPCommon;
using FTERPWeb.Common;
using FTERPWeb.Models;
using FTERPWeb.Common.Filter;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class ApprovalRuleController : Controller
    {
        #region 审批规则列表

        [UserAuthorize]
        [ValidateInput(false)]
        public ActionResult Index(Page<ApproveIndexModel> model)
        {
            #region 获取单据类别列表

            ViewBag.Type = CommonMethod.GetDocTypeList();

            #endregion

            #region 获取审批规则列表

            StringBuilder sql = new StringBuilder(256);
            sql.Append("select ar.ID as Id,")
               .Append("       dt.TypeName as DocType,")
               .Append("       d.Full_Name as Department,")
               .Append("       ar.Process,")
               .Append("       case when ar.Del_Flag = 0 then '是'")
               .Append("            when ar.Del_Flag = 1 then '否' end as DelFlag,")
               .Append("       CONVERT(nvarchar(16),ar.Create_Time,20) as CreateTime ")
               .Append("from Sys_Approval_Rule ar ")
               .Append("left join Sys_Department d on ar.DepartmentID = d.ID ")
               .Append("left join Sys_Document_Type dt on ar.Doc_Type = dt.ID and dt.Del_Flag = 0 ")
               .Append("where ar.Del_Flag = 0");

            #region 查询条件

            ViewBag.backType = string.IsNullOrWhiteSpace(Request.QueryString["backType"]) ? "0" : Request.QueryString["backType"];

            //单据类别
            if (!string.IsNullOrWhiteSpace(Request.Form["type"]))
            {
                sql.Append(" and ar.Doc_Type = ").Append(Request.Form["type"]);

                ViewBag.backType = "1";
            }
            //部门
            if (!string.IsNullOrWhiteSpace(Request.Form["department"]))
            {
                sql.Append(" and d.Full_Name like '%").Append(Server.HtmlEncode(Request.Form["department"])).Append("%'");

                ViewBag.backType = "1";
            }
            //有效状态
            if (!string.IsNullOrWhiteSpace(Request.Form["delFlag"]))
            {
                sql.Append(" and ar.Del_Flag = ").Append(Request.Form["delFlag"]);

                ViewBag.backType = "1";
            }

            #endregion

            if (model.CurrentPage == 0)
            {
                model.CurrentPage = 1;
            }

            var itemsPerPage = string.IsNullOrWhiteSpace(Request["itemsPerPage"]) ?
                SysConfig.ItemsPerPage : 0 == Request["itemsPerPage"].ToInt() ? 1 : Request["itemsPerPage"].ToInt();
            var items = ApproveIndexModel.Page(model.CurrentPage, itemsPerPage, sql.ToString());

            ViewBag.CurrentPage = model.CurrentPage;

            return View(items);

            #endregion
        }

        #endregion

        #region 添加审批规则

        [UserAuthorize]
        [HttpGet]
        public ActionResult Add()
        {
            //获取单据类别
            ViewBag.Type = CommonMethod.GetDocTypeList();


            return View(new AddApprovalModel());
        }

        [HttpPost]
        public string Add(AddApprovalModel model)
        {
            #region 判断是否存在同一部门、同一单据类别的审批规则

            if (ApprovalRuleModel.Exists("Doc_Type = @0 and DepartmentID = @1 and Del_Flag = 0", model.Type, model.DepartmentId))
            {
                return "当前部门已存在同一单据类别的审批规则";
            }

            #endregion

            if (ModelState.IsValid)
            {
                ApprovalRuleModel process = new ApprovalRuleModel();
                process.DocType = model.Type;                             //单据类别
                process.Process = model.Process;                          //审批流程

                process.Roleid = model.RoleId;                            //审批角色
                process.Departmentid = model.DepartmentId.ToInt();        //部门
                process.DelFlag = 0;                                      //有效状态

                process.CreateMan = SysConfig.CurrentUser.Id;             //创建人
                process.CreateTime = DateTime.Now;                        //创建时间
                int result = process.Insert().ToInt();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_Approval_Rule", "添加审批规则");
                    return "1";
                }
            }
            return "0";
        }

        #endregion

        #region 编辑审批规则

        [HttpGet]
        public ActionResult Edit(string id)
        {
            #region 检查参数

            string currentPage = string.IsNullOrWhiteSpace(Request["CurrentPage"]) ? "1" : Request["CurrentPage"];
            string backUrl = "/Home/ApprovalRule/Index?backType=1&CurrentPage=" + currentPage;
            ViewBag.backUrl = backUrl;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect(backUrl);
            }

            #endregion

            #region 获取审批规则信息

            ApprovalRuleModel process = ApprovalRuleModel.SingleOrDefault(id);
            if (null == process)
            {
                return Redirect(backUrl);
            }

            AddApprovalModel model = new AddApprovalModel();
            model.Id = process.Id;
            model.DepartmentId = process.Departmentid.ToString();
            model.Type = process.DocType;
            model.Process = process.Process;
            model.RoleId = process.Roleid;

            string depIdList = "";
            ViewBag.depList = CommonMethod.GetFullDepartmentList(process.Departmentid.ToString(), out depIdList);
            ViewBag.depIdList = depIdList;

            //获取单据类别
            ViewBag.Type = CommonMethod.GetDocTypeList();

            return View(model);

            #endregion
        }

        [HttpPost]
        [ValidateInput(false)]
        public string Edit(AddApprovalModel model)
        {
            #region 判断是否存在同一部门、同一单据类别的审批规则

            if (ApprovalRuleModel.Exists("Doc_Type = @0 and DepartmentID = @1 and Del_Flag = 0 and ID <> @2",
                model.Type, model.DepartmentId, model.Id))
            {
                return "当前部门已存在同一单据类别的审批规则";
            }

            #endregion

            if (ModelState.IsValid)
            {
                ApprovalRuleModel process = ApprovalRuleModel.SingleOrDefault(model.Id);
                process.DocType = model.Type;
                process.Departmentid = model.DepartmentId.ToInt();
                process.Roleid = model.RoleId;
                process.Process = model.Process;
                process.ModifyMan = SysConfig.CurrentUser.Id;
                process.ModifyTime = DateTime.Now;
                int result = process.Update();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_Approval_Rule",
                                     string.Format("编辑主键为【{0}】的审批规则", model.Id));

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 删除审批规则

        [HttpPost]
        public string Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return "0";
            }

            if (ApprovalRuleModel.Delete(string.Format("where ID in ({0})", id)) > 0)
            {
                //记录操作日志
                CommonMethod.Log(SysConfig.CurrentUser.Id, "Delete", "Sys_Approval_Rule");

                return "1";
            }
            return "0";
        }

        #endregion

        #region 规则弹窗

        public ActionResult DefineRule()
        {
            //获取角色列表
            List<RoleModel> list = RoleModel.Fetch("where Del_Flag = 0");
            ViewBag.Role = list;

            return View();
        }

        #endregion

        #region 审批进度弹窗

        [HttpGet]
        public ActionResult ShowProgress(string docId)
        {
            string sql = string.Format(@"select r.Name as RoleName,
                                                u.Name as UserName,
                                                ap.Status,
                                                ap.Remark,
                                                CONVERT(nvarchar(16),ap.Create_Time,20) as Time
                                         from Sys_Approval_Progress ap
                                         inner join Sys_User u on ap.Create_Man = u.ID
                                         inner join Sys_Role r on ap.RoleID = r.ID
                                         inner join Sys_Approval_Document ad on ap.Approval_DocumentID = ad.ID
                                         where ad.DocID = {0}
                                         order by ap.Create_Time", docId);

            List<ApprovalProgressViewModel> progress = ApprovalProgressViewModel.Fetch(sql);

            ViewBag.Progress = progress;

            return View();
        }

        #endregion

        #region 判断某个规则下是否有正在进行审批的单据

        [HttpGet]
        public string HasApprovingDocument(string ruleId)
        {
            ApprovalRuleModel rule = ApprovalRuleModel.SingleOrDefault(ruleId);
            if (rule != null)
            {
                List<ApprovalDocumentModel> list = ApprovalDocumentModel.Fetch(@"where Doc_TypeID = @0 
                                                                                    and Belongs_Department = @1
                                                                                    and Next_RoleId <> '-'
                                                                                    and Next_RoleId <> ''",
                                                                               rule.DocType, rule.Departmentid);
                if (list != null && list.Count > 0)
                {
                    return "1";
                }
            }

            return "0";
        }

        #endregion
    }
}
