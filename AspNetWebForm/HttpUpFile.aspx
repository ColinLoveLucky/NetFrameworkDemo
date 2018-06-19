<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HttpUpFile.aspx.cs" Inherits="AspNetWebForm.HttpUpFile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data" method="post">
        <div>
            <input id="firstFile" type="file" name="firstFile" runat="server"><br />
            &nbsp;<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="上传" /><br />
            <asp:Label ID="Label1" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
