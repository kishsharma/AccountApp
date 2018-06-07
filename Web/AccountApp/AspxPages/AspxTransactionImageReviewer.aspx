<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false" CodeFile="AspxTransactionImageReviewer.aspx.vb" Inherits="AspxPages_AspxTransactionImageReviewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <style type="text/css">
        .imgresp
        {
            width:100%;
            float:left;
        }    
    </style>
        <form id="frmAssignimage" runat="server" role="form">
        <asp:ScriptManager ID="SMLocationMaster" runat="server" EnablePageMethods="true" ScriptMode="Release">
        </asp:ScriptManager>
        <asp:HiddenField ID="HdnIDImage" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="HdnIdentity" runat="server" ClientIDMode="Static" />
        <div class="company_box">
            <div class="company_list">
                    <strong>Status Review:</strong><label class="form-control" runat="server" id="lblReview"></label>
                    <label id="lblStatus" runat="server">Status</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="31">
                        <asp:ListItem Value="1">Approved</asp:ListItem>
                        <asp:ListItem Value="2">Reject</asp:ListItem>
                        <asp:ListItem Value="3">On-Hold</asp:ListItem>
                    </asp:DropDownList>
                    <label id="lblRemark" runat="server">Remark</label>
                    <input type="text" id="txtRemark" runat="server" name="textfield" placeholder=" Remark" clientidmode="Static" tabindex="32" maxlength="255" />
<br /><br /><br />                    <asp:Button ID="btnReview" runat="server" Text="Apply Review" CssClass="submit_btn" />
            </div>
                    <label id="lblPrevRemark" runat="server"></label>
                <div class="imgresp">
                    <img id="imgprvw" runat="server" src='../Upload/Transaction/UploadTransaction.png' alt='' style="max-width:100%;"/>
                </div>
        </div>
    </form>
</asp:Content>

