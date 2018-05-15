using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class Project : CommonAttribute
    {
        public string ProjectNo { get; set; }
        public int CreateUserId { get; set; }
        public int CurrentUserId { get; set; }
        public int State { get; set; }
        public int BusinessId { get; set; }
        public int WorkItemId { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime EndTime { get; set; }

        override public Object Create()
        {
            sql = "insert into oa_Project (id,ProjectNo,name,CREATE_USER_ID,CURRENT_USER_ID,State,BusinessId,WorkItemId,Remark,CREATE_TIME) values (sq_oa_Project.nextval,'{0}','{1}',{2},{3},{4},{5},{6},'{7}',sysdate) ";
            sql = string.Format(sql, ProjectNo,Name, CreateUserId, CurrentUserId, State, BusinessId, WorkItemId, Remark );
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_Project.currval from dual";
                Id=Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
                sql = "select CREATE_TIME from oa_Project where id=" + Id;
                CreateTime = Convert.ToDateTime(DBHelper.Query(sql).Rows[0][0]);
            }

            Dictionary<string, string> backData = new Dictionary<string, string>();
            backData.Add("Id", Id.ToString());
            backData.Add("CreateTime", CreateTime.ToString());
            return backData;
        }

        override public void Delete()
        {
            sql = "delete from oa_Project where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_Project set NAME='{0}',PROJECTNO='{1}',CREATE_USER_ID={2},CURRENT_USER_ID={3},STATE={4},BUSINESSID={5},WORKITEMID={6},REMARK='{7}' where id={8}";
            sql = string.Format(sql, Name, ProjectNo, CreateUserId, CurrentUserId, State, BusinessId, WorkItemId, Remark,Id);
            DBHelper.ExcuetSql(sql);
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            Name = dr["NAME"].ToString();
            ProjectNo = dr["PROJECTNO"].ToString();
            CreateUserId = Convert.ToInt32(dr["CREATE_USER_ID"].ToString());
            CurrentUserId = Convert.ToInt32(dr["CURRENT_USER_ID"].ToString());
            State = Convert.ToInt32(dr["STATE"].ToString());
            BusinessId = Convert.ToInt32(dr["BUSINESSID"].ToString());
            WorkItemId = Convert.ToInt32(dr["WORKITEMID"].ToString());
            Remark = dr["REMARK"].ToString();
            try
            {
                CreateTime = Convert.ToDateTime(dr["CREATE_TIME"]);
                EndTime = Convert.ToDateTime(dr["END_TIME"]);
            }
            catch(Exception e)
            {
                Util.Log.LogHelper.WriteLog(e.Message);
            }
        }
    }
}
