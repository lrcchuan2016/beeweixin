<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bee.Web.BeePageView" %>

<%@ Import Namespace="Bee.Web" %>
<%@ Import Namespace="Bee" %>
<%@ Import Namespace="System.Collections.Generic" %>
<div class="pageContent">
	<form method="post" action="<%=HtmlHelper.ForActionLink("SendMessage") %>" class="required-validate" id="content<%=PageId %>">
	<%=HtmlHelper.ForHidden("openid") %>
		<div class="pageFormContent" layoutH="48">
			<p class="nowrap">
			    <label>内容</label>
			    <textarea name="content" rows="10" cols="55"></textarea>
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

