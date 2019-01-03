using ECJTU.OASystem.Business;
using ECJTU.OASystem.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECJTU.OASystem.App.Controllers
{
    public class UserController : Controller
    {
        private DataService service = null;
        public UserController()
        {
            service = new DataService("User","oa_user");
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount()
        {
            return service.GetDataCount();
        }
        public string GetUserListByPage(int pageIndex,int pageSize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize));
        }
        [HttpDelete]
        public Boolean DeleteUserById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        public Object CreateUser(Model.User user)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(user.Create());
        }
        public Boolean UpdateUser(Model.User user)
        {
            user.Update();
            return true;
        }
        public Model.User GetUserByName(string userName)
        {
            List<CommonAttribute> userList = service.GetDataList("*", "", "and name='" + userName+"'");
            if (userList.Count > 0)
                return (Model.User)userList[0];
            return null;
        }
        public ActionResult UserLogin(string username,string password)
        {
            Model.User user=GetUserByName(username);
            if (user.UserPwd == password)
                return RedirectToRoute(new { controller = "Home", action = "WorkItemList",type=0 });
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        //根据环节id获取用户列表
        public string GetNextActivityUsers(int activityId)
        {
            List<CommonAttribute> userList=service.GetDataList("t.*", 
                                @"inner join OA_USER_ROLE ur on t.id=ur.userid
                                    inner join OA_ACTIVITY_ROLE ar on ar.roleid=ur.roleid", "and ar.activityid=" + activityId);
            return Newtonsoft.Json.JsonConvert.SerializeObject(userList);
        }
        public string GetFAUsersByBusinessId(int businessId)
        {
            List<CommonAttribute> activityList = new DataService("Activity","OA_ACTIVITY").GetDataList("t.*",
                                @"inner join OA_PROCESS p on p.id=t.processid", "and p.businessid="+businessId+" order by t.sort");
            if (activityList.Count > 0)
            {
                return GetNextActivityUsers(activityList[0].Id);
            }
            return "";
        }
        public string GetUserTree(int roleId)
        {
            List<CommonAttribute> selectedUserList=null;
            if (roleId > 0)
            {
                selectedUserList = service.GetDataList("t.*", "inner join oa_user_role ur on ur.userid=t.id", "and ur.roleid="+roleId);
            }
            List<CommonAttribute> userList=service.GetDataList("t.*","","");
            Models.Tree root = new Models.Tree();
            root.id = "0";
            root.text = "所有用户";
            root.state = new Models.State();
            root.state.opened = true;
            List<Models.Tree> children = new List<Models.Tree>();
            foreach(CommonAttribute user in userList)
            {
                Models.Tree child = new Models.Tree();
                child.id = user.Id.ToString();
                child.text = user.Name.ToString();
                if (selectedUserList!=null&&selectedUserList.Exists(u => u.Id == user.Id))
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