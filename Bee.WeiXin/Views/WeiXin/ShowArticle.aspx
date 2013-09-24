<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bee.Web.BeePageView" %>

<%@ Import Namespace="Bee.Web" %>
<%@ Import Namespace="Bee" %>
<%@ Import Namespace="Bee.Util" %>
<%@ Import Namespace="System.Collections.Generic" %>
<!DOCTYPE html>
<html>
<head>
    <title><%=ViewData["title"] %></title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="format-detection" content="telephone=no">
    <link rel="stylesheet" type="text/css" href="http://res.wx.qq.com/mmbizwap/zh_CN/htmledition/style/client-page17e52c.css" />
    <style>
        #nickname
        {
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
            max-width: 90%;
        }
        ol, ul
        {
            list-style-position: inside;
        }
    </style>
    <style>
        #activity-detail .page-content .text
        {
            font-size: 16px;
        }
    </style>
    

</head>
<body id="activity-detail">
    <div class="page-bizinfo">
        <div class="header">
            <h1 id="activity-name">
                <%=ViewData["title"]  %></h1>
            <p class="activity-info">
                <span id="post-date" class="activity-meta no-extra"><%=ViewData["createtime"] %></span>  <a href="javascript:;"
                    id="post-user" class="activity-meta"><span class="text-ellipsis"></span><i class="icon_link_arrow"></i></a></p>
                    
                    <a id="hiddenTagA" style="display:none;"></a>
        </div>
    </div>
    <div class="page-content">
        <div class="media" id="media">
            <img src="<%=ViewData["picurl"] %>"/></div>
        <div class="text">
           <%=HttpUtility.HtmlDecode(ViewData["content"].ToString()) %>
        </div>

    </div>

    <script src="http://res.wx.qq.com/mmbizwap/zh_CN/htmledition/js/jquery-1.8.3.min176ed4.js"></script>

    <script src="http://res.wx.qq.com/mmbizwap/zh_CN/htmledition/js/wxm-core176ed4.js"></script>
</body>
</html>
<!-- ver 1.4 Build01 -->
