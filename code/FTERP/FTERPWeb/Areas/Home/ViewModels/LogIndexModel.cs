using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTERPWeb.Models;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class LogIndexModel : Record<LogIndexModel>
    {
        public string Code { get; set; }

        public string UserName { get; set; }

        public string Remark { get; set; }

        public string CreateTime { get; set; }
    }
}