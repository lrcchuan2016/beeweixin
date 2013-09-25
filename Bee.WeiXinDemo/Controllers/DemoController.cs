using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bee.WeiXin;
using Bee.WeiXin.Controller;
using Bee.Web;

namespace Bee.WeiXinDemo.Controllers
{
    public class DemoController : WeiXinController
    {
        public override ActionResult Subscribe()
        {
            return WeiXinText("感谢关注！");
        }

        public WeiXinTextResult CheckCountry()
        {
            string content = ViewData.TryGetValue<string>("content", string.Empty);

            // 验证输入的国家是否合法
            bool validFlag = content.IndexOf("CN") >= 0;

            if (validFlag)
            {
                return WeiXinText("1 最近3个月销售统计\r\n2 最近6个月销售统计");
            }
            else
            {
                // 由于调用链有上下文， 用户输入错误， 需要将当前应答取消。Current.MessageStack.Pop();
                Current.MessageStack.Pop();
                return WeiXinText("国家不合法，请重新输入国家。");
            }
        }

        public ActionResult SaleInfo()
        {
            string invokeContextString = string.Join(",", (from item in Current.MessageStack select item.Command).ToArray());
            return WeiXinText(invokeContextString + "最近6个月销售统计");
        }

        public ActionResult Other()
        {
            return View();
        }

        public ActionResult Robot()
        {
            return WeiXinText("我还很笨， 不知道说什么！");
        }
    }
}
