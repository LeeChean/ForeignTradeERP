using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FTERPWeb.Common;
using FTERPWeb.Models;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class IndexModel
    {
        /// <summary>
        /// 单据类别列表
        /// </summary>
        public List<DocumentTypeModel> DocTypeList { get; set; }

        /// <summary>
        /// 当前登录用户的待审批列表
        /// </summary>
        public List<UnapprovedIndexModel> UnapprovedList { get; set; }
    }
}