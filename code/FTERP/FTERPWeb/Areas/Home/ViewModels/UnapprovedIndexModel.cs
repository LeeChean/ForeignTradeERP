using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTERPWeb.Models;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class UnapprovedIndexModel : Record<UnapprovedIndexModel>
    {
        public string ApprovalDocumentId { get; set; }

        public string DocId { get; set; }

        public string TableName { get; set; }

        public string Title { get; set; }

        public string DocType { get; set; }

        public string RoleId { get; set; }

        public string CreateMan { get; set; }

        public string CreateTime { get; set; }

        public string Url { get; set; }
    }
}