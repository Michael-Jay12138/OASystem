using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class FormTemp : CommonAttribute
    {
        public byte[] Layout { get; set; }
        override public Object Create()
        {
            sql = "insert into oa_FormTemp (id,name,layout) values (sq_oa_FormTemp.nextval,:name,:layout) ";

            OracleParameter[] parameterValue = new OracleParameter[2];
            parameterValue[0] = new OracleParameter("name", Name);
            parameterValue[1] = new OracleParameter("layout", Layout);
            if (DBHelper.ExcuetSql(sql, parameterValue) > 0)
            {
                sql = "select sq_oa_FormTemp.currval from dual";
                return Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_FormTemp where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_FormTemp set name=:name,layout=:layout where id=:id ";
            sql = string.Format(sql, Name, Layout);

            OracleParameter[] parameterValue = new OracleParameter[3];
            parameterValue[0] = new OracleParameter("name", Name);
            parameterValue[1] = new OracleParameter("layout", Layout);
            parameterValue[2] = new OracleParameter("id", Id);
            DBHelper.ExcuetSql(sql, parameterValue);
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            Name = dr["NAME"].ToString();
            Layout = (byte[])dr["LAYOUT"];
        }
    }
}
