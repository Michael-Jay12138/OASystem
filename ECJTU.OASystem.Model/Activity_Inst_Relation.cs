using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class Activity_Inst_Relation: CommonAttribute
    {
        public int ActivityInstId { get; set; }
        public int PreActivityInstId { get; set; }
        override public Object Create()
        {
            sql = "insert into oa_activity_inst_relation (ACTIVITY_INST_ID,PRE_ACTIVITY_INST_ID) values (sq_oa_activity_inst_relation.nextval,{0},{1}) ";
            sql = string.Format(sql, ActivityInstId, PreActivityInstId);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_activity_inst_relation.currval from dual";
                Id = Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
                return Id;
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_activity_inst_relation where id="+Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            throw new NotImplementedException();
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            ActivityInstId = Convert.ToInt32(dr["ACTIVITY_INST_ID"].ToString());
            PreActivityInstId = Convert.ToInt32(dr["PRE_ACTIVITY_INST_ID"].ToString());
        }
    }
}
