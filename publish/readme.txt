直接运行【启动】.exe, 
用户名密码均为admin

若要使用用户管理及菜单管理 需要配置web.config中的以下配置项

    <!-- 关注用户管理使用-->
    <add key="WeiXinUserName" value="微信的账号"/>
    <add key="WeiXinPassword" value="微信的密码"/>
    
    <!-- 管理菜单使用-->
    <add key ="WeiXinAppId" value="服务号会有AppId"/>
    <add key ="WeiXinAppSec" value="AppSec"/>