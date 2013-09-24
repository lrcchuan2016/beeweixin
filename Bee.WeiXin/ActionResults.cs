using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bee.Web;
using Bee.Models;

namespace Bee.WeiXin
{

    public class WeiXinTextResult : ActionResult
    {
        public WeiXinTextResult()
        {
        }

        public WeiXinTextResult(string content)
        {
            this.Content = content;
        }

        public string Content
        {
            get;
            set;

        }

        public override void Ouput(System.Web.HttpContext context)
        {

            string toUserName = dataAdapter.TryGetValue<string>("fromusername", string.Empty);
            string fromUserName = dataAdapter.TryGetValue<string>("tousername", string.Empty);

            string content = string.Empty;
            if (string.IsNullOrEmpty(Content))
            {
                Bee.WeiXin.InvokeContext invokeContext = Bee.WeiXin.InvokeContextManager.Instance.CurrentContext(toUserName);

                string invokeContextString = string.Join(",", (from item in invokeContext.MessageStack select item.Command).ToArray());

                content = string.Format("{0}\r\n{1}", dataAdapter.ToString(), invokeContextString);
            }
            else
            {
                content = Content;
            }

            XmlBuilder builder = XmlBuilder.New.tag("xml");


            builder = builder.tag("ToUserName").include(toUserName, true).endTag();
            builder = builder.tag("FromUserName").include(fromUserName, true).end;
            builder = builder.tag("MsgType").include("text", true).end;
            builder = builder.tag("CreateTime").include(DateTimeUtil.GetWeixinDateTime(DateTime.Now).ToString(), true).end;
            builder = builder.tag("Content").include(content, true).end;

            builder = builder.end;

            Bee.Logging.Logger.Debug(builder.ToString());

            context.Response.Write(builder.ToString());

        }
    }


    public class WeiXinArticleResult : ActionResult
    {
        private List<Article> list = new List<Article>();

        public void Add(Article article)
        {
            if (list.Count <= 9)
            {
                list.Add(article);
            }
        }

        public void Remain(int length)
        {
            if(length <= 0)
            {
                return;
            }
            if (list.Count > length)
            {
                list.RemoveRange(length, list.Count - length);
            }
        }

        public override void Ouput(System.Web.HttpContext context)
        {
            string toUserName = dataAdapter.TryGetValue<string>("fromusername", string.Empty);
            string fromUserName = dataAdapter.TryGetValue<string>("tousername", string.Empty);

            XmlBuilder builder = XmlBuilder.New.tag("xml");

            builder = builder.tag("ToUserName").include(toUserName, true).end;
            builder = builder.tag("FromUserName").include(fromUserName, true).end;
            builder = builder.tag("MsgType").include("news", true).end;
            builder = builder.tag("CreateTime").include(DateTimeUtil.GetWeixinDateTime(DateTime.Now).ToString(), true).end;
            builder = builder.tag("ArticleCount").include(list.Count.ToString(), true).end;

            builder = builder.tag("Articles");

            foreach (Article item in list)
            {
                builder = builder.tag("item")
                    .tag("Title").include(item.Title, true).end
                    .tag("Description").include(item.Description, true).end
                    .tag("PicUrl").include(item.PicUrl, true).end
                    .tag("Url").include(item.Url, true).end
                    .end;

            }

            builder = builder.end;

            builder = builder.end;

            context.Response.Write(builder.ToString());
        }
    }

    public class WeiXinMusicResult : ActionResult
    {
        public Music Music
        {
            get;
            set;
        }

        public override void Ouput(System.Web.HttpContext context)
        {
            string toUserName = dataAdapter.TryGetValue<string>("fromusername", string.Empty);
            string fromUserName = dataAdapter.TryGetValue<string>("tousername", string.Empty);

            XmlBuilder builder = XmlBuilder.New.tag("xml");

            builder = builder.tag("ToUserName").include(toUserName, true).end;
            builder = builder.tag("FromUserName").include(fromUserName, true).end;
            builder = builder.tag("MsgType").include("music", true).end;
            builder = builder.tag("CreateTime").include(DateTimeUtil.GetWeixinDateTime(DateTime.Now).ToString(), true).end;

            if (Music != null)
            {
                builder = builder.tag("Music")
                    .tag("Title").include(Music.Title, true).end
                    .tag("Description").include(Music.Description, true).end
                    .tag("MusicUrl").include(Music.MusicUrl, true).end
                    .tag("HQMusicUrl").include(Music.HQMusicUrl, true).end
                    .end;
            }


            builder = builder.end;

            context.Response.Write(builder.ToString());
        }
    }
}
