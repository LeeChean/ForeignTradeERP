using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTERPWeb.Models;
using FTERPCommon;
using System.Web.Mvc;
using System.Text;
using FTERPWeb.Home.ViewModels;
using System.Data.SqlClient;
using System.Data;

namespace FTERPWeb.Common
{
    public static class CommonMethod
    {
        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="operType"></param>
        /// <param name="tableName"></param>
        /// <param name="sqlInfo"></param>
        public static void Log(string userId, string operType, string tableName, string remark = "")
        {
            LogModel log = new LogModel();
            log.TableName = tableName;
            log.OperateType = operType;
            log.Remark = remark;
            log.CreateMan = userId;
            log.CreateTime = DateTime.Now;
            log.Insert();
        }

        /// <summary>
        /// 获取目前可用的业务单据编号
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetLatestSerialNo(int type)
        {
            string prefix = "";
            switch (type)
            {
                case SysConfig.SerialNo.ClientNo:
                    prefix = "C";
                    break;
                case SysConfig.SerialNo.SupplierNo:
                    prefix = "E";
                    break;
                case SysConfig.SerialNo.CommodityNo:
                    prefix = "M";
                    break;
            }

            string no = prefix + "0001";
            SerialnoModel model = SerialnoModel.FirstOrDefault("where Type = @0", type);
            if (model != null)
            {
                no = prefix + string.Format("{0:0000}", (model.No.Substring(1).ToInt() + 1));
            }

            return no;
        }

        /// <summary>
        /// 更新业务单据最大编号
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public static void UpdateSerialNo(int type)
        {
            string prefix = "";
            switch (type)
            {
                case SysConfig.SerialNo.ClientNo:
                    prefix = "C";
                    break;
                case SysConfig.SerialNo.SupplierNo:
                    prefix = "E";
                    break;
                case SysConfig.SerialNo.CommodityNo:
                    prefix = "M";
                    break;
            }

            SerialnoModel model = SerialnoModel.FirstOrDefault("where Type = @0", type);
            if (model == null)
            {
                model = new SerialnoModel();

                model.No = prefix + "0001";
                model.Type = type;
                model.Insert();
            }
            else
            {
                model.No = prefix + string.Format("{0:0000}", (model.No.Substring(1).ToInt() + 1));
                model.Update();
            }
        }

        /// <summary>
        /// 获取国家列表
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetCountryList()
        {
            List<SelectListItem> country = new List<SelectListItem>();
            List<CountryModel> list = CountryModel.Fetch("where Del_Flag = 0 order by SortNo");

            foreach (CountryModel item in list)
            {
                country.Add(new SelectListItem() { Text = item.Chname, Value = item.Id });
            }

            return country;
        }

        /// <summary>
        /// 获取某部门下单据的所属人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetBelongsMan(string departmentId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select u.ID 
                         from Sys_User u
                         inner join Sys_User_Role ur on u.ID = ur.UserID
                         inner join Sys_Role r on ur.RoleID = r.ID
                         where u.DepartmentID = @0 and r.Name = '").Append("业务员").Append("'");

            UserModel user = UserModel.FirstOrDefault(sql.ToString(), departmentId);

            return user == null ? 0 : user.Id.ToInt();
        }

        /// <summary>
        /// 获取某组织下单据的所属部门
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetBelongsDepartment(string departmentId)
        {
            #region 获取所有上级部门、当前部门

            List<DepartmentModel> allDepartment = new List<DepartmentModel>();

            //当前部门
            DepartmentModel currentDep = DepartmentModel.SingleOrDefault(departmentId);
            allDepartment.Add(currentDep);

            //所有上级部门
            if (currentDep != null)
            {
                string[] fullPid = currentDep.FullPid.Split('-');
                fullPid = fullPid.Reverse().ToArray();
                foreach (string pid in fullPid)
                {
                    DepartmentModel dep = DepartmentModel.SingleOrDefault(pid);
                    if (dep != null)
                    {
                        allDepartment.Add(dep);
                    }
                }
            }

            #endregion

            #region 获取单据所属部门

            foreach (DepartmentModel item in allDepartment)
            {
                if (item.DocDepartment == 1)
                {
                    return item.Id.ToInt();
                }
            }

            //默认返回当前组织
            return currentDep.Id.ToInt();

            #endregion
        }

