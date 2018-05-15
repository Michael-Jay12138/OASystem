using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class Business : CommonAttribute
    {
        public int BusinessGroupId { get; set; }
        public string Remark { get; set; }
        override public Object Create()
        {
            sql = "insert into oa_Business (id,name,BusinessGroupId,Remark) values (sq_oa_Business.nextval,'{0}',{1},'{2}') ";
            sql = string.Format(sql, Name,BusinessGroupId,Remark);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_Business.currval from dual";
                return Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_Business where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_Business set name='{0}',BusinessGroupId={1},Remark='{2}' where id={3}";
            sql = string.Format(sql, Name, BusinessGroupId, Remark, Id);
            DBHelper.ExcuetSql(sql);
        }
        public void AddFormTemp(int FormTempId)
        {
            sql = "insert into OA_BUSINESS_FORM (id,businessid,formtempid) values (sq_OA_BUSINESS_FORM.nextval,{0},{1})";
            sql = string.Format(sql, Id, FormTempId);
            DBHelper.ExcuetSql(sql);
        }
        public void UpdateFormTemp(int FormTempId)
        {
            sql = "delete from OA_BUSINESS_FORM bf where bf.businessid=" + Id;
            DBHelper.ExcuetSql(sql);
            AddFormTemp(FormTempId);
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            Name = dr["NAME"].ToString();
            BusinessGroupId = Convert.ToInt32(dr["BUSINESSGROUPID"].ToString());
            Remark = dr["REMARK"].ToString();
        }
    }
}
