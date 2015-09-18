using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using FTERPWeb.Models;
using System.Text;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class ApproveIndexModel : Record<ApproveIndexModel>
    {
        [Display(Name = "主键")]
        public string Id { get; set; }

        [Display(Name = "部门")]
        public string Department { get; set; }

        [Display(Name = "单据类别")]
        public string DocType { get; set; }

        [Display(Name = "审批流程")]
        public string Process { get; set; }

        [Display(Name = "是否有效")]
        public string DelFlag { get; set; }

        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }
    }
}