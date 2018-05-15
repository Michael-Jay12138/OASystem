using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model.Common
{
    public class CommonAttribute :ICommonMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string sql;
        virtual public void Assembly(DataRow dr)
        {
            Id = Convert.ToInt32(dr["Id"].ToString());
            Name = dr["NAME"].ToString();
        }

        virtual public Object Create()
        {
            throw new NotImplementedException();
        }

        virtual public void Delete()
        {
            throw new NotImplementedException();
        }

        virtual public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
