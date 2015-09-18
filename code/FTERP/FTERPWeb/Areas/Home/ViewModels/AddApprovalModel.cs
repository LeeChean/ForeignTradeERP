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
    public class AddApprovalModel
    {
        [Display(Name = "主键")]
        public string Id { get; set; }

        [Display(Name = "单据类别")]
        [Required(ErrorMessage = "{0}不可为空")]
        public string Type { get; set; }

        [Display(Name = "审批流程")]
        [Required(ErrorMessage = "{0}不可为空")]
        public string Process { get; set; }

        public string RoleId { get; set; }

        [Display(Name = "部门")]
        [Required(ErrorMessage = "{0}不可为空")]
        public string DepartmentId { get; set; }

        public List<SelectListItem> FirstLevelDepartment
        {
            get
            {
                List<SelectListItem> department = new List<SelectListItem>();
                List<DepartmentModel> departmentModel = DepartmentModel.Fetch("where Del_Flag = 0 and PID = 0");

                foreach (DepartmentModel item in departmentModel)
                {
                    department.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.Id
                    });
                }

                DepartmentId = department[0].Value;

                return department;
            }
        }
    }
}