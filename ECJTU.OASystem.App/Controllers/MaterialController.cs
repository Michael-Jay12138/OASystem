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
        public MaterialController()
        {
            service = new DataService("Material","oa_material");
        }
        // GET: Material
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount()
        {
            return service.GetDataCount();
        }
        public string GetMaterialListByPage(int pageIndex,int pageSize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize));
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
        public Boolean AddMaterial(int materialTempInstId,string materialLocalPath)
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
            Util.Ftp.FtpHelper.UpLoad(ftpFile, materialLocalPath);
            return true;
        }
    }
}