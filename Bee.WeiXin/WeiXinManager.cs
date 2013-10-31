using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bee.Web;
using Bee.Util;
using System.Text.RegularExpressions;
using Bee.WeiXin.Models;
using Bee.Data;
using Bee.WeiXin;
using Bee.Core;

namespace Bee.WeiXin
{
    public class WeiXinManager
    {
        private static WeiXinManager instance = new WeiXinManager();

        private HttpClient httpClient = new HttpClient(null, new HttpClientContext(), true);

        private Regex userListRegex = new Regex(@"{""id"":(?<id>.*?),""nick_name"":""(?<nickname>.*?)"",""remark_name"":""(?<remarkname>.*?)"",""group_id"":(?<groupid>.*?)}");
        private Regex userGroupRegex = new Regex(@"{""id"":(?<id>.*?),""name"":""(?<name>.*?)"",""cnt"":(?<count>.*?)}");

        private Regex mpTokenRegex = new Regex(@"token=(?<token>\d*)");

        private Regex wxGeneralResultRegext = new Regex(@"{""ret"":""(?<ret>.*?)"", .*""msg"":""(?<msg>.*?)""");

        private Regex contactInfoRegex = new Regex(@"""(?<name>.*?)"".*?:.*?""(?<value>.*?)"",");

        private List<WeiXinGroup> weiXinGroupList = null;

        private string mpToken;

        private WeiXinManager()
        {

        }

        public static WeiXinManager Instance
        {
            get
            {
                return instance;
            }
        }

        internal string MPToken
        {
            get
            {
                Init(false);

                return mpToken;
            }
        }

        public void RefreshGroup()
        {
            Init(false);
            httpClient.Url = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&pagesize=10&pageidx=0&type=0&groupid=0&token={0}&lang=zh_CN".FormatWith(MPToken);

            string response = httpClient.GetString();

            List<WeiXinGroup> list = new List<WeiXinGroup>();
            Match match = userGroupRegex.Match(response);
            while (match.Success)
            {
                WeiXinGroup weiXinGroup = new WeiXinGroup();
                weiXinGroup.GroupId = match.Groups["id"].Value;
                weiXinGroup.Name = match.Groups["name"].Value;
                weiXinGroup.CurrentCount = int.Parse(match.Groups["count"].Value);

                list.Add(weiXinGroup);

                match = match.NextMatch();
            }

            using (DbSession dbSession = new DbSession(WeiXinConstants.WeiXinConnString, true))
            {
                List<WeiXinGroup> dbGroup = dbSession.Query<WeiXinGroup>();

                foreach (WeiXinGroup item in list)
                {
                    WeiXinGroup dbItemValue = (from dbItem in dbGroup where dbItem.GroupId == item.GroupId select dbItem).SingleOrDefault();
                    if (dbItemValue != null)
                    {
                        item.Id = dbItemValue.Id;
                        item.LastCount = dbItemValue.CurrentCount;
                        item.CreateTime = dbItemValue.CreateTime;
                    }

                    dbSession.Save(item);
                }

                dbSession.CommitTransaction();
            }

            weiXinGroupList = list;
        }

        public void RefreshAll()
        {
            RefreshGroup();

            foreach (WeiXinGroup weiXinGroup in weiXinGroupList)
            {
                if (weiXinGroup.CurrentCount > 0)
                {
                    RefreshUserList(0, weiXinGroup.CurrentCount, weiXinGroup.GroupId);
                }
            }
        }

        public void RefreshUserList(int pageIndex, int pageSize, string groupId)
        {
            Init(false);
            httpClient.Url = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&pagesize={0}&pageidx={1}&type=0&groupid={2}&token={3}&lang=zh_CN".FormatWith(pageSize, pageIndex, groupId, MPToken);

            string response = httpClient.GetString();

            List<WeiXinUser> list = new List<WeiXinUser>();
            Match match = userListRegex.Match(response);
            while (match.Success)
            {
                WeiXinUser weiXinUser = new WeiXinUser();
                weiXinUser.OpenId = match.Groups["id"].Value;
                weiXinUser.NickName = match.Groups["nickname"].Value;
                weiXinUser.RemarkName = match.Groups["remarkname"].Value;
                weiXinUser.GroupId = match.Groups["groupid"].Value;
                weiXinUser.ValidFlag = true;

                list.Add(weiXinUser);

                match = match.NextMatch();
            }

            using (DbSession dbSession = new DbSession(WeiXinConstants.WeiXinConnString, true))
            {
                List<WeiXinUser> dbUser = dbSession.Query<WeiXinUser>();

                foreach (WeiXinUser item in list)
                {
                    WeiXinUser dbItemValue = (from dbItem in dbUser where dbItem.OpenId == item.OpenId select dbItem).SingleOrDefault();
                    if (dbItemValue != null)
                    {
                        item.Id = dbItemValue.Id;
                        item.FakeId = dbItemValue.FakeId; // FakeId 需要保留
                        item.CreateTime = dbItemValue.CreateTime;
                    }

                    dbSession.Save(item);
                }

                dbSession.CommitTransaction();
            }

            foreach (WeiXinUser item in list)
            {
                if (string.IsNullOrEmpty(item.UserName))
                {
                    RefreshUserContactInfo(item.OpenId);
                }
            }

        }

        internal bool RefreshUserContactInfo(string openId)
        {
            bool result = false;
            Init(false);

            System.Threading.Thread.Sleep(50);

            httpClient.Url = "https://mp.weixin.qq.com/cgi-bin/getcontactinfo";
            httpClient.Context.Referer = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&pagesize=10&pageidx=0&type=0&groupid=100&token={0}&lang=zh_CN".FormatWith(MPToken);

            Dictionary<string, string> postData = httpClient.PostingData;

            postData.Add("fakeid", openId);
            postData.Add("token", MPToken);
            postData.Add("lang", "zh_CN");
            postData.Add("t", "ajax-getcontactinfo");

            string response = httpClient.GetString();

            Dictionary<string, string> dict = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            Match match = contactInfoRegex.Match(response);
            while (match.Success)
            {
                string name = match.Groups["name"].Value;
                string value = match.Groups["value"].Value;

                if (!dict.ContainsKey(name))
                {
                    dict.Add(name, value);
                }

                match = match.NextMatch();
            }

            dict.Remove("fakeid");

            string userName = string.Empty;
            dict.TryGetValue("user_name", out userName);
            dict["username"] = userName;

            if (dict.Count >= 5 && !string.IsNullOrEmpty(userName))
            {
                using (DbSession dbSession = new DbSession(WeiXinConstants.WeiXinConnString))
                {
                    WeiXinUser weiXinUser = dbSession.Query<WeiXinUser>(SqlCriteria.New.Equal("openid", openId)).FirstOrDefault();
                    if (weiXinUser != null)
                    {
                        IEntityProxy entityProxy = EntityProxyManager.Instance.GetEntityProxy<WeiXinUser>();

                        foreach (string key in dict.Keys)
                        {
                            entityProxy.SetPropertyValue(weiXinUser, key, dict[key]);
                        }
                    }

                    dbSession.Save(weiXinUser);

                    result = true;
                }
            }

            return result;
        }

        private void Init(bool forceFlag)
        {
            if (string.IsNullOrEmpty(mpToken) || forceFlag)
            {
                string weiXinUserName = ConfigUtil.GetAppSettingValue<string>("WeiXinUserName");
                string weiXinPassword = ConfigUtil.GetAppSettingValue<string>("WeiXinPassword");

                string password = SecurityUtil.MD5EncryptS(weiXinPassword).ToUpper();

                Dictionary<string, string> postData = httpClient.PostingData;

                postData.Add("username", weiXinUserName);
                postData.Add("pwd", password);
                postData.Add("f", "json");

                httpClient.Url = "https://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN";
                httpClient.Context.Referer = "https://mp.weixin.qq.com/";

                string response = httpClient.GetString();

                Match match = mpTokenRegex.Match(response);

                ThrowExceptionUtil.ArgumentConditionTrue(match.Success, "", "模拟登入微信公众平台失败！");

                if (match.Success)
                {
                    mpToken = match.Groups["token"].Value;
                }
            }
        }


        public bool SendMessage(string openId, string content)
        {
            bool result = false;
            Init(false);

            Dictionary<string, string> postData = httpClient.PostingData;

            postData.Add("tofakeid", openId);
            postData.Add("token", MPToken);
            postData.Add("type", "1");
            postData.Add("content", content);
            postData.Add("ajax", "1");


            httpClient.Url = " https://mp.weixin.qq.com/cgi-bin/singlesend?t=ajax-response&lang=zh_CN ";
            httpClient.Context.Referer =
                "https://mp.weixin.qq.com/cgi-bin/singlesendpage?t=message/send&action=index&tofakeid={0}&token={1}&lang=zh_CN".FormatWith(openId, MPToken);


            string response = httpClient.GetString();

            Match match = wxGeneralResultRegext.Match(response);
            if (match.Success)
            {
                string ret = match.Groups["ret"].Value;
                string msg = match.Groups["msg"].Value;

                if (ret == "0")
                {
                    result = true;
                }
                else
                {
                    Init(true);
                }
            }

            ThrowExceptionUtil.ArgumentConditionTrue(result, "", "发送失败!");

            return result;
        }
    }
}
