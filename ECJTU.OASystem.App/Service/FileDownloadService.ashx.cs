using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ECJTU.OASystem.App.Service
{
    /// <summary>
    /// FileDownloadService 的摘要说明
    /// </summary>
    public class FileDownloadService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            lock (typeof(FileDownloadService))
            {
                string tempFilePath = context.Request.Params["tempFilePath"];
                if (File.Exists(tempFilePath))
                {
                    FileInfo fileInfo = new FileInfo(tempFilePath);
                    //context.Response.ContentType = getContextType(fileInfo.Extension);
                    //context.Response.AddHeader("Content-Disposition", "inline;fileName=" + context.Server.UrlEncode(fileInfo.Name));
                    //context.Response.WriteFile(fileInfo.FullName);

                    //以字符流的形式下载文件
                    FileStream fs = new FileStream(tempFilePath, FileMode.Open);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                    context.Response.ContentType = "application/octet-stream";
                    //通知浏览器下载文件而不是打开
                    context.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(fileInfo.Name, System.Text.Encoding.UTF8));
                    context.Response.BinaryWrite(bytes);
                    context.Response.Flush();
                    context.Response.End();
                }
            }
        }
        private string getContextType(string sType)
        {
            string contentType = string.Empty;
            switch (sType)
            {
                case ".PDF":
                case "PDF":
                    contentType = "application/pdf";
                    break;
                case ".DOC":
                case "DOC":
                    contentType = "application/msword";
                    break;
                case ".DOCX":
                case "DOCX":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".XLS":
                case "XLS":
                    contentType = "application/vnd.ms-excel";
                    break;
                case ".DOTX":
                case "DOTX":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                    break;
                case ".PPTX":
                case "PPTX":
                    contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                case ".PPSX":
                case "PPSX":
                    contentType = "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                    break;
                case ".POTX":
                case "POTX":
                    contentType = "application/vnd.openxmlformats-officedocument.presentationml.template";
                    break;
                case ".XLSX":
                case "XLSX":
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".ZIP":
                case "ZIP":
                    contentType = "application/zip";
                    break;
                case ".AVI":
                case "AVI":
                    contentType = "application/avi";
                    break;
                case ".GIF":
                case "GIF":
                    contentType = "application/gif";
                    break;
                case ".JPG":
                case "JPG":
                    contentType = "application/jpg";
                    break;
                case ".TXT":
                case "TXT":
                    contentType = "application/plain";
                    break;
                case ".HTM":
                case "HTM":
                    contentType = "application/html";
                    break;
                case ".DWG":
                case "DWG":
                case ".DXF":
                case "DXF":
                    contentType = "application/x-autocad";
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }
            return contentType;
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