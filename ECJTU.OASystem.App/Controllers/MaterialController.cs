using ECJTU.OASystem.Business;
using ECJTU.OASystem.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECJTU.OASystem.App.Controllers
{
    public class MaterialController : Controller
    {
        private DataService service = null;
        private string tempFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");
        public MaterialController()
        {
            service = new DataService("Material","oa_material");
        }
        // GET: Material
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount(int workItemId)
        {
            string joinStr = "";
            string whereStr = "";
            if (workItemId > 0)
            {
                joinStr = "inner join OA_MATERIALTEMP_INST mi on t.MATERIALTEMP_INST_ID=mi.ID inner join OA_WORKITEM w on mi.PROJECTID=w.PROJECTID";
                whereStr = "and w.ID=" + workItemId;
            }
            return service.GetDataCount(joinStr, whereStr);
        }
        public string GetMaterialListByPage(int pageIndex,int pageSize,int workItemId=0)
        {
            string joinStr = "";
            string whereStr = "";
            if (workItemId > 0)
            {
                joinStr = "inner join OA_MATERIALTEMP_INST mi on t.MATERIALTEMP_INST_ID=mi.ID inner join OA_WORKITEM w on mi.PROJECTID=w.PROJECTID";
                whereStr = "and w.ID="+workItemId;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize,joinStr,whereStr));
        }
        public int GetMaterialTempInstIdByProjectId(int projectId)
        {
            System.Data.DataTable dt =Util.DB.DBHelper.Query("select t.id from OA_MATERIALTEMP_INST t where t.PROJECTID=" + projectId);
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["ID"]);
            }
            return 0;
        }
        [HttpDelete]
        public Boolean DeleteMaterialById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        public Object CreateMaterial(Model.Material material)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(material.Create());
        }
        public Boolean UpdateMaterial(Model.Material material)
        {
            material.Update();
            return true;
        }
        public string GetMaterial(Model.Material material)
        {
            Util.Ftp.FtpFile ftpFile = new Util.Ftp.FtpFile();
            ftpFile.Title = material.Name;
            ftpFile.FilePath = material.Path;
            string tempFilePath = Path.Combine(tempFileDir, material.Name);
            Util.Ftp.FtpHelper.DownLoad(ftpFile, tempFilePath);
            return ftpFile.Title;
        }
        public Object AddMaterial(int materialTempInstId,string materialLocalPath)
        {
            FileInfo fileInfo = new FileInfo(materialLocalPath);
            Model.Material material = new Model.Material();
            material.MaterialTempInstId = materialTempInstId;
            material.Name = fileInfo.Name;
            material.Extension = fileInfo.Extension;
            int materialId=(int)material.Create();
            material.Path = "/OASystem/" + materialId + material.Extension;
            material.Update();
            Util.Ftp.FtpFile ftpFile = new Util.Ftp.FtpFile();
            ftpFile.FilePath = material.Path;
            ftpFile.Title = material.Name;
            Util.Log.LogHelper.WriteLog( Util.Ftp.FtpHelper.UpLoad(ftpFile, materialLocalPath));
            Dictionary<string, string> backData = new Dictionary<string, string>();
            backData.Add("Id", materialId.ToString());
            backData.Add("Path", material.Path);
            return Newtonsoft.Json.JsonConvert.SerializeObject(backData);
        }
    }
}