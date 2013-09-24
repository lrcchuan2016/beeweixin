<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bee.Web.BeePageView" %>

<%@ Import Namespace="Bee.Web" %>
<%@ Import Namespace="Bee" %>
<%@ Import Namespace="System.Collections.Generic" %>
<form id='pageForm<%=PageId %>' action="<%=HtmlHelper.ForActionLink() %>" method="post">
<div class="formBar">
    <ul style="float: left; margin-right: 350px;">
        <li><a class="button" href="javascript:" onclick="javascript:Create<%=PageId %>();">
            <span>创建子节点</span> </a></li>
        <li><a class="button" target="ajaxTodo" title="是否确认刷新微信调用链缓存" href="/InvokeTree/Refresh.bee">
            <span>刷新缓存</span> </a></li>
        <li><a class="button close" href="javascript:"><span>取消</span> </a></li>
    </ul>
    <ul style="float: left;">
        <li><a class="button" href="javascript:" onclick="javascript:autoSave('content<%=PageId %>');">
            <span>保存</span> </a></li>
        <li><a class="button" href="javascript:" onclick="javascript:Delete();"><span>删除</span>
        </a></li>
    </ul>
</div>
</form>
<% System.Data.DataTable dataTable = Model as System.Data.DataTable;  %>
<div style="float: left; display: block; margin: 10px; overflow: auto; width: 350px;
    border: solid 1px #CCC; line-height: 21px; background: #FFF;" layouth="56">
    <ul id="tree1" class="tree treeFolder expand" oncheck="Edit">
        <%=HtmlHelper.ForTree(dataTable, "parentid", "title", "id", "name asc", 0, "<ul>", "</ul>", 
            "<li><a tvalue={1} onclick='Edit" + PageId +"({1});'>{0}</a>", "</li>")%>
    </ul>
</div>
<div class="pageFormContent" layouth="56">
    <form id='content<%=PageId %>' action="<%=HtmlHelper.ForActionLink("save") %>" method="post"
    class="required-validate alertMsg">
    <p>
        <label>
            上级节点编号：</label>
        <input name="parentid" type='text' size='30' class="required" title="上级权限编号不能为空" />
    </p>
    <p>
        <label>
            节点编号：</label>
        <input name="id" type='text' size='30' readonly="readonly" />
    </p>
    <p>
        <label>
            节点标题：</label>
        <input name="title" type='text' size='30' class="required" title="节点标题不能为空" />
    </p>
    <p>
        <label>
            节点值：</label>
        <input name="name" type='text' size='30' class="required" title="节点名称不能为空" />
    </p>
    <p>
        回复类型：<input type="radio" name="invoketype" value="0" />
        文本 &nbsp;
        <input type="radio" name="invoketype" value="1" />
        图文&nbsp;
        <input type="radio" name="invoketype" value="2" />自定义&nbsp;
    </p>
    <p class="customInvoke">
        <label>
            ControllerName：</label>
        <input name="controllername" type='text' size='30' />
    </p>
    <p class="customInvoke">
        <label>
            ActionName：</label>
        <input name="actionname" type='text' size='30' maxlength="150" />
    </p>
    <p class="textInvoke">
        <span>文本回复内容：（请直接填入文本内容）</span></p>
    <p class="nowrap textInvoke">
        <%--<input name="acontent" type='text' size='30' maxlength="150" />--%>
        <textarea name="acontent" cols="50" rows="5"></textarea>
    </p>
    <p class="articleInvoke">
        图文文内容：（请填入微信文章的编号， 并以,分隔开）</p>
    <p class="nowrap articleInvoke">
        <%--<input name="acontent" type='text' size='30' maxlength="150" />--%>
        <textarea name="bcontent" cols="50" rows="5"></textarea>
    </p>
    <p>
        <label>
            是否进入调用链：</label>
        <select name="remainflag">
            <option value="false">false</option>
            <option value="true">true</option>
        </select>
    </p>
    <p>
        <label>
            是否已删除：</label>
        <select name="delflag">
            <option value="false">false</option>
            <option value="true">true</option>
        </select>
    </p>
    <p>
        <label>
            创建时间：</label>
        <input name="createtime" type='text' size='30' readonly='readonly' />
    </p>
    </form>
</div>

<script type="text/javascript">

$(function(){
var $form = $("#content<%=PageId %>");
 $(".textInvoke, .articleInvoke, .customInvoke", $form).hide();
 
 $(":input[name=invoketype]", $form).change(function(){
    changeInvokeType<%=PageId %>($form, $(this).val());
 });
 
});

    var changeInvokeType<%=PageId %> = function($form, invokeType){
        
        $(".textInvoke, .articleInvoke, .customInvoke", $form).hide();
            if(invokeType == "0"){
                $(".textInvoke", $form).show();
                $("input[name=controllername]", $form).val("");
                $("input[name=actionname]", $form).val("");
                $("input[name=bcontent]", $form).val("");
            }
            else if(invokeType == "1"){
                $(".articleInvoke", $form).show();
                $("input[name=controllername]", $form).val("");
                $("input[name=actionname]", $form).val("");
                $(":input[name=acontent]", $form).val("");
            }
            else{
                $(".customInvoke", $form).show();
                $(":input[name=acontent]", $form).val("");
                $(":input[name=bcontent]", $form).val("");
            }
    }

    var Create<%=PageId %> = function(){
        var $form = $("#content<%=PageId %>");
        var id = $("input[name='id']", $form).val();
        if (id != "") {
            $form[0].reset();
            $("input[name='parentid']", $form).val(id);
        }
    }
    
    function Edit<%=PageId %>(id) {
        bee.PostData("/InvokeTree/Detail.bee", { id: id }, function(data) {
            var $form = $("#content<%=PageId %>");
            autoFill($form, data);
            var invokeType = data["invoketype"];
           
            changeInvokeType<%=PageId %>($form, invokeType);

        });
    }

    function Delete() {
        var $form = $("#content<%=PageId %>");
        var id = $("input[name='id']", $form).val();
        if (id != "") {
            alertMsg.confirm("是否删除？", {
                okCall: function() {
                    bee.PostData("/InvokeTree/Delete.bee", { id: id }, function() { autoList(); });
                }
            });
        }
    }
</script>

