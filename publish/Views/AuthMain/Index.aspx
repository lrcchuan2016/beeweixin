<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bee.Web.BeePageView" %>

<%@ Import Namespace="Bee.Web" %>
<%@ Import Namespace="Bee" %>
<%@ Import Namespace="System.Collections.Generic" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Bee OPOA Platform</title>
    <link href="/themes/default/style.css" rel="stylesheet" type="text/css" />
    <link href="/themes/core.css" rel="stylesheet" type="text/css" />
   

    <script src="/js/jquery-1.8.0.min.js" type="text/javascript"></script>

    <script src="/js/jquery.validate.js" type="text/javascript"></script>

    <script src="/xheditor/xheditor-1.1.13-zh-cn.min.js" type="text/javascript"></script>
    <script src="/uploadify/jquery.uploadify.min.js" type="text/javascript"></script>
    <script src="/tagsinput/jquery.tagsinput.js" type="text/javascript"></script>

    <script src="/js/bee.min.js" type="text/javascript"></script>
    

    <%--<script src="/js/bee.accordion.js" type="text/javascript"></script>

    <script src="/js/bee.jbar.js" type="text/javascript"></script>

    <script src="/js/bee.jdrag.js" type="text/javascript"></script>

    <script src="/js/bee.jdialog.js" type="text/javascript"></script>

    <script src="/js/bee.jtree.js" type="text/javascript"></script>

    <script src="/js/bee.pagenation.js" type="text/javascript"></script>

    <script src="/js/bee.jcheckbox.js" type="text/javascript"></script>

    <script src="/js/bee.navtab.js" type="text/javascript"></script>

    <script src="/js/bee.jtable.js" type="text/javascript"></script>

    <script src="/js/bee.cssTable.js" type="text/javascript"></script>

    <script src="/js/bee.tabs.js" type="text/javascript"></script>

    <script src="/js/bee.database.js" type="text/javascript"></script>

    <script src="/js/bee.contextmenu.js" type="text/javascript"></script>

    <script src="/js/bee.alertMsg.js" type="text/javascript"></script>

    <script src="/js/bee.jdatepicker.js" type="text/javascript"></script>

    <script src="/js/bee.ajax.js" type="text/javascript"></script>

    <script src="/js/bee.jbox.js" type="text/javascript"></script>

    <script src="/js/bee.jselection.js" type="text/javascript"></script>

    <script src="/js/bee.combox.js" type="text/javascript"></script>

    <script src="/js/bee.jtip.js" type="text/javascript"></script>

    <script src="/js/bee.panel.js" type="text/javascript"></script>

    <script src="/js/bee.effects.js" type="text/javascript"></script>

    <script src="/js/bee.sortDrag.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/bee.widget.js"></script>--%>


    <script type="text/javascript">

        function initLayout() {
            var iContentW = $(window).width() - (bee.ui.sbar ? $("#sidebar").width() + 10 : 34) - 5;
            var iContentH = $(window).height() - $("#header").height() - 23;  //- 34;

            $("#container").width(iContentW);
            $("#container .tabsPageContent").height(iContentH - 34).find("[layoutH]").layoutH();
            $("#leftside, #sidebar, #sidebar_s .collapse, #splitBar, #splitBarProxy").height(iContentH - 5);
            //$("#taskbar").css({ top: iContentH + $("#header").height() + 5, width: $(window).width() });
        }

        $(function() {
            bee.init({ callback: function() {
                initLayout();
                bee.initUI();
                //setTimeout(function() { $("#sidebar .toggleCollapse div").trigger("click"); }, 10);

                var headerTog = $("<div style='position: absolute; top: 55px; right: 10px;' class='widget-head'><a class='collapse'></a></div>").appendTo("body");
                headerTog.toggle(function() {
                    $("#header").height(0).fadeOut();
                    //initLayout();

                    $("#leftside, #splitBar, #splitBarProxy, #container").each(function() {
                        $(this).animate({ top: "0px" }, "slow", "swing", function() {
                            $(window).trigger("resize");
                        });
                    });
                    headerTog.animate({ top: "0px" }, "slow");
                    $("#header").hide();
                    headerTog.find("a").css("background-position", "0 -100px");
                }, function() {

                    //$("#header").slideDown(function() { initLayout(); });
                    $("#leftside, #splitBar, #splitBarProxy, #container").each(function() { $(this).animate({ top: "55px" }, "slow"); });
                    headerTog.animate({ top: "55px" }, "slow");
                    $("#header").height(55).fadeIn();
                    $(window).trigger("resize");
                    headerTog.find("a").css("background-position", "0 0px");
                });

                headerTog.trigger("click");
            }
            });

            var ajaxbg = $("#background,#progressBar");
            ajaxbg.hide();
            $(document).ajaxStart(function() {
                ajaxbg.show();
            }).ajaxStop(function() {
                ajaxbg.hide();
            });

            $("div.tabsHeader li, div.tabsPageHeader li, div.accordionHeader, div.accordion").hoverClass("hover");

            $(window).resize(function() {
                initLayout();
            });

            setInterval("HeartBeat()", 300000);
        });
    </script>

