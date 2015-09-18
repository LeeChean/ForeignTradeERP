using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTERPWeb.Models;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class ApprovalProgressViewModel : Record<ApprovalProgressViewModel>
    {
        public string RoleName { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }

        public string Time { get; set; }
    }
}