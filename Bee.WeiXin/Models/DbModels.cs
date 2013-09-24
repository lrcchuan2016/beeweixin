using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bee.Web;

namespace Bee.WeiXin.Models
{
    public enum WeiXinInvokeType
    {
        Text = 0,
        Article = 1,
        Customer = 2
    }


    [Serializable]
    public class WeiXinInvokeTree
    {
        #region Properties

        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Title { get; set; }
        public Int32 ParentId { get; set; }
        public String ControllerName { get; set; }
        public String ActionName { get; set; }
        public string AContent { get; set; }
        public string BContent { get; set; }
        public Boolean DelFlag { get; set; }
        public Boolean RemainFlag { get; set; }
        public DateTime CreateTime { get; set; }

        public WeiXinInvokeType InvokeType
        {
            get
            {
                WeiXinInvokeType result = WeiXinInvokeType.Text;

                if (!string.IsNullOrEmpty(ControllerName) &&
                    !string.IsNullOrEmpty(ActionName))
                {
                    result = WeiXinInvokeType.Customer;
                }

                if (!string.IsNullOrEmpty(BContent))
                {
                    result = WeiXinInvokeType.Article;
                }
                if (!string.IsNullOrEmpty(AContent))
                {
                    result = WeiXinInvokeType.Text;
                }

                return result;
            }
        }

        #endregion

    }

    [Serializable]
    public class WeiXinUser
    {
        #region Properties

        public Int32 Id { get; set; }
        public String OpenId { get; set; }
        [ModelProperty(Queryable = true, QueryType = ModelQueryType.Contains)]
        public String NickName { get; set; }
        [ModelProperty(Queryable = true, QueryType = ModelQueryType.Contains)]
        public String RemarkName { get; set; }

        [ModelProperty(MappingName = "WeiXinGroup",
           Queryable = true, QueryType = ModelQueryType.Equal)]
        public String GroupId { get; set; }
        public string FakeId { get; set; }
        [ModelProperty(
           Queryable = true, QueryType = ModelQueryType.Between)]
        public DateTime CreateTime { get; set; }

        [ModelProperty(MappingName = "是否",
           Queryable = true, QueryType = ModelQueryType.Equal)]
        public Boolean ValidFlag { get; set; }

        public String UserName { get; set; }
        public String Country { get; set; }
        public String City { get; set; }
        public String Province { get; set; }
        [ModelProperty(MappingName = "SexInfo")]
        public Int32 Sex { get; set; }
        public String Signature { get; set; }

        #endregion

    }


    [Serializable]
    public class WeiXinGroup
    {
        #region Properties

        public Int32 Id { get; set; }
        public String GroupId { get; set; }
        public String Name { get; set; }
        public Int32 LastCount { get; set; }
        public Int32 CurrentCount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        #endregion

    }


    [Serializable]
    public class WeiXinArticle
    {
        #region Properties

        public Int32 Id { get; set; }
        [ModelProperty(Queryable = true, QueryType = ModelQueryType.Contains)]
        public String Title { get; set; }
        public String Description { get; set; }
        public String PicUrl { get; set; }
        public String Content { get; set; }
        [ModelProperty(
   Queryable = true, QueryType = ModelQueryType.Equal)]
        public Boolean DelFlag { get; set; }
        [ModelProperty(
   Queryable = true, QueryType = ModelQueryType.Between)]
        public DateTime CreateTime { get; set; }

        #endregion

    }

}
