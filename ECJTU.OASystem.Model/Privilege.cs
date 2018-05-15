﻿using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class Privilege : CommonAttribute
    {
        public int ParentId { get; set; }

        override public Object Create()
        {
            sql = "insert into oa_Privilege (id,name,ParentId) values (sq_oa_Privilege.nextval,'{0}',{1}) ";
            sql = string.Format(sql, Name,ParentId);
            if (DBHelper.ExcuetSql(sql) > 0)
            {
                sql = "select sq_oa_Privilege.currval from dual";
                return Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
            }
            return 0;
        }

        override public void Delete()
        {
            sql = "delete from oa_Privilege where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }

        override public void Update()
        {
            sql = "update oa_Privilege set name='{0}',ParentId='{1}' where id={2}";
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
