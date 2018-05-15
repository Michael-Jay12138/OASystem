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
    public class FormTempController : Controller
    {
        private DataService service = null;
        string path = AppDomain.CurrentDomain.BaseDirectory + "\\temp.html";
        public FormTempController()
        {
            service = new DataService("FormTemp","oa_formTemp");
        }
        // GET: FormTemp
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount()
        {
            return service.GetDataCount();
        }
        public string GetFormTempListByPage(int pageIndex,int pageSize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize));
        }
        [HttpDelete]
        public Boolean DeleteFormTempById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        //html字符串转html文件
        [ValidateInput(false)]
        public Boolean formHtml2htmlFile(string formHtml)
        {
            try
            {
                FileStream fs = System.IO.File.Open(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //sw.WriteLine("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n<meta charset=\"utf-8\" />\r\n</head>\r\n<body>\r\n");
                sw.WriteLine(formHtml);
                //sw.WriteLine("</body>\r\n</html>");
                sw.Close();
                fs.Close();
            }
            catch(Exception e)
            {
                Util.Log.LogHelper.WriteLog(e.Message);
                return false;
            }
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
        public string GetFormFile()
        {
            string formHtml = "";
            try
            {
                string[] str = System.IO.File.ReadAllLines(path, System.Text.Encoding.UTF8);
                for (int i = 5; i < str.Length - 2; i++)
                {
                    formHtml += str[i];
                }
                //FileStream fs = System.IO.File.Open(path, FileMode.Open);
                //StreamReader sd = new StreamReader(fs);
                //formHtml = sd.ReadToEnd();
                //sd.Close();
                //fs.Close();
            }
            catch (Exception e)
            {
                Util.Log.LogHelper.WriteLog(e.Message);
                formHtml = "";
            }
            return formHtml;
        }
        //根据业务id获取表单模板
        public string GetFormByBusinessId(int businessId)
        {
            List<CommonAttribute> formTempList=service.GetDataList("*", "inner join OA_BUSINESS_FORM bf on t.id=bf.formtempid", "and bf.businessid=" + businessId);
            formBytes2formFile(((Model.FormTemp)formTempList[0]).Layout);
            return formFile2formHtml();
        }
        public CommonAttribute GetFormTempByBusinessId(int businessId)
        {
            List<CommonAttribute> formTempList = service.GetDataList("*", "inner join OA_BUSINESS_FORM bf on t.id=bf.formtempid", "and bf.businessid=" + businessId);
            if (formTempList.Count > 0)
                return formTempList[0];
            return null;
        }
        public Object CreateFormTemp(Model.FormTemp formTemp)
        {
            formTemp.Layout=System.IO.File.ReadAllBytes(path);
            return Newtonsoft.Json.JsonConvert.SerializeObject(formTemp.Create());
        }
        public Boolean UpdateFormTemp(Model.FormTemp formTemp)
        {
            formTemp.Layout = System.IO.File.ReadAllBytes(path);
            formTemp.Update();
            return true;
        }
    }
}