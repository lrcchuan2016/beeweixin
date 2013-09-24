using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Bee.Util;
using Bee.Logging;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using Bee.WeiXin;

namespace Topease.WeiXin
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DemoHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            WeiXinEngine.Process(context);
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
