using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Home.ViewModels;
using FTERPWeb.Models;
using FTERPCommon;
using System.Data.SqlClient;
using FTERPWeb.Common;
using System.Data;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class ValidateController : Controller
    {
        /// <summary>
        /// 检查登录名是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonResult ValidateUserCode(AddUserModel model)
        {
            if (model != null && !string.IsNullOrWhiteSpace(model.Code))
            {
                //添加用户
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    if (UserModel.Exists("Code = @0", model.Code))
                    {
                        return Json("登录名已存在", JsonRequestBehavior.AllowGet);
                    }
                }
                //编辑用户
                else
                {
                    if (UserModel.Exists("Code = @0 and ID != @1", model.Code, model.Id))
                    {
                        return Json("登录名已存在", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查旧密码是否正确
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult ValidateOldPwd(EditPwdViewModel model)
        {
            if (model != null && !string.IsNullOrWhiteSpace(model.OldPwd) && !string.IsNullOrWhiteSpace(model.Id))
            {
                if (!UserModel.Exists("Password = @0 and ID = @1", model.OldPwd.VariationMd5(), model.Id))
                {
                    return Json("旧密码错误", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查确认密码和新密码是否一致
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult ValidateSamePwd(EditPwdViewModel model)
        {
            if (model != null && !string.IsNullOrWhiteSpace(model.NewPwd))
            {
                if (model.NewPwd != model.RepeatPwd)
                {
                    return Json("密码不一致", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查是否有权限编辑(单条)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        [HttpGet]
        public string HasAccess(string id, string tableName, string operateType)
        {
            string noAccessId = CommonMethod.HasDocumentAccess(id, tableName);
            operateType = operateType == "1" ? "编辑" : "删除";
            if (!string.IsNullOrWhiteSpace(noAccessId))
            {
                return string.Format("您不是编号为{0}记录的制单人，也不是单据所属业务员，不能{1}！", noAccessId, operateType);
            }

            return "1";
        }
    }
}
