using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Model.Common
{
    public interface ICommonMethod
    {
        int Id { get; set; }
        string Name { get; set; }
        Object Create();
        void Delete();
        void Update();
        void Assembly(System.Data.DataRow dr);
    }
}
