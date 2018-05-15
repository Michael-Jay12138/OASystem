using ECJTU.OASystem.Business;
using ECJTU.OASystem.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECJTU.OASystem.App.Controllers
{
    public class ActivityController : Controller
    {
        private DataService service = null;
        public ActivityController()
        {
            service = new DataService("Activity","oa_activity");
        }
        // GET: Activity
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount()
        {
            return service.GetDataCount();
        }
        public CommonAttribute GetActivityByProcessId(int processId)
        {
            List<CommonAttribute> activityList = service.GetDataList("*","","and processid=" + processId);
            if (activityList.Count > 0)
                return activityList[0];
            return null;
        }
        public string GetActivityListByPage(int pageIndex,int pageSize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize));
        }
        [HttpDelete]
        public Boolean DeleteActivityById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        public Object CreateActivity(Model.Activity activity)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(activity.Create());
        }
        public Boolean UpdateActivity(Model.Activity activity)
        {
            activity.Update();
            return true;
        }
        public Model.Activity GetActivityById(int activityId)
        {
            return (Model.Activity)service.GetDataList("t.*", "", "and t.id=" + activityId)[0];
        }
        public Boolean AddRoles(int activityId, int[] roleIds)
        {
            Model.Activity activity = GetActivityById(activityId);
            foreach (int roleId in roleIds)
            {
                activity.AddRole(roleId);
            }
            return true;
        }
        public Boolean UpdateRoles(int activityId, int[] roleIds)
        {
            Model.Activity activity = GetActivityById(activityId);
            activity.UpdateRole(roleIds);
            return true;
        }
        //根据环节实例id获取下一环节信息
        public string GetNextActivity(int activityInstId)
        {
            Model.Activity activity=(Model.Activity)service.GetDataList("t.*", @"inner join OA_ACTIVITY_INST ai on ai.activityid=t.id", "and ai.id="+ activityInstId)[0];
            List<CommonAttribute> listActivity=service.GetDataList("t.*", "", "and t.processid="+ activity.ProcessId+" order by t.sort");
            Model.Activity nextActivity = new Model.Activity();
            for (int i = 0; i < listActivity.Count; i++)
            {
                if (listActivity[i].Id == activity.Id)
                {
                    if(i<(listActivity.Count-1))
                        nextActivity = (Model.Activity)listActivity[i + 1];
                    else
                    {
                        //下一环节为结束
                        nextActivity.Id = 0;
                        nextActivity.Name = "结束环节";
                        nextActivity.ProcessId = activity.ProcessId;
                        nextActivity.Sort = 9999;
                    }
                    break;
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(nextActivity);
        }
    }
}