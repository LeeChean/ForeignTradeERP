using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class EditFuncModel
    {
        public string Id { get; set; }

        [Display(Name = "英文名称")]
        [Required(ErrorMessage = "{0}不可为空")]
        public string Name { get; set; }

        [Display(Name = "中文名称")]
        [Required(ErrorMessage = "{0}不可为空")]
        public string Title { get; set; }

        public int? Type { get; set; }

        public int? Pid { get; set; }

        public int? FuncLevel { get; set; }

        public string Url { get; set; }

        public string Framename { get; set; }

        public int? Sortno { get; set; }

        public int? DisplayFlag { get; set; }

        public string FullPid { get; set; }
    }
}