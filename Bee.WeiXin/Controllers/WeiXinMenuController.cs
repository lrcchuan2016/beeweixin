using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bee.Web;
using Bee.Util;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;

namespace Bee.Controllers
{
    public class WeiXinMenuController : ControllerBase
    {
        private static string Access_token = string.Empty;
        public PageResult Index()
        {

            return View();
        }

        public string GetMenu()
        {
            return HttpUtil.HttpGet(string.Format(@"https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", AccessToken), null);
        }

        public string PostMenu(string menu)
        {
            menu = HttpUtility.HtmlDecode(menu);
            using (MemoryStream ms = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(menu);
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", AccessToken);

                return HttpUtil.HttpPost(url, ms, null);
            }
        }

        public string DeleteMenu()
        {
            return HttpUtil.HttpGet(string.Format(@"https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", AccessToken), null);
        }

        private string AccessToken
        {
            get
            {
                if (string.IsNullOrEmpty(Access_token))
                {
                    string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}",
                        ConfigUtil.GetAppSettingValue<string>("WeiXinAppId"), ConfigUtil.GetAppSettingValue<string>("WeiXinAppSec"));

                    string response = HttpUtil.HttpGet(url, null);

                    Regex regex = new Regex(@"""access_token"":""(?<token>.*)"",");
                    Match match = regex.Match(response);
                    if (match.Success)
                    {
                        Access_token = match.Groups["token"].Value;
                    }
                }

                return Access_token;
            }
        }
    }
}
