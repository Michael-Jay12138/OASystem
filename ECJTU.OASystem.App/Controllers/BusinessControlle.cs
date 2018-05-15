using ECJTU.OASystem.Business;
using ECJTU.OASystem.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECJTU.OASystem.App.Controllers
{
    public class BusinessController : Controller
    {
        private DataService service = null;
        public BusinessController()
        {
            service = new DataService("Business","oa_business");
        }
        // GET: Business
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount()
        {
            return service.GetDataCount();
        }
        public string GetBusinessList()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList());
        }
        public string GetBusinessIdByFormTempId(int formTempId)
        {
            string result = "";
            System.Data.DataTable dt = Util.DB.DBHelper.Query("select bf.businessid from OA_BUSINESS_FORM bf where bf.formtempid=" + formTempId);
            if (dt.Rows.Count > 0)
                result = dt.Rows[0]["BUSINESSID"].ToString();
            return result;
        }
        public Model.Business GetBusinessById(int businessId)
        {
            return (Model.Business)service.GetDataList("t.*", "", "and t.id=" + businessId)[0];
        }
        public Boolean AddFormTemp(int businessId, int formTempId)
        {
            Model.Business business = GetBusinessById(businessId);
            business.AddFormTemp(formTempId);
            return true;
        }
        public Boolean UpdateFormTemp(int businessId, int formTempId)
        {
            Model.Business business = GetBusinessById(businessId);
            business.UpdateFormTemp(formTempId);
            return true;
        }
        public string GetBusinessListByPage(int pageIndex,int pageSize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize));
        }
        [HttpDelete]
        public Boolean DeleteBusinessById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        public Object CreateBusiness(Model.Business business)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(business.Create());
        }
        public Boolean UpdateBusiness(Model.Business business)
        {
            business.Update();
            return true;
        }
    }
}