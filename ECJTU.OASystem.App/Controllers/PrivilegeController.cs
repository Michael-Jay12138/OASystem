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

        public string GetNavigators(string userName)
        {
            Model.User user=new UserController().GetUserByName(userName);
            List<CommonAttribute> privilegeList = service.GetDataList("t.*", @"inner join OA_ROLE_PRIVILEGE rp on t.ID=rp.PRIVILEGE
                                                                                inner join OA_USER_ROLE ur on rp.ROLEID=ur.ROLEID", "and ur.USERID="+ user.Id);

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
                    string parentName = node.Attributes["name"].Value;
                    if (!privilegeList.Any(p=>p.Name==parentName))
                    {
                        continue;
                    }
                    if (node.HasChildNodes)
                    {
                        tempChildHtml = "";
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            string childName= child.Attributes["name"].Value;
                            if (!privilegeList.Any(p => p.Name == childName))
                            {
                                continue;
                            }
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
        public Model.Privilege GetPrivilegeById(int privilegeId)
        {
            return (Model.Privilege)service.GetDataList("t.*", "", "and t.id=" + privilegeId)[0];
        }
        public Boolean AddRoles(int privilegeId, int[] roleIds)
        {
            Model.Privilege privilege = GetPrivilegeById(privilegeId);
            foreach (int roleId in roleIds)
            {
                privilege.AddRole(roleId);
            }
            return true;
        }
        public Boolean UpdateRoles(int privilegeId, int[] roleIds)
        {
            Model.Privilege privilege = GetPrivilegeById(privilegeId);
            privilege.UpdateRole(roleIds);
            return true;
        }
    }
}