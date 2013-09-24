using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bee.Service;

namespace Bee.WeiXin
{
    public class InvokeContextManager
    {
        private static InvokeContextManager instance = new InvokeContextManager();

        private List<InvokeContext> list = new List<InvokeContext>();

        private InvokeContextManager()
        {
            ServiceManager.Instance.AppendTask(new InvokeContextService());
        }

        public static InvokeContextManager Instance
        {
            get
            {
                return instance;
            }
        }

        public InvokeContext CurrentContext(string fromUser)
        {
            lock (this)
            {
                InvokeContext result = (from item in list where item.UserName == fromUser select item).SingleOrDefault();
                if (result == null)
                {
                    result = new InvokeContext() { UserName = fromUser, LastActive = DateTime.Now };

                    list.Add(result);
                }
                else
                {
                    result.LastActive = DateTime.Now;
                }

                return result;
            }
        }

        internal void CheckContext()
        {
            lock (this)
            {
                List<InvokeContext> needRemovedList = new List<InvokeContext>();
                foreach (InvokeContext item in list)
                {
                    if (item.LastActive < DateTime.Now.AddMinutes(-1))
                    {
                        needRemovedList.Add(item);
                    }
                }

                foreach (InvokeContext item in needRemovedList)
                {
                    list.Remove(item);
                }
            }
        }
    }

    public class InvokeContextService : BaseRunService
    {
        protected override void Run()
        {
            InvokeContextManager.Instance.CheckContext();
        }
    }
}