        /// <summary>
        /// 获取某用户的所有角色名称
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetRoleName(string userId)
        {
            List<RoleModel> roleList = RoleModel.Fetch(@"select r.Name 
                                                         from Sys_Role r
                                                         inner join Sys_User_Role ur on r.ID = ur.RoleID
                                                         inner join Sys_User u on u.ID = ur.UserID
                                                         where u.ID = @0 and r.Del_Flag = 0 order by r.Create_Time ", userId);

            List<string> roleName = new List<string>();
            foreach (RoleModel item in roleList)
            {
                roleName.Add(item.Name);
            }

            return string.Join(",", roleName.ToArray());
        }

        /// <summary>
        /// 根据fullPid获取部门全称
        /// </summary>
        /// <param name="fullPid"></param>
        /// <returns></returns>
        public static string GetDepFullName(string fullPid)
        {
            StringBuilder fullName = new StringBuilder();
            string[] pidList = fullPid.Split('-');
            foreach (string item in pidList)
            {
                DepartmentModel department = DepartmentModel.SingleOrDefault(item);
                if (department != null)
                {
                    fullName.Append(department.Name);
                }
            }
            return fullName.ToString();
        }

        /// <summary>
        /// 根据主键获取部门全称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDepNameById(string id)
        {
            DepartmentModel department = DepartmentModel.SingleOrDefault(id);
            return department == null ? "" : department.FullName;
        }

        /// <summary>
        /// 所有的审批状态
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> ApprovalStatus
        {
            get
            {
                List<SelectListItem> approvalStatus = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="全部",Value=""},
                    new SelectListItem(){Text="未提交",Value="1"},
                    new SelectListItem(){Text="变更未提交",Value="2"},
                    new SelectListItem(){Text="已提交",Value="3"},
                    new SelectListItem(){Text="已生效",Value="4"},
                    new SelectListItem(){Text="已退回",Value="5"}
                };

                return approvalStatus;
            }
        }

        /// <summary>
        /// 获取单据类别列表
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetDocTypeList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<DocumentTypeModel> docTypeList = DocumentTypeModel.Fetch("where Del_Flag = 0");
            foreach (DocumentTypeModel item in docTypeList)
            {
                list.Add(new SelectListItem() { Text = item.Typename, Value = item.Id });
            }

            return list;
        }

        /// <summary>
        /// 获取某部门的下级部门
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetSubDepartment(string pId)
        {
            List<SelectListItem> depItem = new List<SelectListItem>();

            if (pId != "0")
            {
                depItem.Add(new SelectListItem() { Text = "请选择", Value = "" });
            }
            List<DepartmentModel> departmentModel = DepartmentModel.Fetch("where Del_Flag = 0 and PID = @0 order by SortNo", pId);
            foreach (DepartmentModel item in departmentModel)
            {
                depItem.Add(new SelectListItem() { Text = item.Name, Value = item.Id });
            }

            return depItem;
        }

        /// <summary>
        /// 获取某用户的待审批列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<UnapprovedIndexModel> GetUnapprovedList(int department, string role)
        {
            #region 根据单据审批表获取需要当前用户审批的单据

            #region old

            //            List<UnapprovedIndexModel> docList = new List<UnapprovedIndexModel>();

            //            StringBuilder sql = new StringBuilder();
            //            sql.Append(@"select ID,
            //                                Table_Name,
            //                                DocID,
            //                                Next_RoleID,
            //                                Doc_TypeID,
            //                                
            //                         from Sys_Approval_Document ad
            //                         inner join Sys_User u on ad.Create_Man = u.ID and u.Del_Flag = 0
            //                         where belongs_department in
            //                           (
            //                             select ID from Sys_Department 
            //                             where Full_PID like '%").Append(department).Append("%' ")
            //                    .Append("union ")
            //                    .Append("select ").Append(department)
            //                   .Append(")")
            //                   .Append("and Next_RoleID in (").Append(string.Join(",", role)).Append(") ")
            //               .Append("order by Modify_Time desc");
            //            List<ApprovalDocumentModel> list = ApprovalDocumentModel.Fetch(sql.ToString());

            //            if (list != null)
            //            {
            //                foreach (ApprovalDocumentModel item in list)
            //                {
            //                    switch (item.TableName.ToLower())
            //                    {
            //                        //客户表
            //                        case "sys_client":
            //                            ClientModel client = ClientModel.SingleOrDefault(item.Docid);
            //                            if (client != null)
            //                            {
            //                                docList.Add(new UnapprovedIndexModel()
            //                                {
            //                                    DocId = client.Id,
            //                                    DocType = item.DocTypeid.ToString(),
            //                                    TableName = "客户表",
            //                                    CreateMan = GetUserNameByID(client.CreateMan),
            //                                    CreateTime = client.CreateTime.Value.ToString("yyyy-MM-dd"),
            //                                    Url = "/Home/Client/Approve?aId=" + item.Id + "&docId=" + client.Id + "&roleId=" + item.NextRoleid,
            //                                    Title = client.ChName
            //                                });
            //                            }
            //                            break;
            //                        //供应商表
            //                        case "sys_supplier":
            //                            SupplierModel supplier = SupplierModel.SingleOrDefault(item.Docid);
            //                            if (supplier != null)
            //                            {
            //                                docList.Add(new UnapprovedIndexModel()
            //                                {
            //                                    DocId = supplier.Id,
            //                                    DocType = item.DocTypeid.ToString(),
            //                                    TableName = "供应商表",
            //                                    CreateMan = GetUserNameByID(supplier.CreateMan),
            //                                    CreateTime = supplier.CreateTime.Value.ToString("yyyy-MM-dd"),
            //                                    Url = "/Home/Supplier/Approve?aId=" + item.Id + "&docId=" + supplier.Id + "&roleId=" + item.NextRoleid,
            //                                    Title = supplier.ChName
            //                                });
            //                            }
            //                            break;
            //                    }
            //                }
            //            }

            #endregion

            StringBuilder sql = new StringBuilder();
            sql.Append(@"select ad.ID as ApprovalDocumentId,
                                ad.DocId,
                                ad.Table_Name as TableName,
                                dt.TypeName as DocType,
                                ad.Next_RoleID as RoleId,
                                u.Name as CreateMan,
                                CONVERT(nvarchar(10),ad.Create_Time,20) as CreateTime
                         from Sys_Approval_Document ad
                         inner join Sys_Document_Type dt on ad.Doc_TypeID = dt.ID
                         inner join Sys_User u on ad.Create_Man = u.ID and u.Del_Flag = 0
                         where ad.Belongs_Department in
                           (
                             select ID from Sys_Department 
                             where Full_PID like '%").Append(department).Append("%' ")
                    .Append("union ")
                    .Append("select ").Append(department)
                   .Append(")")
                   .Append("and ad.Next_RoleID in (").Append(string.Join(",", role)).Append(") ")
               .Append("order by ad.Modify_Time desc");
            List<UnapprovedIndexModel> list = UnapprovedIndexModel.Fetch(sql.ToString());

            if (list != null)
            {
                foreach (UnapprovedIndexModel item in list)
                {
                    switch (item.TableName.ToLower())
                    {
                        //客户表
                        case "sys_client":
                            ClientModel client = ClientModel.SingleOrDefault(item.DocId);
                            if (client != null)
                            {
                                item.Title = client.No;
                                item.Url = "/Home/Client/Approve?aId=" + item.ApprovalDocumentId +
                                           "&docId=" + item.DocId + "&roleId=" + item.RoleId;
                            }
                            break;
                        //供应商表
                        case "sys_supplier":
                            SupplierModel supplier = SupplierModel.SingleOrDefault(item.DocId);
                            if (supplier != null)
                            {
                                item.Title = supplier.No;
                                item.Url = "/Home/Supplier/Approve?aId=" + item.ApprovalDocumentId +
                                           "&docId=" + item.DocId + "&roleId=" + item.RoleId;

                            }
                            break;
                    }
                }
            }

            return list;

            #endregion
        }

        /// <summary>
        /// 提交某种单据(可批量)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string Submit(string docType, string departmentId, string tableName, string id)
        {
            #region 检查参数是否为空

            if (string.IsNullOrWhiteSpace(id))
            {
                return "0";
            }

            #endregion

            #region 检查是否有权限提交

            string noAccessId = HasDocumentAccess(id, tableName);
            if (!string.IsNullOrWhiteSpace(noAccessId))
            {
                return "您不是编号为" + noAccessId + "记录的制单人，也不是单据所属业务员，不能提交！";
            }

            #endregion

            #region 检查是否重复提交

            SqlConnection conn = SqlHelper.CreateConn();

            List<string> submitedId = new List<string>();

            foreach (string item in id.Split(','))
            {
                string sql_Check = string.Format(@"select No 
                                                   from {0} 
                                                   where ID = {1} and Del_Flag = 0 
                                                       and Approval_Status = 3", tableName, item);

                IDataReader reader = SqlHelper.ExecuteReader(conn, null, CommandType.Text, sql_Check);
                while (reader.Read())
                {
                    submitedId.Add(reader[0].ToString());
                }
                reader.Close();
            }

            if (submitedId.Count > 0)
            {
                return "编号为" + string.Join(",", submitedId) + "的记录已经提交，不能重复操作！";
            }

            #endregion

            #region 通过事务修改单据的审批状态 同时初始化一条单据审批记录

            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                #region 更新单据的审批状态

                string sql_Doc = string.Format(@"update {0} set Approval_Status = 3
                                                where ID in ({1})", tableName, id);

                SqlHelper.ExecuteNonQuery(conn, tran, CommandType.Text, sql_Doc);

                #endregion

                #region 初始化单据审批表

                foreach (string item in id.Split(','))
                {
                    string sql_Progress = string.Format(@"insert into Sys_Approval_Document(
                                                                    Table_Name,
                                                                    Doc_TypeID,
                                                                    DocID,
                                                                    Next_RoleID,
                                                                    Belongs_Department,
                                                                    Create_Man,
                                                                    Create_Time,
                                                                    Modify_Man,
                                                                    Modify_Time)
                                                          values ('{0}',{1},{2},'{3}',{4},{5},getdate(),{6},getdate())",
                                                        tableName,
                                                        docType,
                                                        item.ToInt(),
                                                        GetNextRoleID(docType, departmentId),
                                                        departmentId,
                                                        SysConfig.CurrentUser.Id,
                                                        SysConfig.CurrentUser.Id);

                    SqlHelper.ExecuteNonQuery(conn, tran, CommandType.Text, sql_Progress);
                }

                #endregion

                #region 记录日志

                string sql_log1 = string.Format(@"insert into Sys_Log(
                                                                Table_Name,
                                                                Operate_Type,
                                                                Create_Man,
                                                                Create_Time,
                                                                Remark)
                                                      values('{0}','{1}',{2},getdate(),'{3}')",
                                                tableName, "Update", SysConfig.CurrentUser.Id,
                                                string.Format("将主键为{0}的记录的审批状态改为\"已提交\"", id));

                string sql_log2 = string.Format(@"insert into Sys_Log(
                                                                Table_Name,
                                                                Operate_Type,
                                                                Create_Man,
                                                                Create_Time,
                                                                Remark)
                                                      values('{0}','{1}',{2},getdate(),'{3}')",
                                                "Sys_Approval_Document", "Insert", SysConfig.CurrentUser.Id,
                                                string.Format("为主键{0}的单据初始化单据审批记录", id));

                SqlHelper.ExecuteNonQuery(conn, tran, CommandType.Text, sql_log1);
                SqlHelper.ExecuteNonQuery(conn, tran, CommandType.Text, sql_log2);

                #endregion

                tran.Commit();

                return "1";
            }
            catch (Exception e)
            {
                tran.Rollback();

                return "0";
            }
            finally
            {
                conn.Close();
            }

            #endregion
        }

        /// <summary>
        /// 审批某种单据
        /// </summary>
        /// <param name="docType"></param>
        /// <param name="tableName"></param>
        /// <param name="docId"></param>
        /// <param name="approvalDocumentId"></param>
        /// <param name="type"></param>
        /// <param name="roleId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static string Approve(string docType, string tableName, string docId, string approvalDocumentId,
            string type, string roleId, string remark)
        {
            #region 检查参数

            if (string.IsNullOrWhiteSpace(docType) || string.IsNullOrWhiteSpace(docId) ||
                string.IsNullOrWhiteSpace(approvalDocumentId) || string.IsNullOrWhiteSpace(type)
                || string.IsNullOrWhiteSpace(roleId))
            {
                return "0";
            }

            #endregion

            #region 通过事务进行审批

            SqlConnection conn = SqlHelper.CreateConn();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                #region 首先在审批进度表中添加记录

                string sql_Progress = string.Format(@"insert into Sys_Approval_Progress(
                                                                    Approval_DocumentID,
                                                                    RoleID,
                                                                    Status,
                                                                    Remark,
                                                                    Create_Man,
                                                                    Create_Time)
                                                        values({0},{1},'{2}','{3}',{4},getdate())",
                                                   approvalDocumentId,
                                                   roleId,
                                                   type,
                                                   remark,
                                                   SysConfig.CurrentUser.Id);

                SqlHelper.ExecuteNonQuery(conn, tran, CommandType.Text, sql_Progress);

                #endregion

                #region 然后在单据审批表中更新对应记录

                ApprovalDocumentModel approvalDocument = ApprovalDocumentModel.SingleOrDefault(approvalDocumentId);
                if (approvalDocument == null)
                {
                    tran.Rollback();
                    return "0";
                }

                string nextRoleId = "";

                //如果驳回 则把下一个审批角色置为-
                if (type.Equals("驳回"))
                {
                    nextRoleId = "-";
                }
                //否则 按照审批规则取下一个审批角色
                else
                {
                    nextRoleId = CommonMethod.GetNextRoleID(docType, approvalDocument.BelongsDepartment.ToString(), roleId);
                }

                string sql_Approval_Document = string.Format(@"update Sys_Approval_Document set
                                                                    Next_RoleID = '{0}',
                                                                    Modify_Man = {1},
                                                                    Modify_Time = getdate()
                                                               where ID = {2}",
                                                             nextRoleId,
                                                             SysConfig.CurrentUser.Id,
                                                             approvalDocumentId);

                SqlHelper.ExecuteNonQuery(conn, tran, CommandType.Text, sql_Approval_Document);

                #endregion

                #region 最后更新单据表中对应记录的审批状态

                if (string.IsNullOrWhiteSpace(nextRoleId) || nextRoleId == "-")
                {
                    string status = nextRoleId == "-" ? "5" : "4";
                    string sql_Document = string.Format(@"update {0} set
                                                               Approval_Status = '{1}',
                                                               Modify_Man = {2},
                                                               Modify_Time = getdate()
                                                          where ID = {3}",
                                                        tableName,
                                                        status,
                                                        SysConfig.CurrentUser.Id,
                                                        docId);

                    SqlHelper.ExecuteNonQuery(conn, tran, CommandType.Text, sql_Document);
                }

                #endregion

                tran.Commit();

                #region 记录日志

                CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_Approval_Progress",
                                      string.Format("添加关于Sys_Approval_Document表中主键为{0}的审批历史数据", approvalDocumentId));

                CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_Approval_Document", string.Format("更新下一个需要审批的角色"));

                if (string.IsNullOrWhiteSpace(nextRoleId))
                {
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", tableName, string.Format("审批结束，更新主键为{0}的审批状态", docId));
                }

                #endregion

                return "1";
            }
            catch (Exception e)
            {
                tran.Rollback();

                return "0";
            }
            finally
            {
                conn.Close();
            }

            #endregion
        }

        /// <summary>
        /// 获取下一个审批角色
        /// </summary>
        /// <returns></returns>
        public static string GetNextRoleID(string docType, string departmentId, string currentRoleId = "")
        {
            ApprovalRuleModel rule = ApprovalRuleModel.FirstOrDefault("where Del_Flag = 0 and Doc_Type = @0 and DepartmentID = @1",
                                                                      docType, departmentId);

            if (rule != null)
            {
                string[] roleList = rule.Roleid.Split(',');
                if (string.IsNullOrWhiteSpace(currentRoleId))
                {
                    return roleList[0];
                }
                else
                {
                    int index = roleList.ToList().IndexOf(currentRoleId) + 1;
                    return index >= roleList.Length ? "" : roleList[index];
                }
            }

            return "";
        }

        /// <summary>
        /// 根据ID获取用户姓名
        /// </summary>
        /// <returns></returns>
        public static string GetUserNameByID(string id)
        {
            UserModel user = UserModel.SingleOrDefault(id);
            return user == null ? "" : user.Name;
        }

        /// <summary>
        /// 获取某部门的所有上级和下级部门列表
        /// </summary>
        /// <returns></returns>
        public static List<List<SelectListItem>> GetFullDepartmentList(string departmentId, out string depIdList)
        {
            DepartmentModel department = DepartmentModel.SingleOrDefault(departmentId);
            List<List<SelectListItem>> depList = new List<List<SelectListItem>>();

            if (null != department)
            {
                string[] pIdArray = department.FullPid.Split('-');

                foreach (string pId in pIdArray)
                {
                    List<SelectListItem> depItem = GetSubDepartment(pId);
                    depList.Add(depItem);
                }

                depIdList = (department.FullPid + "-" + departmentId).Remove(0, 2);
            }
            else
            {
                List<SelectListItem> depItem = GetSubDepartment("0");
                depList.Add(depItem);

                depIdList = "0";
            }

            List<SelectListItem> subDep = GetSubDepartment(departmentId);
            depList.Add(subDep);

            return depList;
        }

        /// <summary>
        /// 获取某个单据的审批历史流程
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        public static List<ApprovalProgressViewModel> GetApprovalHistoryList(string docId)
        {
            string sql = string.Format(@"select r.Name as RoleName,
                                                u.Name as UserName,
                                                case when ah.status = 0 then '驳回'
                                                     when ah.status = 1 then '通过' end as Status,
                                                ah.Remark,
                                                convert(nvarchar(16),ah.create_time,20) as Time
                                         from sys_approval_history ah
                                         inner join Sys_Approval_Progress ap on ah.progressID = ap.ID
                                         left join Sys_User u on ah.create_man = u.ID
                                         left join Sys_Role r on ah.roleid = r.ID
                                         where ap.DocID = {0}
                                         order by ah.create_Time", docId);

            List<ApprovalProgressViewModel> history = ApprovalProgressViewModel.Fetch(sql);
            return history;
        }

        /// <summary>
        /// 获取某用户的所有角色
        /// </summary>
        /// <returns></returns>
        public static List<RoleModel> GetRoleByUserId(string userId)
        {
            List<RoleModel> roleList = RoleModel.Fetch(@"select r.ID,r.Name 
                                                         from Sys_Role r
                                                         inner join Sys_User_Role ur on ur.RoleID = r.ID
                                                         where ur.UserID = @0", userId);

            return roleList;
        }

        /// <summary>
        /// 获取某用户的所有模块权限
        /// </summary>
        /// <returns></returns>
        public static List<FuncModel> GetAccessByUserId(string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select distinct f.ID,
                                         f.PID,
                                         f.Name,
                                         f.Title,
                                         f.Url,
                                         f.Func_Level,
                                         f.Display_Flag,
                                         f.SortNo
                         from sys_func f ");

            if (SysConfig.CurrentRoleNames.Contains("管理员"))
            {
                sql.Append(@"where f.Del_Flag = 0 order by f.SortNo");
            }
            else
            {
                sql.Append(@"inner join sys_access a on f.ID = a.funcid
                             inner join Sys_User_Role ur on ur.RoleID = a.roleid
                             where f.Del_Flag = 0 and ur.UserID = @0
                             order by f.SortNo");
            }
            List<FuncModel> funcList = FuncModel.Fetch(sql.ToString(), userId);

            return funcList;
        }

        /// <summary>
        /// 获取某用户的所有部门权限
        /// </summary>
        /// <returns></returns>
        public static string GetDepAccess(string roleId, string departmentId)
        {
            List<string> depList = new List<string>();
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select distinct da.DepID
                         from Sys_DepAccess da
                         where BelongDepID = ").Append(departmentId)
                   .Append(" and RoleID in (").Append(roleId).Append(")");

            SqlConnection conn = SqlHelper.CreateConn();
            IDataReader reader = SqlHelper.ExecuteReader(conn, null, CommandType.Text, sql.ToString());
            while (reader.Read())
            {
                depList.Add(reader[0].ToString());
            }
            reader.Close();

            if (depList.Count == 0)
            {
                depList.Add("0");
            }

            return string.Join(",", depList);
        }

        /// <summary>
        /// 判断当前用户没有哪些单据的权限（既不是制单人也不是所属业务员）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string HasDocumentAccess(string id, string tableName)
        {
            string[] roleArray = SysConfig.CurrentRoleNames.Split(',');
            if (roleArray.Contains("管理员"))
            {
                return "";
            }

            SqlConnection conn = SqlHelper.CreateConn();

            List<string> submitedId = new List<string>();

            foreach (string item in id.Split(','))
            {
                string sql_Check = string.Format(@"select No,Create_Man,Belongs_Man from {0}
                                                   where ID = {1} and Del_Flag = 0", tableName, item);

                IDataReader reader = SqlHelper.ExecuteReader(conn, null, CommandType.Text, sql_Check);

                while (reader.Read())
                {
                    if (reader[1].ToString() != SysConfig.CurrentUser.Id && reader[2].ToString() != SysConfig.CurrentUser.Id)
                    {
                        submitedId.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }

            return string.Join(",", submitedId);
        }
    }
}