using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Home.ViewModels;
using FTERPWeb.Models;
using FTERPCommon;
using System.Text;
using FTERPWeb.Common.Filter;
using FTERPWeb.Common;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class FuncController : Controller
    {
        [UserAuthorize]
        public ActionResult Index()
        {
            return View(new EditFuncModel());
        }

        #region 添加下级模块

        [HttpPost]
        [ValidateInput(false)]
        public string AddSub(EditFuncModel model)
        {
            if (ModelState.IsValid)
            {
                FuncModel func = new FuncModel();
                func.Name = model.Name;                         //英文名称
                func.Title = model.Title;                       //中文名称
                func.DisplayFlag = model.DisplayFlag;           //是否显示
                func.Sortno = model.Sortno;                     //序号
                func.Url = model.Url;                           //链接url
                func.Pid = model.Id.ToInt();                    //上级模块
                func.FullPid = model.FullPid + "-" + model.Id;  //所有上级模块
                func.FuncLevel = model.FuncLevel + 1;           //模块层级
                func.CreateMan = SysConfig.CurrentUser.Id;      //创建人
                func.CreateTime = DateTime.Now;                 //创建时间

                int result = (int)func.Insert();

                if (result > 0)
                {
                    //记录操作日志
                    FuncModel parent = FuncModel.SingleOrDefault(model.Id);
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_Func",
                                  string.Format("给[{0}]模块添加【{1}】子模块",
                                  parent == null ? "" : parent.Title, func.Title));

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 添加同级模块

        [HttpPost]
        public string Add(EditFuncModel model)
        {
            if (ModelState.IsValid)
            {
                FuncModel func = new FuncModel();
                func.Name = model.Name;                         //英文名称
                func.Title = model.Title;                       //中文名称
                func.DisplayFlag = model.DisplayFlag;           //是否显示
                func.Sortno = model.Sortno;                     //序号
                func.Url = model.Url;                           //链接url
                func.Pid = model.Pid;                           //上级模块
                func.FullPid = model.FullPid == null ? "0" : model.FullPid;     //所有上级模块
                func.FuncLevel = model.FuncLevel;               //模块层级
                func.CreateMan = SysConfig.CurrentUser.Id;      //创建人
                func.CreateTime = DateTime.Now;                 //创建时间

                int result = (int)func.Insert();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Insert", "Sys_Func",
                                  string.Format("添加【{0}】模块", func.Title));

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 编辑模块

        [HttpPost]
        public string Edit(EditFuncModel model)
        {
            if (ModelState.IsValid)
            {
                FuncModel func = FuncModel.SingleOrDefault(model.Id.ToInt());
                if (null == func)
                {
                    return "0";
                }

                func.Name = model.Name;                         //英文名称
                func.Title = model.Title;                       //中文名称
                func.DisplayFlag = model.DisplayFlag.ToInt();   //是否显示
                func.Sortno = model.Sortno;                     //序号
                func.FuncLevel = model.FuncLevel;               //模块层级
                func.Url = model.Url;                           //链接url
                func.ModifyMan = SysConfig.CurrentUser.Id;      //创建人
                func.ModifyTime = DateTime.Now;                 //创建时间
                //func.FullPid = model.FullPid == null ? "0" : model.FullPid;     //所有上级部门

                int result = func.Update();

                if (result > 0)
                {
                    //记录操作日志
                    CommonMethod.Log(SysConfig.CurrentUser.Id, "Update", "Sys_Func",
                                  string.Format("编辑【{0}】模块", func.Title));

                    return "1";
                }
            }

            return "0";
        }

        #endregion

        #region 删除模块

        [HttpPost]
        public string Delete(string funcId)
        {
            if (string.IsNullOrWhiteSpace(funcId))
            {
                return "0";
            }

            try
            {
                FuncModel func = FuncModel.SingleOrDefault(funcId);

                FuncModel.Delete(string.Format("where ID = {0} or Full_PID like '%{1}%'", funcId, funcId));

                //记录操作日志
                CommonMethod.Log(SysConfig.CurrentUser.Id, "Delete", "Sys_Func",
                              string.Format("删除【{0}】模块", func == null ? "" : func.Title));

                return "1";
            }
            catch (Exception e)
            {
                return "0";
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获得所有的模块
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFuncTree()
        {
            List<FuncModel> nodeList = FuncModel.Fetch(" where Del_Flag = 0 order by SortNo");

            return Json(nodeList, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
