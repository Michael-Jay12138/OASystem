using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class Material : CommonAttribute
    {
        public string Extension { get; set; }
        public string Path { get; set; }
        public int MaterialTempInstId { get; set; }
        override public Object Create()
        {
            sql = "insert into oa_Material (id,name,Extension,Path,MATERIALTEMP_INST_ID) values (sq_oa_Material.nextval,'{0}','{1}','{2}',{3}) ";
            sql = string.Format(sql, Name, Extension, Path, MaterialTempInstId);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_Material.currval from dual";
                return Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_Material where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            throw new NotImplementedException();
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            Name = dr["NAME"].ToString();
            Extension = dr["EXTENSION"].ToString();
            Path = dr["PATH"].ToString();
            MaterialTempInstId = Convert.ToInt32(dr["MATERIALTEMP_INST_ID"].ToString());
        }
    }
}
