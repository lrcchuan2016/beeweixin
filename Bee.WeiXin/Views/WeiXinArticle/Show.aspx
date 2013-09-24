<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bee.Web.BeePageView" %>

<%@ Import Namespace="Bee.Web" %>
<%@ Import Namespace="Bee" %>
<%@ Import Namespace="Bee.Util" %>
<%@ Import Namespace="System.Collections.Generic" %>
<link href="/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
<div class="pageContent">
    <form id="content<%=PageId %>" action="<%=HtmlHelper.ForActionLink("save") %>" method="post"
    class="required-validate alertMsg">
    <%=HtmlHelper.ForHidden("id") %>
    <%=HtmlHelper.ForHidden("picurl") %>
    <%=HtmlHelper.ForHidden("CreateTime")%>
    <div class="pageFormContent" layouth="56">
        <div style="float: left; width: 500px;">
            <dl class="nowrap">
                <dt>标题</dt>
                <%=HtmlHelper.ForTextBox("title", "size=50", "class=required") %>
            </dl>
            <dl class="nowrap">
                <dt>摘要</dt>
                <%=HtmlHelper.ForTextBox("description", "size=50", "class=required") %>
            </dl>
        </div>
        <div style="float: left; margin-left: 20px;">
            <dl class="nowrap">
                <dt>封面</dt>
                <dd>
                    <input id="testFileInput" type="file" name="filedata" uploaderoption="{
                height:20,
			swf:'/uploadify/uploadify.swf',
			uploader:'/WeixinArticle/Upload.bee',
			formData:{ASPSESSID:'<%=(Session.SessionID.ToString()) %>', ajax:1},
			buttonText:'请选择文件',
			fileSizeLimit:'2000KB',
			fileTypeDesc:'*.jpg;*.jpeg;*.png;',
			fileTypeExts:'*.jpg;*.jpeg;*.png;',
			auto:true,
			multi:false,
			onUploadSuccess:uploaded<%=PageId %>
		}" />
                    <img id="img<%=PageId %>" width="320" height="160" src="<%=ViewData["picurl"] %>" />
                </dd>
            </dl>
        </div>
        <dl class="nowrap">
            <dt>正文</dt>
            <textarea name="content" style="width: 98%" rows="25" cols="50" class="editor" upimgurl="/xheditor/server/upload.aspx"
                upimgext="jpg,jpeg,gif,png"><%=ViewData["content"] %></textarea>
        </dl>
    </div>
    <div class="formBar">
        <ul>
            <li><a class="button" href="javascript:" onclick="javascript:autoSave('content<%=PageId %>');">
                <span>保存</span> </a></li>
            <li><a class="button close" href="javascript:"><span>取消</span> </a></li>
        </ul>
    </div>
    </form>
</div>

<script type="text/javascript">

    var $form = $("#content<%=PageId %>");

     var uploaded<%=PageId %> = function(file, data, response){
        debugger;
        var json = bee.jsonEval(data);
        if(!json.result){
            alertMsg.warn(json.msg);
        }
        else{           
            $form.find("#img<%=PageId %>").attr("src", json.result);
            $form.find("input[name=picurl]").val(json.result);
        }
    }

</script>

