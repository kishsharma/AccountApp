<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false" CodeFile="AspxUploadHSNCode.aspx.vb" Inherits="AspxPages_AspxUploadHSNCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script src="../Resource/ResponsiveList/media/js/jquery.dataTables.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.15/css/jquery.dataTables.min.css" />
    <script type="text/javascript" language="javascript" class="init">
        $(document).ready(function () {
            $('#DataList').DataTable();
        });
    </script>
    <form id="FrmHSNUpload" runat="server" clientidmode="Static" role="form">
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button ID="btnUpload" runat="server" Text="Upload"/>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Has Header ?" Visible="false" />
        <asp:RadioButtonList ID="rbHDR" runat="server" Visible="false">
            <asp:ListItem Text = "Yes" Value = "Yes" Selected = "True" ></asp:ListItem>
            <asp:ListItem Text = "No" Value = "No"></asp:ListItem>
        </asp:RadioButtonList>
         <div id="FilterRibbon" runat="server" class="filter_box">
            <asp:Button ID="BtnUploadHSN" name="button" runat="server"  class="add_view_btn" Text="I Agree to upload for HSN Code provided with Excel" Visible="false" />
       <input type="submit" name="button" id="BtnSearch" value="Search" class="add_view_btn" runat="server" />
            <input type="submit" name="button" id="BtnAddHREF" value="Add New" class="add_view_btn" runat="server" />
        </div>
        <div class="filter_list_box">
            <asp:Label ID="LblSearchData" runat="server" />
        </div>
    </form>
</asp:Content>

