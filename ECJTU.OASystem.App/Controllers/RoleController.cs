using ECJTU.OASystem.Business;
using ECJTU.OASystem.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECJTU.OASystem.App.Controllers
{
    public class RoleController : Controller
    {
        private DataService service = null;
        public RoleController()
        {
            service = new DataService("Role","oa_role");
        }
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount()
        {
            return service.GetDataCount();
        }
        public string GetRoleListByPage(int pageIndex,int pageSize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize));
        }
        [HttpDelete]
        public Boolean DeleteRoleById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        public Object CreateRole(Model.Role role)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(role.Create());
        }
        public Boolean UpdateRole(Model.Role role)
        {
            role.Update();
            return true;
        }
        public Model.Role GetRoleById(int roleId)
        {
            return (Model.Role)service.GetDataList("t.*", "", "and t.id=" + roleId)[0];
        }
        public Boolean AddUsers(int roleId,int[] userIds)
        {
            Model.Role role = GetRoleById(roleId);
            foreach(int userId in userIds)
            {
                role.AddUser(userId);
            }
            return true;
        }
        public Boolean UpdateUsers(int roleId,int[] userIds)
        {
            Model.Role role = GetRoleById(roleId);
            role.UpdateUser(userIds);
            return true;
        }
        public string GetRoleTree(int activityId)
        {
            List<CommonAttribute> selectedRoleList = null;
            if (activityId > 0)
            {
                selectedRoleList = service.GetDataList("t.*", "inner join OA_ACTIVITY_ROLE ar on ar.roleid=t.id", "and ar.activityid=" + activityId);
            }
            List<CommonAttribute> roleList = service.GetDataList("t.*", "", "");
            Models.Tree root = new Models.Tree();
            root.id = "0";
            root.text = "所有角色";
            root.state = new Models.State();
            root.state.opened = true;
            List<Models.Tree> children = new List<Models.Tree>();
            foreach (CommonAttribute role in roleList)
            {
                Models.Tree child = new Models.Tree();
                child.id = role.Id.ToString();
                child.text = role.Name.ToString();
                if (selectedRoleList != null && selectedRoleList.Exists(r => r.Id == role.Id))
                {
                    child.state = new Models.State();
                    child.state.selected = true;
                }
                children.Add(child);
            }
            root.children = children;
            return Newtonsoft.Json.JsonConvert.SerializeObject(root);
        }
    }
}