using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class WorkItem : CommonAttribute
    {
        public int ProjectId { get; set; }
        public int ActivityInstId { get; set; }
        public int State { get; set; }
        public int ReceiveUserId { get; set; }

        override public Object Create()
        {
            sql = "insert into oa_WorkItem (id,ProjectId,Activity_Inst_Id,State,Receive_User_Id) values (sq_oa_WorkItem.nextval,{0},{1},{2},{3}) ";
            sql = string.Format(sql, ProjectId, ActivityInstId, State, ReceiveUserId);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_WorkItem.currval from dual";
                Id = Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
                return Id;
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_WorkItem where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_WorkItem set ProjectId={0},ACTIVITY_INST_ID={1},State={2},RECEIVE_USER_ID={3} where id={4}";
            sql = string.Format(sql, ProjectId, ActivityInstId, State, ReceiveUserId,Id);
            DBHelper.ExcuetSql(sql);
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            ProjectId = Convert.ToInt32(dr["PROJECTID"].ToString());
            ActivityInstId = Convert.ToInt32(dr["ACTIVITY_INST_ID"].ToString());
            State = Convert.ToInt32(dr["STATE"].ToString());
            ReceiveUserId = Convert.ToInt32(dr["RECEIVE_USER_ID"].ToString());
            try
            {
                Name = dr["NAME"].ToString();
            }
            catch (Exception e)
            {
                Util.Log.LogHelper.WriteLog(e.Message);
            }
        }
    }
}
