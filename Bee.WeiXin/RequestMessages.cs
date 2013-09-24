using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bee.WeiXin
{
    public class RequestMessage
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public DateTime CreateTime { get; set; }
        public RequestMsgType MsgType
        {
            get;
            set;
        }
        public long MsgId { get; set; }

        #region Text Message

        public string Content { get; set; }

        #endregion

        #region Location Message

        public double Location_X
        {
            get;
            set;
        }

        public double Location_Y
        {
            get;
            set;
        }

        public int Scale
        {
            get;
            set;
        }
        public string Label
        {
            get;
            set;
        }

        #endregion

        #region Link Message

        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        #endregion

        #region Image Mesage

        public string PicUrl { get; set; }

        #endregion

        #region Voice Message

        public string MediaId{get;set;}
        public string Format{get;set;}
        public string Recognition{get;set;}

        #endregion

        #region Event Message

        public EventType Event { get; set; }
        public String EventKey { get; set; }

        #endregion

        public string Command
        {
            get
            {
                string result = string.Empty;
                if (MsgType == RequestMsgType.Event && Event == EventType.Click)
                {
                    result = EventKey;
                }
                else if (MsgType == RequestMsgType.Text)
                {
                    result = Content;
                }
                else if (Event == EventType.Click)
                {
                    result = EventKey;
                }
                else
                {
                    // do nothing here.
                }

                return result;
            }
        }

        public string InvokeTreeName
        {
            get;
            set;
        }
    }

    //public class RequestMessageText : RequestMessageBase
    //{
    //    public override RequestMsgType MsgType
    //    {
    //        get { return RequestMsgType.Text; }
    //    }
    //    public string Content { get; set; }
    //}

    //public class RequestMessageLocation : RequestMessageBase
    //{
    //    public override RequestMsgType MsgType
    //    {
    //        get
    //        {
    //            return RequestMsgType.Location;
    //        }
    //    }

    //    public float Location_X
    //    {
    //        get;
    //        set;
    //    }

    //    public float Location_Y
    //    {
    //        get;
    //        set;
    //    }

    //    public int Scale
    //    {
    //        get;
    //        set;
    //    }
    //    public string Label
    //    {
    //        get;
    //        set;
    //    }
    //}

    //public class RequestMessageLink : RequestMessageBase
    //{
    //    public override RequestMsgType MsgType
    //    {
    //        get 
    //        { 
    //            return RequestMsgType.Link;
    //        }
    //    }
    //    public string Title { get; set; }
    //    public string Description { get; set; }
    //    public string Url { get; set; }
    //}

    //public class RequestMessageImage : RequestMessageBase
    //{
    //    public override RequestMsgType MsgType
    //    {
    //        get { return RequestMsgType.Image; }
    //    }
    //    public string PicUrl { get; set; }
    //}


    //public class RequestMessageEvent : RequestMessageBase
    //{
    //    public override RequestMsgType MsgType
    //    {
    //        get { return RequestMsgType.Event; }
    //    }
    //    public EventType EventType { get; set; }
    //    public String EventKey { get; set; }
    //}
}
