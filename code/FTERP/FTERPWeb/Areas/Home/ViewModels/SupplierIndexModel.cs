using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTERPWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class SupplierIndexModel : Record<SupplierIndexModel>
    {
        [Display(Name = "主键")]
        public string Id { get; set; }

        [Display(Name = "供应商编号")]
        public string No { get; set; }

        [Display(Name = "供应商类别")]
        public string Type { get; set; }

        [Display(Name = "中文名称")]
        public string ChName { get; set; }

        [Display(Name = "英文名称")]
        public string EnName { get; set; }

        [Display(Name = "简称")]
        public string ShortName { get; set; }

        [Display(Name = "国家")]
        public string Country { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "电话")]
        public string Phone { get; set; }

        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Display(Name = "是否有效")]
        public string DelFlag { get; set; }

        [Display(Name = "审批状态")]
        public string ApprovalStatus { get; set; }

        [Display(Name = "制单人")]
        public string CreateMan { get; set; }

        [Display(Name = "制单时间")]
        public string CreateTime { get; set; }

        [Display(Name = "业务员")]
        public string BelongsMan { get; set; }

        [Display(Name = "业务部门")]
        public string BelongsDepartment { get; set; }
    }
}