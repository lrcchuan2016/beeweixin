using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bee.Web;
using System.Xml;
using Bee.Logging;
using Bee.Util;

namespace Bee.WeiXin
{
    public class WeiXinDispatcher : MvcDispatcher
    {
        private static readonly string MainControllerName = ConfigUtil.GetAppSettingValue<string>("WeiXinController");

        protected override BeeDataAdapter GetRouteData(System.Web.HttpContext context)
        {
            XmlDocument document = new XmlDocument();

            document.Load(context.Request.InputStream);

            BeeDataAdapter dataAdapter = new BeeDataAdapter();
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                dataAdapter.Add(node.Name, node.InnerText);
            }

            string msgType = dataAdapter.TryGetValue<string>("msgtype", string.Empty);

            if (string.Compare("event", msgType, true) == 0)
            {
                string eventName = dataAdapter.TryGetValue<string>("event", string.Empty);

                dataAdapter.Add(Constants.BeeControllerName, MainControllerName);
                dataAdapter.Add(Constants.BeeActionName, eventName);
            }
            else
            {
                dataAdapter.Add(Constants.BeeControllerName, MainControllerName);
                dataAdapter.Add(Constants.BeeActionName, msgType);
            }

            Logger.Debug(dataAdapter.ToString());

            // 实现一个调用链
            /*
             * menuid-1-2-3 
             * 
             * 
             */

            InvokeTreeManager.Instance.Check(dataAdapter);

            return dataAdapter;
        }
    }
}
