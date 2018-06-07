<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false" CodeFile="AspxFileUpload.aspx.vb" Inherits="AspxPages_AspxFileUpload" %>

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
 <link href="../Resource/js/Chosen/chosen.css" rel="stylesheet" type="text/css" />
    <form id="frmUsers" runat="server"  clientidmode="Static" role="form">
    <asp:HiddenField ID="HdnIdentity" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HdnIdSelected" runat="server" />
    <asp:HiddenField ID="HdnSelected" runat="server" />
    <div class="filter_list_box" id="divMain" runat="server" style="margin: 40px 0 0 0">
        <div class="company_list">
            <label>Report Name</label>
        <input type="text" class="form-control" runat="server" id="TxtReportName" placeholder="Report Name"
            clientidmode="Static" tabindex="1" maxlength="255" onkeypress="return IsAlphaNumeric(event);" />
        </div>
        <div class="company_list" id="Companylist" runat="server">
            <label>
                For Company</label>
            <asp:DropDownList ID="DDLCompany" class="chosen-select" runat="server" TabIndex="2" AutoPostBack="true" />
        </div>
        <div class="company_list" style="visibility:collapse;">
            <label id="Label10" runat="server">
                Role *</label><br />
            <asp:DropDownList ID="DDLIDRole" class="chosen-select" runat="server" AutoPostBack="true"
                TabIndex="3" />
        </div>
        <div class="company_list">
            <label>Remark</label>
        <input type="text" class="form-control" runat="server" id="txtRemark" placeholder="Remark"
            clientidmode="Static" tabindex="4" maxlength="255" onkeypress="return IsAlphaNumeric(event);" />
        </div>
    </div>
    <div id="divImage" runat="server" align="center">
        <div class="company_list" style="width: 35%;">
           
            <img id="imgprvw" alt="No File Uploaded yet..." runat="server" clientidmode="Static" src="../Resource/Images/Super.png"
                style="height: auto; border: 1px solid black; float: left; max-width: 100%; margin: 0px 10px 0px 0px;" />
            <br />
            <%--<div class="company_list">--%>
            <asp:FileUpload ID="flupload" runat="server" ClientIDMode="Static" CssClass="btnupload"
                TabIndex="14" Style="float: left; background-color: #fff; border: 0px none; padding: 5px;
                width: auto; margin: 0px 0px 10px;" />
            <%--</div>--%>
            <br />
            <asp:Button ID="btnUpload" runat="server" ClientIDMode="Static" CssClass="submit_btn"
                OnClick="fnUpload" Text="Submit" Style="float: left; padding: 5px 10px; font-size: 12px;"
                OnClientClick="return validateImageUpload()" TabIndex="15" />
            <%--OnClick="fnUpload" --%>
            <asp:Button ID="BtnClearImage" runat="server" ClientIDMode="Static" CssClass="submit_btn"
                Text="Clear" Style="float: left; padding: 5px 10px; font-size: 12px;" TabIndex="16" />
            <%--OnClick="fnClear"--%>
        </div>
      
    </div>
     <div class="filter_list_box">
        <asp:Label ID="LblSearchData" runat="server" />
    </div>
    </form>
     <script type="text/javascript" language="javascript">
         function validateImageUpload() {
             var avatar = document.getElementById("<%=flupload.ClientID%>");
             if (avatar.value == "") {
                 alert("Please browse the image first!");
                 return false
             }
             return true
         }
        
    </script>
     <script src="../Resource/js/Chosen/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        var config = {
            '.chosen-select': {},
            '.chosen-select-deselect': { allow_single_deselect: true },
            '.chosen-select-no-single': { disable_search_threshold: 10 },
            '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
            '.chosen-select-width': { width: "95%" }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        }
    </script>
</asp:Content>

