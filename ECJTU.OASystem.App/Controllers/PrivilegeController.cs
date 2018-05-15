using ECJTU.OASystem.Business;
using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace ECJTU.OASystem.App.Controllers
{
    public class PrivilegeController : Controller
    {
        private DataService service = null;
        public PrivilegeController()
        {
            service = new DataService("Privilege","oa_privilege");
        }

        public string GetNavigators(int UserId)
        {
            string parentMenu = @"<li class='nav-item '>
                              <a href = 'javascript:;' class='nav-link nav-toggle'>
                                <i class='{1}'></i>
                                <span class='title'>{0}</span>
                                <span class='arrow'></span>
                            </a>
                            <ul class='sub-menu'>
                            {2}
                            </ul>
                            </li>";
            string subMenu = @"<li class='nav-item '>
                                      <a href = '{1}' class='nav-link '>
                                        <span class='title'>{0}</span>
                                    </a>
                                </li>";

            string menuHtml = "";
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Navigator.xml");
                XmlNodeList xmlList = xmlDocument.SelectNodes("Navigator/Items");
                string temp = "";
                string tempChildHtml = "";
                string tempChild = "";
                foreach (XmlElement node in xmlList)
                {
                    if (node.HasChildNodes)
                    {
                        tempChildHtml = "";
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            tempChild = string.Format(subMenu, child.Attributes["name"].Value, child.Attributes["path"].Value);
                            tempChildHtml += tempChild;
                            tempChild = "";
                        }
                    }
                    temp = string.Format(parentMenu, node.Attributes["name"].Value, node.Attributes["img"].Value, tempChildHtml);
                    menuHtml += temp;
                    temp = "";
                }
            }
            catch(Exception e)
            {
                LogHelper.WriteLog(e.Message);
                menuHtml = "";
            }
            
            return menuHtml;
        }
        // GET: Privilege
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount()
        {
            return service.GetDataCount();
        }
        public string GetPrivilegeListByPage(int pageIndex,int pageSize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize));
        }
        [HttpDelete]
        public Boolean DeletePrivilegeById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        public Object CreatePrivilege(Model.Privilege privilege)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(privilege.Create());
        }
        public Boolean UpdatePrivilege(Model.Privilege privilege)
        {
            privilege.Update();
            return true;
        }
    }
}