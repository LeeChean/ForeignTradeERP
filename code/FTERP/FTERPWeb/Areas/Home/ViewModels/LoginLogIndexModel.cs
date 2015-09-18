using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTERPWeb.Models;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class LoginLogIndexModel : Record<LoginLogIndexModel>
    {
        public string Code { get; set; }

        public string UserName { get; set; }

        public string Department { get; set; }

        public string LoginTime { get; set; }

        public string LoginIP { get; set; }
    }
}