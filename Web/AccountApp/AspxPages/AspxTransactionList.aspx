﻿<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false" CodeFile="AspxTransactionList.aspx.vb" Inherits="AspxPages_AspxTransactionList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%-- <script src="../Resource/ResponsiveList/media/js/jquery.js" type="text/javascript"></script>--%>
    <script src="../Resource/ResponsiveList/media/js/jquery.dataTables.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.15/css/jquery.dataTables.min.css" />
    <script type="text/javascript" language="javascript" class="init">
        $(document).ready(function () {
            $('#DataList').DataTable();
        });
    </script>
    <form id="FrmSearchList" runat="server">
    <asp:HiddenField ID="HdnIdBlock" runat="server" ClientIDMode="Static" />
    
    <input id="LblScreenName" runat="server" value="Screen Name" class="LableHeader"
        visible="false" />
    <%--<div id="FilterRibbon" runat="server" class="filter_box">
        <input type="submit" name="button" id="BtnAddHREF" value="Add New" class="add_view_btn" runat="server" />
    </div>--%>
    <div class="filter_list_box">
        <asp:Label ID="LblSearchData" runat="server" />
    </div>
    </form>
</asp:Content>

