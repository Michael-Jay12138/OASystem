using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class MaterialTemp : CommonAttribute
    {
        public int ParentId { get; set; }
        public int Sort { get; set; }
        override public Object Create()
        {
            sql = "insert into oa_MaterialTemp (id,name,ParentId,Sort) values (sq_oa_MaterialTemp.nextval,'{0}',{1},{2}) ";
            sql = string.Format(sql, Name, ParentId, Sort);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_MaterialTemp.currval from dual";
                return Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_MaterialTemp where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_MaterialTemp set name='{0}',ParentId={1},Sort={2} where id={3}";
            sql = string.Format(sql, Name, ParentId,Sort, Id);
            DBHelper.ExcuetSql(sql);
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            Name = dr["NAME"].ToString();
            ParentId = Convert.ToInt32(dr["PARENTID"].ToString());
            Sort = Convert.ToInt32(dr["SORT"].ToString());
        }
    }
}
