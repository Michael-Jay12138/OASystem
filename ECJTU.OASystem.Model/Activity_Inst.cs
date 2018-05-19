using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class Activity_Inst : CommonAttribute
    {
        public int ProjectId { get; set; }
        public int ActivityId { get; set; }
        public int ProcessInstId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        override public Object Create()
        {
            sql = "insert into oa_Activity_Inst (id,ProjectId,ActivityId,Process_Inst_Id,Start_Time) values (sq_oa_Activity_Inst.nextval,{0},{1},{2},sysdate) ";
            sql = string.Format(sql, ProjectId, ActivityId, ProcessInstId);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_Activity_Inst.currval from dual";
                Id = Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
                return Id;
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_Activity_Inst where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_Activity_Inst set ProjectId={0},ActivityId={1},Process_Inst_Id={2},End_Time=sysdate where id={3}";
            sql = string.Format(sql, ProjectId, ActivityId, ProcessInstId,Id);
            DBHelper.ExcuetSql(sql);
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            ProjectId = Convert.ToInt32(dr["PROJECTID"].ToString());
            ActivityId = Convert.ToInt32(dr["ACTIVITYID"].ToString());
            ProcessInstId = Convert.ToInt32(dr["PROCESS_INST_ID"].ToString());
            try
            {
                StartTime = Convert.ToDateTime(dr["START_TIME"].ToString());
            }
            catch(Exception e)
            {
                Util.Log.LogHelper.WriteLog(e.ToString());
            }
            try
            {
                EndTime = Convert.ToDateTime(dr["END_TIME"].ToString());
            }
            catch (Exception e)
            {
                Util.Log.LogHelper.WriteLog(e.ToString());
            }
        }
    }
}
