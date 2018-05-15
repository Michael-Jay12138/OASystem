using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model
{
    public class User:CommonAttribute
    {
        public string UserPwd { get; set; }
        public string UserPhone { get; set; }
        public int UserState { get; set; }
        public DateTime UserRegisterDate{ get; set; }

        override public Object Create()
        {
            sql = "insert into oa_user (id,name,pwd,phone,state,registerdate) values (sq_oa_user.nextval,'{0}','{1}','{2}',1,sysdate) ";
            sql = string.Format(sql, Name, UserPwd, UserPhone);
            if(DBHelper.ExcuetSql(sql)>0)
            {
                sql = "select sq_oa_user.currval from dual";
                Id = Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
                sql = "select RegisterDate from oa_user where id=" + Id;
                UserRegisterDate = Convert.ToDateTime(DBHelper.Query(sql).Rows[0][0]);
            }
            Dictionary<string, string> backData = new Dictionary<string, string>();
            backData.Add("Id", Id.ToString());
            backData.Add("UserRegisterDate", UserRegisterDate.ToString());
            return backData;

        }
        override public void Update()
        {
            sql = "update oa_user set name='{0}',pwd='{1}',phone='{2}',state={3} where id={4}";
            sql = string.Format(sql, Name, UserPwd, UserPhone, UserState, Id);
            DBHelper.ExcuetSql(sql);
        }
        override public void Delete()
        {
            sql = "delete from oa_user where id=" + Id;
            DBHelper.ExcuetSql(sql);
        }
        override public void Assembly(System.Data.DataRow dr)
        {
            Id = Convert.ToInt32(dr["ID"].ToString());
            Name = dr["NAME"].ToString();
            UserPwd = dr["PWD"].ToString();
            UserPhone = dr["PHONE"].ToString();
            UserState = Convert.ToInt32(dr["STATE"].ToString());
            try
            {
                UserRegisterDate = Convert.ToDateTime(dr["REGISTERDATE"].ToString());
            }
            catch(Exception e)
            {
                Util.Log.LogHelper.WriteLog(e.Message);
            }
        }
    }
}
