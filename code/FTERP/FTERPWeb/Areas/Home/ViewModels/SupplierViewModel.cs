using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using FTERPWeb.Models;
using FTERPWeb.Common;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class SupplierViewModel
    {
        #region 字段

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
        public string Countryid { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "电话")]
        public string Phone { get; set; }

        [Display(Name = "邮箱")]
        [RegularExpression(@"^[0-9A-Za-z][A-Za-z0-9\._-]{0,}@[A-Za-z0-9-]{1,}[A-Za-z0-9]\.[A-Za-z\.]{1,}[A-Za-z]$", ErrorMessage = "{0}格式不正确")]
        public string Email { get; set; }

        [Display(Name = "联系人")]
        public string Linkman { get; set; }

        [Display(Name = "开户行")]
        public string OpeningBank { get; set; }

        [Display(Name = "开户行账号")]
        public string BankAccount { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "审批状态")]
        public string ApprovalStatus { get; set; }

        #endregion

        #region 构造函数

        public SupplierViewModel()
        {
        }

        public SupplierViewModel(SupplierModel model)
        {
            this.Id = model.Id;                                   //主键
            this.No = model.No;                                   //供应商编号
            this.Type = model.Type;                               //供应商类别

            this.ChName = model.ChName;                           //中文名称
            this.EnName = model.EnName;                           //英文名称
            this.ShortName = model.ShortName;                     //简称

            this.Countryid = model.Countryid.ToString();          //国家
            this.Address = model.Address;                         //地址
            this.Phone = model.Phone;                             //电话

            this.Email = model.Email;                             //邮箱
            this.Linkman = model.Linkman;                         //联系人
            this.Remark = model.Remark;                           //备注

            this.ApprovalStatus = model.ApprovalStatus;           //审批状态
            this.OpeningBank = model.OpeningBank;                 //开户行
            this.BankAccount = model.BankAccount;                 //开户行账号
        }

        #endregion
    }
}