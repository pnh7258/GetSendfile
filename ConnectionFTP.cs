using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GetFileR.Common
{
    public static class ConnectionFTP
    {
        public static string link { get; set; }
        public static string user { get; set; }
        public static string password { get; set; }
       
        public static bool Instance()
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(link);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(user, password);
                request.Abort();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string[] ListDirectory()
        {
            var list = new List<string>();

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(link);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(user, password);

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, true))
                    {
                        while (!reader.EndOfStream)
                        {
                            list.Add(reader.ReadLine());
                        }
                    }
                }
            }

            return list.ToArray();
        }

        public static bool DownloadFile(ref List<FileAttribute> listFile, string folder)
        {
            try
            {
                foreach (FileAttribute item in listFile)
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(item.Folder + "/" + item.FileName);
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.KeepAlive = true;
                    request.UsePassive = false;
                    request.UseBinary = false;

                    request.Credentials = new NetworkCredential(user, password);
                    try
                    {
                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        using (FileStream writer = new FileStream(folder + "\\" + item.FileName, FileMode.Create))
                        {

                            long length = response.ContentLength;
                            int bufferSize = 2048;
                            int readCount;
                            byte[] buffer = new byte[2048];

                            readCount = responseStream.Read(buffer, 0, bufferSize);
                            while (readCount > 0)
                            {
                                writer.Write(buffer, 0, readCount);
                                readCount = responseStream.Read(buffer, 0, bufferSize);
                            }
                        }
                        reader.Close();
                        response.Close();
                        item.isFinish = true;
                    }
                    catch (Exception)
                    {
                        item.isFinish = false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }

    public class FileAttribute
    {
        public string Folder { get; set; }
        public string FileName { get; set; }
        public bool isFinish { get; set; }
    }
}
