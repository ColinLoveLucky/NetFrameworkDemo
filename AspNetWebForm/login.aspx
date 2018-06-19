<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="AspNetWebForm.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label>Name:</label>
            <asp:TextBox runat="server" ID="txtName"></asp:TextBox>
            <label>Password:</label>
            <asp:TextBox runat="server" ID="txtPassword" TextMode="Password"></asp:TextBox>
            <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="登陆" />
        </div>
    </form>
</body>
</html>
