using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class Process_Inst : CommonAttribute
    {
        public int ProjectId { get; set; }
        public int ProcessId { get; set; }
        public int CreateUserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        override public Object Create()
        {
            sql = "insert into oa_Process_Inst (id,PROJECTID,PROCESSID,CREATE_USER_ID,START_TIME) values (sq_oa_Process_Inst.nextval,{0},{1},{2},sysdate)";
            sql = string.Format(sql, ProjectId, ProcessId, CreateUserId);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_Process_Inst.currval from dual";
                return Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_Process_Inst where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_Process_Inst set PROJECTID={0},PROCESSID={1},CREATE_USER_ID={2} where id={3}";
            sql = string.Format(sql, ProjectId, ProcessId, CreateUserId, Id);
            DBHelper.ExcuetSql(sql);
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            ProjectId = Convert.ToInt32(dr["PROJECTID"].ToString());
            ProcessId = Convert.ToInt32(dr["PROCESSID"].ToString());
            CreateUserId = Convert.ToInt32(dr["CREATE_USER_ID"].ToString());
            try
            {
                StartTime = Convert.ToDateTime(dr["START_TIME"]);
            }
            catch(Exception e)
            {
                Util.Log.LogHelper.WriteLog(e.Message);
            }
        }
    }
}
