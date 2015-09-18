using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTERPWeb.Common;
using FTERPWeb.Home.Models;
using FTERPWeb.Models;

namespace FTERPWeb.Areas.Home.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            IndexModel model = new IndexModel();
            return View(model);
        }

    }
}
