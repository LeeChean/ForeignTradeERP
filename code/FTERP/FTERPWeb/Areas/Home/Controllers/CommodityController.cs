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

namespace FTERPWeb.Areas.Home.Controllers
{
    public class CommodityController : Controller
    {
        #region 商品列表

        [UserAuthorize]
        [ValidateInput(false)]
        public ActionResult Index(Page<CommodityIndexModel> model)
        {
            #region 获取商品列表

            StringBuilder sql = new StringBuilder();
            sql.Append("select c.ID,")
               .Append("       c.No,")
               .Append("       c.Type,")
               .Append("       c.Ch_Name as ChName,")
               .Append("       c.En_Name as EnName,")
               .Append("       c.Product_Code as ProductCode,")
               .Append("       c.Customs_No as CustomsNo,")
               .Append("       c.Tariff_Rate as TariffRate,")
               .Append("       c.Vat_Rate as VatRate,")
               .Append("       c.Refund_Rate as RefundRate,")
               .Append("       case when c.Del_Flag = 0 then '是'")
               .Append("            when c.Del_Flag = 1 then '否' end as DelFlag,")
               .Append("       u2.Name as BelongsMan,")
               .Append("       d.Full_Name as BelongsDepartment,")
               .Append("       u1.Name as CreateMan ")
               .Append("from Sys_Commodity c ")
               .Append("left join Sys_User u1 on c.Create_Man = u1.ID and u1.Del_Flag = 0 ")
               .Append("left join Sys_User u2 on c.Belongs_Man = u2.ID and u2.Del_Flag = 0 ")
               .Append("left join Sys_Department d on c.Belongs_Department = d.ID and d.Del_Flag = 0 ")
               .Append("where c.Del_Flag = 0");

            //管理员可以看到所有单据 其他用户只能看到3种数据：本人创建的、属于本人的、有权限组织的单据
            string[] roleArray = SysConfig.CurrentRoleNames.Split(',');
            if (!roleArray.Contains("管理员"))
            {
                sql.Append(" and (")
                          .Append("c.Create_Man in(")
                                              .Append("select ID from Sys_User where DepartmentID in(")
                                                       .Append(SysConfig.CurrentDepAuthInfo).Append(") ")
                                              .Append("union ")
                                              .Append("select ").Append(SysConfig.CurrentUser.Id)
                                          .Append(")")
                         .Append(" or c.Belongs_Man in(")
                                              .Append("select ID from Sys_User where DepartmentID in(")
                                                       .Append(SysConfig.CurrentDepAuthInfo).Append(") ")
                                              .Append("union ")
                                              .Append("select ").Append(SysConfig.CurrentUser.Id)
                                          .Append(")")
                         .Append(")");
            }

            #region 查询条件

            ViewBag.backType = string.IsNullOrWhiteSpace(Request.QueryString["backType"]) ? "0" : Request.QueryString["backType"];

            //商品编号
            if (!string.IsNullOrWhiteSpace(Request.Form["no"]))
            {
                sql.Append(" and c.No like '%").Append(Server.HtmlEncode(Request.Form["no"])).Append("%'");

                ViewBag.backType = "1";
            }
            //中文名称
            if (!string.IsNullOrWhiteSpace(Request.Form["chName"]))
            {
                sql.Append(" and c.Ch_Name like '%").Append(Server.HtmlEncode(Request.Form["chName"])).Append("%'");

                ViewBag.backType = "1";
            }
            //英文名称
            if (!string.IsNullOrWhiteSpace(Request.Form["enName"]))
            {
                sql.Append(" and c.En_Name like '%").Append(Server.HtmlEncode(Request.Form["enName"])).Append("%'");

                ViewBag.backType = "1";
            }
            //海关编码
            if (!string.IsNullOrWhiteSpace(Request.Form["customsNo"]))
            {
                sql.Append(" and c.Customs_No = ").Append(Request.Form["customsNo"]);

                ViewBag.backType = "1";
            }
            //是否状态
            if (!string.IsNullOrWhiteSpace(Request.Form["delFlag"]))
            {
                sql.Append(" and c.Del_Flag = ").Append(Request.Form["delFlag"]);

                ViewBag.backType = "1";
            }

            sql.Append(" order by c.Create_Time desc");

            #endregion

            #region 导出

            if (!string.IsNullOrWhiteSpace(Request["exportFlag"]) && Request["exportFlag"] == "1")
            {
                List<CommodityIndexModel> commodityList = CommodityIndexModel.Fetch(sql.ToString());

                //标题行
                List<string> titleList = new List<string>();
                titleList.Add("Type");
                titleList.Add("CustomsNo");
                titleList.Add("ChName");
                titleList.Add("EnName");
                titleList.Add("ProductCode");
                titleList.Add("TariffRate");
                titleList.Add("VatRate");
                titleList.Add("RefundRate");
                titleList.Add("DelFlag");
                titleList.Add("CreateMan");
                titleList.Add("BelongsMan");
                titleList.Add("BelongsDepartment");

                string fileName = Path.Combine("商品信息", DateTime.Now.ToString("yyyyMMddHHmmss"));

                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                ExcelHelper.DataToExcel(commodityList, "", titleList, fileName, this.HttpContext);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            #endregion

            if (model.CurrentPage == 0)
            {
                model.CurrentPage = 1;
            }

            var itemsPerPage = string.IsNullOrWhiteSpace(Request["itemsPerPage"]) ?
                SysConfig.ItemsPerPage : 0 == Request["itemsPerPage"].ToInt() ? 1 : Request["itemsPerPage"].ToInt();
            var items = CommodityIndexModel.Page(model.CurrentPage, SysConfig.ItemsPerPage, sql.ToString());

            ViewBag.CurrentPage = model.CurrentPage;

            return View(items);

            #endregion
        }

        #endregion

        #region 商品添加

        [UserAuthorize]
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Add()
        {
            return View(new CommodityViewModel());
        }

        [HttpPost]
        public string Add(CommodityViewModel model)
        {
#if DEBUG
            SysConfig.CurrentUser = UserModel.SingleOrDefault("3");
#endif
            if (ModelState.IsValid)
            {
                CommodityModel commodity = new CommodityModel();
                commodity.No = CommonMethod.GetLatestSerialNo(SysConfig.SerialNo.CommodityNo);    //商品编号
                commodity.ChName = model.ChName;                                                  //中文名称

                commodity.EnName = model.EnName;                                                  //英文名称
                commodity.ProductCode = model.ProductCode;                                        //商品货号
                commodity.Type = model.Type;                                                      //商品类别

                commodity.Unit = model.Unit;                                                      //商品单位
                commodity.TariffRate = model.TariffRate;                                          //关税率
                commodity.VatRate = model.VatRate;                                                //增值税率

                commodity.RefundRate = model.RefundRate;                                          //退税率
                commodity.CustomsNo = model.CustomsNo;                                            //海关编码
                commodity.Remark = model.Remark;                                                  //备注

                commodity.BelongsMan = CommonMethod.GetBelongsMan(                                //业务员
                    SysConfig.CurrentUser.Departmentid.ToString());
                commodity.BelongsDepartment = CommonMethod.GetBelongsDepartment(                  //业务部门
                    SysConfig.CurrentUser.Departmentid.ToString());

                commodity.ApprovalStatus = "4";                                                   //默认生效

                commodity.DelFlag = 0;                                                            //有效状态
                commodity.CreateMan = SysConfig.CurrentUser.Id;                                   //制单人
                commodity.CreateTime = DateTime.Now;                                              //制单时间

                int result = commodity.Insert().ToInt();

                if (result > 0)
                {
                    //更新商品最大序列号
                    CommonMethod.UpdateSerialNo(SysConfig.SerialNo.CommodityNo);

                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_Commodity");
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_SerialNo");

                    return "1";
                }
            }
            return "0";
        }

        #endregion

        #region 商品编辑

        [HttpGet]
        public ActionResult Edit(string id)
        {
            #region 检查参数

            string currentPage = string.IsNullOrWhiteSpace(Request["CurrentPage"]) ? "1" : Request["CurrentPage"];
            string backUrl = "/Home/Commodity/Index?backType=1&CurrentPage=" + currentPage;
            ViewBag.backUrl = backUrl;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect(backUrl);
            }

            #endregion

            #region 获取商品信息

            CommodityModel commodity = CommodityModel.SingleOrDefault(id);
            if (null == commodity)
            {
                return Redirect(backUrl);
            }

            CommodityViewModel model = new CommodityViewModel(commodity);

            return View(model);

            #endregion
        }

