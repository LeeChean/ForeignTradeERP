using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTERPWeb.Models;

namespace FTERPWeb.Home.ViewModels
{
    [Serializable]
    public class RoleAccessModel : Record<RoleAccessModel>
    {
        public string Id { get; set; }

        public string Pid { get; set; }

        public string Title { get; set; }

        public string Checked { get; set; }
    }
}