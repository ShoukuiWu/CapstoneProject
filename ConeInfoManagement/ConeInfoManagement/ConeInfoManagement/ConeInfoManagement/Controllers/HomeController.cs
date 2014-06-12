using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConeInfoManagement.Models
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "You can jump start your Search in Conestoga College.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This wep-app is Conestoga Info.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "You should contact to Conestoga Info Management Team.";

            return View();
        }
    }
}
