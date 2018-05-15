using ECJTU.OASystem.Business;
using ECJTU.OASystem.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECJTU.OASystem.App.Controllers
{
    public class ProcessController : Controller
    {
        private DataService service = null;
        public ProcessController()
        {
            service = new DataService("Process","oa_process");
        }
        // GET: Process
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount()
        {
            return service.GetDataCount();
        }
        public CommonAttribute GetProcessByBusinessId(int businessId)
        {
            List<CommonAttribute> processList = service.GetDataList("*", "","and businessid=" + businessId);
            if (processList.Count > 0)
                return processList[0];
            return null;
        }
        public string GetProcessList()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList());
        }
        public string GetProcessListByPage(int pageIndex,int pageSize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize));
        }
        [HttpDelete]
        public Boolean DeleteProcessById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        public Object CreateProcess(Model.Process process)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(process.Create());
        }
        public Boolean UpdateProcess(Model.Process process)
        {
            process.Update();
            return true;
        }
    }
}