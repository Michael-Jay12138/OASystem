using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class Activity : CommonAttribute
    {
        public int ProcessId { get; set; }
        public int Sort { get; set; }
        override public Object Create()
        {
            sql = "insert into oa_Activity (id,name,ProcessId,Sort) values (sq_oa_Activity.nextval,'{0}',{1},{2}) ";
            sql = string.Format(sql, Name, ProcessId, Sort);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_Activity.currval from dual";
                Id = Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
                return Id;
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_Activity where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_Activity set name='{0}',ProcessId={1},Sort={2} where id={3}";
            sql = string.Format(sql, Name, ProcessId, Sort, Id);
            DBHelper.ExcuetSql(sql);
        }
        public void AddRole(int RoleId)
        {
            sql = "insert into OA_ACTIVITY_ROLE (id,roleid,activityid) values (sq_OA_ACTIVITY_ROLE.nextval,{0},{1})";
            sql = string.Format(sql, RoleId, Id);
            DBHelper.ExcuetSql(sql);
        }
        public void UpdateRole(int[] roleIds)
        {
            sql = "delete from OA_ACTIVITY_ROLE ar where ar.activityid=" + Id;
            DBHelper.ExcuetSql(sql);
            foreach (int roleId in roleIds)
            {
                AddRole(roleId);
            }
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            Name = dr["NAME"].ToString();
            ProcessId = Convert.ToInt32(dr["PROCESSID"].ToString());
            Sort = Convert.ToInt32(dr["SORT"].ToString());
        }
    }
}
