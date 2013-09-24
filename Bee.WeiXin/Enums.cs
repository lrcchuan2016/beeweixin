using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bee.WeiXin
{
    /// <summary>
    /// 接收消息类型
    /// </summary>
    public enum RequestMsgType
    {
        Text, //文本
        Location, //地理位置
        Image, //图片
        Voice, //语音
        Link, //连接信息
        Event, //事件推送
    }

    public enum EventType
    {
        None,
        Subscribe,
        Unsubscribe,
        Click
    }

    /// <summary>
    /// 发送消息类型
    /// </summary>
    public enum ResponseMsgType
    {
        Text,
        News,
        Music
    }
}
