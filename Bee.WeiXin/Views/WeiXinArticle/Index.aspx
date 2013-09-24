<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bee.Web.BeePageView" %>

<%@ Import Namespace="Bee.Web" %>
<%@ Import Namespace="Bee" %>
<%@ Import Namespace="Bee.Util" %>
<%@ Import Namespace="System.Collections.Generic" %>
<% System.Data.DataTable dataTable = Model as System.Data.DataTable;  

%>
<div class="pageHeader">
    <form id="pageForm<%=PageId %>" action="<%=HtmlHelper.ForActionLink("Index") %>"
    method="post" class="required-validate alertMsg">
    <%=HtmlHelper.ForHidden("pageNum")%>
    <%=HtmlHelper.ForHidden("pageSize")%>
    <%=HtmlHelper.ForHidden("orderField")%>
    <%=HtmlHelper.ForHidden("orderDirection")%>
    <%=HtmlHelper.ForHidden("recordCount")%>
    <div class="searchBar">
        <ul class="searchContent">
            <li class="unitBox">
                <label>
                    标题：</label>
                <%=HtmlHelper.ForTextBox("title") %>
            </li>
            <li>
                <label>
                    创建时间：</label><input type='text' style='width: 70px' name='createtimebegin' value='<%=ViewData["createtimebegin"] %>'
                        class='date' />
                -
                <input style='width: 70px' type='text' name='createtimeend' value='<%=ViewData["createtimeend"] %>'
                    class='date' />
            </li>
        </ul>
    </div>
    </form>
</div>
<div class="pageContent">
    <div class="panelBar">
        <ul class="toolBar">
            <li><a class="add" href="<%=HtmlHelper.ForActionLink("Show") %>?id=-1" target="dialog"
                max="true" mask="true" width="800" height="600" title="创建" rel="CD<%=PageId %>">
                <span>添加</span></a></li>
        </ul>
        <ul class="searchBar">
            <li><a class="button" href="javascript:" onclick="javascript:autoList();"><span>检索</span>
            </a></li>
        </ul>
    </div>
    <table id="table<%=PageId %>" class="table" width="1000" layouth="136">
        <thead>
            <th width='25'>
                <input type='checkbox' group='ids' class='checkboxCtrl'>
            </th>
            <th <%=HtmlHelper.ForSortOrder("Id") %>>
                Id
            </th>
            <th <%=HtmlHelper.ForSortOrder("NickName") %>>
                标题
            </th>
            <th>
                摘要
            </th>
            <th>
                封面图片
            </th>
            <th>
                创建时间
            </th>
            <th width='210'>
                操作
            </th>
        </thead>
        <tbody>
            <%
                if (dataTable != null)
                {
                    foreach (System.Data.DataRow row in dataTable.Rows)
                    {%>
            <tr>
                <td>
                    <input name='ids' value='<%=row[0] %>' type='checkbox'>
                </td>
                <td>
                    <%=HtmlHelper.AutoFormatRowItem(row, "id")%>
                </td>
                <td>
                    <%=HtmlHelper.AutoFormatRowItem(row, "title")%>
                </td>
                <td>
                    <%=HtmlHelper.AutoFormatRowItem(row, "description")%>
                </td>
                <td>
                    <a href="<%=HtmlHelper.AutoFormatRowItem(row, "picurl")%>" target="imgtip">预览</a>
                </td>
                <td>
                    <%=row.ForDatetime("createtime", "yyyy-MM-dd HH:mm:ss")%>
                </td>
                <td>
                    <a title='编辑' target="dialog" mask="true" max="true" width="800" height="600" rel="CD<%=PageId %>"
                        href='<%=HtmlHelper.ForActionLink("Show") %>?id=<%=row[0].ToString() %>' class='btnEdit'>
                        编辑</a> <a title='是否确认删除？' target="ajaxTodo" href='<%=HtmlHelper.ForActionLink("delete") %>.bee?id=<%=row[0].ToString() %>'
                            class='btnDel'>删除</a>
                </td>
            </tr>
            <%}
                } %>
        </tbody>
    </table>
    <div class='panelBar'>
        <div class='pages'>
            <span>显示</span>
            <select class='combox' name='numPerPage' onchange="javascript:autoChangePageSize(this);">
                <%=HtmlHelper.ForPageSizeSelect() %>
            </select>
            <span>条，共<%=ViewData["recordcount"] %>条</span>
        </div>
        <div class='pagination' totalcount='<%=ViewData["recordCount"] %>' numperpage='<%=ViewData["pagesize"] %>'
            pagenumshown='10' currentpage='<%=ViewData["pagenum"] %>' click="javascript:autoJumpTo(#pageNum#);">
        </div>
    </div>
</div>
