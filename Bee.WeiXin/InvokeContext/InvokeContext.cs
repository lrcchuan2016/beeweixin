using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bee.WeiXin
{
    public class InvokeContext
    {
        public InvokeContext()
        {
            MessageStack = new Stack<RequestMessage>();
        }

        public string UserName { get; set; }
        public DateTime LastActive { get; set; }
        public Stack<RequestMessage> MessageStack
        {
            get;
            set;
        }
    }
}
