using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class Role: CommonAttribute
    {

        override public Object Create()
        {
            sql = "insert into oa_role (id,name) values (sq_oa_role.nextval,'{0}') ";
            sql = string.Format(sql, Name);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_role.currval from dual";
                return Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_role where id="+Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_role set name='{0}' where id={1}";
            sql = string.Format(sql, Name, Id);
            DBHelper.ExcuetSql(sql);
        }
        public void AddUser(int UserId)
        {
            sql = "insert into oa_user_role (id,userid,roleid) values (sq_oa_user_role.nextval,{0},{1})";
            sql = string.Format(sql, UserId, Id);
            DBHelper.ExcuetSql(sql);
        }
        public void UpdateUser(int[] userIds)
        {
            sql = "delete from oa_user_role ur where ur.roleid=" + Id;
            DBHelper.ExcuetSql(sql);
            foreach (int userId in userIds)
            {
                AddUser(userId);
            }
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            Name = dr["NAME"].ToString();
        }
    }
}
