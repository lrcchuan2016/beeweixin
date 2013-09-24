<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bee.Web.BeePageView" %>

<%@ Import Namespace="Bee.Web" %>
<%@ Import Namespace="Bee" %>
<%@ Import Namespace="System.Collections.Generic" %>
<xml>
 <ToUserName><![CDATA[<%=ViewData["FromUserName"] %>]]></ToUserName>
 <FromUserName><![CDATA[<%=ViewData["ToUserName"] %>]]></FromUserName>
 <CreateTime><%=Bee.WeiXin.DateTimeUtil.GetWeixinDateTime(DateTime.Now) %></CreateTime>
 <MsgType><![CDATA[text]]></MsgType>
 <Content>回复1 联系方式 
 回复2 在线咨询</Content>
</xml>