</head>
<body scroll="no">
    <div id="layout">
        <div id="header">
            <div class="headerNav">
                <a class="logo">标志</a>
                <ul class="nav">
                    <li><span style="color: #B9CCDA">你好：<%=ViewData["username"] %></span></li>
                    <li><a id="logout" href="/AuthMain/Logout.bee">退出</a>
                    <span style="display:none;"><a id="innerLogin" href="/AuthMain/InnerLogin.bee?username=<%=ViewData["username"] %>" target="dialog" mask="true" width="410" height="316">重新登入</a></span>
                    </li>
                </ul>
            </div>
        </div>
        <div id="leftside">
            <div id="sidebar_s">
                <div class="collapse">
                    <div class="toggleCollapse">
                        <div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="sidebar">
                <div class="toggleCollapse">
                    <h2>
                        主菜单</h2>
                    <div>
                        收缩</div>
                </div>
                <div class="accordion" fillspace="sidebar">
                    <% System.Data.DataTable dataTable = Model as System.Data.DataTable;
                       if (dataTable != null && dataTable.Rows.Count > 0)
                       {
                           System.Data.DataRow[] rows = dataTable.Select("parentid=0", "dispindex asc");
                           foreach (System.Data.DataRow row in rows)
                           {
                               int parentId = int.Parse(row["id"].ToString());
                    %>
                    <div class="accordionHeader">
                        <h2>
                            <span>Folder</span><%=row["title"]%></h2>
                    </div>
                    <div class="accordionContent">
                        <ul class="tree treeFolder expand">
                            <%=HtmlHelper.InnerForLeftMenu(dataTable, parentId)%>
                        </ul>
                    </div>
                    <%}
                       }%>
                </div>
            </div>
        </div>
        <div id="container">
            <div id="navTab" class="tabsPage">
                <div class="tabsPageHeader" style="margin-right: 34px">
                    <div class="tabsPageHeaderContent">
                        <!-- 显示左右控制时添加 class="tabsPageHeaderMargin" -->
                        <ul class="navTab-tab">
                            <li tabid="main" class="main"><a href="javascript:;"><span><span class="home_icon">我的主页</span></span></a></li>
                        </ul>
                    </div>
                    <div class="tabsLeft">
                        left</div>
                    <!-- 禁用只需要添加一个样式 class="tabsLeft tabsLeftDisabled" -->
                    <div class="tabsRight">
                        right</div>
                    <!-- 禁用只需要添加一个样式 class="tabsRight tabsRightDisabled" -->
                    <div class="tabsMore">
                        more</div>
                </div>
                <ul class="tabsMoreList">
                    <li><a href="javascript:;">我的主页</a></li>
                </ul>
                <div class="navTab-panel tabsPageContent">
                    <div class="page unitBox">
                        <div layouth="2">
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="splitBar">
        </div>
        <div id="splitBarProxy">
        </div>
    </div>
    <div id="footer">
        Copyright © 2012 Powered by <a target="_blank" href="http://beeopoa.codeplex.com">Bee OPOA Platform</a></div>
    <!--拖动效果-->
    <div class="resizable">
    </div>
    <!--阴影-->
    <div class="shadow" style="width: 508px; top: 148px; left: 296px;">
        <div class="shadow_h">
            <div class="shadow_h_l">
            </div>
            <div class="shadow_h_r">
            </div>
            <div class="shadow_h_c">
            </div>
        </div>
        <div class="shadow_c">
            <div class="shadow_c_l" style="height: 296px;">
            </div>
            <div class="shadow_c_r" style="height: 296px;">
            </div>
            <div class="shadow_c_c" style="height: 296px;">
            </div>
        </div>
        <div class="shadow_f">
            <div class="shadow_f_l">
            </div>
            <div class="shadow_f_r">
            </div>
            <div class="shadow_f_c">
            </div>
        </div>
    </div>
    <!--遮盖屏幕-->
    <div id="alertBackground" class="alertBackground">
    </div>
    <div id="dialogBackground" class="dialogBackground">
    </div>
    <div id='background' class='background'>
    </div>
    <div id='progressBar' class='progressBar'>
        数据加载中，请稍等...</div>
</body>
</html>
