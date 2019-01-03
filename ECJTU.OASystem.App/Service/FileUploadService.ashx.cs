using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ECJTU.OASystem.App.Service
{
    /// <summary>
    /// FileService 的摘要说明
    /// </summary>
    public class FileUploadService : IHttpHandler
    {
        string tempFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");
        public void ProcessRequest(HttpContext context)
        {
            lock (typeof(FileUploadService))
            {
                HttpFileCollection files = context.Request.Files;
                string tempFilePath = "";
                if (files.Count > 0)
                {
                    string fileName = files[0].FileName;
                    tempFilePath = Path.Combine(tempFileDir, fileName);
                    files[0].SaveAs(tempFilePath);
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write(tempFilePath);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}