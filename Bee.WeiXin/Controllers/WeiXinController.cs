using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bee.Web;
using Bee.Models;
using Bee.Util;
using Bee.Logging;
using Bee.Data;
using Bee.WeiXin.Models;

namespace Bee.WeiXin.Controller
{
    public class WeiXinController : WeiXinControllerBase
    {
        public ActionResult InvokeTreeText(string acontent)
        {
            return WeiXinText(acontent);
        }

        public ActionResult InvokeTreeArticle(string bcontent)
        {
            List<Article> result = null;
            using (DbSession dbSession = new DbSession(WeiXinConstants.WeiXinConnString))
            {
                List<WeiXinArticle> list = dbSession.Query<WeiXinArticle>(SqlCriteria.New.In("id", bcontent).Equal("delflag", 0));

                List<WeiXinArticle> articleList = new List<WeiXinArticle>();

                foreach (string item in bcontent.Split(','))
                {
                    WeiXinArticle article = (from articleItem in list where articleItem.Id.ToString() == item select articleItem).SingleOrDefault();

                    if(article != null)
                    {
                        articleList.Add(article);
                    }
                }

                result = (from item in articleList
                          select new Article()
                          {
                              Description = item.Description,
                              Title = item.Title,
                              PicUrl = string.Format("http://{0}{1}", HttpContext.Request.Url.Host, item.PicUrl),
                              Url = string.Format("http://{0}/WeiXin/ShowArticle.bee?id={1}&", HttpContext.Request.Url.Host, item.Id)
                          }).ToList();
                
            }

            return WeiXinArticle(result);
        }

        public PageResult ShowArticle(int id)
        {
            WeiXinArticle article = null;
            using (DbSession dbSession = new DbSession(WeiXinConstants.WeiXinConnString))
            {
                article = dbSession.Query<WeiXinArticle>(SqlCriteria.New.Equal("id", id)).FirstOrDefault();
            }

            if (article == null)
            {
                return null;
            }
            else
            {
                ViewData.Merge(BeeDataAdapter.From(article), false);
                return View("ShowArticle");
            }
        }

        public virtual ActionResult Text(string content)
        {
            int index = content.IndexOf("$校验码$");
            if (index > 0)
            {
                string code = content.Substring(index + 5);

                try
                {
                    string openId = SecurityUtil.DecryptS(code, ConfigUtil.GetAppSettingValue<string>("WeiXinToken").Substring(0, 8));

                    using(DbSession dbSession = new DbSession(WeiXinConstants.WeiXinConnString))
                    {
                        WeiXinUser weiXinUser = dbSession.Query<WeiXinUser>(SqlCriteria.New.Equal("openid", openId)).FirstOrDefault();
                        if(weiXinUser != null)
                        {
                            weiXinUser.FakeId = ViewData["FromUserName"].ToString();

                            dbSession.Save(weiXinUser);
                        }
                        else
                        {
                            Logger.Debug("绑定未成功!" + ViewData.ToString());
                            return WeiXinText("绑定未成功！");
                        }
                        
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("校验出错!" + ViewData.ToString(), e);
                    return WeiXinText("校验出错！");
                }

                return WeiXinText("绑定成功！");
            }
            
            return WeiXinDebug();
        }

        public virtual ActionResult Voice()
        {
            return WeiXinDebug();
        }

        public virtual ActionResult Image(string picurl)
        {
            return WeiXinDebug();
        }

        public virtual ActionResult Location(float location_x, float location_y, int scale, string label)
        {
            return WeiXinDebug();
        }

        public virtual ActionResult Link(string title, string description, string url)
        {
            return WeiXinDebug();
        }

        public virtual ActionResult Subscribe()
        {
            return WeiXinDebug();
        }

        public virtual ActionResult Unsubscribe()
        {
            return WeiXinDebug();
        }

        public virtual ActionResult Click(string eventKey)
        {
            return WeiXinDebug();
        }

        public void Send()
        {
            WeiXinManager.Instance.RefreshAll();
        }

        //public PageResult Link(string s)
        //{
        //    try
        //    {
        //        string openId = SecurityUtil.DecryptS(s, ConfigUtil.GetAppSettingValue<string>("WeiXinToken"));

        //        ViewData["result"] = "欢迎体验, 绑定成功。";
        //    }
        //    catch(Exception e)
        //    {
        //        Logger.Error("WeiXin.Link Error!\r\n" + ViewData.ToString(), e);

        //        ViewData["result"] = "无效的校验码， 绑定失败";
        //    }

        //    return View();
        //}
    }

    public class WeiXinControllerBase : ControllerBase
    {
        /// <summary>
        /// 输出调试消息
        /// </summary>
        /// <returns></returns>
        protected WeiXinTextResult WeiXinDebug()
        {
            WeiXinTextResult result = new WeiXinTextResult();

            return result;
        }

        /// <summary>
        /// 输出文本消息
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected WeiXinTextResult WeiXinText(string content)
        {
            WeiXinTextResult result = new WeiXinTextResult(content);

            return result;
        }

        /// <summary>
        /// 输出图文消息
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        protected WeiXinArticleResult WeiXinArticle(Article article)
        {
            WeiXinArticleResult result = new WeiXinArticleResult();
            result.Add(article);
            return result;
        }

        /// <summary>
        /// 输出图文消息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected WeiXinArticleResult WeiXinArticle(List<Article> list)
        {
            WeiXinArticleResult result = new WeiXinArticleResult();
            foreach (Article item in list)
            {
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// 输出音乐消息
        /// </summary>
        /// <param name="music"></param>
        /// <returns></returns>
        protected WeiXinMusicResult WeiXinMusic(Music music)
        {
            WeiXinMusicResult result = new WeiXinMusicResult();
            result.Music = music;

            return result;
        }

        /// <summary>
        /// 当前用户的调用链上下文
        /// </summary>
        protected InvokeContext Current
        {
            get
            {
                string fromUserName = ViewData.TryGetValue<string>("fromusername", string.Empty);


                return InvokeContextManager.Instance.CurrentContext(fromUserName);
            }
        }

        /// <summary>
        /// 当前请求
        /// </summary>
        protected RequestMessage CurrentRequest
        {
            get
            {
                return ConvertUtil.ConvertDataToObject<RequestMessage>(ViewData);
            }
        }

        protected void PushInvokeTree()
        {
            Current.MessageStack.Push(CurrentRequest);
        }

        /// <summary>
        /// 在调用链中， 如果用户输入的参数不符合， 请调用该函数。
        /// </summary>
        protected void PopInvokeTree()
        {
            Current.MessageStack.Pop();
        }
    }

}
