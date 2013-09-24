using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bee.Data;
using System.Data;
using Bee.Util;
using Bee.Core;
using Bee.WeiXin.Models;

namespace Bee.WeiXin
{
    public class InvokeTreeManager
    {
        private static InvokeTreeManager instance = new InvokeTreeManager();

        private List<WeiXinInvokeTree> invokeTreeList = new List<WeiXinInvokeTree>();

        private InvokeTreeManager()
        {
            Refresh();
        }

        public static InvokeTreeManager Instance
        {
            get
            {
                return instance;
            }
        }

        public void Refresh()
        {
            using (DbSession dbSession = new DbSession(WeiXinConstants.WeiXinConnString))
            {
                invokeTreeList = dbSession.Query<WeiXinInvokeTree>(SqlCriteria.New.Equal("delflag", false));
            }

        }

        private WeiXinInvokeTree FindChild(int parentId, string command)
        {
            return (from invokeItem in invokeTreeList
                    where invokeItem.ParentId == parentId && invokeItem.Name == command
                    select invokeItem).FirstOrDefault();
        }

        private WeiXinInvokeTree FindCurrentInvoke(BeeDataAdapter dataAdapter)
        {
            WeiXinInvokeTree result = null;

            string fromUserName = dataAdapter.TryGetValue<string>("fromusername", string.Empty);
            InvokeContext currentContext = InvokeContextManager.Instance.CurrentContext(fromUserName);

            // 调用链目前仅仅关心到菜单起始的文本输入链
            string currentCommand = dataAdapter.TryGetValue<string>("content", string.Empty);
            if (string.IsNullOrEmpty(currentCommand))
            {
                currentCommand = dataAdapter.TryGetValue<string>("eventkey", string.Empty);
            }

            string eventName = dataAdapter.TryGetValue<string>("event", string.Empty);
            if (string.Compare("click", eventName, true) == 0)
            {
                currentContext.MessageStack.Clear();
            }

            WeiXinInvokeTree parentInvokeTree = null;
            int parentId = 0;

            List<string> list = (from item in currentContext.MessageStack select item.InvokeTreeName).ToList();
            list.Reverse();

            foreach (string item in list)
            {
                parentInvokeTree = FindChild(parentId, item);
                if (parentInvokeTree != null)
                {
                    parentId = parentInvokeTree.Id;
                }
                else
                {
                    break;
                }
            }

            if (parentInvokeTree != null)
            {
                parentId = parentInvokeTree.Id;
            }

            result = FindChild(parentId, currentCommand);

            if (result == null)
            {
                result = FindChild(parentId, "*");
            }


            if (result == null)
            {
                result = parentInvokeTree;
            }
            else
            {
                dataAdapter["createtime"] = DateTimeUtil.GetDateTimeFromXml(dataAdapter["createtime"].ToString());

                RequestMessage message = ConvertUtil.ConvertDataToObject<RequestMessage>(dataAdapter);
                message.InvokeTreeName = result.Name;

                currentContext.MessageStack.Push(message);
            }

            return result;
        }

        public void Check(BeeDataAdapter dataAdapter)
        {
            WeiXinInvokeTree invokeTree = FindCurrentInvoke(dataAdapter);
            if (invokeTree == null)
            {

            }
            else
            {
                if (!string.IsNullOrEmpty(invokeTree.AContent))
                {
                    dataAdapter[Constants.BeeActionName] = "InvokeTreeText";
                    dataAdapter["acontent"] = System.Web.HttpUtility.HtmlDecode(invokeTree.AContent);
                }
                else if (!string.IsNullOrEmpty(invokeTree.BContent))
                {
                    dataAdapter[Constants.BeeActionName] = "InvokeTreeArticle";
                    dataAdapter["bcontent"] = System.Web.HttpUtility.HtmlDecode(invokeTree.BContent);
                }
                else
                {
                    if (!string.IsNullOrEmpty(invokeTree.ControllerName) && !string.IsNullOrEmpty(invokeTree.ActionName))
                    {
                        dataAdapter[Constants.BeeControllerName] = invokeTree.ControllerName;
                        dataAdapter[Constants.BeeActionName] = invokeTree.ActionName;
                    }
                }

                if(!invokeTree.RemainFlag)
                {
                    InvokeContextManager.Instance.CurrentContext(dataAdapter["fromusername"].ToString()).MessageStack.Pop();
                }
            }
        }
    }


}
