using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.OracleClient;

namespace ECJTU.OASystem.Util.DB
{
    public class DBConn
    {
        private  OracleDataReader odr = null;
        private  OracleConnection conn=null;
        public OracleConnection GetConn()
        {
            this.conn = new OracleConnection(this.GetConnStr());
            
                try
                {
                    if (this.conn.State == ConnectionState.Closed)
                    {
                        this.conn.Open();
                    }
                    return conn;
                }
                catch (Exception exception)
                {
                    Log.LogHelper.WriteLog(exception.Message);
                    this.conn.Close();
                    return null;
                }
        }


        

        private string GetConnStr() =>
            ("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1) (PORT=1521)))(CONNECT_DATA=(SERVICE_NAME= orcl)));User Id=oasys; Password=oasys");
    }
}
