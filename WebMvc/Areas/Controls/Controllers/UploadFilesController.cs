using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WebMvc.Areas.Controls.Controllers
{
    public class UploadFilesController : Controller
    {
        //
        // GET: /Controls/UploadFiles/

        public ActionResult Index()
        {
            return View();
        }

        public string Upload()
        {
            Response.ContentType = "text/plain";
            string str1 = Request.Form["str1"];
            string str2 = Request.Form["str2"];

            var obj = RoadFlow.Cache.IO.Opation.Get(str1 ?? "");
            if (str1.IsNullOrEmpty() || str2.IsNullOrEmpty() || obj == null || obj.ToString() != str2)
            {
                return "您不能上传文件";
            }

            //接收上传后的文件
            HttpPostedFileBase file = Request.Files["Filedata"];

            if (!RoadFlow.Utility.Config.UploadFileType.Contains(Path.GetExtension(file.FileName).TrimStart('.'), StringComparison.CurrentCultureIgnoreCase))
            {
                return "您上传的文件类型不被允许";
            }

            //获取文件的保存路径
            string uploadPath;
            string uploadFullPath = Server.MapPath(getFilePath(out uploadPath));

            //判断上传的文件是否为空
            if (file != null)
            {
                if (!Directory.Exists(uploadFullPath))
                {
                    Directory.CreateDirectory(uploadFullPath);
                }
                //保存文件
                string newFileName = getFileName(uploadFullPath, file.FileName);
                string newFileFullPath = uploadFullPath + newFileName;
                try
                {
                    long fileLength = file.ContentLength;
                    file.SaveAs(newFileFullPath);

                    //图片压缩
                    string file_extension = Path.GetExtension(file.FileName);//扩展名
                    if (file_extension == ".jpg" || file_extension == ".JPG" || file_extension == ".png")
                    {
                        string compressFileName = "compress" + newFileName;
                        GetPicThumbnail(uploadFullPath + newFileName, uploadFullPath + compressFileName, 540, 720, 90);
                        FileInfo fileInfo = new FileInfo(uploadFullPath + compressFileName);
                        fileLength = fileInfo.Length;//获取压缩之后的大小
                        return "1|" + (uploadPath + compressFileName) + "|" + (fileLength / 1000).ToString("###,###") + "|" + compressFileName;
                    }


                    return "1|" + (uploadPath + newFileName) + "|" + (fileLength / 1000).ToString("###,###") + "|" + newFileName;
                }
                catch
                {
                    return "上传文件发生了错误";
                }
            }
            else
            {
                return "上传文件为空";
            }
        }

        /// <summary>
        /// 无损压缩图片,等比缩放
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth">宽度</param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>
        public bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
            {
                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(sW, sH);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.DrawImage(iSource, new Rectangle(0, 0, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

                ImageCodecInfo jpegICIinfo = null;

                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();

            }
        }

        /// <summary>
        /// 得到上传文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string getFileName(string filePath, string fileName)
        {
            while (System.IO.File.Exists(filePath + fileName))
            {
                fileName = Path.GetFileNameWithoutExtension(fileName) + "_" + RoadFlow.Utility.Tools.GetRandomString() + Path.GetExtension(fileName); 
                //fileName = Guid.NewGuid() + Path.GetExtension(fileName);//为了防止中文等意外字符造成乱码，直接用guid作为文件名
            }
            return fileName;
        }

        /// <summary>
        /// 得到文件保存路径
        /// </summary>
        /// <returns></returns>
        private string getFilePath(out string path1)
        {
            DateTime date = DateTime.Now;
            path1 = Url.Content("~/Content/UploadFiles/" + date.ToString("yyyyMM") + "/" + date.ToString("dd") + "/");//upload/inform/
            return path1;
        }

        public string Delete()
        {
            string str1 = Request.QueryString["str1"];
            string str2 = Request.QueryString["str2"];
            var obj = RoadFlow.Cache.IO.Opation.Get(str1 ?? "");
            if (str1.IsNullOrEmpty() || str2.IsNullOrEmpty() || obj == null || obj.ToString() != str2)
            {
                return "var json = {\"success\":0,\"message\":\"您不能删除文件\"}";
            }
            string file = Request.QueryString["file"];
            if (!file.IsNullOrEmpty())
            {
                try
                {
                    //System.IO.File.Delete(Server.MapPath(Path.Combine(Url.Content("~/Content/Controls/UploadFiles/"), file)));
                    return "var json = {\"success\":1,\"message\":\"\"}";
                }
                catch (Exception e)
                {
                    return "var json = {\"success\":0,\"message\":\"" + e.Message + "\"}";
                }
            }
            return "";
        }
    }
}
