using ECJTU.OASystem.Business;
using ECJTU.OASystem.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECJTU.OASystem.App.Controllers
{
    public class MaterialTempController : Controller
    {
        private DataService service = null;
        public MaterialTempController()
        {
            service = new DataService("MaterialTemp","oa_materialTemp");
        }
        // GET: MaterialTemp
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount()
        {
            return service.GetDataCount();
        }
        public string GetMaterialTempListByPage(int pageIndex,int pageSize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize));
        }
        [HttpDelete]
        public Boolean DeleteMaterialTempById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        public Object CreateMaterialTemp(int businessId,Model.MaterialTemp materialTemp)
        {
            int materialTempId = (int)materialTemp.Create();
            Util.DB.DBHelper.ExcuetSql(string.Format("insert into OA_BUSINESS_MATERIALTEMP (ID,BUSINESSID,MATERIALTEMPID) values (SQ_OA_BUSINESS_MATERIALTEMP.nextval,{0},{1})", businessId, materialTempId));
            return materialTempId;
        }
        public Boolean UpdateMaterialTemp(int businessId,Model.MaterialTemp materialTemp)
        {
            materialTemp.Update();
            Util.DB.DBHelper.ExcuetSql(string.Format("update OA_BUSINESS_MATERIALTEMP bm set bm.BUSINESSID={0} where bm.MATERIALTEMPID={1}", businessId, materialTemp.Id));
            return true;
        }
        public CommonAttribute GetMaterialTempByBusinessId(int businessId)
        {
            List<CommonAttribute> materialTempList= service.GetDataList("t.*", "inner join OA_BUSINESS_MATERIALTEMP bm on bm.materialtempid=t.id","and bm.businessid="+businessId);
            if(materialTempList.Count>0)
            {
                return materialTempList[0];
            }
            return null;
        }
    }
}