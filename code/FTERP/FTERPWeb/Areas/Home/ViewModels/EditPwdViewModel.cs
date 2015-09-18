using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using FTERPWeb.Common;

namespace FTERPWeb.Home.ViewModels
{
    public class EditPwdViewModel
    {
        public string Id
        {
            get
            {
                return SysConfig.CurrentUser == null ? "" : SysConfig.CurrentUser.Id;
            }
        }

        [Display(Name = "旧密码")]
        [Required(ErrorMessage = "{0}不可为空")]
        [Remote("ValidateOldPwd", "Validate", "Home")]
        public string OldPwd { get; set; }

        [Display(Name = "新密码")]
        [Required(ErrorMessage = "{0}不可为空")]
        public string NewPwd { get; set; }

        [Display(Name = "确认密码")]
        [Required(ErrorMessage = "请再次输入新密码")]
        [Remote("ValidateSamePwd", "Validate", "Home", AdditionalFields = "NewPwd")]
        public string RepeatPwd { get; set; }
    }
}