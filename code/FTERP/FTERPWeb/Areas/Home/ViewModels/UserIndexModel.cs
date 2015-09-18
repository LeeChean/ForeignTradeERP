using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTERPWeb.Models;
using System.ComponentModel.DataAnnotations;
using FTERPWeb.Common;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class UserIndexModel : Record<UserIndexModel>
    {
        [Display(Name = "主键")]
        public string ID { get; set; }

        [Display(Name = "登录名")]
        public string Code { get; set; }

        [Display(Name = "部门")]
        public string DepartmentName { get; set; }

        [Display(Name = "角色")]
        public string RoleName
        {
            get
            {
                return CommonMethod.GetRoleName(ID);
            }
        }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "电话")]
        public string Phone { get; set; }

        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }

        [Display(Name = "性别")]
        public string Gender { get; set; }

        [Display(Name = "用户状态")]
        public string Status { get; set; }

        [Display(Name = "是否有效")]
        public string DelFlag { get; set; }

    }
}