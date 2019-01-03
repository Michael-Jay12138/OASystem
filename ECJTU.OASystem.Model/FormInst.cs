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
    public class FormInst : CommonAttribute
    {
        public int ProjectId { get; set; }
        public int FormTempId { get; set; }
        public byte[] Content { get; set; }
        override public Object Create()
        {
            sql = "insert into oa_FormInst (id,ProjectId,FormTempId,Content) values (sq_oa_FormInst.nextval,:projectid,:formtempid,:content) ";
            OracleParameter[] parameterValue = new OracleParameter[3];
            parameterValue[0] = new OracleParameter("projectid", ProjectId);
            parameterValue[1] = new OracleParameter("formtempid", FormTempId);
            parameterValue[2] = new OracleParameter("content", Content);
            if (DBHelper.ExcuetSql(sql,parameterValue) > 0)
            {
                sql = "select sq_oa_FormInst.currval from dual";
                Id = Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
                return Id;
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_FormInst where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_FormInst set ProjectId=:projectid,FormTempId=:formtempid,Content=:content where Id="+Id;
            OracleParameter[] parameterValue = new OracleParameter[3];
            parameterValue[0] = new OracleParameter("projectid", ProjectId);
            parameterValue[1] = new OracleParameter("formtempid", FormTempId);
            parameterValue[2] = new OracleParameter("content", Content);
            DBHelper.ExcuetSql(sql, parameterValue);
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            ProjectId= Convert.ToInt32(dr["PROJECTID"].ToString());
            FormTempId = Convert.ToInt32(dr["FORMTEMPID"]);
            Content = (byte[])dr["CONTENT"];
        }
    }
}
