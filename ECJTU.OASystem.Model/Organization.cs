using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class Organization : CommonAttribute
    {
        public int ParentId { get; set; }
        public Object Create()
        {
            sql = "insert into oa_organization (id,name,parentid) values (sq_oa_organization.nextval,'{0}',{1}) ";
            sql = string.Format(sql, Name,ParentId);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_organization.currval from dual";
                Id = Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
                return Id;
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_organization where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_organization set name='{0}',ParentId={1} where id={2}";
            sql = string.Format(sql, Name, ParentId, Id);
            DBHelper.ExcuetSql(sql);
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            Name = dr["NAME"].ToString();
            ParentId = Convert.ToInt32(dr["PARENTID"].ToString());
        }
    }
}
