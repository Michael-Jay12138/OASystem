using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECJTU.OASystem.App.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult UserList()
        {

            return View();
        }
        public ActionResult RoleList()
        {

            return View();
        }
        public ActionResult BusinessList()
        {

            return View();
        }
        public ActionResult ProcessList()
        {

            return View();
        }
        public ActionResult MaterialList()
        {

            return View();
        }
        public ActionResult MaterialTempList()
        {

            return View();
        }
        public ActionResult ActivityList()
        {

            return View();
        }
        public ActionResult OrganizationList()
        {

            return View();
        }
        public ActionResult PrivilegeList()
        {

            return View();
        }
        public ActionResult FormTempList()
        {

            return View();
        }
        public ActionResult ProjectList()
        {

            return View();
        }
        
        public ActionResult WorkItemList(int type)
        {
            ViewBag.Type = type;
            return View();
        }
    }
}