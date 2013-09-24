using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bee.Web;
using Bee.WeiXin.Models;
using System.Data;
using Bee.Data;

namespace Bee.WeiXin.Controller
{
    public class InvokeTreeController : ControllerBase<WeiXinInvokeTree>
    {
        public override PageResult Index(BeeDataAdapter dataAdapter)
        {
            DataTable result;
            using (DbSession dbSession = GetDbSession())
            {
                result = dbSession.Query(TableName, null);
            }

            return View("Index", result);
        }

        public WeiXinInvokeTree Detail(int id)
        {
            WeiXinInvokeTree result = null;

            using (DbSession dbSession = GetDbSession())
            {
                result = dbSession.Query<WeiXinInvokeTree>(SqlCriteria.New.Equal("id", id)).FirstOrDefault();
            }

            return result;
        }

        public void Refresh()
        {
            InvokeTreeManager.Instance.Refresh();
        }

        protected override Bee.Data.DbSession GetDbSession(bool useTransaction)
        {
            return new Bee.Data.DbSession("WeiXin", useTransaction);
        }
    }
}
