using ECJTU.OASystem.Model;
using ECJTU.OASystem.Model.Common;
using ECJTU.OASystem.Util.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Business
{
    public class DataService
    {
        string modelPath = "ECJTU.OASystem.Model";
        string sql;
        string entityName;
        string tableName;
        public DataService(string entityName,string tableName)
        {
            this.entityName = entityName;
            this.tableName = tableName;
        }
        private static object CreateObjectNoCache(string AssemblyPath,string classNamespace)
        {
            try
            {
                //使用 Assembly 来定义和加载程序集，加载程序集清单中列出的模块，以及在此程序集中定位一个类型并创建一个它的实例。
                object objType = System.Reflection.Assembly.Load(AssemblyPath).CreateInstance(classNamespace);
                return objType;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Boolean DeleteDataById(int Id)
        {
            CommonAttribute item = (CommonAttribute)CreateObjectNoCache(modelPath, modelPath+"." + entityName);
            item.Id = Id;
            item.Delete();
            return true;
        }
        /// <summary>
        /// 获取表中行数
        /// </summary>
        /// <returns></returns>
        public int GetDataCount(string joinStr = "", string whereStr = "")
        {
            sql = "select count(t.id) from "+tableName+" t {0} where 1=1 {1}";
            sql = string.Format(sql, joinStr, whereStr);
            return Convert.ToInt32(DBHelper.Query(sql).Rows[0][0].ToString());
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="joinStr"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public List<CommonAttribute> GetDataList(int pageIndex,int pageSize,string joinStr="",string whereStr="",string selectStr="t.*")
        {
            sql = "select {0} from "+ tableName + " t {1} where 1=1 {2}";
            sql = string.Format(sql, selectStr,joinStr, whereStr);
            sql=DBHelper.GetPageSql(sql, pageIndex, pageSize);
            DataTable dt=DBHelper.Query(sql);
            return AssemblyDataList(dt);
        }
        public List<CommonAttribute> GetDataList(string selectStr="*",string joinStr = "", string whereStr = "")
        {
            sql = "select {0} from " + tableName + " t {1} where 1=1 {2}";
            sql = string.Format(sql, selectStr, joinStr, whereStr);
            DataTable dt = DBHelper.Query(sql);
            return AssemblyDataList(dt);
        }
        /// <summary>
        /// 组装数据列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<CommonAttribute> AssemblyDataList(DataTable dt)
        {
            List<CommonAttribute> list = new List<CommonAttribute>();
            foreach (DataRow dr in dt.Rows)
            {
                CommonAttribute child = (CommonAttribute)CreateObjectNoCache(modelPath, modelPath+"." + entityName);
                child.Assembly(dr);
                list.Add(child);
            }
            return list;
        }
    }
}
