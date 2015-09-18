using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class AddRoleModel
    {
        public string Id { get; set; }

        [Display(Name = "角色名称")]
        [Required(ErrorMessage = "{0}不可为空")]
        public string Name { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }
    }
}