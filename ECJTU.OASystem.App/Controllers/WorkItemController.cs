using ECJTU.OASystem.Business;
using ECJTU.OASystem.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECJTU.OASystem.App.Controllers
{
    public class WorkItemController : Controller
    {
        private DataService service = null;
        public WorkItemController()
        {
            service = new DataService("WorkItem","oa_workitem");
        }
        // GET: WorkItem
        public ActionResult Index()
        {
            return View();
        }
        public int GetDataCount(int type, string userName)
        {
            return service.GetDataCount("inner join oa_user u on u.id=t.receive_user_id", "and t.state=" + type + " and u.name=" + userName);
        }
        public string GetWorkItemListByPage(int pageIndex,int pageSize,int type,string userName)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize, "inner join oa_user u on u.id=t.receive_user_id inner join oa_project p on p.id=t.projectid", "and t.state=" + type + " and u.name=" + userName,"t.*,p.NAME"));
        }
        public string GetProcessingListInfo(string userName)
        {
            List<CommonAttribute> processingList=service.GetDataList("t.*,p.NAME", "inner join oa_user u on u.id=t.receive_user_id inner join oa_project p on p.id=t.projectid", "and t.state=0 and u.name=" + userName);
            if (processingList.Count > 0)
            {
                List<string> content = new List<string>();
                foreach (CommonAttribute workitem in processingList)
                {
                    string temp = @"<li>
                                        <a href='javascript:;'>
                                             <span class='time'>{0}</span>
                                            <span class='details'>
                                                <span class='label label-sm label-icon label-success'>
                                                    <i class='fa fa-plus'></i>
                                                </span> {1}
                                            </span>
                                        </a>
                                    </li>";
                    content.Add(string.Format(temp, workitem.Id, workitem.Name));
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(content);
            }
            return "";
        }
        [HttpDelete]
        public Boolean DeleteWorkItemById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        public Object CreateWorkItem(Model.WorkItem workitem)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(workitem.Create());
        }
        public Boolean UpdateWorkItem(Model.WorkItem workitem)
        {
            workitem.Update();
            return true;
        }
        public Boolean SendWorkItem(int workItemId,int receiveUserId,int preActivityInstId,int nextActivityId)
        {
            //老工作项更新
            Model.WorkItem preWorkItem = (Model.WorkItem)service.GetDataList("t.*", "", "and t.id=" + workItemId)[0];
            preWorkItem.State = 1;
            preWorkItem.Update();
            //老环节实例更新
            Model.Activity_Inst preActivityInst=(Model.Activity_Inst)new DataService("Activity_Inst", "OA_Activity_Inst").GetDataList("t.*", "", "and t.id=" + preActivityInstId)[0];
            preActivityInst.EndTime = DateTime.Now;
            preActivityInst.Update();
            //表单实例更新
            Model.FormInst formInst = (Model.FormInst)new DataService("FormInst", "OA_FORMINST").GetDataList("t.*", "", "and t.projectid=" + preWorkItem.ProjectId)[0];
            formInst.Content= System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "\\temp.html");
            formInst.Update();
            //获取项目
            Model.Project project = (Model.Project)new DataService("Project", "OA_PROJECT").GetDataList("t.*", "", "and t.id=" + preWorkItem.ProjectId)[0];
            if (nextActivityId==0)
            {
                //项目标记结束
                project.State = 1;
                project.EndTime = DateTime.Now;
                project.Update();
            }
            else
            {
                //新建环节实例
                Model.Activity_Inst nextActivityInst = new Model.Activity_Inst();
                nextActivityInst.ActivityId = nextActivityId;
                nextActivityInst.ProcessInstId = preActivityInst.ProcessInstId;
                nextActivityInst.ProjectId = preActivityInst.ProjectId;
                int nextActivityInstId = (int)nextActivityInst.Create();
                //新建工作项
                Model.WorkItem nextWorkItem = new Model.WorkItem();
                nextWorkItem.ProjectId = preWorkItem.ProjectId;
                nextWorkItem.ReceiveUserId = receiveUserId;
                nextWorkItem.State = 0;
                nextWorkItem.ActivityInstId = nextActivityInstId;
                int nextWorkItemId=(int)nextWorkItem.Create();
                //新建环节实例关系
                Model.Activity_Inst_Relation activity_Inst_Relation = new Model.Activity_Inst_Relation();
                activity_Inst_Relation.ActivityInstId = nextActivityInstId;
                activity_Inst_Relation.PreActivityInstId = preActivityInst.Id;
                activity_Inst_Relation.Create();
                //更新项目信息
                project.CurrentUserId = receiveUserId;
                project.WorkItemId = nextWorkItemId;
                project.Update();
            }
            
            return true;
        }
    }
}