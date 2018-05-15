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
    public class FormInstController : Controller
    {
        private DataService service = null;
        string path = AppDomain.CurrentDomain.BaseDirectory + "\\temp.html";
        public FormInstController()
        {
            service = new DataService("FormInst","oa_forminst");
        }
        // GET: FormInst
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount()
        {
            return service.GetDataCount();
        }
        public string GetFormInstListByPage(int pageIndex,int pageSize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize));
        }
        [HttpDelete]
        public Boolean DeleteFormInstById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        public Object CreateFormInst(Model.FormInst forminst)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(forminst.Create());
        }
        public Boolean UpdateFormInst(Model.FormInst forminst)
        {
            forminst.Update();
            return true;
        }
        //html字节数组转html文件
        public Boolean formBytes2formFile(byte[] formByte)
        {
            try
            {
                FileStream fs = System.IO.File.Open(path, FileMode.Create);
                fs.Write(formByte, 0, formByte.Length);
                fs.Close();
            }
            catch (Exception e)
            {
                Util.Log.LogHelper.WriteLog(e.Message);
                return false;
            }
            return true;
        }
        //html文件转html字符串
        public string formFile2formHtml()
        {
            string formHtml = "";
            try
            {

                FileStream fs = System.IO.File.Open(path, FileMode.Open);
                StreamReader sd = new StreamReader(fs);
                formHtml = sd.ReadToEnd();
                sd.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                Util.Log.LogHelper.WriteLog(e.Message);
                formHtml = "";
            }
            return formHtml;
        }
        public string GetFormInstByProjectId(int projectId)
        {
            List<CommonAttribute> formInstList=service.GetDataList("*", "", "and projectid=" + projectId);
            if(formInstList.Count>0)
            {
                formBytes2formFile(((Model.FormInst)formInstList[0]).Content);
                return formFile2formHtml();
            }
            return "";
        }
    }
}