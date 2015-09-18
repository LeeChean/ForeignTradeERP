using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FTERPWeb.Models;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class AddUserModel
    {
        public string Id { get; set; }

        [Display(Name = "用户编号")]
        public string No { get; set; }

        [Display(Name = "登录名")]
        [Required(ErrorMessage = "{0}不可为空")]
        [Remote("ValidateUserCode", "Validate", "Home", AdditionalFields = "Id")]
        public string Code { get; set; }

        public string Password { get; set; }

        [Display(Name = "部门")]
        public string Departmentid { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}不可为空")]
        public string Name { get; set; }

        [Display(Name = "年龄")]
        public string Age { get; set; }

        [Display(Name = "电话")]
        [Required(ErrorMessage = "{0}不可为空")]
        public string Phone { get; set; }

        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Display(Name = "性别")]
        public int? Gender { get; set; }

        [Display(Name = "用户状态")]
        public int? Status { get; set; }

        public List<SelectListItem> FirstLevelDepartment
        {
            get
            {
                List<SelectListItem> department = new List<SelectListItem>();
                List<DepartmentModel> departmentModel = DepartmentModel.Fetch("where Del_Flag = 0 and PID = 0");

                department.Add(new SelectListItem() { Text = "请选择", Value = "" });

                foreach (DepartmentModel item in departmentModel)
                {
                    department.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.Id
                    });
                }

                return department;
            }
        }
    }
}