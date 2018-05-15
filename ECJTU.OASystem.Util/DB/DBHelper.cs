using ECJTU.OASystem.Util.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Util.DB
{
    public class DBHelper
    {
        static private OracleConnection conn;
        static private void InitConn()
        {
            if (conn == null)
                conn = new DBConn().GetConn();
        }
        static public int ExcuetSql(string sql)
        {
            try
            {
                InitConn();
                OracleCommand command = conn.CreateCommand();
                command.CommandText = sql;
                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(e.ToString());
                return 0;
            }
        }
        static public int ExcuetSql(string sql,OracleParameter[] parameterValue)
        {
            try
            {
                InitConn();
                OracleCommand command = conn.CreateCommand();
                command.CommandText = sql;
                for(int i = 0; i < parameterValue.Length; i++)
                {
                    command.Parameters.Add(parameterValue[i]);
                }
                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(e.ToString());
                return 0;
            }
        }
        /// <summary>
        /// 获取分页脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static string GetPageSql(string sql, int pageIndex, int pageSize)
        {
            StringBuilder pageSql = new StringBuilder();
            pageSql.Append("select * from (");
            pageSql.AppendFormat("select rownum no,A.* from ({0})A", sql);
            pageSql.AppendFormat("  where rownum<={0})B", (pageIndex + 1) * pageSize);
            pageSql.AppendFormat("  where B.no>={0}", pageIndex * pageSize);
            return pageSql.ToString();
        }
        static public DataTable Query(string sql)
        {
            try
            {
                InitConn();
                OracleDataAdapter adapter = new OracleDataAdapter(sql, conn);
                DataTable dataTable = new DataTable();
                dataTable.TableName = "Data";
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(e.ToString());
                throw e;
            }
        }
        static public DataSet QueryDataSet(string sql)
        {
            try
            {
                InitConn();
                OracleDataAdapter adapter = new OracleDataAdapter(sql, conn);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                return dataSet;
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(e.ToString());
                throw e;
            }
        }

    }
}
