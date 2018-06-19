<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WindowsAuthentication.aspx.cs" Inherits="AspNetWebForm.WindowsAuthentication" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>WindowsAuthentication DEMO  - http://www.cnblogs.com/fish-li/</title>
</head>
<body>
    <form runat="server">
        <% 
            if (Request.IsAuthenticated)
            { %>
    当前登录全名：<%= Context.User.Identity.Name%>
        <br />
        <% var user = AspNetWebForm.UserHelper.GetCurrentUserInfo(Context); %>
        <% if (user != null)
           { %>
        用户短名：<%= user.GivenName%>
        <br />
        用户全名：<%= user.FullName %>
        <br />
        邮箱地址：<%= user.Email %>
          认证类型：<%= HttpContext.Current.User.Identity.AuthenticationType %>
        
        <% } %>
        <% }
           else
           { %>
        <div>
            <label>Name:</label>
            <asp:TextBox runat="server" ID="txtName"></asp:TextBox>
            <label>Password:</label>
            <asp:TextBox runat="server" ID="txtPassword" TextMode="Password"></asp:TextBox>
            <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="登陆" />
        </div>
        <% } %>
    </form>
</body>
</html>
