<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false" CodeFile="AspxRejectedImage.aspx.vb" Inherits="AspxPages_AspxRejectedImage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <form id="frmAssignimage" runat="server" role="form">
        <asp:ScriptManager ID="SMLocationMaster" runat="server" EnablePageMethods="true" ScriptMode="Release">
        </asp:ScriptManager>
        <asp:HiddenField ID="Hdnstatus" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="HdnIDImage" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="HdnIdentity" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnIDPurchaseMaster" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnIDPurchaseMasteredit" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnClientid" runat="server" ClientIDMode="Static" />
        <div class="company_box">
            <img id="imgprvw" runat="server" src='../Upload/Transaction/UploadTransaction.png' alt='' width="1050" />
        </div>
    </form>
</asp:Content>

