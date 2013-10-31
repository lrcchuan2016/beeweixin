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
using Bee.Logging;
using Bee.Core;
using System.Data;

namespace Bee.Controllers
{
    public class WeiXinUserController : ControllerBase<WeiXinUser>
    {
        static WeiXinUserController()
        {
            DataMapping.Instance.Register("WeiXinGroup", () =>
            {
                using (DbSession dbSession = new DbSession(WeiXinConstants.WeiXinConnString))
                {
                    return dbSession.ExecuteCommand("select groupid as id, name from weixingroup", null);
                }
            });
        }

        public override PageResult Index(BeeDataAdapter dataAdapter)
        {
            DbSession dbSession = GetDbSession();

            DataTable dataTable = null;
            try
            {
                InitPagePara(dataAdapter);

                EntityProxy<WeiXinUser> entityProxy = EntityProxyManager.Instance.GetEntityProxy<WeiXinUser>();

                SqlCriteria sqlCriteria = GetQueryCondition(dataAdapter);

                if (dataAdapter.ContainsKey("linkflag"))
                {
                    bool linkFlag = dataAdapter.TryGetValue<Boolean>("linkflag", false);
                    if (!linkFlag)
                    {
                        sqlCriteria.Equal("fakeid", "");
                    }
                    else
                    {
                        sqlCriteria.NotEqual("fakeid", "");
                    }
                }

                string selectClause = GetQuerySelectClause(typeof(WeiXinUser));

                dataTable = InnerQuery("WeiXinUser", selectClause, dataAdapter, sqlCriteria);
            }
            catch (Exception e)
            {
                Logger.Error("List object({0}) Error".FormatWith(typeof(WeiXinUser)), e);
            }
            finally
            {
                dbSession.Dispose();
            }

            return View(dataTable);
        }

        public override PageResult Show(int id)
        {
            return View(base.Show(id).Model);
        }

        public void SynchronizeAll()
        {
            WeiXinManager.Instance.RefreshAll();
        }

        public PageResult SendMessage(string openId)
        {
            return View();
        }

        public void SendMessage(string openId, string content)
        {
            content = HttpUtility.HtmlDecode(content);
            bool result = WeiXinManager.Instance.SendMessage(openId, content);

            ThrowExceptionUtil.ArgumentConditionTrue(result, "", "发送失败！");
        }

        public void Link(string openId)
        {
            bool result = WeiXinManager.Instance.SendMessage(openId, "邀请您体验推送功能， 请复制该消息并回复。$校验码${0}".FormatWith(SecurityUtil.EncryptS(openId, ConfigUtil.GetAppSettingValue<string>("WeiXinToken").Substring(0, 8))));

            ThrowExceptionUtil.ArgumentConditionTrue(result, "", "发送失败！");
        }

        protected override Bee.Data.DbSession GetDbSession(bool useTransaction)
        {
            return new Bee.Data.DbSession(WeiXinConstants.WeiXinConnString, useTransaction);
        }
    }
}
