<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bee.Web.BeePageView" %>

<%@ Import Namespace="Bee.Web" %>
<%@ Import Namespace="Bee" %>
<%@ Import Namespace="System.Collections.Generic" %>
<form id='pageForm<%=PageId %>' action="<%=HtmlHelper.ForActionLink() %>" method="post">
<div class="formBar">
    <ul style="float: left; margin-right: 350px;">
        <li><a class="button"  onclick="javascript:Get<%=PageId %>();">
            <span>获取菜单</span> </a></li>

        <li><a class="button close" href="javascript:"><span>取消</span> </a></li>
    </ul>
    <ul style="float: left;">
        <li><a class="button" onclick="javascript:Save<%=PageId %>();">
            <span>保存菜单</span> </a></li>
        <li><a class="button" href="/WeiXinMenu/DeleteMenu.bee" target="ajaxTodo" title="你确认要删除菜单？"><span>删除</span>
        </a></li>
    </ul>
</div>
</form>

<div class="pageFormContent" layouth="56">
    <form id='content<%=PageId %>' action="<%=HtmlHelper.ForActionLink("PostMenu") %>" method="post"
    class="required-validate alertMsg">
    
    <p class="nowrap">
        <label>
            菜单内容：</label>
        <textarea name="menu" cols="100" rows="20"></textarea>
    </p>
        <p class="nowrap">
        <span id="weiXinMenuResult"></span>
        </p>
   
    </form>
</div>

<script type="text/javascript">

    var Get<%=PageId %> = function(){
         bee.PostData("/Weixinmenu/GetMenu.bee", null, function(data) {
            var $form = $("#content<%=PageId %>");
            $form.find("[name=menu]").val(data);

        });
    }
    
    function Save<%=PageId %>(id) {
        var $form = $("#content<%=PageId %>");
        bee.PostData("/WeiXinMenu/PostMenu.bee", $form.serializeArray(), function(data) {
            $form.find("#weiXinMenuResult").html(data);
        });
    }
</script>

