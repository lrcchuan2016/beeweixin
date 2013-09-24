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
                    微信昵称：</label>
                <%=HtmlHelper.ForTextBox("NickName") %>
            </li>
            <li>
                <label>
                    备注名：</label>
                <%=HtmlHelper.ForTextBox("RemarkName") %>
            </li>
            <li>
                <label>
                    用户组：</label>
                <%=HtmlHelper.ForSelect("groupid", "weixingroup", true)%>
            </li>
            <li>
                <label>
                    是否关联：</label>
                <%=HtmlHelper.ForSelect("linkFlag", "是否", true)%>
            </li>
            <li>
                <label>
                    是否关注：</label>
                <%=HtmlHelper.ForSelect("validFlag", "是否", true)%>
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
            <li><a class="" href="<%=HtmlHelper.ForActionLink("SynchronizeAll") %>" target="ajaxTodo">
                <span>重新同步所有用户</span></a></li>
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
                微信昵称
            </th>
            <th <%=HtmlHelper.ForSortOrder("remarkname") %>>
                备注名
            </th>
            <th <%=HtmlHelper.ForSortOrder("groupid") %>>
                用户组
            </th>
            <th>
                国家
            </th>
            <th>
                省份
            </th>
            <th>
                城市
            </th>
            <th>
                性别
            </th>
            <th>
                是否关联
            </th>
            <th>
                是否关注
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
                    <%=HtmlHelper.AutoFormatRowItem(row, "nickname")%>
                </td>
                <td>
                    <%=HtmlHelper.AutoFormatRowItem(row, "remarkname")%>
                </td>
                <td>
                    <%=HtmlHelper.AutoFormatRowItem(row, "groupid")%>
                </td>
                <td>
                    <%=HtmlHelper.AutoFormatRowItem(row, "country")%>
                </td>
                <td>
                    <%=HtmlHelper.AutoFormatRowItem(row, "province")%>
                </td>
                <td>
                    <%=HtmlHelper.AutoFormatRowItem(row, "city")%>
                </td>
                <td>
                    <%=HtmlHelper.AutoFormatRowItem(row, "sex")%>
                </td>
                <td>
                    <%=HtmlHelper.ForDataMapping("是否", row["fakeid"].ToString() != "")%>
                </td>
                <td>
                    <%=HtmlHelper.ForDataMapping("是否", row["validflag"].ToString())%>
                </td>
                <td>
                    <%=row.ForDatetime("createtime", "yyyy-MM-dd HH:mm:ss")%>
                </td>
                <td>
                    <%--                    <a title='编辑' target="dialog" mask="true" width="800" height="480" rel="CD<%=PageId %>"
                        href='/WeiXinUser/Show.bee?id=<%=row[0].ToString() %>' class='btnEdit'>编辑</a>--%>
                    <a title='发送消息' target="dialog" mask="true" width="400" height="300" rel="CD<%=PageId %>"
                        href='/WeiXinUser/SendMessage.bee?openid=<%=row["openid"].ToString() %>' class='button'>
                        <span>发送消息</span></a>
                    <%if (row["fakeid"].ToString() == "")
                      { %>
                    <a title='是否确认邀请关联' target="ajaxTodo" href='<%=HtmlHelper.ForActionLink("Link") %>?openId=<%=row["openid"].ToString() %>'
                        class="button"><span>邀请绑定</span></a>
                    <%} %>
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
