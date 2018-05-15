using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class Process : CommonAttribute
    {
        public string Remark { get; set; }
        public int BusinessId { get; set; }
        public int State { get; set; }

        override public Object Create()
        {
            sql = "insert into oa_Process (id,name,remark,BusinessId,State) values (sq_oa_Process.nextval,'{0}','{1}',{2},{3}) ";
            sql = string.Format(sql, Name, Remark, BusinessId, State);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_Process.currval from dual";
                return Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_Process where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_Process set name='{0}',BusinessId={1},Remark='{2}',State={3} where id={4}";
            sql = string.Format(sql, Name, BusinessId, Remark, State, Id);
            DBHelper.ExcuetSql(sql);
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            Name = dr["NAME"].ToString();
            Remark = dr["REMARK"].ToString();
            BusinessId = Convert.ToInt32(dr["BUSINESSID"].ToString());
            State = Convert.ToInt32(dr["STATE"].ToString());
        }
    }
}
