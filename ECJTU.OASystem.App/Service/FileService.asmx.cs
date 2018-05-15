using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ECJTU.OASystem.App.Service
{
    /// <summary>
    /// FileService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class FileService : System.Web.Services.WebService
    {
        static string tempFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");
        public FileService()
        {
            if (!Directory.Exists(tempFileDir))
            {
                Directory.CreateDirectory(tempFileDir);
            }
        }
        [WebMethod(Description = "上传文件到Temp目录")]
        public string UpLoadFile(string fileName,byte[] fileBytes)
        {
            string tempFilePath = Path.Combine(tempFileDir, fileName);
            if (!File.Exists(tempFilePath))
            {
                FileStream fs = new FileStream(tempFilePath, FileMode.Create);
                byte[] data = fileBytes;
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
            }
            else
            {
                //追加文件
                using (FileStream f = new FileStream(tempFilePath, FileMode.Append, FileAccess.Write))
                {
                    if (f.CanWrite)
                    {
                        byte[] b = fileBytes;
                        f.Write(b, 0, b.Length);
                        f.Close();
                        f.Dispose();
                    }
                }
            }
            return tempFilePath;
        }
    }
}
