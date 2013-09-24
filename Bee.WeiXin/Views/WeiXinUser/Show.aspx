<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bee.Web.BeePageView" %>

<%@ Import Namespace="Bee.Web" %>
<%@ Import Namespace="Bee" %>
<%@ Import Namespace="Bee.Util" %>
<%@ Import Namespace="System.Collections.Generic" %>

<div class="pageContent">
	<form id="content<%=PageId %>">
	<%=HtmlHelper.ForHidden("id") %>
		<div class="pageFormContent" layoutH="56">
			<p>
			    <label>微信昵称</label>
			    <%=ViewData["nickname"] %>
			</p>
			<p>
			    <label>备注名</label>
			    <%=ViewData["remarkname"] %>
			</p>
			<p>
			    <label>用户组</label>
			    <%=HtmlHelper.ForDataMapping("weixingroup", ViewData["groupid"]) %>
			</p>
		</div>
		<div class="formBar">
			<ul>
				<li> <a class="button" href="javascript:" onclick="javascript:autoSave('content<%=PageId %>');"><span>发送</span> </a></li>
				<li> <a class="button close" href="javascript:"><span>取消</span> </a></li>
			</ul>
		</div>
	</form>
</div>