using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Bee.Util;
using System.Xml.Linq;
using System.Xml;
using Bee.Logging;
using Bee.Web;
using Bee.Core;

namespace Bee.WeiXin
{
    public static class WeiXinEngine
    {
        private static readonly string Token = ConfigUtil.GetAppSettingValue<string>("WeiXinToken");
        private static readonly bool DebugFlag = ConfigUtil.GetAppSettingValue<bool>("Debug");

        private static readonly WeiXinDispatcher Dispatcher = new WeiXinDispatcher();

        public static void Process(HttpContext context)
        {
            try
            {
                System.Web.HttpRequest Request = context.Request;

                string signature = Request["signature"];
                string timestamp = Request["timestamp"];
                string nonce = Request["nonce"];
                string echostr = Request["echostr"];

                if (Request.HttpMethod == "GET")
                {
                    //get method - 仅在微信后台填写URL验证时触发
                    if (CheckSignature(signature, timestamp, nonce, Token))
                    {
                        context.Response.Write(echostr); //返回随机字符串则表示验证通过
                    }
                    else
                    {
                        context.Response.Write("failed:" + signature + "," + GetSignature(timestamp, nonce, Token));
                    }
                    context.Response.End();
                }
                else
                {
                    if (DebugFlag)
                    {
                        Dispatcher.ProcessRequest(context);
                    }
                    else
                    {
                        if (CheckSignature(signature, timestamp, nonce, Token))
                        {
                            Dispatcher.ProcessRequest(context);
                        }
                        else
                        {
                            context.Response.Write("failed:" + signature + "," + GetSignature(timestamp, nonce, Token));

                        }
                    }

                }
            }
            catch (Exception e)
            {
                Logger.Error("error!", e);
            }
        }

        private static string GetSignature(string timestamp, string nonce, string token)
        {
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);

            return SecurityUtil.Sha1EncrptS(arrString);
        }

        private static bool CheckSignature(string signature, string timestamp, string nonce, string token)
        {
            return GetSignature(timestamp, nonce, token) == signature;
        }
    }
}
