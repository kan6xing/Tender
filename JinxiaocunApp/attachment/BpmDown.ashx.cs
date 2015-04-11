using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.IO;
using Microsoft.Win32;

namespace JinxiaocunApp.attachment
{
    /// <summary>
    /// BpmDown 的摘要说明
    /// </summary>
    public class BpmDown : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            
            if (context.Request.QueryString.Count <= 0)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("坏小孩");
                return;
            }
            string fileId = context.Request.QueryString.ToString();

            DataTable dt= JinxiaocunApp.GenericDataAccess.Query("select * from YZAppAttachment where FileID=@fid", new string[,] { { "@fid", fileId, "DbType.String", null } });
            if (dt == null||dt.Rows.Count<=0)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("文件不存在");
                return;
            }
            

            DataRow dr=dt.Rows[0];
            string fileName = dr["Name"].ToString();
            string fileExt = dr["Ext"].ToString();
            long fileSize =long.Parse( dr["Size"].ToString());
            string filePath = Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (fileId.Substring(0, 4) + @"\" + fileId.Substring(4, 4) + @"\" + fileId.Substring(8)));
            if (!File.Exists(filePath))
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("文件不存在"+" "+filePath);
                //context.Response.Write("文件不存在!");
                return;
            }
            //JinxiaocunApp.GenericDataAccess.ExecuteSelectCommandSql("");

            //try
            //{
                
            //    Attachment attachment;
            //    using (IDbConnection cn = QueryManager.CurrentProvider.OpenConnection())
            //    {
            //        attachment = YZAttachmentHelper.GetAttachmentInfo(cn, fileId);
            //    }

            //    string fileName = attachment.Name;
            //    string fileExt = attachment.Ext;
            //    long fileSize = attachment.Size;
            //    string filePath = Attachment.FileIDToPath(fileId, YZAttachmentHelper.AttachmentRootPath);

            //    if (!File.Exists(filePath))
            //        throw new Exception(String.Format("sldkf{0}", fileId));

            bool contentDisposition = true;
            string range = context.Request.Headers["HTTP_RANGE"];
            string content_type = "application/octet-stream";
            RegistryKey file_key = Registry.ClassesRoot.OpenSubKey(fileExt);
            if (file_key != null)
                content_type = file_key.GetValue("Content Type", content_type).ToString();

            context.Response.AppendHeader("Content-Type", content_type);
           
            if (contentDisposition)
                context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + context.Server.UrlPathEncode(fileName));

            context.Response.AppendHeader("Accept-Ranges", "bytes");


            context.Response.AppendHeader("Content-Length", fileSize.ToString());
            context.Response.CacheControl = HttpCacheability.Public.ToString();
            context.Response.Cache.AppendCacheExtension("max-age=" + 365 * 24 * 60 * 60);
            context.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
            context.Response.AppendHeader("ETag", "Never_Modify");
            context.Response.Cache.SetETag("Never_Modify");
            context.Response.Cache.SetLastModified(DateTime.Now.AddMinutes(-1));

            context.Response.TransmitFile(filePath);
            return;


            if (range == null)
            {
                //全新下载
                context.Response.AppendHeader("Content-Length", fileSize.ToString());
                context.Response.CacheControl = HttpCacheability.Public.ToString();
                context.Response.Cache.AppendCacheExtension("max-age=" + 365 * 24 * 60 * 60);
                context.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
                context.Response.AppendHeader("ETag", "Never_Modify");
                context.Response.Cache.SetETag("Never_Modify");
                context.Response.Cache.SetLastModified(DateTime.Now.AddMinutes(-1));

                context.Response.TransmitFile(filePath);
            }
            else
            {
                //断点续传以及多线程下载支持
                string[] file_range = range.Substring(6).Split(new char[1] { '-' });
                context.Response.Status = "206 Partial Content";
                context.Response.AppendHeader("Content-Range", "bytes " + file_range[0] + "-" + file_range[1] + "/" + fileSize.ToString());
                context.Response.AppendHeader("Content-Length", (Int32.Parse(file_range[1]) - Int32.Parse(file_range[0]) + 1).ToString());
                context.Response.TransmitFile(filePath, long.Parse(file_range[0]), (long)(Int32.Parse(file_range[1]) - Int32.Parse(file_range[0]) + 1));
            }
            //}
            //catch (Exception exp)
            //{
            //    JsonItem rv = new JsonItem();
            //    rv.Attributes.Add("success", false);
            //    rv.Attributes.Add("errorMessage", exp.Message);

            //    context.Response.Write(rv.ToString());
            //}
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