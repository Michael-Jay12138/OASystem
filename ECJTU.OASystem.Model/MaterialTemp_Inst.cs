using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class MaterialTemp_Inst : CommonAttribute
    {
        public int ProjectId { get; set; }
        public int ParentId { get; set; }
        public int Sort { get; set; }
        public int MaterialTempId { get; set; }
        override public Object Create()
        {
            sql = "insert into oa_MaterialTemp_Inst (ID,PROJECTID,NAME,ParentId,Sort,MATERIALTEMPID) values (sq_oa_MaterialTemp_Inst.nextval,{0},'{1}',{2},{3},{4}) ";
            sql = string.Format(sql, ProjectId, Name, ParentId, Sort, MaterialTempId);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_MaterialTemp_Inst.currval from dual";
                Id = Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
                return Id;
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_MaterialTemp_Inst where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_MaterialTemp_Inst set PROJECTID={0}, NAME='{1}',ParentId={2},Sort={3},MATERIALTEMPID={4} where id={5}";
            sql = string.Format(sql, ProjectId, Name, ParentId, Sort, MaterialTempId);
            DBHelper.ExcuetSql(sql);
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            Name = dr["NAME"].ToString();
            ParentId = Convert.ToInt32(dr["PARENTID"].ToString());
            Sort = Convert.ToInt32(dr["SORT"].ToString());
            ProjectId = Convert.ToInt32(dr["PROJECTID"].ToString());
            MaterialTempId = Convert.ToInt32(dr["MATERIALTEMPID"].ToString());
        }
    }
}
