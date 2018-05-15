using ECJTU.OASystem.Business;
using ECJTU.OASystem.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECJTU.OASystem.App.Controllers
{
    public class OrganizationController : Controller
    {
        private DataService service = null;
        public OrganizationController()
        {
            service = new DataService("Organization","oa_organization");
        }
        // GET: Organization
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount()
        {
            return service.GetDataCount();
        }
        public string GetOrganizationListByPage(int pageIndex,int pageSize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize));
        }
        [HttpDelete]
        public Boolean DeleteOrganizationById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        public Object CreateOrganization(Model.Organization organization)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(organization.Create());
        }
        public Boolean UpdateOrganization(Model.Organization organization)
        {
            organization.Update();
            return true;
        }
    }
}