<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false"
    CodeFile="AspxSearchList.aspx.vb" Inherits="AspxPages_AspxSearchList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Resource/js/Chosen/chosen.css" rel="stylesheet" type="text/css" />
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
    <div id="FilterRibbon" runat="server" class="filter_box">
        <div class="company_list" id="Companylist" runat="server">
            <label>
                Company</label>
            <asp:DropDownList ID="DDLCompany" class="chosen-select" runat="server" TabIndex="2" />
        </div>
        <input type="submit" name="button" id="BtnSearch" value="Search" class="add_view_btn" runat="server" />
        <input type="submit" name="button" id="BtnAddHREF" value="Add New" class="add_view_btn" runat="server" />
    </div>
    <div class="filter_list_box">
        <asp:Label ID="LblSearchData" runat="server" />
    </div>
    </form>
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
