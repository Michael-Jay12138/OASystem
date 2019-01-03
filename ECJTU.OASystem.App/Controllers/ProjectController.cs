using ECJTU.OASystem.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECJTU.OASystem.App.Controllers
{
    public class ProjectController : Controller
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "\\temp.html";
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }
        private DataService service = null;
        public ProjectController()
        {
            service = new DataService("Project", "oa_project");
        }
        public int GetDataCount()
        {
            return service.GetDataCount();
        }
        public string GetProjectListByPage(int pageIndex, int pageSize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(service.GetDataList(--pageIndex, pageSize));
        }
        public Object CreateProject(Model.Project project,string userName,int NextUserId)
        {
            //获取创建人
            Model.User user = new UserController().GetUserByName(userName);
            project.CreateUserId = user.Id;
            project.CurrentUserId = NextUserId;
            //新建项目
            Dictionary<string, string> backData = (Dictionary<string, string>)project.Create();
            int projectId= Convert.ToInt32(backData["Id"]);
            try
            {
                //新建表单实例
                Model.FormInst formInst = new Model.FormInst();
                formInst.ProjectId = projectId;
                FormTempController formTempController = new FormTempController();
                Model.FormTemp formTemp = (Model.FormTemp)formTempController.GetFormTempByBusinessId(project.BusinessId);
                formInst.Name = formTemp.Name;
                formInst.FormTempId = formTemp.Id;
                formInst.Content = System.IO.File.ReadAllBytes(path);
                formInst.Create();
                //新建流程实例
                ProcessController processController = new ProcessController();
                Model.Process process = (Model.Process)processController.GetProcessByBusinessId(project.BusinessId);
                Model.Process_Inst process_Inst = new Model.Process_Inst();
                process_Inst.ProcessId = process.Id;
                process_Inst.ProjectId = projectId;
                process_Inst.CreateUserId = user.Id;
                int processInstId = (int)process_Inst.Create();
                //新建环节实例
                ActivityController activityController = new ActivityController();
                Model.Activity activity = (Model.Activity)activityController.GetActivityByProcessId(process.Id);
                Model.Activity_Inst activity_Inst = new Model.Activity_Inst();
                activity_Inst.ActivityId = activity.Id;
                activity_Inst.ProcessInstId = processInstId;
                activity_Inst.ProjectId = project.Id;
                int activityInstId = (int)activity_Inst.Create();
                //新建工作项
                Model.WorkItem workItem = new Model.WorkItem();
                workItem.ProjectId = projectId;
                workItem.State = 0;
                workItem.ActivityInstId = activityInstId;
                workItem.ReceiveUserId = NextUserId;
                int workItemId=(int)workItem.Create();
                project.WorkItemId = workItemId;
                project.Update();
                //新建材料清单模板实例
                MaterialTempController materialTempController = new MaterialTempController();
                Model.MaterialTemp materialTemp=(Model.MaterialTemp)materialTempController.GetMaterialTempByBusinessId(project.BusinessId);
                Model.MaterialTemp_Inst materialTemp_Inst = new Model.MaterialTemp_Inst();
                materialTemp_Inst.MaterialTempId = materialTemp.Id;
                materialTemp_Inst.Name = materialTemp.Name;
                materialTemp_Inst.ProjectId= projectId;
                materialTemp_Inst.Create();
            }
            catch(Exception e)
            {
                Util.Log.LogHelper.WriteLog(e.Message);
            }
            
            //返回项目数据
            return Newtonsoft.Json.JsonConvert.SerializeObject(backData);
        }
        [HttpDelete]
        public Boolean DeleteProjectById(int Id)
        {
            return service.DeleteDataById(Id);
        }
        public Boolean UpdateProject(Model.Project project)
        {
            project.Update();
            return true;
        }
    }
}