        [HttpPost]
        [ValidateInput(false)]
        public string Edit(CommodityViewModel model)
        {
#if DEBUG
            SysConfig.CurrentUser = UserModel.SingleOrDefault(3);
#endif
            if (ModelState.IsValid)
            {
                CommodityModel commodity = CommodityModel.SingleOrDefault(model.Id);

                commodity.ChName = model.ChName;                           //中文名称
                commodity.EnName = model.EnName;                           //英文名称

                commodity.ProductCode = model.ProductCode;                 //商品货号
                commodity.Type = model.Type;                               //商品类别
                commodity.Unit = model.Unit;                               //商品单位

                commodity.TariffRate = model.TariffRate;                   //关税率
                commodity.VatRate = model.VatRate;                         //增值税率
                commodity.RefundRate = model.RefundRate;                   //退税率

                commodity.CustomsNo = model.CustomsNo;                     //海关编码
                commodity.Remark = model.Remark;                           //备注

                commodity.ModifyMan = SysConfig.CurrentUser.Id;            //修改人
                commodity.ModifyTime = DateTime.Now;                       //修改时间
                int result = commodity.Update();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_Commodity");

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 商品详情

        public ActionResult Details(string id)
        {
            #region 检查参数

            string currentPage = string.IsNullOrWhiteSpace(Request["CurrentPage"]) ? "1" : Request["CurrentPage"];
            string backUrl = "/Home/Commodity/Index?backType=1&CurrentPage=" + currentPage;
            ViewBag.backUrl = backUrl;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect(backUrl);
            }

            #endregion

            #region 获取商品信息

            CommodityModel commodity = CommodityModel.SingleOrDefault(id);
            if (null == commodity)
            {
                return Redirect(backUrl);
            }

            //业务员
            ViewBag.BelongsMan = CommonMethod.GetUserNameByID(commodity.BelongsMan.ToString());

            //业务部门
            ViewBag.BelongsDepartment = CommonMethod.GetDepNameById(commodity.BelongsDepartment.ToString());

            //制单人
            ViewBag.CreateMan = CommonMethod.GetUserNameByID(commodity.CreateMan);

            //制单时间
            ViewBag.CreateTime = commodity.CreateTime.Value.ToString("yyyy-MM-dd");

            return View(commodity);

            #endregion
        }

        #endregion

        #region 商品删除

        [HttpPost]
        public string Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return "0";
            }

            if (CommodityModel.Update(string.Format("set Del_Flag = 1 where ID in ({0})", id)) > 0)
            {
                //记录操作日志
                CommonMethod.Log(SysConfig.CurrentUser.Id, "Delete", "Sys_Commodity",
                              string.Format("将主键为{0}的记录置为无效", id));

                return "1";
            }
            return "0";

        }

        #endregion
    }
}
