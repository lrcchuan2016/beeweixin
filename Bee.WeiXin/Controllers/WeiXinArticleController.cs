using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bee.WeiXin.Models;
using Bee.Web;
using Bee.WeiXin;
using Bee.Data;
using Bee.Util;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace Bee.Controllers
{
    public class WeiXinArticleController : ControllerBase<WeiXinArticle>
    {
        private static readonly int MaxFileLength;

        protected static string ExtArrayString = string.Empty;

        static WeiXinArticleController()
        {
            ExtArrayString = "jpg,jpeg,png";
            MaxFileLength = 2000 * 1024;
        }

        public override PageResult Index(BeeDataAdapter dataAdapter)
        {
            dataAdapter.Add("delflag", false);

            return View(List(dataAdapter).Model);
        }

        public override void Delete(int id)
        {
            using (DbSession dbSession = GetDbSession())
            {
                BeeDataAdapter data = new BeeDataAdapter();
                data["delflag"] = true;
                dbSession.Update("WeiXinArticle", data, SqlCriteria.New.Equal("id", id));
            }
        }

        public override PageResult Show(int id)
        {
            return View(base.Show(id).Model);
        }

        public string Upload()
        {
            string inputname = "filedata";
            string attachdir = "/Resources/upimages";     // 上传文件保存路径，结尾不要带/

            System.Web.HttpRequest request = HttpContextUtil.CurrentHttpContext.Request;


            string disposition = request.ServerVariables["HTTP_CONTENT_DISPOSITION"];
            Stream fileData;
            string localname;
            if (disposition != null)
            {
                // HTML5上传
                byte[] file = request.BinaryRead(request.TotalBytes);
                fileData = new MemoryStream(file);
                localname = HttpUtility.UrlDecode(Regex.Match(disposition, "filename=\"(.+?)\"").Groups[1].Value);// 读取原始文件名
            }
            else
            {
                HttpFileCollection filecollection = request.Files;
                HttpPostedFile postedfile = filecollection.Get(inputname);

                // 读取原始文件名
                localname = postedfile.FileName;

                fileData = postedfile.InputStream;

                filecollection = null;
            }

            string extension = localname.Substring(localname.LastIndexOf('.') + 1);
            bool extFlag = ExtArrayString.IndexOf(extension) >= 0;

            ThrowExceptionUtil.ArgumentConditionTrue(extFlag, "fileName", "不能上传该文件。");

            ThrowExceptionUtil.ArgumentConditionTrue(fileData.Length > 0 && fileData.Length < MaxFileLength, "filedata", "文件大小超过{0}字节".FormatWith(MaxFileLength));

            string attach_dir, attach_subdir, err;
 
            attach_subdir = "day_" + DateTime.Now.ToString("yyMMdd");

            attach_dir = attachdir + "/" + attach_subdir + "/";

            Random random = new Random(DateTime.Now.Millisecond);
            string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + random.Next(10000) + "." + extension;

            try
            {
                string dir = HttpContextUtil.CurrentHttpContext.Server.MapPath(attach_dir);
                IOUtil.SafeCreateDirectory(dir);

                System.IO.FileStream fs = new System.IO.FileStream(IOUtil.CombinePath(dir, filename)
                    , System.IO.FileMode.Create, System.IO.FileAccess.Write);

                int buffSize = (int)fileData.Length;
                byte[] buff = new Byte[buffSize];
                fileData.Read(buff, 0, buffSize);
                fileData.Close();

                fs.Write(buff, 0, buffSize);

                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                err = ex.Message.ToString();
            }


            return attach_dir + filename;
        }

        private string JsonString(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("/", "\\/");
            str = str.Replace("'", "\\'");
            return str;
        }

        protected override Bee.Data.DbSession GetDbSession(bool useTransaction)
        {
            return new Bee.Data.DbSession(WeiXinConstants.WeiXinConnString, useTransaction);
        }
    }
}
