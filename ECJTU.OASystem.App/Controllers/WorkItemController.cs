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
            Util.Ftp.FtpFile ftpFile = new Util.Ftp.FtpFile();
            ftpFile.Title = "测试文件.txt";
            ftpFile.FilePath= "/OASystem/测试文件.txt";
            Util.Ftp.FtpHelper.DownLoad(ftpFile, @"E:\测试文件.txt");
            return View();
        }
        public int GetDataCount(int type, string userName)
        {
            return service.GetDataCount("inner join oa_user u on u.id=t.receive_user_id", "and t.state=" + type + " and u.name=" + userName);
        }
        public string GetWorkItemListByPage(int pageIndex,int pageSize,int type,string userName)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize, "inner join oa_user u on u.id=t.receive_user_id", "and t.state=" + type + " and u.name=" + userName));
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