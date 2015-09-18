using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTERPWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class CommodityIndexModel : Record<CommodityIndexModel>
    {
        [Display(Name = "主键")]
        public string Id { get; set; }

        [Display(Name = "商品编号")]
        public string No { get; set; }

        [Display(Name = "商品类别")]
        public string Type { get; set; }

        [Display(Name = "中文名称")]
        public string ChName { get; set; }

        [Display(Name = "英文名称")]
        public string EnName { get; set; }

        [Display(Name = "商品货号")]
        public string ProductCode { get; set; }

        [Display(Name = "海关编码")]
        public string CustomsNo { get; set; }

        [Display(Name = "关税率")]
        public string TariffRate { get; set; }

        [Display(Name = "增值税率")]
        public string VatRate { get; set; }

        [Display(Name = "退税率")]
        public string RefundRate { get; set; }

        [Display(Name = "是否有效")]
        public string DelFlag { get; set; }

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