using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTERPWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class CommodityViewModel
    {
        #region 字段

        public string Id { get; set; }

        [Display(Name = "商品编号")]
        public string No { get; set; }

        [Display(Name = "中文名称")]
        public string ChName { get; set; }

        [Display(Name = "英文名称")]
        public string EnName { get; set; }

        [Display(Name = "商品货号")]
        public string ProductCode { get; set; }

        [Display(Name = "商品类别")]
        public string Type { get; set; }

        [Display(Name = "商品单位")]
        public string Unit { get; set; }

        [Display(Name = "关税率")]
        public string TariffRate { get; set; }

        [Display(Name = "增值税率")]
        public string VatRate { get; set; }

        [Display(Name = "退税率")]
        public string RefundRate { get; set; }

        [Display(Name = "海关编码")]
        public string CustomsNo { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }


        #endregion

        #region 构造函数

        public CommodityViewModel()
        {
        }

        public CommodityViewModel(CommodityModel model)
        {
            this.Id = model.Id;
            this.No = model.No;
            this.ChName = model.ChName;

            this.EnName = model.EnName;
            this.ProductCode = model.ProductCode;
            this.Type = model.Type;

            this.Unit = model.Unit;
            this.TariffRate = model.TariffRate;
            this.VatRate = model.VatRate;

            this.RefundRate = model.RefundRate;
            this.CustomsNo = model.CustomsNo;
            this.Remark = model.Remark;
        }

        #endregion
    }
}