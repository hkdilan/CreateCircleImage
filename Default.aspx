<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
        <br />
        <asp:TextBox ID="tbDiameter" runat="server"></asp:TextBox>
        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />

        <br />

        <div style="float:left">
            <asp:Image runat="server" ID="oldImage" />
        </div>
        
        <div style="background-color:black; float:left">
            <asp:Image runat="server" ID="newImage" />
        </div>
    </form>
</body>
</html>
