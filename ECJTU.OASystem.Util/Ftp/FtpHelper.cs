using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECJTU.OASystem.Util.Ftp
{
    public class FtpHelper
    {
        static public string FtpIp="127.0.0.1";
        static public string FtpUserName="ftp";
        static public string FtpUserPwd="123456";
        static public string DownLoad(FtpFile ftpfile, string outputpath)
        {
            Stream responseStream = null;
            FileStream stream2 = null;
            FtpWebResponse response = null;
            string message;
            try
            {
                if (ftpfile != null)
                {
                    string ftpPath= "ftp://" + FtpIp + ftpfile.FilePath;
                    string outPutDir = Path.GetDirectoryName(outputpath);
                    if (!Directory.Exists(outPutDir))
                    {
                        Directory.CreateDirectory(outPutDir);
                    }
                    stream2 = new FileStream(outputpath, FileMode.Create);
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
                    request.Method = "RETR";
                    request.UseBinary = true;
                    request.KeepAlive = false;
                    request.Credentials = new NetworkCredential(FtpUserName, FtpUserPwd);
                    response = (FtpWebResponse)request.GetResponse();
                    responseStream = response.GetResponseStream();
                    byte[] buffer = new byte[0x800];
                    if (responseStream != null)
                    {
                        for (int i = responseStream.Read(buffer, 0, 0x800); i > 0; i = responseStream.Read(buffer, 0, 0x800))
                        {
                            stream2.Write(buffer, 0, i);
                        }
                        responseStream.Close();
                    }
                    stream2.Close();
                    response.Close();
                    return "下载成功";
                }
                message = "没有需要下载的文件";
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            finally
            {
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (stream2 != null)
                {
                    stream2.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
            return message;
        }
        static public string UpLoad(FtpFile ftpfile, string localfile)
        {
            string requestUriString = "ftp://"+FtpIp +ftpfile.FilePath;
            FtpCheckDirectoryExist(ftpfile.FilePath);
            string path = localfile;
            if (!File.Exists(path))
            {
                return ("文件'" + path + "'不存在！");
            }
            FileInfo info = new FileInfo(path);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(requestUriString);
            request.Credentials = new NetworkCredential(FtpUserName, FtpUserPwd);
            request.KeepAlive = false;
            request.Method = "STOR";
            request.UseBinary = true;
            request.ContentLength = info.Length;
            int count = 0x800;
            byte[] buffer = new byte[count];
            FileStream stream = info.OpenRead();
            try
            {
                Stream requestStream = request.GetRequestStream();
                for (int i = stream.Read(buffer, 0, count); i != 0; i = stream.Read(buffer, 0, count))
                {
                    requestStream.Write(buffer, 0, i);
                }
                requestStream.Close();
                stream.Close();
                return ("文件'" + info.Name + "'上传成功！\r\n");
            }
            catch (Exception exception)
            {
                return ("上传文件'" + info.Name + "'时，发生错误：" + exception.Message + "\r\n");
            }
        }
        static public void FtpCheckDirectoryExist(string destFilePath)
        {
            string[] strArray = FtpParseDirectory(destFilePath).Split(new char[] { '/' });
            string localFile = "/";
            for (int i = 0; i < strArray.Length; i++)
            {
                string str3 = strArray[i];
                if ((str3 != null) && (str3.Length > 0))
                {
                    try
                    {
                        localFile = localFile + str3 + "/";
                        FtpMakeDir(localFile);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        static public string FtpParseDirectory(string destFilePath) =>
            destFilePath.Substring(0, destFilePath.LastIndexOf("/"));
        static public bool FtpMakeDir(string localFile)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + FtpIp + localFile);
            request.Credentials = new NetworkCredential(FtpUserName, FtpUserPwd);
            request.Method = "MKD";
            try
            {
                ((FtpWebResponse)request.GetResponse()).Close();
            }
            catch (Exception)
            {
                request.Abort();
                return false;
            }
            request.Abort();
            return true;
        }
    }
}
