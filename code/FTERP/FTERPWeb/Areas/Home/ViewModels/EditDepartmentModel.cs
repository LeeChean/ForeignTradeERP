using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class EditDepartmentModel
    {
        [Display(Name = "主键")]
        public string Id { get; set; }

        [Display(Name = "部门名称")]
        [Required(ErrorMessage = "不可为空")]
        public string Name { get; set; }

        [Display(Name = "部门编号")]
        public string No { get; set; }

        [Display(Name = "部门经理")]
        public int? Managerid { get; set; }

        [Display(Name = "上级部门")]
        public string Pid { get; set; }

        [Display(Name = "排序编号")]
        public int? Sortno { get; set; }

        [Display(Name = "部门层级")]
        public int? DepLevel { get; set; }

        public string FullPid { get; set; }

        public string DocDepartment { get; set; }

    }
}